using UnityEngine;

public class DoorStateChecker : MonoBehaviour
{
    [SerializeField] private EventTrigger eventTrigger;
    [SerializeField] private Door mainDoor;

    private void OnEnable()
    {
        eventTrigger.OnTrigger += CheckDoor;
    }

    private void OnDisable()
    {
        eventTrigger.OnTrigger -= CheckDoor;
    }
    private void CheckDoor()
    {
        if (mainDoor.IsOpen())
        {
            mainDoor.Close();
        }
    }
}
