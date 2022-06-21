using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSONQuest;
    [SerializeField] private TextAsset inkJSONRegular;
    [SerializeField] private float speed;

    private TextMeshProUGUI text;
    private GameObject dialogueBox;

    private bool isPlayerInside;
    private bool isPrintingQuestLines;

    private Story regularStory;
    private Story questStory;

    private IQuestElement questElement;

    private void Start()
    {
        text = Globals.Instance.DialogueText.GetComponent<TextMeshProUGUI>();
        dialogueBox = Globals.Instance.DialogueBox;
        text.text = string.Empty;
        dialogueBox.SetActive(false);

        print(inkJSONRegular.text);
        regularStory = new Story(inkJSONRegular.text);
        questStory = new Story(inkJSONQuest.text);

        InitializeChoiceButtons(questStory); //choices are only during quest dialogues

        questElement = GetComponent<IQuestElement>();
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            if (isPrintingQuestLines) printLines(questStory);
            else printLines(regularStory);
        }
    }

    private void printLines(Story story)
    {
        if (InputManager.Instance.GetInteractionPressed())
        {
            if (text.text == story.currentText)
            {
                nextLine(story);
            }
            else
            {
                StopAllCoroutines();
                text.text = story.currentText;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputManager.Instance.GetInteractionPressed() && !isPlayerInside) // isPlayerInside check so it does not call startDialogue() multiple times while inside
        {
            isPlayerInside = true;
            startDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            endDialogue();
        }
    }

    private void startDialogue()
    {
        text.text = string.Empty;
        if (questElement.StageBegin())
        {
            questStory.ResetState();
            dialogueBox.SetActive(true);
            isPrintingQuestLines = true;
            nextLine(questStory);
        }
        else
        {
            regularStory.ResetState();
            dialogueBox.SetActive(true);
            isPrintingQuestLines = false;
            nextLine(regularStory);
        }
    }

    private void nextLine(Story story)
    {
        if (story.canContinue)
        {
            text.text = string.Empty;
            StartCoroutine(typeLine(story));

            if (story.currentChoices.Count != 0)
            {
                displayChoices(story);
            }
            else
            {
                hideChoices();
            }
        }
        else 
        {
            endStage();
        }
    }

    private void displayChoices(Story story)
    {
        int index = 0;
        foreach (Choice choice in story.currentChoices)
        {
            Globals.Instance.Choices[index].SetActive(true);
            Globals.Instance.Choices[index].GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            index++;
        }
    }

    private void InitializeChoiceButtons(Story story)
    {
        Globals.Instance.Choices[0].GetComponent<Button>().onClick.AddListener(() => MakeChoice(story, 0));
        Globals.Instance.Choices[1].GetComponent<Button>().onClick.AddListener(() => MakeChoice(story, 1));
        Globals.Instance.Choices[2].GetComponent<Button>().onClick.AddListener(() => MakeChoice(story, 2));
    }

    public void MakeChoice(Story story, int index)
    {
        if (index >= 0 && index < story.currentChoices.Count)
        {
            story.ChooseChoiceIndex(index);
            nextLine(story);

            if (index == 0) Scales.Instance.AddPleasure(1);
            else if (index == 1) Scales.Instance.AddFear(1);
            else if (index == 2) Scales.Instance.AddRealism(1);
        }

    }

    private void hideChoices()
    {
        for (int i = 0; i < Globals.Instance.Choices.Length; i++)
        {
            GameObject choice = Globals.Instance.Choices[i];
            choice.SetActive(false);
        }
    }

    private void endStage()
    {
        if (isPrintingQuestLines) questElement.StageComplete();

        endDialogue();
    }

    private void endDialogue()
    {
        StopAllCoroutines();
        text.text = string.Empty;
        dialogueBox.SetActive(false);
    }

    private IEnumerator typeLine(Story story)
    {
        foreach (char c in story.Continue().ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(speed);
        }
    }
}
