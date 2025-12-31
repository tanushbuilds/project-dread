using System;
using UnityEngine;

public class DoorLock : MonoBehaviour, ISecondaryInteractable
{
    private bool canLock = false;
    private bool isLocked = true;

    public event Action OnDoorLock;
    [SerializeField] private Door door;
    [SerializeField] private AudioSource lockSound;
    [SerializeField] private Animator doorAnim;
    [SerializeField] private Transform keyPrefabTransform;


    private void Start()
    {
        if (isLocked) {
            door.Disable();
        }
        else
        {
            door.Enable();
        }
    }

    public void Interact()
    {
        if (canLock && !isLocked && !door.IsOpen()) {

            door.Disable();

            canLock = false;
            isLocked = true;

            OnDoorLock?.Invoke();

            if (lockSound != null) lockSound.Play();

        }
        else if(isLocked)
        {
            GameEvents.OnRequestCameraLookAt?.Invoke(keyPrefabTransform);
            GameEvents.OnRequestBodyLookAt?.Invoke(keyPrefabTransform);
            GameEvents.OnRequestDisableMovement?.Invoke();
            GameEvents.OnRequestDisableHeadBob?.Invoke();


            doorAnim.SetTrigger("Unlock");
            door.Enable();
            isLocked = false;
        }
    }
    public void SetCanLock(bool canLock)
    {
        this.canLock = canLock;
    }
    public void OnUnlockAnimationFinished()
    {
        GameEvents.OnRequestStopCameraLookAt?.Invoke();
        GameEvents.OnRequestStopBodyLookAt?.Invoke();
        
        GameEvents.OnRequestEnableMovement?.Invoke();
        GameEvents.OnRequestEnableHeadBob?.Invoke();
    }
}
