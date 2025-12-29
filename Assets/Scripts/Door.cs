using System;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _enabled = false;
    [SerializeField] private Animator anim;

    public event Action OnDoorOpen;

    [SerializeField] private AudioSource doorOpen;
    [SerializeField] private AudioSource doorClose;

    public void Interact()
    {
        if (!_enabled) return;
        if (!IsOpen())
        {
            Open();
        }
        else
            Close();
    }

    public void Open()
    {
        OnDoorOpen?.Invoke();

        doorOpen.Play();
        anim.SetBool("IsDoorOpen", true);
    }

    public void Close()
    {
        doorClose.Play();
        anim.SetBool("IsDoorOpen", false);
    }

    public bool IsOpen()
    {
        return anim.GetBool("IsDoorOpen");
    }
    public void Disable()
    {
        _enabled = false;
    }
}
