using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public float waitTimeBetweenLetters;

    public GameObject DialogueBox;
    public GameObject DialogueText;
    public GameObject ChoicePrefab;
    public GameObject ChoicesParent;

    private Button[] choices;
    private Vector3 startPosForChoice = new Vector3(0, -75, 0);

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
        ChoicesParent.SetActive(false);
        DialogueText.GetComponent<TextMeshProUGUI>().text = string.Empty;
        DialogueBox.SetActive(false);
    }

    public Button[] GetChoices()
    {
        return choices;
    }

    public void ShowBox()
    {
        DialogueBox.SetActive(true);
    }

    public Button[] ShowChoices(int quantity)
    {
        ChoicesParent.SetActive(true);
        choices = new Button[quantity];
        for (int i = 0; i < quantity; i++)
        {
            choices[i] = Instantiate(ChoicePrefab, ChoicesParent.transform).GetComponent<Button>();
            choices[i].GetComponent<RectTransform>().localPosition =
                new Vector3(startPosForChoice.x, startPosForChoice.y - 100 * i, startPosForChoice.z);
        }

        return choices;
    }

    public void HideChoices()
    {
        for (int i = 0; i < choices.Length; i++)
        {
            Destroy(choices[i].gameObject);
        }
    }

    public void HideBox()
    {
        DialogueBox.SetActive(false);
    }
}
