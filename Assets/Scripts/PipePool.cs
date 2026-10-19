
using System.Collections.Generic;
using UnityEngine;

public class PipePool : MonoBehaviour
{
    [Header("Object Pool")]
    [SerializeField] private PipePair pipePrefab;
    [SerializeField] private int poolSize = 5;

    [Header("Dynamic Difficulty")]
    [SerializeField] private float baseSpeed = 2.5f;
    [SerializeField] private float speedIncreasePerStep = 0.75f;
    [SerializeField] private int scorePerStep = 5;
    [SerializeField] private int maxDifficultyLevel = 8;
    [SerializeField] private float maxSpeed = 8f;

    [Header("Pipe Spacing")]
    [SerializeField] private float spacing = 4f;

    [Header("Spawn Position")]
    [SerializeField] private float startX = 5.5f;
    [SerializeField] private float leftBoundary = -7f;

    [Header("Random Height")]
    [SerializeField] private float minY = -1.5f;
    [SerializeField] private float maxY = 1.5f;

    private readonly List<PipePair> pipes = new();

    private int lastDifficultyStep = -1;

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

    private float GetCurrentSpeed()
    {
        int step = GameManager.Instance.Score /
                Mathf.Max(1, scorePerStep);

        // Giới hạn tối đa 8 cấp độ
        step = Mathf.Clamp(
            step,
            0,
            Mathf.Max(1, maxDifficultyLevel) - 1
        );

        float currentSpeed = Mathf.Min(
            baseSpeed + step * speedIncreasePerStep,
            maxSpeed
        );

        if (step != lastDifficultyStep)
        {
            lastDifficultyStep = step;

            Debug.Log(
                $"LEVEL {step + 1}/{maxDifficultyLevel} | " +
                $"Score: {GameManager.Instance.Score} | " +
                $"Pipe Speed: {currentSpeed:F2}"
            );
        }

        return currentSpeed;
    }

    private void MovePipes()
    {
        float currentSpeed = GetCurrentSpeed();

        foreach (PipePair pipe in pipes)
        {
            pipe.transform.position +=
                Vector3.left * currentSpeed * Time.deltaTime;
        }
    }

    private void RecyclePipes()
    {
        foreach (PipePair pipe in pipes)
        {
            if (pipe.transform.position.x >= leftBoundary)
                continue;

            float newX = GetRightMostX() + spacing;
            float newY = Random.Range(minY, maxY);

            pipe.ResetPipe(newX, newY);
        }
    }

    private float GetRightMostX()
    {
        float maxX = float.MinValue;

        foreach (PipePair pipe in pipes)
        {
            maxX = Mathf.Max(
                maxX,
                pipe.transform.position.x
            );
        }

        return maxX;
    }
}
