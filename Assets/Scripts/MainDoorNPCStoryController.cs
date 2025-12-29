using System;
using UnityEngine;

public class MainDoorNPCStoryController : MonoBehaviour
{
    [SerializeField] private GameObject npc;
    [SerializeField] private NPCController mainDoorNPC;
    [SerializeField] private Transform target_0;
    [SerializeField] private Transform target_1;
    [SerializeField] private EventTrigger eventTrigger_01;
    [SerializeField] private EventTrigger eventTrigger_02;
    [SerializeField] private Door mainDoor;
    private TalkablePerson npcTalkable;
    private bool isPeeEnd;


    private Transform currentTarget;
    public event Action OnReachFlat;

    private void OnEnable()
    {
        GameEvents.OnPeeEnd += mainDoorNPC.SetNPCActive;
        GameEvents.OnPeeEnd += SetOnPeeEnd;
        mainDoor.OnDoorOpen += Initiate;
        GameEvents.OnEndDialogue += WalkAway;
        eventTrigger_01.OnTrigger += SetTarget;
        eventTrigger_02.OnTrigger += mainDoorNPC.DespawnNPC;
    }
    private void OnDisable()
    {
        GameEvents.OnPeeEnd -= mainDoorNPC.SetNPCActive;
        GameEvents.OnPeeEnd -= SetOnPeeEnd;
        mainDoor.OnDoorOpen -= Initiate;
        GameEvents.OnEndDialogue -= WalkAway;
        eventTrigger_01.OnTrigger -= SetTarget;
        eventTrigger_02.OnTrigger -= mainDoorNPC.DespawnNPC;
    }
    
    private void Start()
    {
        npcTalkable = npc.GetComponent<TalkablePerson>();
    }

    private void SetTarget()
    {
        OnReachFlat?.Invoke();
        mainDoorNPC.SetTarget(target_1);
    }

    private void WalkAway()
    {
        currentTarget = target_0;

        mainDoorNPC.Turn(currentTarget);
        mainDoorNPC.WalkForward();
    }
    private void Initiate()
    {
        if (!isPeeEnd) return;
        npcTalkable.Initiate();
    }
    private void SetOnPeeEnd()
    {
        isPeeEnd = true;
    }
}
