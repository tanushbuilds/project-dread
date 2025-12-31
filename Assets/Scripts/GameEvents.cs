using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    /* =========================
     * NOTIFICATIONS (EVENTS)
     * ========================= */

    public static event Action OnLookAtInteractable;
    public static event Action OnLookAway;

    public static event Action OnFoodEatStart;
    public static event Action OnFoodEaten;

    public static event Action OnPeeStart;
    public static event Action OnPeeEnd;

    public static event Action OnKnock;

    public static event Action<DialogueNode> OnTalk;
    public static event Action OnEndDialogue;

    public static event Action OnSit;
    public static event Action OnUnSit;

    /* =========================
     * REQUESTS / COMMANDS
     * ========================= */

    // Look control
    public static Action<Transform> OnRequestCameraLookAt;
    public static Action<Transform> OnRequestBodyLookAt;
    public static Action OnRequestStopCameraLookAt;
    public static Action OnRequestStopBodyLookAt;

    // Movement & control
    public static Action OnRequestDisableMovement;
    public static Action OnRequestDisableLook;
    public static Action OnRequestDisableBodyLook;
    public static Action OnRequestDisableHeadBob;

    public static Action OnRequestEnableMovement;
    public static Action OnRequestEnableLook;
    public static Action OnRequestEnableBodyLook;
    public static Action OnRequestEnableHeadBob;

    // Interaction requests
    public static Action<Transform> OnRequestSit;
    public static Action OnRequestUnSit;
    public static Action OnRequestPee;

    // UI
    public static Action<float, string> OnRequestProgressBar;

    /* =========================
     * RAISE / REQUEST METHODS
     * ========================= */

    // Dialogue
    public static void RaiseOnTalk(DialogueNode node)
    {
        OnTalk?.Invoke(node);
    }

    public static void RaiseOnEndDialogue()
    {
        OnEndDialogue?.Invoke();
    }

    // Interaction feedback
    public static void RaiseLookAtInteractable()
    {
        OnLookAtInteractable?.Invoke();
    }

    public static void RaiseLookAway()
    {
        OnLookAway?.Invoke();
    }

    public static void RaiseFoodEatStart()
    {
        OnFoodEatStart?.Invoke();
    }

    public static void RaiseFoodEaten()
    {
        OnFoodEaten?.Invoke();
    }

    public static void RaisePeeStart()
    {
        OnPeeStart?.Invoke();
    }

    public static void RaisePeeEnd()
    {
        OnPeeEnd?.Invoke();
    }

    public static void RaiseKnock()
    {
        OnKnock?.Invoke();
    }

    public static void RaiseOnSit()
    {
        OnSit?.Invoke();
    }

    public static void RaiseOnUnSit()
    {
        OnUnSit?.Invoke();
    }

    /* =========================
     * REQUEST HELPERS (OPTIONAL BUT CLEAN)
     * ========================= */

    public static void RequestCameraLookAt(Transform target)
    {
        OnRequestCameraLookAt?.Invoke(target);
    }

    public static void RequestBodyLookAt(Transform target)
    {
        OnRequestBodyLookAt?.Invoke(target);
    }

    public static void RequestStopCameraLookAt()
    {
        OnRequestStopCameraLookAt?.Invoke();
    }

    public static void RequestStopBodyLookAt()
    {
        OnRequestStopBodyLookAt?.Invoke();
    }
}
