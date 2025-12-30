using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressAction;


    private float duration;
    private float timePassed;
    private bool progressSliderRequired;

    private void OnEnable()
    {
        GameEvents.OnRequestProgressBar += SetUpProgressSlider;
    }
    private void OnDisable()
    {
        GameEvents.OnRequestProgressBar -= SetUpProgressSlider;
    }

    private void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.value = 0f;
            HideProgressSlider();
        }
    }

    private void Update()
    {
        if (!progressSliderRequired) return;
        PlayProgressBar();
    }
    private void SetUpProgressSlider(float duration, string actionName)
    {
        this.duration = duration;
        progressSliderRequired = true;
        ShowProgressSlider();
        progressAction.text = actionName;
    }
    private void PlayProgressBar()
    {
        timePassed += Time.deltaTime;
        progressSlider.value = Mathf.Clamp01(timePassed / duration);


        if (timePassed >= duration)
        {
            Reset();
        }
    }
    private void ShowProgressSlider()
    {
        progressSlider.gameObject.SetActive(true);
    }
    private void HideProgressSlider()
    {
        progressSlider.gameObject.SetActive(false);
    }
    private void Reset()
    {
        progressAction.text = "";
        timePassed = 0f;
        progressSlider.value = 0f;
        HideProgressSlider();
        progressSliderRequired = false;
    }
}
