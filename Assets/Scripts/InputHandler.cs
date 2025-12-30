using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private GameInput gameInput;
    private Player player;
    private Headbob headBob;
    private PlayerLookController playerLookController;

    public static Action OnInteractKeyPressed;
    public static Action OnGrabKeyPressed;
    public static Action OnSecondaryInteractPressed;


    private InputAction sprintAction;

    private void Awake()
    {
        gameInput = new GameInput();
        sprintAction = gameInput.Player.FastWalk;
        gameInput.Enable();

        player = Player.Instance;
        playerLookController = PlayerLookController.Instance;
        headBob = Headbob.Instance;
    }

    private void OnEnable()
    {
        gameInput.Player.Interact.performed += ctx => OnInteractKeyPressed?.Invoke();
        gameInput.Player.Grab.performed += ctx => OnGrabKeyPressed?.Invoke();
        gameInput.Player.SecondaryInteract.performed += ctx => OnSecondaryInteractPressed?.Invoke();
    }

    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void Update()
    {
        GetMovementNormalized();
        GetLookVector();
    }

    private void GetMovementNormalized()
    {
        Vector2 moveInput2D = gameInput.Player.Movement.ReadValue<Vector2>();
        Vector3 moveInput = new Vector3(moveInput2D.x, 0f, moveInput2D.y).normalized;
        player.HandleMovement(moveInput, sprintAction.IsPressed());
        headBob.SetIsFastWalk(sprintAction.IsPressed());
    }

    private void GetLookVector()
    {
        Vector2 lookInput = gameInput.Player.Look.ReadValue<Vector2>();
        playerLookController.HandleLook(lookInput);
        player.HandleLook(lookInput);
    }
}
