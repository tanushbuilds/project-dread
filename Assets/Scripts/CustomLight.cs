using UnityEngine;

public class CustomLight : MonoBehaviour
{
    public float lightIntensity = 1f;
    private Light customLight;

    private void Awake()
    {
        customLight = GetComponent<Light>();
        customLight.intensity = GetComponent<Light>().intensity > 0f ? lightIntensity : 0f;
    }
}
