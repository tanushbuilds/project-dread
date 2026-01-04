using System.Collections;
using UnityEngine;

public class Microwave : MonoBehaviour, IInteractable, IItemReceiver
{
    [SerializeField] private Transform itemSlot;
    [SerializeField] private MicrowaveDoor microwaveDoor;
    [SerializeField] private AudioSource microwaveSound;

    private PlayerGrabHandler grabHandler;
    private Grabbable currentItem;

    private void Start()
    {
        grabHandler = PlayerGrabHandler.Instance;
    }

    public void Interact()
    {
        bool itemCanBeHeated = microwaveDoor != null && currentItem != null && !microwaveDoor.IsOpen() ;

        if (itemCanBeHeated)
        {
            microwaveDoor.Disable();
            HeatItem();
        }
        else if (microwaveDoor != null && microwaveDoor.IsOpen())
        {
            if (!grabHandler.IsHolding) return;

            Grabbable heldItem = grabHandler.GetCurrentGrabbable();
            if (!CanReceive(heldItem)) return;

            grabHandler.Drop();
            ReceiveItem(heldItem);
        }
    }

    private void HeatItem()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Microwaveable microwaveableItem))
        {
            microwaveableItem.OnHeatCompleted += microwaveDoor.Enable;
            microwaveableItem.OnHeat += PlayMicrowaveSound;

            // Start heating the item
            microwaveableItem.HandleHeat();
        }
        else
        {
            Debug.LogError("No microwaveable item found in the microwave.");
        }
    }

    public bool CanReceive(Grabbable item)
    {
        if (item == null)
        {
            Debug.LogWarning("Item is null and cannot be received.");
            return false;
        }

        return itemSlot.childCount == 0 && item.TryGetComponent<Microwaveable>(out _);
    }

    public void ReceiveItem(Grabbable item)
    {
        if (item == null)
        {
            Debug.LogWarning("Received item is null.");
            return;
        }

        currentItem = item;
        item.transform.SetParent(itemSlot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    private void PlayMicrowaveSound()
    {
        if (microwaveSound == null)
        {
            Debug.LogWarning("Microwave sound is not assigned.");
            return;
        }

        microwaveSound.Play();
    }

    private void OnDisable()
    {
        if (currentItem != null && currentItem.TryGetComponent(out Microwaveable microwaveableItem))
        {
            microwaveableItem.OnHeatCompleted -= microwaveDoor.Enable;
            microwaveableItem.OnHeat -= PlayMicrowaveSound;
        }
    }
}
