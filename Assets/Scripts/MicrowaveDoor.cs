using UnityEngine;

public class MicrowaveDoor : MonoBehaviour, IInteractable {

    [SerializeField] private Animator anim;
    
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip doorClose;


    private bool isOpen = false;
    private bool _enabled = true;


    public void Interact()
    {
        if (!_enabled) return;
        if (IsOpen())
        {
            Close();
            isOpen = false;
        }
        else
        {
            Open();
            isOpen = true;
        }
    }

    private void Open()
    {
        if (anim == null || source == null || doorOpen == null)
            return;

        anim.SetBool("IsOpen", true);
        source.PlayOneShot(doorOpen);
    }
    private void Close()
    {
        if (anim == null || source == null || doorClose == null)
            return;

        anim.SetBool("IsOpen", false);
        source.PlayOneShot(doorClose);
    }
    public bool IsOpen()
    {
        return isOpen;
    }

    public void Enable()
    {
        _enabled = true;
    }

    public void Disable()
    {
        _enabled = false;
    }
}
