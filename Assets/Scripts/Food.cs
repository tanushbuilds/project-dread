using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float eatDuration;
    [SerializeField] private string actionName;
    [SerializeField] private AudioSource eatSound;
    [SerializeField] private GameObject fries;

    public void Eat()
    {
        StartCoroutine(EatFoodRoutine());
    }
    private IEnumerator EatFoodRoutine()
    {
        GameEvents.OnRequestProgressBar?.Invoke(eatDuration, actionName);
        GameEvents.RaiseFoodEatStart();

        eatSound.Play();

        yield return new WaitForSeconds(eatDuration);

        fries.SetActive(false);
        GameEvents.RaiseFoodEaten();
    }
}
