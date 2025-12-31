using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform sitPosition;
    private bool hasSat;
    private bool canUnsit = true;

    public void Interact()
    {
        if (hasSat && canUnsit)
        {
            GameEvents.OnRequestUnSit?.Invoke();
            GameEvents.OnRequestEnableHeadBob?.Invoke();

            hasSat = false;
            return;
        }
        else if(!hasSat)
        {
            GameEvents.OnRequestSit?.Invoke(sitPosition);
            GameEvents.OnRequestDisableHeadBob?.Invoke();
            GameEvents.OnRequestDisableMovement?.Invoke();

            hasSat = true;
        }
    }
    public void SetCanUnsit(bool canUnsit)
    {
        this.canUnsit = canUnsit;
    }
}
