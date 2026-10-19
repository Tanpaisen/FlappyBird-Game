using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButtonDiagnostic : MonoBehaviour
{
    private RectTransform rectTransform;
    private Button button;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        bool inside = RectTransformUtility.RectangleContainsScreenPoint(
            rectTransform,
            Input.mousePosition,
            null
        );

        Debug.Log(
            "PAUSE DIAG | " +
            "inside=" + inside +
            " | active=" + gameObject.activeInHierarchy +
            " | interactable=" + button.interactable +
            " | mouse=" + Input.mousePosition +
            " | rectPos=" + rectTransform.position +
            " | size=" + rectTransform.rect.size
        );
    }

    public void TestDirectClick()
    {
        Debug.Log("PAUSE BUTTON ONCLICK FIRED");
    }
}