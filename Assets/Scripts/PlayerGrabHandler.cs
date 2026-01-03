using UnityEngine;

public class PlayerGrabHandler : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private Transform grabSlot;
    [SerializeField] private Transform interactableGrabSlot;

    public static PlayerGrabHandler Instance;

    private Vector3 grabSlotLastPosition;
    private Vector3 interactableGrabSlotLastPosition;

    private CharacterController playerController;
    private float playerHeight;

    [Header("Throw Settings")]
    [SerializeField] private float throwForce;
    [SerializeField] private float grabSlotSitHeightMultiplier = -0.25f;
    [SerializeField] private float interactableGrabSlotSitHeightMultiplier = 0.25f;

    private Grabbable currentGrabbable;

    private bool isHolding;
    public bool IsHolding => isHolding;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerController = GetComponent<CharacterController>();
        playerHeight = playerController.height;
    }

    private void OnEnable()
    {
        GameEvents.OnSit += OnSit;
        GameEvents.OnStandUp += OnStandUp;
        GameEvents.OnPlace += Drop;
    }

    private void OnDisable()
    {
        GameEvents.OnSit -= OnSit;
        GameEvents.OnStandUp -= OnStandUp;
        GameEvents.OnPlace -= Drop;
    }

    // =========================
    // GRAB LOGIC
    // =========================

    public void TryGrab(Grabbable grabbable)
    {
        if (isHolding || grabbable == null)
            return;

        currentGrabbable = grabbable;
        isHolding = true;

        AttachToSlot(grabSlot, grabbable.CameraOffset);
        SetLayerRecursively(grabbable.gameObject, LayerMask.NameToLayer("Grabbed"));

        grabbable.OnGrabbed();
    }

    public void Drop()
    {
        if (!isHolding)
            return;

        currentGrabbable.OnDropped();
        Detach();
        ClearState();

    }

    public void Throw()
    {
        if (!isHolding || !currentGrabbable.CanBeThrown)
            return;


        Rigidbody rb = currentGrabbable.GetOrAddRigidbody();
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        Drop();
    }

    // =========================
    // SLOT ATTACHMENT
    // =========================

    private void AttachToSlot(Transform slot, Vector3 offset)
    {
        currentGrabbable.transform.SetParent(slot);
        currentGrabbable.transform.localPosition = offset;
        currentGrabbable.transform.localRotation = Quaternion.identity;
    }

    public void AttachToInteractableSlot()
    {
        if (!isHolding) return;
        AttachToSlot(interactableGrabSlot, currentGrabbable.CameraOffset);
    }

    public void AttachToDefaultSlot()
    {
        if (!isHolding) return;
        AttachToSlot(grabSlot, currentGrabbable.CameraOffset);
    }

    private void Detach()
    {
        currentGrabbable.transform.SetParent(null);
        SetLayerRecursively(currentGrabbable.gameObject, LayerMask.NameToLayer("Default"));
    }

    private void ClearState()
    {
        currentGrabbable = null;
        isHolding = false;
    }

    // =========================
    // SITTING ADJUSTMENTS
    // =========================

    private void OnSit()
    {
        grabSlotLastPosition = grabSlot.localPosition;
        interactableGrabSlotLastPosition = interactableGrabSlot.localPosition;

        grabSlot.localPosition = new Vector3(
            grabSlot.localPosition.x,
            playerHeight * grabSlotSitHeightMultiplier,
            grabSlot.localPosition.z
        );

        interactableGrabSlot.localPosition = new Vector3(
            interactableGrabSlot.localPosition.x,
            playerHeight * interactableGrabSlotSitHeightMultiplier,
            interactableGrabSlot.localPosition.z
        );
    }

    private void OnStandUp()
    {
        grabSlot.localPosition = grabSlotLastPosition;
        interactableGrabSlot.localPosition = interactableGrabSlotLastPosition;
    }

    // =========================
    // UTILITIES
    // =========================

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public Grabbable GetCurrentGrabbable()
    {
        return currentGrabbable;
    }
}
