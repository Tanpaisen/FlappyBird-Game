
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ParallaxLayer clouds;
    [SerializeField] private ParallaxLayer farMountains;
    [SerializeField] private ParallaxLayer nearMountains;

    [Header("Cloud Sprites")]
    [SerializeField] private Sprite morningCloud;
    [SerializeField] private Sprite noonCloud;
    [SerializeField] private Sprite eveningCloud;
    [SerializeField] private Sprite nightCloud;

    [Header("Cycle Settings")]
    [SerializeField] private int pointsPerPhase = 10;
    [SerializeField] private float transitionDuration = 2f;

    private readonly Color[] skyColors =
    {
        new Color32(173, 216, 235, 255), // Morning
        new Color32(78, 198, 235, 255),  // Noon
        new Color32(239, 143, 119, 255), // Evening
        new Color32(35, 39, 79, 255)     // Night
    };

    private readonly Color[] cloudColors =
    {
        Color.white,
        new Color32(255, 250, 235, 255),
        new Color32(255, 172, 135, 255),
        new Color32(115, 105, 165, 255)
    };

    private readonly Color[] farColors =
    {
        Color.white,
        new Color32(220, 255, 245, 255),
        new Color32(200, 150, 165, 255),
        new Color32(65, 85, 125, 255)
    };

    private readonly Color[] nearColors =
    {
        Color.white,
        Color.white,
        new Color32(185, 120, 145, 255),
        new Color32(45, 65, 100, 255)
    };

    private int currentPhase;
    private float elapsed;

    private Color startSky;
    private Color startClouds;
    private Color startFar;
    private Color startNear;

    private Color currentClouds;
    private Color currentFar;
    private Color currentNear;

    private bool transitioning;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("DayNightCycle: Camera missing!");
            enabled = false;
            return;
        }

        currentPhase = 0;
        transitioning = false;

        ApplyPhase(0);
        UpdateCloudSprite(0);
    }

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        int score = GameManager.Instance.Score;
        int phase = (score / Mathf.Max(1, pointsPerPhase)) % 4;

        if (phase != currentPhase)
        {
            startSky = mainCamera.backgroundColor;
            startClouds = currentClouds;
            startFar = currentFar;
            startNear = currentNear;

            currentPhase = phase;
            elapsed = 0f;
            transitioning = true;
            UpdateCloudSprite(currentPhase);

            Debug.Log(
                $"DAY/NIGHT | Score: {score} | Phase: {phase}"
            );
        }

        if (!transitioning)
            return;

        elapsed += Time.deltaTime;

        float t = Mathf.Clamp01(
            elapsed / Mathf.Max(0.01f, transitionDuration)
        );

        // Chuyển màu mượt
        t = t * t * (3f - 2f * t);

        mainCamera.backgroundColor =
            Color.Lerp(startSky, skyColors[currentPhase], t);

        currentClouds =
            Color.Lerp(startClouds, cloudColors[currentPhase], t);

        currentFar =
            Color.Lerp(startFar, farColors[currentPhase], t);

        currentNear =
            Color.Lerp(startNear, nearColors[currentPhase], t);

        ApplyLayerColors();

        if (elapsed >= transitionDuration)
        {
            transitioning = false;
            ApplyPhase(currentPhase);
        }
    }

    private void ApplyPhase(int phase)
    {
        mainCamera.backgroundColor = skyColors[phase];

        currentClouds = cloudColors[phase];
        currentFar = farColors[phase];
        currentNear = nearColors[phase];

        ApplyLayerColors();
    }

    private void ApplyLayerColors()
    {
        if (clouds != null)
            clouds.SetTint(currentClouds);

        if (farMountains != null)
            farMountains.SetTint(currentFar);

        if (nearMountains != null)
            nearMountains.SetTint(currentNear);
    }

    
    private void UpdateCloudSprite(int phase)
    {
        if (clouds == null)
            return;

        Sprite selectedSprite = null;

        switch (phase)
        {
            case 0:
                selectedSprite = morningCloud;
                break;

            case 1:
                selectedSprite = noonCloud;
                break;

            case 2:
                selectedSprite = eveningCloud;
                break;

            case 3:
                selectedSprite = nightCloud;
                break;
        }

        clouds.SetCloudSprite(selectedSprite);
    }

}
