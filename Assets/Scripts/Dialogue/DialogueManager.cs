using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject DialogueBox;
    public GameObject DialogueText;
    public GameObject[] Choices;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        foreach (GameObject button in Choices)
        {
            button.SetActive(false);
        }
        DialogueText.GetComponent<TextMeshProUGUI>().text = string.Empty;
        DialogueBox.SetActive(false);
    }

    public void ShowBox()
    {
        DialogueBox.SetActive(true);
    }

    public void HideBox()
    {
        DialogueBox.SetActive(false);
    }
}
