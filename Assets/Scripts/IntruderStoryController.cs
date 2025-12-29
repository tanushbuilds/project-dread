using System.Collections;
using UnityEngine;

public class IntruderStoryController : MonoBehaviour
{
    [SerializeField] private GameObject intruderVisual;
    [SerializeField] private GameObject intruderInHouseVisual;
    [SerializeField] private Door mainDoor;
    private DoorLock mainDoorLock;
    [SerializeField] private EventTrigger eventTrigger;
    [SerializeField] private NPCController intruder;
    [SerializeField] private NPCController intruderInHouse;
    [SerializeField] private Transform target;
    [SerializeField] private float intruderStayDuration;
    [SerializeField] private Transform spawnPointInHouse;
    [SerializeField] private EventTrigger livingRoomTrigger;


    private DoorStoryTrigger doorStoryTrigger;

    private void Awake()
    {
        doorStoryTrigger = mainDoor.GetComponent<DoorStoryTrigger>();
        mainDoorLock = mainDoor.GetComponent<DoorLock>();
    }

    private void Start()
    {
        intruder.SetNPCInactive();
    }

    private void OnEnable()
    {
        GameEvents.OnFoodEaten += intruder.SetNPCActive;
        doorStoryTrigger.OnPlayerOpenedDoor += HandleWalkAway;
        eventTrigger.OnTrigger += HandleIntruderDespawn;
        mainDoorLock.OnDoorLock += SpawnIntruderInHouse;
        livingRoomTrigger.OnTrigger += ShowIntruderInHouse;
    }

    private void OnDisable()
    {
        GameEvents.OnFoodEaten -= intruder.SetNPCActive;
        doorStoryTrigger.OnPlayerOpenedDoor -= HandleWalkAway;
        eventTrigger.OnTrigger -= HandleIntruderDespawn;
        mainDoorLock.OnDoorLock -= SpawnIntruderInHouse;
        livingRoomTrigger.OnTrigger -= ShowIntruderInHouse;
    }

    private IEnumerator WalkAway()
    {
        yield return new WaitForSeconds(intruderStayDuration);

        intruder.Turn(target);
        intruder.WalkForward();
    }

    private void HandleWalkAway()
    {
        StartCoroutine(WalkAway());
    }

    private void HandleIntruderDespawn()
    {
        intruder.DespawnNPC();
    }
    private void SpawnIntruderInHouse()
    {
        intruderInHouse.SetNPCActive();
    }
    private void ShowIntruderInHouse()
    {
        intruderInHouseVisual.GetComponent<Animator>().SetTrigger("CoverToStand");
        intruderInHouse.Turn(Player.Instance.transform);
    }
}
