
using System.Collections;
using TMPro;
using UnityEngine;

public class ScorePopupEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private float duration = 0.65f;
    [SerializeField] private float moveDistance = 65f;

    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private Color initialColor;
    private Coroutine currentAnimation;

    private void Awake()
    {
        if (popupText == null)
            popupText = GetComponent<TMP_Text>();

        rectTransform = GetComponent<RectTransform>();

        initialPosition = rectTransform.anchoredPosition;
        initialColor = popupText.color;

        popupText.enabled = false;
    }

    public void Play()
    {
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        rectTransform.anchoredPosition = initialPosition;

        popupText.color = initialColor;
        popupText.text = "+1";
        popupText.enabled = true;

        currentAnimation = StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Time.deltaTime giúp hiệu ứng dừng khi Pause
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / Mathf.Max(0.01f, duration)
            );

            rectTransform.anchoredPosition =
                initialPosition + Vector2.up * (moveDistance * t);

            Color color = initialColor;
            color.a = initialColor.a * (1f - t);
            popupText.color = color;

            yield return null;
        }

        popupText.enabled = false;
        currentAnimation = null;
    }
}
