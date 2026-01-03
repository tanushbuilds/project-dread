using UnityEngine;

public class MicrowaveDoor : MonoBehaviour, IInteractable {

    [SerializeField] private Animator anim;
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
        anim.SetBool("IsOpen", true);
    }
    private void Close()
    {
        anim.SetBool("IsOpen", false);
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
