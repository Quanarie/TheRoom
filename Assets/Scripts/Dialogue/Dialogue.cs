using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private TextAsset story;

    private string[] text;
    private int index;

    private bool isPlayerInRange;

    private TextMeshProUGUI dialogueText;

    private string currentTypingText;

    private const int ampersand = 38;
    private const int asterisk = 42;
    private const int slash = 47;
    private const int circumflex = 94;
    private const int percent = 37;
    private const string pleasure = "Pleasure";
    private const string anxiety = "Anxiety";
    private const string realistic = "Realistic";

    private void Start()
    {
        text = story.text.Split("\n");
        index = 0;
        isPlayerInRange = false;
        dialogueText = DialogueManager.Instance.DialogueText.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            bool isFirstCharNextLineIsSpecial = false;
            if (canReadNext())
            {
                analyzeFirstCharNextLine(ref isFirstCharNextLineIsSpecial);
            }

            if (isFirstCharNextLineIsSpecial == false)
            {
                input();
            }
        }
    }

    private void input()
    {
        if (InputManager.Instance.GetInteractionPressed())
        {
            if (dialogueText.text == currentTypingText)
            {
                index++;
                if (canRead())
                {
                    printCurrentLine();
                }
                else
                {
                    endDialogue();
                }
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = currentTypingText;
            }
        }
    }

    private void analyzeFirstCharNextLine(ref bool isFirstCharNextLineIsSpecial)
    {
        char firstCharNextLine = readNextLine().ToCharArray()[0];
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

    private void displayChoices()
    {
        string[] line = readNextLine().Split(";");
        line[0] = line[0].Replace("*", "");
        Button[] choices = DialogueManager.Instance.ShowChoices(line.Length);

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(true);
            choices[i].GetComponentInChildren<TextMeshProUGUI>().text = line[i];
            int capturedi = i;
            choices[i].onClick.AddListener(() => reactChoice(line[capturedi]));
        }

        index++; // go to * line so displayChoices() is not called multiple times from update
    }

    private void reactChoice(string choice)
    {
        DialogueManager.Instance.HideChoices();
        while (readLine().Replace("&", "") != choice)
        {
            index++;
        }
        index++;
        printCurrentLine();
    }

    private void printCurrentLine()
    {
        StopAllCoroutines();
        dialogueText.text = string.Empty;
        if (readLine().ToCharArray()[readLine().Length - 1] == ampersand)
        {
            string line = readLine().Remove(readLine().Length - 1);
            StartCoroutine(typeCurrentLine(line));

            while (readLine().ToCharArray()[readLine().Length - 1] != percent)
            {
                index++;
            }
        }
        else if (readLine().ToCharArray()[readLine().Length - 1] == percent)
        {
            string line = readLine().Remove(readLine().Length - 1);
            StartCoroutine(typeCurrentLine(line));
        }
        else
        {
            StartCoroutine(typeCurrentLine(readLine()));
        }
    }

    private IEnumerator typeCurrentLine(string line)
    {
        currentTypingText = line;
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(DialogueManager.Instance.waitTimeBetweenLetters);
        }
    }

    private string readLine()   //call after canRead() check only
    {
        return text[index];
    }

    private string readNextLine()   //call after canReadNext() check only
    {
        return text[index + 1];
    }

    private bool canRead()
    {
        return index < text.Length;
    }

    private bool canReadNext()
    {
        return index + 1 < text.Length;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputManager.Instance.GetInteractionPressed() && !isPlayerInRange)
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
        printCurrentLine();
    }

    private void endDialogue()
    {
        DialogueManager.Instance.HideBox();
        isPlayerInRange = false;
        index = 0;
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
}