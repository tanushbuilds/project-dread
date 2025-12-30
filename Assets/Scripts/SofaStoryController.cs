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
        SetCanUnsit(false);
    }

    private void OnFoodEaten()
    {
        SetCanUnsit(true);
    }

    private void SetCanUnsit(bool canUnSit)
    {
        sofa.SetCanUnsit(canUnSit);
    }
}
