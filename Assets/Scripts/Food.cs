using System.Collections;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float eatDuration;

    public void Eat()
    {
        StartCoroutine(EatFoodRoutine());
    }
    private IEnumerator EatFoodRoutine()
    {
        yield return new WaitForSeconds(eatDuration);

        GameEvents.RaiseFoodEaten();
        Debug.Log("Ate Food");
    }
}
