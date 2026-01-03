using UnityEngine;

public class SofaStoryController : MonoBehaviour
{
    [SerializeField] private Sofa sofa;

    private void OnEnable()
    {
        GameEvents.OnFoodEatStart += OnFoodEatStart;
        GameEvents.OnFoodEaten += OnFoodEaten;
    }

    private void OnDisable()
    {
        GameEvents.OnFoodEatStart -= OnFoodEatStart;
        GameEvents.OnFoodEaten -= OnFoodEaten;
    }

    private void OnFoodEatStart()
    {
        SetCanStandUp(false);
    }

    private void OnFoodEaten()
    {
        SetCanStandUp(true);
    }

    private void SetCanStandUp(bool canStandUp)
    {
        sofa.SetCanStandUp(canStandUp);
    }
}
