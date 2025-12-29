using UnityEngine;

public class LookDetector : MonoBehaviour
{
    [SerializeField] private float rayDistance;

    private bool isLookingAtInteractable;

    private void Update()
    {
        bool hitInteractable = DetectInteractable();

        if (hitInteractable && !isLookingAtInteractable)
        {
            isLookingAtInteractable = true;
            GameEvents.RaiseLookAtInteractable();
        }
        else if (!hitInteractable && isLookingAtInteractable)
        {
            isLookingAtInteractable = false;
            GameEvents.RaiseLookAway();
        }
    }

    private bool DetectInteractable()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance))
        {
            return hit.collider.GetComponent<IInteractable>() != null
                || hit.collider.GetComponent<Grabbable>() != null
                || hit.collider.GetComponent<ITalkable>() != null;
        }
        return false;
    }
}
