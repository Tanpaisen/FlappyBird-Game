using System.Collections;
using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip flapSound;
    [SerializeField] private AudioClip scoreSound;
    [SerializeField] private AudioClip hitGroundSound;
    [SerializeField] private AudioClip hitPipeSound;
    [SerializeField] private AudioClip gameOverSound;

    [Header("UI (optional)")]
    [SerializeField] private TMP_Text musicButtonText;

    private bool musicEnabled;
    private Coroutine gameOverCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
    }

    private void Start()
    {
        if (musicSource != null)
        {
            musicSource.playOnAwake = false;
            musicSource.loop = true;
            musicSource.spatialBlend = 0f;
            musicSource.volume = 0.25f;
            if (backgroundMusic != null) musicSource.clip = backgroundMusic;
        }
        if (sfxSource != null)
        {
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            sfxSource.spatialBlend = 0f;
            sfxSource.volume = 0.7f;
        }
        UpdateMusicUI();
    }

    public void PlayMusic()
    {
        if (musicEnabled && musicSource != null && musicSource.clip != null && !musicSource.isPlaying)
            musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayFlap() => PlaySFX(flapSound);
    public void PlayScore() => PlaySFX(scoreSound);
    public void PlayHitGround() => PlaySFX(hitGroundSound);
    public void PlayHitPipe() => PlaySFX(hitPipeSound);

    public void PlayGameOver()
    {
        if (gameOverCoroutine != null) StopCoroutine(gameOverCoroutine);
        gameOverCoroutine = StartCoroutine(PlayGameOverAfterImpact());
    }

    private IEnumerator PlayGameOverAfterImpact()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        PlaySFX(gameOverSound);
        gameOverCoroutine = null;
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.Save();
        if (!musicEnabled) StopMusic();
        else if (GameManager.Instance != null && GameManager.Instance.IsPlaying) PlayMusic();
        UpdateMusicUI();
    }

    private void UpdateMusicUI()
    {
        if (musicButtonText != null)
            musicButtonText.text = musicEnabled ? "Music: ON" : "Music: OFF";
    }
}
