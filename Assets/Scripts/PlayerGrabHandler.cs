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


    private void OnEnable()
    {
        GameEvents.OnSit += OnSit;
        GameEvents.OnUnSit += ReevaluateVectorsOnUnSit;
    }

    private void OnDisable()
    {
        GameEvents.OnSit -= OnSit;
        GameEvents.OnUnSit -= ReevaluateVectorsOnUnSit;
    }

    private void Awake()
    {
        Instance = this;
        playerController = GetComponent<CharacterController>();
        playerHeight = playerController.height;
    }

    public void TryGrab(Grabbable grabbable)
    {
        if (isHolding || grabbable == null)
            return;

        currentGrabbable = grabbable;
        isHolding = true;

        AttachToSlot(grabSlot, grabbable.CameraOffset);
        SetLayerRecursively(currentGrabbable.gameObject, LayerMask.NameToLayer("Grabbed"));

        currentGrabbable.OnGrabbed();
    }

    public void Throw()
    {
        if (!isHolding || !currentGrabbable.CanBeThrown)
            return;

        currentGrabbable.OnThrown();

        Rigidbody rb = currentGrabbable.GetOrAddRigidbody();
        rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        Drop();
    }

    public void Drop()
    {
        if (!isHolding)
            return;

        Detach();
        ClearState();
    }

    private void AttachToSlot(Transform slot, Vector3 offset)
    {
        currentGrabbable.transform.SetParent(slot);
        currentGrabbable.transform.localPosition = offset;
        currentGrabbable.transform.localRotation = Quaternion.identity;
    }

    public void AttachToInteractableSlot()
    {
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

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }

    private void OnSit()
    {
        ReevaluateVectorsOnSit(playerHeight);
    }

    private void ReevaluateVectorsOnSit(float playerHeight)
    {

        grabSlotLastPosition = grabSlot.localPosition;
        interactableGrabSlotLastPosition = interactableGrabSlot.localPosition;

        float grabSlotYOffset =
    playerHeight * grabSlotSitHeightMultiplier;

        float interactableSlotYOffset =
            playerHeight * interactableGrabSlotSitHeightMultiplier;

        grabSlot.localPosition = new Vector3(
            grabSlot.localPosition.x,
            grabSlotYOffset,
            grabSlot.localPosition.z
        );

        interactableGrabSlot.localPosition = new Vector3(
            interactableGrabSlot.localPosition.x,
            interactableSlotYOffset,
            interactableGrabSlot.localPosition.z
        );
    }
    private void ReevaluateVectorsOnUnSit()
    {

        grabSlot.localPosition = grabSlotLastPosition;
        interactableGrabSlot.localPosition = interactableGrabSlotLastPosition;
    }
}
