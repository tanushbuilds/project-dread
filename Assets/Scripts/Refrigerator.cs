using UnityEngine;

public class Refrigerator : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator anim;

    private bool isOpen;

    public void Interact()
    {
        if (GetIsOpen())
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        if (anim != null)
        {
            anim.SetBool("IsOpen", true);
            isOpen = true;
        }
    }
    private void Close()
    {
        if (anim != null)
        {
            anim.SetBool("IsOpen", false);
            isOpen = false;
        }
    }

    private bool GetIsOpen()
    {
        return isOpen;
    }
}
