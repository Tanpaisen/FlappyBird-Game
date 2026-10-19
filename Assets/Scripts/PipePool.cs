using System.Collections.Generic;
using UnityEngine;

public class PipePool : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private PipePair pipePrefab;
    [SerializeField] private int poolSize = 5;

    [Header("Movement")]
    [SerializeField] private float baseSpeed = 2.5f;

    [SerializeField] private float speedIncreasePerStep = 0.35f;

    [SerializeField] private int scorePerStep = 5;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float spacing = 4f;

    [Header("Spawn")]
    [SerializeField] private float startX = 5.5f;
    [SerializeField] private float leftBoundary = -7f;

    [Header("Random Height")]
    [SerializeField] private float minY = -1.5f;
    [SerializeField] private float maxY = 1.5f;

    private readonly List<PipePair> pipes = new();

    private void Start()
    {
        CreatePool();
    }

    private void Update()
    {
        if (GameManager.Instance == null ||
            !GameManager.Instance.IsPlaying)
        {
            return;
        }

        MovePipes();
        RecyclePipes();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            PipePair pipe = Instantiate(pipePrefab, transform);

            float x = startX + i * spacing;
            float y = Random.Range(minY, maxY);

            pipe.ResetPipe(x, y);

            pipes.Add(pipe);
        }
    }

    private void MovePipes()
    {
        float currentSpeed = GetCurrentSpeed();

        foreach (PipePair pipe in pipes)
        {
            pipe.transform.position +=
                Vector3.left *
                currentSpeed *
                Time.deltaTime;
        }
    }

    private void RecyclePipes()
    {
        foreach (PipePair pipe in pipes)
        {
            if (pipe.transform.position.x < leftBoundary)
            {
                float newX = GetRightMostX() + spacing;
                float newY = Random.Range(minY, maxY);

                pipe.ResetPipe(newX, newY);
            }
        }
    }

    private float GetRightMostX()
    {
        float maxX = float.MinValue;

        foreach (PipePair pipe in pipes)
        {
            if (pipe.transform.position.x > maxX)
            {
                maxX = pipe.transform.position.x;
            }
        }

        return maxX;
    }

    private float GetCurrentSpeed()
    {
        int step =
            GameManager.Instance.Score / scorePerStep;

        float currentSpeed =
            baseSpeed + step * speedIncreasePerStep;

        return Mathf.Min(
            currentSpeed,
            maxSpeed
        );
    }
}