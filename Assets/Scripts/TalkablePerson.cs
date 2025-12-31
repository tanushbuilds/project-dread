using UnityEngine;

public class TalkablePerson : MonoBehaviour, ITalkable
{
    [SerializeField] private Transform head;
    [SerializeField] private float rotateSmoothSpeed;
    [SerializeField] private DialogueNode startNode;

    private bool hasTalked;
    private Transform playerTransform;
    private bool canRotate = true;

    private void OnEnable()
    {
        GameEvents.OnEndDialogue += OnPlayerResponse;
    }
    private void OnDisable()
    {
        GameEvents.OnEndDialogue -= OnPlayerResponse;
    }

    private void Start()
    {
        playerTransform = Player.Instance.transform;
    }

    private void Update()
    {
        if (hasTalked && canRotate)
        {
            RotateTowardsXTarget(playerTransform);
        }
    }

    public void Talk()
    {
        if (!hasTalked)
        {
            hasTalked = true;

            GameEvents.OnRequestCameraLookAt?.Invoke(head);
            GameEvents.OnRequestBodyLookAt?.Invoke(transform);
            GameEvents.OnRequestDisableMovement?.Invoke();
            GameEvents.OnRequestDisableHeadBob?.Invoke();



            GameEvents.RaiseOnTalk(startNode);
        }
    }
    private void RotateTowardsXTarget(Transform targetToLookAtX)
    {
        if (!canRotate) return;
        Vector3 direction = targetToLookAtX.position - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSmoothSpeed * Time.deltaTime);
    }

    private void OnPlayerResponse()
    {
        SetCanRotate(false);
    }

    private void SetCanRotate(bool canRotate)
    {
        this.canRotate = canRotate;
    }

    public void Initiate()
    {
        Talk();
    }
}
