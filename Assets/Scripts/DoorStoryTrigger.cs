using System;
using UnityEngine;

public class DoorStoryTrigger : MonoBehaviour
{
    [SerializeField] private Door mainDoor;
    private DoorKnock doorKnockController;

    private DoorLock mainDoorLock;
    private bool hasPreviouslyOpened = false;
    public event Action OnPlayerOpenedDoor;
    
    [SerializeField] private EventTrigger eventTrigger;
    [SerializeField] private float knockAfterFoodDelay;
    [SerializeField] private float knockAfterPeeDelay;

    private bool hasEaten;

    private void OnEnable()
    {
        if (mainDoorLock == null)
            mainDoorLock = mainDoor.GetComponent<DoorLock>();

        if (doorKnockController == null)
            doorKnockController = mainDoor.GetComponent<DoorKnock>();

        mainDoor.OnDoorOpen += OnDoorOpened;
        mainDoorLock.OnDoorLock += OnLock;

        GameEvents.OnFoodEaten += OnFoodEaten;
        GameEvents.OnPeeEnd += OnPeeEnd;
    }

    private void OnDisable()
    {
        mainDoor.OnDoorOpen -= OnDoorOpened;
        mainDoorLock.OnDoorLock -= OnLock;

        GameEvents.OnFoodEaten += OnFoodEaten;
        GameEvents.OnPeeEnd += OnPeeEnd;
    }

    private void OnFoodEaten()
    {
        hasEaten = true;
        StartCoroutine(doorKnockController.KnockRoutine(knockAfterFoodDelay));
    }

    private void OnPeeEnd()
    {
        StartCoroutine(doorKnockController.KnockRoutine(knockAfterPeeDelay));
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
