using UnityEngine;

public class PipePair : MonoBehaviour
{
    private ScoreZone scoreZone;

    private void Awake()
    {
        scoreZone = GetComponentInChildren<ScoreZone>();
    }

    public void ResetPipe(float x, float y)
    {
        transform.position = new Vector3(x, y, 0f);

        if (scoreZone != null)
        {
            scoreZone.ResetScoreZone();
        }
    }
}