using System;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private GameObject visual;
    [SerializeField] private float rotationSpeed = 3;

    private Animator anim;
    private bool rotate;
    private Transform currentTarget;
    public event Action OnDespawn;

    private void Awake()
    {
        if (anim == null)
        {
            anim = visual.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (visual != null && rotate)
        {
            Vector3 direction = currentTarget.position - visual.transform.position;

            Quaternion targetRot = Quaternion.LookRotation(direction);

            visual.transform.rotation = Quaternion.Lerp(visual.transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

    }

    public void Turn(Transform target)
    {
        rotate = true;
        currentTarget = target;
    }


    public void WalkForward()
    {
        if (anim != null)
        {
            anim.SetTrigger("Walk");
        }
    }

    public void SetNPCActive()
    {
        if (visual != null)
        {
            visual.SetActive(true);
        }
    }
    public void SetNPCInactive()
    {
        if (visual != null)
        {
            visual.SetActive(false);
        }
    }
    public void DespawnNPC()
    {
        if (visual != null)
        {
            Destroy(visual);
            OnDespawn?.Invoke();
        }
    }
    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }
}
