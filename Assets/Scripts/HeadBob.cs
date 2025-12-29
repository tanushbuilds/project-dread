using UnityEngine;

public class Headbob : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _enabled = true;
    [SerializeField] private float walkAmplitudeY = 0.015f;
    [SerializeField] private float walkAmplitudeX = 0.015f;

    [SerializeField] private float walkFrequency = 10f;

    [Space(10)]
    [SerializeField] private Transform cameraHolder;

    private CharacterController controller;
    private Vector3 startPos;
    public static Headbob instance;

    private void OnEnable()
    {
        GameEvents.OnRequestPee += DisableHeadBob;
        GameEvents.OnPeeEnd += EnableHeadBob;
        GameEvents.OnSit += DisableHeadBob;
        GameEvents.OnUnSit += EnableHeadBob;
        GameEvents.OnTalk += _OnTalk;
        GameEvents.OnEndDialogue += EnableHeadBob;
        GameEvents.OnSleep += DisableHeadBob;
    }

    private void OnDisable()
    {
        GameEvents.OnRequestPee -= DisableHeadBob;
        GameEvents.OnPeeEnd -= EnableHeadBob;
        GameEvents.OnSit -= DisableHeadBob;
        GameEvents.OnUnSit -= EnableHeadBob;
        GameEvents.OnTalk -= _OnTalk;
        GameEvents.OnEndDialogue -= EnableHeadBob;
        GameEvents.OnSleep -= DisableHeadBob;
    }


    private void Awake()
    {
        instance = this;
        controller = GetComponent<CharacterController>();
        startPos = cameraHolder.localPosition;
    }

    private void LateUpdate()
    {
        if (!_enabled) return;

        if (IsMoving())
        {
            Vector3 motion = FootstepMotion();
            cameraHolder.localPosition = Vector3.Lerp(
                cameraHolder.localPosition,
                startPos + motion,
                Time.deltaTime * 12f
            );
        }
        else
        {
            cameraHolder.localPosition = Vector3.Lerp(
                cameraHolder.localPosition,
                startPos,
                Time.deltaTime * 8f
            );
        }
    }

    private bool IsMoving()
    {
        if (controller == null) return false;

        Vector3 flatVel = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        return controller.isGrounded && flatVel.magnitude > 0.1f;
    }

    private Vector3 FootstepMotion()
    {
        Vector3 motion = Vector3.zero;

        float ampY = walkAmplitudeY;
        float ampX = walkAmplitudeX;

        float freq = walkFrequency;

        motion.y = Mathf.Sin(Time.time * freq) * ampY;
        motion.x = Mathf.Cos(Time.time * freq * 0.5f) * ampX * 0.5f;

        return motion;
    }

    private void _OnTalk(DialogueNode _)
    {
        DisableHeadBob();
    }

    private void DisableHeadBob()
    {
        _enabled = false;
    }
    private void EnableHeadBob()
    {
        _enabled = true; 
    }
}
