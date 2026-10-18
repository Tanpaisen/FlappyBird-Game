using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2.5f;

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}