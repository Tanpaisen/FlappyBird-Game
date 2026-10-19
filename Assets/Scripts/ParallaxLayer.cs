
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f;

    private SpriteRenderer first;
    private SpriteRenderer second;
    private Camera mainCamera;

    private void Start()
    {
        first = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        if (first.sprite == null || mainCamera == null)
        {
            enabled = false;
            return;
        }

        // Tạo thêm một bản sao chỉ chứa SpriteRenderer
        GameObject copy = new GameObject(gameObject.name + "_Copy");
        copy.transform.SetParent(transform.parent, false);
        copy.transform.localScale = transform.localScale;
        copy.transform.rotation = transform.rotation;

        second = copy.AddComponent<SpriteRenderer>();
        second.sprite = first.sprite;
        second.color = first.color;
        second.sharedMaterial = first.sharedMaterial;
        second.sortingLayerID = first.sortingLayerID;
        second.sortingOrder = first.sortingOrder;

        
        // Mép trái của vùng Camera nhìn thấy
        float cameraLeft = mainCamera.transform.position.x
            - mainCamera.orthographicSize * mainCamera.aspect;

        // Căn ảnh đầu tiên phủ kín mép trái ngay khi bắt đầu
        float offset = cameraLeft - first.bounds.min.x - 0.05f;

        first.transform.position += Vector3.right * offset;

        // Đặt bản sao nối tiếp ngay bên phải ảnh đầu tiên
        copy.transform.position = first.transform.position
            + Vector3.right * first.bounds.size.x;

    }

    private void Update()
    {
        if (GameManager.Instance == null ||
            !GameManager.Instance.IsPlaying)
            return;

        float movement = scrollSpeed * Time.deltaTime;

        first.transform.position += Vector3.left * movement;
        second.transform.position += Vector3.left * movement;

        float leftEdge = mainCamera.transform.position.x -
            mainCamera.orthographicSize * mainCamera.aspect;

        // Tái sử dụng ảnh khi chạy khỏi màn hình
        if (first.bounds.max.x < leftEdge)
        {
            first.transform.position = new Vector3(
                second.bounds.max.x + first.bounds.extents.x,
                first.transform.position.y,
                first.transform.position.z
            );
        }

        if (second.bounds.max.x < leftEdge)
        {
            second.transform.position = new Vector3(
                first.bounds.max.x + second.bounds.extents.x,
                second.transform.position.y,
                second.transform.position.z
            );
        }
    }
}
