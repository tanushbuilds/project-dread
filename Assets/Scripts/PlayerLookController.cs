using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
    public static PlayerLookController Instance;

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private RotationSettings rotationSettings;

    private float xRotation;
    private bool canLook = true;

    private Transform lookTarget;
    private const float CAMERA_OFFSET = 0.2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnRequestDisableLook += DisableLook;
        GameEvents.OnRequestEnableLook += EnableLook;

        GameEvents.OnRequestCameraLookAt += SetLookTarget;
        GameEvents.OnRequestStopCameraLookAt += ClearLookTarget;

        GameEvents.OnSit += AlignCameraOnSit;
        GameEvents.OnStandUp += ResetCameraOnStandUp;
    }

    private void OnDisable()
    {
        GameEvents.OnRequestDisableLook -= DisableLook;
        GameEvents.OnRequestEnableLook -= EnableLook;

        GameEvents.OnRequestCameraLookAt -= SetLookTarget;
        GameEvents.OnRequestStopCameraLookAt -= ClearLookTarget;

        GameEvents.OnSit -= AlignCameraOnSit;
        GameEvents.OnStandUp -= ResetCameraOnStandUp;
    }

    private void Update()
    {
        if (lookTarget != null)
            RotateTowardsTarget();
    }

    public void HandleLook(Vector2 lookInput)
    {
        if (!canLook || lookTarget != null) return;

        xRotation -= lookInput.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void SetLookTarget(Transform target)
    {
        lookTarget = target;
    }

    private void ClearLookTarget()
    {
        lookTarget = null;
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = lookTarget.position - playerCamera.position;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        playerCamera.rotation = Quaternion.Slerp(
            playerCamera.rotation,
            targetRotation,
            rotationSettings.rotateSmoothSpeed * Time.deltaTime
        );
    }

    private void EnableLook() => canLook = true;
    private void DisableLook() => canLook = false;

    private void AlignCameraOnSit()
    {
        xRotation = 0f;

        transform.localPosition = new Vector3(
            transform.localPosition.x,
            playerController.height - CAMERA_OFFSET,
            transform.localPosition.z
        );
    }

    private void ResetCameraOnStandUp()
    {
        transform.localPosition = Vector3.up * (playerController.height - CAMERA_OFFSET);
    }
}
