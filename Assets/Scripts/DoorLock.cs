using System;
using UnityEngine;

public class DoorLock : MonoBehaviour, ISecondaryInteractable
{
    private bool canLock = false;

    public event Action OnDoorLock;
    [SerializeField] private Door door;
    [SerializeField] private Animator doorAnim;
    [SerializeField] private Transform keyPrefabTransform;

    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip lockClip;
    [SerializeField] private AudioClip unlockClip;
    [SerializeField] private AudioClip keyRemovalClip;
    [SerializeField] private bool isLocked;

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

            if (lockClip != null) doorAudioSource.PlayOneShot(lockClip);

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
    public void OnUnlockDoor()
    {
        doorAudioSource.PlayOneShot(unlockClip);
    }
    public void OnKeyRemoval()
    {
        doorAudioSource.PlayOneShot(keyRemovalClip);
    }
}
