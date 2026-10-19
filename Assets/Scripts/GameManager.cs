using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject gameOverPanel;

    private int score;
    private int highScore;

    public bool IsPlaying { get; private set; } = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        IsPlaying = true;

        gameOverPanel.SetActive(false);
        UpdateUI();
    }

    public void AddScore()
    {
        if (!IsPlaying)
            return;

        score++;

        if (score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = $"Best: {highScore}";
    }

    public void GameOver()
    {
        if (!IsPlaying)
            return;

        IsPlaying = false;

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}