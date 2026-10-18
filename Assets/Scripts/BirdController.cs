using UnityEngine;

public class BirdController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb;
    private bool isAlive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAlive)
            return;

        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive)
            return;

        isAlive = false;

        GameManager.Instance.GameOver();
    }
}