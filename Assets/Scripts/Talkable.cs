using UnityEngine;

public class Talkable : MonoBehaviour, ITalkable
{
    [SerializeField] private DialogueNode startNode;
    [SerializeField] private EventTrigger eventTrigger;

    private bool hasTalked;


    private void OnEnable()
    {
        eventTrigger.OnTrigger += Talk;
    }

    private void OnDisable()
    {
        eventTrigger.OnTrigger -= Talk;
    }

    public void Talk()
    {
        if (!hasTalked)
        {
            hasTalked = true;

            GameEvents.OnTalk?.Invoke(startNode);
        }
    }

}
