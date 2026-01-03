using System;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    [Header("Rules")]
    [SerializeField] private bool canBeThrown = true;

    [Header("Visual")]
    [SerializeField] private Vector3 cameraOffset;

    public bool CanBeThrown => canBeThrown;
    public Vector3 CameraOffset => cameraOffset;

    public event Action OnGrab;
    public event Action OnDrop;

    public void OnGrabbed()
    {
        OnGrab?.Invoke();
        RemoveRigidbody();
    }

    public void OnDropped()
    {
        OnDrop?.Invoke();
    }
    public Rigidbody GetOrAddRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        return rb;
    }

    private void RemoveRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            Destroy(rb);
    }
}
