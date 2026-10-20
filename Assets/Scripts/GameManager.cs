
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Ready,
        Playing,
        Paused,
        GameOver
    }

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game Over Results")]
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalBestText;
    [SerializeField] private TMP_Text newBestText;

    private bool brokeHighScore;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;

    private int score;
    private int highScore;

    public int Score => score;
    public GameState CurrentState { get; private set; }

    public bool IsPlaying =>
        CurrentState == GameState.Playing;

    public bool IsPaused =>
        CurrentState == GameState.Paused;

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
        brokeHighScore = false;

        CurrentState = GameState.Ready;

        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(false);

        UpdateUI();

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPlaying)
                PauseGame();
            else if (IsPaused)
                ResumeGame();
        }
    }

    public void StartGame()
    {
        if (CurrentState != GameState.Ready)
            return;

        CurrentState = GameState.Playing;
        scoreText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);

        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);

        Time.timeScale = 1f;

        AudioManager.Instance?.PlayMusic();
    }

    public void PauseGame()
    {
        if (!IsPlaying)
            return;

        CurrentState = GameState.Paused;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.SetActive(false);

        Time.timeScale = 0f;

        AudioManager.Instance?.PauseMusic();
    }

    public void ResumeGame()
    {
        if (!IsPaused)
            return;

        CurrentState = GameState.Playing;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(true);

        Time.timeScale = 1f;

        AudioManager.Instance?.ResumeMusic();
    }

    public void AddScore()
    {
        if (!IsPlaying)
            return;

        score++;
   
        if (score > highScore)
        {
            highScore = score;
            brokeHighScore = true;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        UpdateUI();

        AudioManager.Instance?.PlayScore();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
        highScoreText.text = $"Best: {highScore}";
    }

    private void UpdateGameOverUI()
    {
        if (finalScoreText != null)
            finalScoreText.text = $"SCORE: {score}";

        if (finalBestText != null)
            finalBestText.text = $"BEST: {highScore}";

        if (newBestText != null)
            newBestText.gameObject.SetActive(brokeHighScore);
    }

    public void GameOver()
    {
        if (!IsPlaying)
            return;

        CurrentState = GameState.GameOver;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayGameOver();
        }

        UpdateGameOverUI();
        scoreText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(false);

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
