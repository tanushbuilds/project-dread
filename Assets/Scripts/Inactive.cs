using UnityEngine;

public class Inactive : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDisable;

    private void Start()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
