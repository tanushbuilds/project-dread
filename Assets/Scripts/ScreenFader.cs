using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    [Header("Common Fade Durations")]
    [SerializeField] private float defaultFadeInDuration;
    [SerializeField] private float defaultFadeOutDuration;
    [SerializeField] private Image fadeImage;
    [SerializeField] private TMP_Text fadeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < defaultFadeOutDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - (t / defaultFadeOutDuration));
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeRemain(float duration)
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(duration);
    }

    public IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < defaultFadeInDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t / defaultFadeInDuration);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }



    public IEnumerator FadeWithText(
        string text,
        float textDuration,
        System.Action midAction = null
    )
    {
        yield return StartCoroutine(FadeIn());

        if (fadeText != null)
        {
            fadeText.text = text;
            fadeText.alpha = 1;
        }

        midAction?.Invoke();

        yield return new WaitForSeconds(textDuration);

        if (fadeText != null)
            fadeText.alpha = 0;

        yield return StartCoroutine(FadeOut());
    }
}
