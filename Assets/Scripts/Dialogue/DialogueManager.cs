using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private float waitTimeBetweenLetters;
    [SerializeField] private int maximumSymbolsInRow;
    [SerializeField] private int maximumRows;

    public float GetTimeBetweenLetters() => waitTimeBetweenLetters;
    public float GetMaximumSymbolsInRow() => maximumSymbolsInRow;
    public float GetMaximumRows() => maximumRows;

    public GameObject DialogueBox;
    public GameObject DialogueText;
    public GameObject ChoicePrefab;
    public GameObject ChoicesParent;

    private List<Button> choices;
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

    public List<Button> GetChoices()
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
        HideChoices();
        DialogueText.GetComponent<TextMeshProUGUI>().text = "";
    }

    public List<Button> ShowChoices(int quantity)
    {
        ChoicesParent.SetActive(true);
        choices = new List<Button>();
        for (int i = 0; i < quantity; i++)
        {
            Button choice = Instantiate(ChoicePrefab, ChoicesParent.transform).GetComponent<Button>();
            choice.GetComponent<RectTransform>().localPosition =
                new Vector3(startPosForChoice.x, startPosForChoice.y - 100 * i, startPosForChoice.z);
            choices.Add(choice);
        }

        return choices;
    }

    public Button AddChoice()
    {
        Button choice = Instantiate(ChoicePrefab, ChoicesParent.transform).GetComponent<Button>();
        choice.GetComponent<RectTransform>().localPosition =
                new Vector3(startPosForChoice.x, startPosForChoice.y - 100 * choices.Count, startPosForChoice.z);
        choices.Add(choice);
        return choice;
    }

    public void RandomizeChoices()
    {
        if (choices == null) return;

        Vector3[] randomizedPositions = new Vector3[choices.Count];
        for (int i = 0; i < choices.Count; i++)
        {
            randomizedPositions[i] = choices[i].GetComponent<RectTransform>().position;
        }

        randomizedPositions = reShuffle(randomizedPositions);

        for (int i = 0; i < choices.Count; i++)
        {
            choices[i].GetComponent<RectTransform>().position = randomizedPositions[i];
        }
    }

    private Vector3[] reShuffle(Vector3[] positions)
    {
        for (int t = 0; t < positions.Length; t++)
        {
            Vector3 tmp = positions[t];
            int r = Random.Range(t, positions.Length);
            positions[t] = positions[r];
            positions[r] = tmp;
        }
        return positions;
    }

    public void HideChoices()
    {
        if (!IsChoiceActive()) return;

        for (int i = 0; i < choices.Count; i++)
        {
            if (choices[i])
                Destroy(choices[i].gameObject);
        }
        choices = null;
    }
}
