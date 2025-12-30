using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float eatDuration;
    [SerializeField] private string actionName;
    [SerializeField] private AudioSource eatSound;

    [SerializeField] private List<GameObject> friesSets;


    public void Eat()
    {
        StartCoroutine(EatFoodRoutine());
    }
    private IEnumerator EatFoodRoutine()
    {
        GameEvents.OnRequestProgressBar?.Invoke(eatDuration, actionName);
        GameEvents.RaiseFoodEatStart();

        if (eatSound != null)
            eatSound.Play();

        float stepTime = eatDuration / friesSets.Count;

        for (int i = friesSets.Count - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(stepTime);
            friesSets[i].SetActive(false);
        }

        GameEvents.RaiseFoodEaten();
    }
}
