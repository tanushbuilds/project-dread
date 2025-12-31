using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject responsePanel;
    [SerializeField] private float typeRate;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private TextMeshProUGUI dialogueTextUI;
    [SerializeField] private GameObject responseButtonPrefab;
    [SerializeField] private Vector2 clickSoundPitchRange;

    private DialogueNode currentNode;
    private string dialogueText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnTalk += ShowDialogue;
    }

    private void OnDisable()
    {
        GameEvents.OnTalk -= ShowDialogue;
    }

    public void ShowDialogue(DialogueNode node)
    {
        currentNode = node;
        dialogueText = node.npcText;

        dialogueTextUI.text = "";
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeDialogue());
    }

    private IEnumerator TypeDialogue()
    {
        for (int i = 0; i < dialogueText.Length; i++)
        {
            dialogueTextUI.text += dialogueText[i];

            if (i % 3 == 0)
            {
                clickSource.pitch = Random.Range(clickSoundPitchRange.x, clickSoundPitchRange.y);
                clickSource.PlayOneShot(clickSound, 0.25f);
            }

            yield return new WaitForSeconds(typeRate);
        }

        ShowResponses();
    }

    private void ShowResponses()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        responsePanel.SetActive(true);

        foreach (Transform child in responsePanel.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < currentNode.playerResponses.Length; i++)
        {
            int index = i;
            GameObject buttonObj = Instantiate(responseButtonPrefab, responsePanel.transform);
            Button btn = buttonObj.GetComponent<Button>();
            TextMeshProUGUI btnText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            btnText.text = currentNode.playerResponses[i];
            btn.onClick.AddListener(() => HandleResponse(index));
        }
    }

    private void HandleResponse(int index)
    {
        responsePanel.SetActive(false);

        if (currentNode.nextNodes != null &&
            index < currentNode.nextNodes.Length &&
            currentNode.nextNodes[index] != null)
        {
            ShowDialogue(currentNode.nextNodes[index]);
        }
        else
        {
            EndDialogue();
        }
    }
    private void EndDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        dialoguePanel.SetActive(false);
        responsePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameEvents.RaiseOnEndDialogue();

        GameEvents.OnRequestStopCameraLookAt?.Invoke();
        GameEvents.OnRequestStopBodyLookAt?.Invoke();
        GameEvents.OnRequestEnableBodyLook?.Invoke();
        GameEvents.OnRequestEnableHeadBob?.Invoke();
        GameEvents.OnRequestEnableMovement?.Invoke();
        GameEvents.OnRequestEnableLook?.Invoke();

    }
}
