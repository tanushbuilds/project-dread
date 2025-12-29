using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    private CharacterController playerController;
    private Vector3 velocity;

    private bool canLook = true;
    private float playerHeight;
    private Vector3 lastPosition;

    [SerializeField] private float peeDuration;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float fastWalkSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private float stepInterval;
    [SerializeField] private Vector2 footstepSoundPitchRange;

    private float stepTimer;


    [SerializeField] private float playerHeightWhileSitting;

    private bool isGrounded;
    private float speed;
    private bool canMove = true;
    private Transform currentSitPosition;

    private void OnEnable()
    {
        GameEvents.OnRequestSit += SitWithFade;
        GameEvents.OnRequestUnSit += UnSitWithFade;
        GameEvents.OnRequestPee += Pee;
        GameEvents.OnTalk += _OnTalk;
        GameEvents.OnEndDialogue += EnableMovement;
        GameEvents.OnEndDialogue += EnableLook;
        GameEvents.OnSleep += DisableMovement;
        GameEvents.OnSleep += DisableLook;


    }

    private void OnDisable()
    {
        GameEvents.OnRequestSit -= SitWithFade;
        GameEvents.OnRequestUnSit -= UnSitWithFade;
        GameEvents.OnRequestPee -= Pee;
        GameEvents.OnTalk -= _OnTalk;
        GameEvents.OnEndDialogue -= EnableMovement;
        GameEvents.OnEndDialogue -= EnableLook;
        GameEvents.OnSleep -= DisableMovement;
        GameEvents.OnSleep -= DisableLook;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Instance == null)
        {
            Instance = this;
        }
        playerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();
    }

    public void HandleMovement(Vector3 moveDir, bool isFastWalk)
    {
        if (!canMove) return;
        speed = isFastWalk ? fastWalkSpeed : walkSpeed;

        Vector3 move = transform.forward * moveDir.z + transform.right * moveDir.x;
        move *= speed;

        move.y = velocity.y;

        playerController.Move(move * Time.deltaTime);

        if (canMove && moveDir.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                footstep.pitch = Random.Range(footstepSoundPitchRange.x, footstepSoundPitchRange.y);
                footstep.Play(); 
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }

    }
    public void HandleLook(Vector2 lookDir)
    {
        if (!canLook) return;
        transform.Rotate(Vector3.up * lookDir.x * mouseSensitivity * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
    }


    private void Sit()
    {
        playerHeight = playerController.height;
        lastPosition = transform.position;

        DisableMovement();
        playerController.enabled = false;

        transform.position = currentSitPosition.position;
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        playerController.height = playerHeightWhileSitting;
        playerController.center = Vector3.up * (playerController.height / 2f);
        playerController.enabled = true;

    }

    private void SitWithFade(Transform sitPosition)
    {
        currentSitPosition = sitPosition;
        StartCoroutine(SitRoutine());
    }

    private IEnumerator SitRoutine()
    {
        yield return StartCoroutine(ScreenFader.Instance.FadeIn());

        Sit();
        GameEvents.RaiseOnSit();

        yield return StartCoroutine(ScreenFader.Instance.FadeOut());

    }


    private void UnSit()
    {
        playerController.enabled = false;

        playerController.height = playerHeight;
        playerController.center = Vector3.up * (playerController.height / 2f);

        transform.position = lastPosition;
        transform.rotation = Quaternion.identity;

        playerController.enabled = true;
        EnableMovement();
    }
    private void UnSitWithFade()
    {
        StartCoroutine(UnSitRoutine());
    }

    private IEnumerator UnSitRoutine()
    {
        yield return StartCoroutine(ScreenFader.Instance.FadeIn());

        UnSit();
        GameEvents.RaiseOnUnSit();

        yield return StartCoroutine(ScreenFader.Instance.FadeOut());

    }

    private void _OnTalk(DialogueNode _)
    {
        DisableMovement();
        DisableLook();
    }

    private void Pee()
    {
        StartCoroutine(PeeRoutine());
    }

    private IEnumerator PeeRoutine()
    {
        DisableMovement();

        yield return StartCoroutine(ScreenFader.Instance.FadeIn());


        GameEvents.RaisePeeStart();

        yield return StartCoroutine(ScreenFader.Instance.FadeRemain(peeDuration));

        Debug.Log("Finished peeing");

        yield return StartCoroutine(ScreenFader.Instance.FadeOut());


        GameEvents.RaisePeeEnd();
        EnableMovement();
    }

    private void DisableMovement()
    {
        canMove = false;
    }
    private void EnableMovement()
    {
        canMove = true;
    }
    private void DisableLook()
    {
        canLook = false;
    }
    private void EnableLook()
    {
        canLook = true;
    }
}
