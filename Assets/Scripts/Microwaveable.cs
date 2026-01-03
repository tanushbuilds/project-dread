using System;
using System.Collections;
using UnityEngine;

public class Microwaveable : MonoBehaviour
{
    [SerializeField] private float heatDuration;
    private bool isHeating;
    public event Action OnHeat;
    public event Action OnHeatCompleted;

    public void HandleHeat()
    {
        if (!isHeating)
        {
            isHeating = true;
            OnHeat?.Invoke();
            StartCoroutine(HeatRoutine());
        }
    }

    private IEnumerator HeatRoutine()
    {
        yield return new WaitForSeconds(heatDuration);

        isHeating = false;
        OnHeatCompleted?.Invoke();
        Debug.Log("Food Heated!");
    }

    public bool GetIsHeating()
    {
        return isHeating;
    }
}
