using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [Header("Collision classification")]
    [Tooltip("Select the Ground layer here. Other collisions will use the pipe sound.")]
    [SerializeField] private LayerMask groundLayers;

    private Rigidbody2D rb;
    private bool isAlive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive) return;
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            Jump();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        if (AudioManager.Instance != null) AudioManager.Instance.PlayFlap();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive || GameManager.Instance == null || !GameManager.Instance.IsPlaying) return;
        isAlive = false;

        if (AudioManager.Instance != null)
        {
            bool isGround = (groundLayers.value & (1 << collision.gameObject.layer)) != 0;
            if (isGround) AudioManager.Instance.PlayHitGround();
            else AudioManager.Instance.PlayHitPipe();
        }

        GameManager.Instance.GameOver();
    }
}
