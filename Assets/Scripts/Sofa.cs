using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform sitPosition;
    private bool hasSat;

    public void Interact()
    {
        if (hasSat) {
            GameEvents.OnRequestUnSit?.Invoke();
            hasSat = false;
            return;
        }
        GameEvents.OnRequestSit?.Invoke(sitPosition);
        hasSat = true;
    }
}
