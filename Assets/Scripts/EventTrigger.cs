using System;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public event Action OnTrigger;
    [SerializeField] private GameObject obj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == obj) {
            OnTrigger?.Invoke();
        }
    }
}
