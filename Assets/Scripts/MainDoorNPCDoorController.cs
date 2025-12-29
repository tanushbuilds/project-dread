using UnityEngine;

public class MainDoorNPCDoorController : MonoBehaviour
{
    [SerializeField] private MainDoorNPCStoryController mainDoorNPC;

    private NPCController mainDoorNPCController;
    private Door door;

    private void OnEnable()
    {
        mainDoorNPC.OnReachFlat += door.Open;
        mainDoorNPCController.OnDespawn += door.Close;
    }
    private void OnDisable()
    {
        mainDoorNPC.OnReachFlat -= door.Open;
        mainDoorNPCController.OnDespawn -= door.Close;
    }
    private void Awake()
    {
        door = GetComponent<Door>();
        mainDoorNPCController = mainDoorNPC.GetComponent<NPCController>();
    }
}
