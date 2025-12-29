using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        UIMessageEvents.OnMessageChanged += UpdateText;
    }

    private void OnDisable()
    {
        UIMessageEvents.OnMessageChanged -= UpdateText;
    }

    public void UpdateText(string updatedText)
    {
        text.text = updatedText;
    }
}
