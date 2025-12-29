using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private GameObject lightSet;
    [SerializeField] private GameObject switchOn;
    [SerializeField] private GameObject switchOff;
    [SerializeField] private AudioSource switchAudioSource;

    private Light[] allLights;

    private bool isOn;

    private void Start()
    {
        UpdateSwitchVisuals();
    }

    public void Interact()
    {
        if (switchAudioSource) switchAudioSource.Play();

        isOn = !isOn;
        UpdateSwitchVisuals();

        for (int i = 0; i < allLights.Length; i++)
        {
            if (allLights[i].TryGetComponent(out CustomLight custom))
                allLights[i].intensity = isOn ? custom.lightIntensity : 0f;
            else
                allLights[i].intensity = isOn ? 1f : 0f;
        }
    }

    private void UpdateSwitchVisuals()
    {
        if (switchOn) switchOn.SetActive(isOn);
        if (switchOff) switchOff.SetActive(!isOn);
    }
}
