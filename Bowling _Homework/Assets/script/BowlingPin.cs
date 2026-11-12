using UnityEngine;

public class BowlingPin : MonoBehaviour
{
    public bool isScore = true;
    public ScoreSystem scoreSystem;

    public float scoreAngle = 45f;

    void Update()
    {
        float angleX = Mathf.DeltaAngle(0f, transform.localEulerAngles.x);
        float angleZ = Mathf.DeltaAngle(0f, transform.localEulerAngles.z);
        
        if (Mathf.Abs(angleX) > scoreAngle && isScore)
        {
            scoreSystem.AddScore(1);
            isScore = false;
        }

        if (Mathf.Abs(angleZ) > scoreAngle && isScore)
        {
            scoreSystem.AddScore(1);
            isScore = false;
        }
    }
}