using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerLookController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController playerController;
    [SerializeField] private float rotateSmoothSpeed;

    public static PlayerLookController Instance;
    
    private float xRotation;
    private bool canLook = true;
    private Transform targetToLookAtX;
    private Transform targetToLookAtY;
    private float CAMERA_OFFSET = 0.2f;


    private void OnEnable()
    {
        GameEvents.OnSit += AlignCameraOnSit;
        GameEvents.OnUnSit += ResetCameraOnUnSit;
        GameEvents.OnRequestPee += DisableLook;
        GameEvents.OnPeeEnd += EnableLook;
        GameEvents.OnRequestLookAt += SetLookTargets;
        GameEvents.OnTalk += _OnTalk;
        GameEvents.OnEndDialogue += EnableLook;
        GameEvents.OnEndDialogue += SetLookTargetsNull;
        GameEvents.OnSleep += DisableLook;

    }
    private void OnDisable()
    {
        GameEvents.OnSit -= AlignCameraOnSit;
        GameEvents.OnUnSit -= ResetCameraOnUnSit;
        GameEvents.OnRequestPee -= DisableLook;
        GameEvents.OnPeeEnd -= EnableLook;
        GameEvents.OnRequestLookAt -= SetLookTargets;
        GameEvents.OnTalk -= _OnTalk;
        GameEvents.OnEndDialogue -= EnableLook;
        GameEvents.OnEndDialogue -= SetLookTargetsNull;
        GameEvents.OnSleep -= DisableLook;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    private void Update()
    {
        if (targetToLookAtX != null)
        {
            RotateTowardsXTarget();
        }
        if (targetToLookAtY != null)
        {
            RotateTowardsYTarget();
        }
    }

    public void HandleLook(Vector2 lookDir)
    {
        if (!canLook) return;
        xRotation -= lookDir.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    private void SetLookTargets(Transform targetX, Transform targetY)
    {
        DisableLook();

        targetToLookAtX = targetX;
        targetToLookAtY = targetY;
    }
    private void SetLookTargetsNull()
    {
        targetToLookAtX = null;
        targetToLookAtY = null;
    }

    private void AlignCameraOnSit()
    {
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            playerController.height - CAMERA_OFFSET,
            transform.localPosition.z
        );
    }
    private void ResetCameraOnUnSit()
    {
        transform.localPosition = Vector2.up * (playerController.height - CAMERA_OFFSET);
    }

    private void RotateTowardsXTarget()
    {
        Vector3 direction = targetToLookAtX.position - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSmoothSpeed * Time.deltaTime);
    }

    private void RotateTowardsYTarget()
    {
        Vector3 direction = targetToLookAtY.position - playerCamera.position;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRotation, rotateSmoothSpeed * Time.deltaTime);
    }

    private void _OnTalk(DialogueNode _)
    {
        DisableLook();
    }

    private void EnableLook()
    {
        canLook = true;
    }
    private void DisableLook()
    {
        canLook = false;
    }
}
