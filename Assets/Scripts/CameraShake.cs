
using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float magnitude = 0.08f;

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        transform.localPosition = originalPosition;
        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float fade = 1f - elapsed / Mathf.Max(0.01f, duration);

            Vector2 offset = Random.insideUnitCircle
                * magnitude * fade;

            transform.localPosition =
                originalPosition +
                new Vector3(offset.x, offset.y, 0f);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }

    private void OnDisable()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }

        transform.localPosition = originalPosition;
    }
}
