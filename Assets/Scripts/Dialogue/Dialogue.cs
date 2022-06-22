using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextAsset story;
    [SerializeField] private float speed;

    private string[] text;
    private int index;

    private Button[] choices;

    private bool isPlayerInRange;

    private TextMeshProUGUI dialogueText;

    private const int ampersand = 38;
    private const int asterisk = 42;
    private const int slash = 47;
    private const int circumflex = 94;
    private const string pleasure = "Pleasure";
    private const string anxiety = "Anxiety";
    private const string realistic = "Realistic";

    private void Start()
    {
        text = story.text.Split("\n");
        index = 0;
        isPlayerInRange = false;
        dialogueText = DialogueManager.Instance.DialogueText.GetComponent<TextMeshProUGUI>();

        choices = new Button[DialogueManager.Instance.Choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i] = DialogueManager.Instance.Choices[i].GetComponent<Button>();
        }
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            bool isFirstCharNextLineIsSpecial = false;
            char firstCharNextLine;
            if (canReadNext())
            {
                firstCharNextLine = readNextLine().ToCharArray()[0];
                if (firstCharNextLine == asterisk) // choice
                {
                    displayChoices();
                    isFirstCharNextLineIsSpecial = true;
                }
                else if (firstCharNextLine == ampersand) // one of the choices
                {
                    isFirstCharNextLineIsSpecial = true;
                }
                else if (firstCharNextLine == slash) // scale change
                {
                    isFirstCharNextLineIsSpecial = true;
                    string[] line = readNextLine().Split(";");
                    line[0] = line[0].Replace("/", ""); // hardcode
                    changeScale(line[0], int.Parse(line[1]));
                    index++;
                }
                else if (firstCharNextLine == circumflex) // stage change
                {
                    isFirstCharNextLineIsSpecial = true;
                    string[] line = readNextLine().Split(";");
                    line[0] = line[0].Replace("^", ""); // hardcode
                    changeStage(int.Parse(line[0]), int.Parse(line[1]));
                    index++;
                }
            }

            if (InputManager.Instance.GetInteractionPressed() && !isFirstCharNextLineIsSpecial)
            {
                index++;
                if (canRead())
                {
                    currentLine();
                }
                else
                {
                    endDialogue();
                }
            }
        }
    }

    private void changeStage(int questId, int result)
    {
        QuestManager.Instance.Quests[questId].SetCurrentStage(result);
    }

    private void changeScale(string name, int delta)
    {
        print("called scale");
        if (name == pleasure)
        {
            Scales.Instance.AddPleasure(delta);
        }
        else if (name == anxiety)
        {
            Scales.Instance.AddAnxiety(delta);
        }
        else if (name == realistic)
        {
            Scales.Instance.AddRealistic(delta);
        }
    }

    private void displayChoices()
    {
        string[] line = readNextLine().Split(";");
        line[0] = line[0].Replace("*", "");

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(true);
            choices[i].GetComponentInChildren<TextMeshProUGUI>().text = line[i];
            int capturedi = i;
            choices[i].onClick.AddListener(() => reactChoice(line[capturedi]));
        }

        index++; // go to * line so displayChoices() is not called multiple times from update
    }

    private void hideChoices()
    {
        foreach(Button choice in choices)
        {
            choice.gameObject.SetActive(false);
            choice.onClick.RemoveAllListeners();
        }
    }

    private void reactChoice(string choice)
    {
        while (canReadNext() && readNextLine().ToCharArray()[0] == ampersand)
        {
            if (readNextLine().Split(":")[0].Replace("&", "") == choice)
            {
                index++; // go to current choice line
                currentLine(":", 1);
                hideChoices();
                while (readNextLine().ToCharArray()[0] == ampersand) // point to the place after the choice
                {
                    index++;
                }
            }
            else
            {
                index++;
            }
        }
    }

    private void currentLine()
    {
        dialogueText.text = string.Empty;
        StartCoroutine(typeCurrentLine(readLine()));
    }

    private void currentLine(string splitOn, int index)
    {
        dialogueText.text = string.Empty;
        string line = readLine().Split(splitOn)[index];
        StartCoroutine(typeCurrentLine(line));
    }

    private string readLine()   //call after canRead() check only
    {
        return text[index];
    }

    private string readNextLine()   //call after canReadNext() check only
    {
        return text[index + 1];
    }

    private IEnumerator typeCurrentLine(string line)
    {
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    private bool canRead()
    {
        return index < text.Length;
    }

    private bool canReadNext()
    {
        return index + 1 < text.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            startDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            endDialogue();
        }
    }

    private void startDialogue()
    {
        DialogueManager.Instance.ShowBox();
        isPlayerInRange = true;
        currentLine();
    }

    private void endDialogue()
    {
        DialogueManager.Instance.HideBox();
        isPlayerInRange = false;
        index = 0;
    }





    /*[SerializeField] private TextAsset inkJSONQuest;
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
    }*/
}
