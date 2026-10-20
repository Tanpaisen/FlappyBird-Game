
using System.Collections;
using TMPro;
using UnityEngine;

public class NewBestEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text bestText;
    [SerializeField] private float duration = 0.6f;
    [SerializeField] private float maxScale = 1.25f;

    private Vector3 originalScale;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        if (bestText == null)
            bestText = GetComponent<TMP_Text>();

        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        animationCoroutine = StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            float scale = 1f + (maxScale - 1f)
                * Mathf.Sin(t * Mathf.PI);

            transform.localScale = originalScale * scale;

            yield return null;
        }

        transform.localScale = originalScale;
        animationCoroutine = null;
    }

    private void OnDisable()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        transform.localScale = originalScale;
    }
}
