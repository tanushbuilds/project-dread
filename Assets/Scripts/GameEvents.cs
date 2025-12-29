using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event Action OnLookAtInteractable;
    public static event Action OnLookAway;
    public static event Action OnFoodEaten;
    public static event Action OnPeeStart;
    public static event Action OnPeeEnd;
    public static event Action OnKnock;
    public static Action OnEndDialogue;
    public static Action<DialogueNode> OnTalk;
    public static Action<Transform, Transform> OnRequestLookAt;
    public static Action<Transform> OnRequestSit;
    public static Action OnRequestUnSit;
    public static Action OnRequestPee;
    public static event Action OnSit;
    public static event Action OnUnSit;
    public static Action OnSleep;


    public static void RaiseLookAtInteractable(){
        OnLookAtInteractable?.Invoke();
    }
    public static void RaiseOnSit()
    {
        OnSit?.Invoke();
    }
    public static void RaiseOnUnSit()
    {
        OnUnSit?.Invoke();
    }

    public static void RaiseLookAway()
    {
        OnLookAway?.Invoke();
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
}
