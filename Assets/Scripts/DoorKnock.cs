using System.Collections;
using UnityEngine;

public class DoorKnock : MonoBehaviour
{
    [SerializeField] private AudioSource knock;
    [SerializeField] private float knockAfterFoodDelay;
    [SerializeField] private float knockAfterPeeDelay;


    private void OnEnable()
    {
        GameEvents.OnFoodEaten += HandleKnockAfterFood;
        GameEvents.OnPeeEnd += HandleKnockAfterPee;
    }

    private void OnDisable()
    {
        GameEvents.OnFoodEaten -= HandleKnockAfterFood;
        GameEvents.OnPeeEnd -= HandleKnockAfterPee;
    }

    private void HandleKnockAfterFood()
    {
        StartCoroutine(KnockRoutine(knockAfterFoodDelay));
    }

    private void HandleKnockAfterPee()
    {
        StartCoroutine(KnockRoutine(knockAfterPeeDelay));
    }


    private IEnumerator KnockRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Knock();
        GameEvents.RaiseKnock();
    }

    private void Knock()
    {
        knock.Play();
    }
}
