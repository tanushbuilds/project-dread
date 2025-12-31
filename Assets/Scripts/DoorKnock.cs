using System.Collections;
using UnityEngine;

public class DoorKnock : MonoBehaviour
{
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip knockClip;

    public IEnumerator KnockRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Knock();
        GameEvents.RaiseKnock();
    }

    private void Knock()
    {
        doorAudioSource.PlayOneShot(knockClip);
    }
}
