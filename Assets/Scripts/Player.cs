using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private CharacterController playerController;
    private Vector3 velocity;

    private bool canLook = true;
    private bool canBodyLook = true;
    private bool canMove = true;

    private float playerHeight;
    private Vector3 lastPosition;

    private Transform lookTarget;
    private Transform currentSitPosition;

    [SerializeField] private float peeDuration;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float fastWalkSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float mouseSensitivity;

    [SerializeField] private AudioSource footstep;
    private float stepInterval;
    [SerializeField] private float walkStepInterval;
    [SerializeField] private float fastWalkStepInterval;

    [SerializeField] private Vector2 footstepSoundPitchRange;

    [SerializeField] private float playerHeightWhileSitting;
    [SerializeField] private RotationSettings rotationSettings;

    private float stepTimer;
    private bool isGrounded;


    private void OnEnable()
    {
        GameEvents.OnRequestDisableMovement += DisableMovement;
        GameEvents.OnRequestEnableMovement += EnableMovement;

        GameEvents.OnRequestDisableBodyLook += DisableBodyLook;
        GameEvents.OnRequestEnableBodyLook += EnableBodyLook;

        GameEvents.OnRequestBodyLookAt += SetLookTarget;
        GameEvents.OnRequestStopBodyLookAt += ClearLookTarget;

        GameEvents.OnRequestSit += SitWithFade;
        GameEvents.OnRequestStandUp += StandUpWithFade;
        GameEvents.OnRequestPee += Pee;
    }

    private void OnDisable()
    {
        GameEvents.OnRequestDisableMovement -= DisableMovement;
        GameEvents.OnRequestEnableMovement -= EnableMovement;

        GameEvents.OnRequestDisableBodyLook -= DisableBodyLook;
        GameEvents.OnRequestEnableBodyLook -= EnableBodyLook;

        GameEvents.OnRequestBodyLookAt -= SetLookTarget;
        GameEvents.OnRequestStopBodyLookAt -= ClearLookTarget;

        GameEvents.OnRequestSit -= SitWithFade;
        GameEvents.OnRequestStandUp -= StandUpWithFade;
        GameEvents.OnRequestPee -= Pee;
    }


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Instance == null)
            Instance = this;

        playerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();

        if (lookTarget != null)
            RotateTowardsTarget();
    }


    public void HandleMovement(Vector3 moveDir, bool isFastWalk)
    {
        if (!canMove) return;

        float speed = isFastWalk ? fastWalkSpeed : walkSpeed;

        Vector3 move = transform.forward * moveDir.z + transform.right * moveDir.x;
        move *= speed;
        move.y = velocity.y;

        playerController.Move(move * Time.deltaTime);

        if (moveDir.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                footstep.pitch = Random.Range(
                    footstepSoundPitchRange.x,
                    footstepSoundPitchRange.y
                );
                footstep.Play();
                stepTimer = isFastWalk ? fastWalkStepInterval : walkStepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    public void HandleLook(Vector2 lookDir)
    {
        if (!canLook || !canBodyLook || lookTarget != null) return;

        transform.Rotate(
            Vector3.up * lookDir.x * mouseSensitivity * Time.deltaTime
        );
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
        Vector3 direction = lookTarget.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSettings.rotateSmoothSpeed * Time.deltaTime
        );
    }


    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundCheckDistance,
            groundMask
        );

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
    }


    private void SitWithFade(Transform sitPosition)
    {
        currentSitPosition = sitPosition;
        StartCoroutine(SitRoutine());
    }

    private IEnumerator SitRoutine()
    {
        DisableMovement();
        GameEvents.OnRequestDisableHeadBob();

        yield return ScreenFader.Instance.FadeIn();

        playerHeight = playerController.height;
        lastPosition = transform.position;

        transform.position = currentSitPosition.position;
        playerController.height = playerHeightWhileSitting;

        transform.rotation = Quaternion.identity;
        GameEvents.RaiseOnSit();

        yield return ScreenFader.Instance.FadeOut();
    }

    private void StandUpWithFade()
    {
        StartCoroutine(StandUpRoutine());
    }

    private IEnumerator StandUpRoutine()
    {
        yield return ScreenFader.Instance.FadeIn();

        playerController.height = playerHeight;
        transform.position = lastPosition;

        EnableMovement();
        GameEvents.RaiseStandUp();
        GameEvents.OnRequestEnableHeadBob();

        yield return ScreenFader.Instance.FadeOut();
    }


    private void Pee()
    {
        StartCoroutine(PeeRoutine());
    }

    private IEnumerator PeeRoutine()
    {
        DisableMovement();
        
        GameEvents.OnRequestDisableHeadBob?.Invoke();
        GameEvents.OnRequestDisableLook?.Invoke();

        yield return ScreenFader.Instance.FadeIn();

        GameEvents.RaisePeeStart();
        GameEvents.OnRequestProgressBar?.Invoke(peeDuration, "Peeing");

        yield return new WaitForSeconds(peeDuration);

        yield return ScreenFader.Instance.FadeOut();

        EnableMovement();
        
        GameEvents.RaisePeeEnd();

        GameEvents.OnRequestEnableHeadBob();
        GameEvents.OnRequestEnableLook();

    }


    private void EnableMovement() => canMove = true;
    private void DisableMovement() => canMove = false;

    private void EnableBodyLook() => canBodyLook = true;
    private void DisableBodyLook() => canBodyLook = false;
}
