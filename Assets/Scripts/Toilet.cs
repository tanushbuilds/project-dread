using UnityEngine;

public class Toilet : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioSource sound;
    [SerializeField] private BoxCollider toiletCollider;

    private bool hasPeed;

    private void OnEnable()
    {
        GameEvents.OnFoodEaten += EnableCollider;
        GameEvents.OnPeeStart += PlaySound;
    }
    private void OnDisable()
    {
        GameEvents.OnFoodEaten -= EnableCollider;
        GameEvents.OnPeeStart -= PlaySound;
    }

    private void Start()
    {
        toiletCollider.enabled = false;
    }

    public void Interact()
    {
        if (hasPeed) return;

        GameEvents.OnRequestPee?.Invoke();
        
        hasPeed = true;
    }
    private void PlaySound()
    {
        sound.Play();
    }

    private void EnableCollider()
    {
        toiletCollider.enabled = true;
    }
}
