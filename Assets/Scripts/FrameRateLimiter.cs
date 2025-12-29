using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 165;
    }
}
