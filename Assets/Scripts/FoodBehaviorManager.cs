using UnityEngine;

public class FoodBehaviourManager : MonoBehaviour, IInteractable
{
    [SerializeField] private Grabbable foodItem;
    private Food foodComponent;

    private CapsuleCollider foodCollider;
    private Microwaveable microwaveableFood;

    [SerializeField] private FoodState foodState = FoodState.Unheated;

    private bool isSitting;
    private bool isFoodGrabbed;
    private bool hasEatenFood;

    private void OnEnable()
    {
        foodItem.OnGrab += HandleFoodGrab;
        foodItem.OnDrop += HandleFoodDrop;
        
        microwaveableFood.OnHeat += HandleFoodHeating;
        microwaveableFood.OnHeatCompleted += HandleFoodHeating;
        microwaveableFood.OnHeatCompleted += () => foodState = FoodState.Heated;

        GameEvents.OnSit += HandleSitDown;
        GameEvents.OnStandUp += HandleStandUp;
    }

    private void OnDisable()
    {
        foodItem.OnGrab -= HandleFoodGrab;
        foodItem.OnDrop -= HandleFoodDrop;

        microwaveableFood.OnHeat -= HandleFoodHeating;
        microwaveableFood.OnHeatCompleted -= HandleFoodHeating;

        GameEvents.OnSit -= HandleSitDown;
        GameEvents.OnStandUp -= HandleStandUp;
    }

    private void Awake()
    {
        foodCollider = foodItem.GetComponent<CapsuleCollider>();
        foodComponent = foodItem.GetComponent<Food>();
        microwaveableFood = foodItem.GetComponent<Microwaveable>();
    }

    public void Interact()
    {
        if (foodComponent != null && foodState == FoodState.Heated && isSitting && isFoodGrabbed && !hasEatenFood)
        {
            foodComponent.Eat();
            hasEatenFood = true;
        }
    }

    private void HandleFoodHeating()
    {
        UpdateFoodInteractionState();
    }

    private void HandleFoodGrab()
    {
        isFoodGrabbed = true;
        UpdateFoodInteractionState();
    }

    private void HandleFoodDrop()
    {
        isFoodGrabbed = false;
        UpdateFoodInteractionState();
    }

    private void HandleSitDown()
    {
        isSitting = true;
        UpdateFoodInteractionState();
    }

    private void HandleStandUp()
    {
        isSitting = false;
        UpdateFoodInteractionState();
    }

    private void UpdateFoodInteractionState()
    {
        bool canInteractWithFood = (microwaveableFood != null && !isFoodGrabbed && !microwaveableFood.GetIsHeating())
                                   || (isFoodGrabbed && foodState == FoodState.Heated && isSitting);

        if (canInteractWithFood)
        {
            if (isSitting && foodState == FoodState.Heated)
            {
                PlayerGrabHandler.Instance.AttachToInteractableSlot();
            }
            EnableCollider();
        }
        else
        {
            DisableCollider();
        }
    }


    private void EnableCollider()
    {
        if (foodCollider == null) return;

        foodCollider.enabled = true;
    }

    private void DisableCollider()
    {
        if (foodCollider == null) return;

        foodCollider.enabled = false;
    }
}
