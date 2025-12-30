using System;
using UnityEngine;

public class DoorLock : MonoBehaviour, ISecondaryInteractable
{
    private bool canLock;
    public event Action OnDoorLock;
    [SerializeField] private Door door;
    [SerializeField] private AudioSource lockSound;


    public void Interact()
    {
        if (canLock && !door.IsOpen()) {
            door.Disable();
            canLock = false;
            OnDoorLock?.Invoke();
            if (lockSound != null) lockSound.Play();
        }
    }
    public void SetCanLock(bool canLock)
    {
        this.canLock = canLock;
    }
}
