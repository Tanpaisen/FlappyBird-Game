using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Ready, Playing, GameOver }

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;

    private int score;
    private int highScore;

    public int Score => score;
    public GameState CurrentState { get; private set; }
    public bool IsPlaying => CurrentState == GameState.Playing;

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
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        CurrentState = GameState.Ready;
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        UpdateUI();
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        CurrentState = GameState.Playing;
        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayMusic();
    }

    public void AddScore()
    {
        if (!IsPlaying) return;
        score++;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        UpdateUI();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayScore();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = $"Best: {highScore}";
    }

    public void GameOver()
    {
        if (!IsPlaying) return;
        CurrentState = GameState.GameOver;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayGameOver();
        }
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
