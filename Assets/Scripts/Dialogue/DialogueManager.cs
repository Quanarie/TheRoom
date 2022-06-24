using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public float waitTimeBetweenLetters;
    public int maximumSymbolsInRow;
    public int maximumRows;

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

        ChoicesParent.SetActive(false);
        DialogueText.GetComponent<TextMeshProUGUI>().text = string.Empty;
        DialogueBox.SetActive(false);
    }

    public Button[] GetChoices()
    {
        return choices;
    }

    public bool IsChoiceActive()
    {
        return choices != null;
    }

    public bool IsDialogueOn()
    {
        return DialogueBox.activeSelf;
    }

    public void Show()
    {
        DialogueBox.SetActive(true);
        DialogueText.SetActive(true);
    }

    public void Hide()
    {
        DialogueBox.SetActive(false);
        DialogueText.SetActive(false);
        DialogueText.GetComponent<TextMeshProUGUI>().text = "";
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
        if (!IsChoiceActive()) return;

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i])
                Destroy(choices[i].gameObject);
        }
        choices = null;
    }
}
