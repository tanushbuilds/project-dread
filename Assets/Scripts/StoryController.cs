using System;
using UnityEngine;

public class StoryController : MonoBehaviour
{
    [SerializeField] private NPCController mainDoorNPC;
    [SerializeField] private DoorLock mainDoor;

    private void OnEnable()
    {
        mainDoorNPC.OnDespawn += OnDespawn;
    }

    private void OnDisable()
    {
        mainDoorNPC.OnDespawn -= OnDespawn;
    }

    private void OnDespawn()
    {
        UIMessageEvents.OnMessageChanged?.Invoke("I made sure to lock the door");
        mainDoor.SetCanLock(true);
    }
}
