using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float fastWalkSpeed;
    [SerializeField] private float gravity;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private RotationSettings rotationSettings;

    [Header("Sit / Pee")]
    [SerializeField] private float peeDuration;
    [SerializeField] private float playerHeightWhileSitting;

    private CharacterController controller;
    private Vector3 velocity;

    private bool canMove = true;
    private bool canBodyLook = true;

    private Transform lookTarget;
    private float originalHeight;
    private Vector3 lastPosition;
    private Transform currentSitPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        GameEvents.OnRequestDisableMovement += DisableMovement;
        GameEvents.OnRequestEnableMovement += EnableMovement;

        GameEvents.OnRequestDisableBodyLook += DisableBodyLook;
        GameEvents.OnRequestEnableBodyLook += EnableBodyLook;

        GameEvents.OnRequestBodyLookAt += SetLookTarget;
        GameEvents.OnRequestStopBodyLookAt += ClearLookTarget;

        GameEvents.OnRequestSit += SitWithFade;
        GameEvents.OnRequestUnSit += UnSitWithFade;
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
        GameEvents.OnRequestUnSit -= UnSitWithFade;
        GameEvents.OnRequestPee -= Pee;
    }

    private void Update()
    {
        ApplyGravity();

        if (lookTarget != null)
            RotateTowardsTarget();
    }

    public void HandleMovement(Vector3 input, bool fastWalk)
    {
        if (!canMove) return;

        float speed = fastWalk ? fastWalkSpeed : walkSpeed;
        Vector3 move = transform.forward * input.z + transform.right * input.x;

        move *= speed;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);
    }

    public void HandleLook(Vector2 lookInput)
    {
        if (!canBodyLook || lookTarget != null) return;

        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity * Time.deltaTime);
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

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSettings.rotateSmoothSpeed * Time.deltaTime
        );
    }


    private void ApplyGravity()
    {
        bool grounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (grounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
    }

    private void SitWithFade(Transform sitPoint)
    {
        currentSitPosition = sitPoint;
        StartCoroutine(SitRoutine());
    }

    private IEnumerator SitRoutine()
    {
        DisableMovement();
        yield return ScreenFader.Instance.FadeIn();

        originalHeight = controller.height;
        lastPosition = transform.position;

        controller.enabled = false;
        transform.position = currentSitPosition.position;
        controller.height = playerHeightWhileSitting;
        controller.enabled = true;

        GameEvents.RaiseOnSit();
        yield return ScreenFader.Instance.FadeOut();
    }

    private void UnSitWithFade()
    {
        StartCoroutine(UnSitRoutine());
    }

    private IEnumerator UnSitRoutine()
    {
        yield return ScreenFader.Instance.FadeIn();

        controller.enabled = false;
        controller.height = originalHeight;
        transform.position = lastPosition;
        controller.enabled = true;

        EnableMovement();
        GameEvents.RaiseOnUnSit();

        yield return ScreenFader.Instance.FadeOut();
    }

    private void Pee()
    {
        StartCoroutine(PeeRoutine());
    }

    private IEnumerator PeeRoutine()
    {
        DisableMovement();

        yield return ScreenFader.Instance.FadeIn();
        GameEvents.RaisePeeStart();
        GameEvents.OnRequestProgressBar?.Invoke(peeDuration, "Peeing");

        yield return new WaitForSeconds(peeDuration);

        yield return ScreenFader.Instance.FadeOut();
        GameEvents.RaisePeeEnd();

        EnableMovement();
    }

    private void EnableMovement() => canMove = true;
    private void DisableMovement() => canMove = false;

    private void EnableBodyLook() => canBodyLook = true;
    private void DisableBodyLook() => canBodyLook = false;
}
