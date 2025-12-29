using System.Collections;
using UnityEngine;

public class LightStoryController : MonoBehaviour
{
    [SerializeField] private EventTrigger eventTrigger;

    private void OnEnable()
    {
        eventTrigger.OnTrigger += OnEventTriggered;
    }

    private void OnDisable()
    {
        eventTrigger.OnTrigger -= OnEventTriggered;
    }

    private IEnumerator ShowDelayedMessage()
    {
        yield return new WaitForSeconds(10f);

        GetComponent<LightController>().SwitchLightOff();

    }

    private void OnEventTriggered()
    {
        StartCoroutine(ShowDelayedMessage());
    }

}
