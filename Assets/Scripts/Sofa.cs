using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform sitPosition;
    private bool hasSat;
    private bool canStandUp = true;

    public void Interact()
    {
        if (hasSat && canStandUp)
        {
            GameEvents.OnRequestStandUp?.Invoke();
            hasSat = false;
            return;
        }
        else if(!hasSat)
        {
            GameEvents.OnRequestSit?.Invoke(sitPosition);
            hasSat = true;
        }
    }
    public void SetCanStandUp(bool canStandUp)
    {
        this.canStandUp = canStandUp;
    }
}
