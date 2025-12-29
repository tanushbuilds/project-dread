using System;
using UnityEngine;

public class DoorStoryTrigger : MonoBehaviour
{
    [SerializeField] private Door mainDoor;

    private DoorLock mainDoorLock;
    private bool hasPreviouslyOpened = false;
    public event Action OnPlayerOpenedDoor;
    
    [SerializeField] private EventTrigger eventTrigger;


    private bool hasEaten;

    private void OnEnable()
    {
        if (mainDoorLock == null)
            mainDoorLock = mainDoor.GetComponent<DoorLock>();
        mainDoor.OnDoorOpen += OnDoorOpened;
        GameEvents.OnFoodEaten += HasEaten;
        mainDoorLock.OnDoorLock += OnLock;
    }

    private void OnDisable()
    {
        mainDoor.OnDoorOpen -= OnDoorOpened;
        GameEvents.OnFoodEaten -= HasEaten;
        mainDoorLock.OnDoorLock -= OnLock;
    }

    private void HasEaten()
    {
        hasEaten = true;
    }
    private void OnDoorOpened()
    {
        if (hasEaten && !hasPreviouslyOpened)
        {
            OnPlayerOpenedDoor?.Invoke();
            hasPreviouslyOpened = false;
        }
    }
    private void OnLock()
    {
        eventTrigger.gameObject.SetActive(true);
    }
}
