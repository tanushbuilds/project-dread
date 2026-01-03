using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactDistance;
    [SerializeField] private PlayerGrabHandler grabHandler;
    [SerializeField] private Transform playerCamera;

    private void OnEnable()
    {
        InputHandler.OnInteractKeyPressed += HandleInteract;
        InputHandler.OnGrabKeyPressed += HandleGrab;
        InputHandler.OnSecondaryInteractPressed += SecondaryInteract;
    }
    private void OnDisable()
    {
        InputHandler.OnInteractKeyPressed -= HandleInteract;
        InputHandler.OnGrabKeyPressed -= HandleGrab;
        InputHandler.OnSecondaryInteractPressed -= SecondaryInteract;
    }
    private void HandleInteract()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
            }
            else if (hit.collider.TryGetComponent<ITalkable>(out ITalkable talkable))
            {
                talkable.Talk();
            }
            else if(hit.collider.TryGetComponent<ISecondaryInteractable>(out ISecondaryInteractable secondaryInteractable))
            {
                secondaryInteractable.Interact();
            }
        }
    }
    private void SecondaryInteract()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent<ISecondaryInteractable>(out ISecondaryInteractable secondaryInteractable))
            {
                secondaryInteractable.Interact();
            }
        }
    }


    private void HandleGrab()
    {
        if (grabHandler.IsHolding)
        {
            grabHandler.Throw();
            return;
        }

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.TryGetComponent<Grabbable>(out Grabbable grabbable))
            {
                grabHandler.TryGrab(grabbable);
            }
        }
    }
}
