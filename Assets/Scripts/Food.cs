using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float eatDuration;
    [SerializeField] private string actionName;

    public void Eat()
    {
        StartCoroutine(EatFoodRoutine());
    }
    private IEnumerator EatFoodRoutine()
    {
        GameEvents.OnRequestProgressBar?.Invoke(eatDuration, actionName);
        GameEvents.RaiseFoodEatStart();
        yield return new WaitForSeconds(eatDuration);

        GameEvents.RaiseFoodEaten();
        Debug.Log("Ate Food");
    }
}
