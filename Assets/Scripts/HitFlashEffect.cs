
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HitFlashEffect : MonoBehaviour
{
    [SerializeField] private Image flashImage;
    [SerializeField] private float duration = 0.15f;
    [SerializeField, Range(0f, 1f)]
    private float maxAlpha = 0.55f;

    private Coroutine flashCoroutine;

    private void Awake()
    {
        if (flashImage == null)
            flashImage = GetComponent<Image>();

        if (flashImage == null)
        {
            Debug.LogError("HitFlashEffect: Missing Image!");
            enabled = false;
            return;
        }

        flashImage.raycastTarget = false;
        SetAlpha(0f);
    }

    public void Play()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                elapsed / Mathf.Max(0.01f, duration)
            );

            SetAlpha(maxAlpha * (1f - t));

            yield return null;
        }

        SetAlpha(0f);
        flashCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        Color color = Color.white;
        color.a = alpha;
        flashImage.color = color;
    }
}
