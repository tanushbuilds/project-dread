using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private GameObject crosshairActive;
    [SerializeField] private GameObject crosshairInactive;

    private void OnEnable()
    {
        GameEvents.OnLookAtInteractable += HandleLookAt;
        GameEvents.OnLookAway += HandleLookAway;
    }

    private void OnDisable()
    {
        GameEvents.OnLookAtInteractable -= HandleLookAt;
        GameEvents.OnLookAway -= HandleLookAway;
    }

    private void HandleLookAt()
    {
        crosshairActive.SetActive(true);
        crosshairInactive.SetActive(false);
    }

    private void HandleLookAway()
    {
        crosshairActive.SetActive(false);
        crosshairInactive.SetActive(true);
    }
}
