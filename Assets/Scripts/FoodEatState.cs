using UnityEngine;

public class FoodEatState : MonoBehaviour, IInteractable
{
    [SerializeField] private Grabbable food;
    private Food _food;

    private CapsuleCollider foodCollider;

    private bool hasSat;
    private bool hasGrabbed;
    private bool hasEaten;

    private void OnEnable()
    {
        food.OnGrab += HandleGrab;
        food.OnThrow += HandleThrow;

        GameEvents.OnSit += HandleSit;
        GameEvents.OnUnSit += HandleUnSit;
    }

    private void OnDisable()
    {
        food.OnGrab -= HandleGrab;
        food.OnThrow -= HandleThrow;

        GameEvents.OnSit -= HandleSit;
        GameEvents.OnUnSit -= HandleUnSit;
    }

    private void Awake()
    {
        foodCollider = food.GetComponent<CapsuleCollider>();
        _food = food.GetComponent<Food>();
    }

    public void Interact()
    {
        if(hasSat && hasGrabbed && !hasEaten)
        {
            _food.Eat();
            hasEaten = true;
        }
    }

    private void HandleGrab()
    {
        hasGrabbed = true;
        EvaluateState();
    }

    private void HandleThrow()
    {
        hasGrabbed = false;
        EvaluateState();
    }

    private void HandleSit()
    {
        hasSat = true;
        EvaluateState();
    }

    private void HandleUnSit()
    {
        hasSat = false;
        EvaluateState();
    }

    private void EvaluateState()
    {
        if(!hasGrabbed || hasSat)
        {
            if(hasSat && hasGrabbed)
            {
                PlayerGrabHandler.Instance.AttachToInteractableSlot();
            }
            foodCollider.enabled = true;
            return;
        }
        foodCollider.enabled = false;
        if (hasSat)
        {
            return;
        }
        PlayerGrabHandler.Instance.AttachToDefaultSlot();

    }
}
