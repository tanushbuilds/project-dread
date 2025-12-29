using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light _light;
    private CustomLight customLight;

    private void Awake()
    {
        _light = GetComponent<Light>();
        if (TryGetComponent<CustomLight>(out CustomLight customLight))
        {
            this.customLight = customLight;
        }
    }
    public void SwitchLightOff()
    {
        _light.intensity = 0f;
    }

    public void SwitchLightOn()
    {
        _light.intensity = customLight.lightIntensity;
    }
}
