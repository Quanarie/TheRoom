using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class Dialogue : MonoBehaviour
{
    public delegate void EndOfDialogue();
    public event EndOfDialogue OnEndOfDialogue;

    public TextAsset story;

    private List<string> text;
    private int index;

    public bool isDialogueOn = false;
    private bool mustMoveForward = false;

    private TextMeshProUGUI dialogueText;
    private string currentTypingText;

    private Stack<int> endsOfChoice = new();
    private Stack<int> endsOfOption = new();

    private const string pleasure = "Pleasure";
    private const string anxiety = "Anxiety";
    private const string realistic = "Realistic";

    public void Start()
    {
        dialogueText = DialogueManager.Instance.DialogueText.GetComponent<TextMeshProUGUI>();
        text = story.text.Split("\n").ToList();
        formatText();
    }

    private void formatText()
    {
        for (int k = 0; k < text.Count; k++)
        {
            if (text[k].Contains('&') || text[k].Contains('*') || text[k].Contains('%') || text[k].Contains('/') || text[k].Contains('^'))
                continue;
            if (text[k] == "")
                continue;

            char[] chars = text[k].ToCharArray();
            text.Remove(text[k]);

            int j = 0;
            int lines = 0;
            string newLine = "";
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '#' || i == chars.Length - 1)
                {
                    int tempLines = Mathf.CeilToInt((float)(i - j - 1) / DialogueManager.Instance.maximumSymbolsInRow);
                    if (lines + tempLines <= DialogueManager.Instance.maximumRows)
                    {
                        lines += tempLines;
                        for (; j < i; j++)
                        {
                            newLine += chars[j];
                        }
                    }
                    else
                    {
                        lines = tempLines;
                        text.Insert(k, newLine);
                        newLine = "";
                        j++;
                        for (; j < i; j++)
                        {
                            newLine += chars[j];
                        }
                        k++;
                    }
                }
            }
            text.Insert(k, newLine);
            k++;
        }
    }

    private void Update()
    {
        if (DialogueManager.Instance.IsChoiceActive()) return;

        if (!isCurrentLineEmpty(index) && isDialogueOn && endsOfChoice.Count == 0)
        {
            if (InputManager.Instance.GetInteractionPressed() || mustMoveForward)
            {
                mustMoveForward = false;
                readLine();
            }
        }

        if (endsOfOption.Count != 0)
        {
            if (index <= endsOfOption.Peek())
            {
                if (InputManager.Instance.GetInteractionPressed() || mustMoveForward)
                {
                    mustMoveForward = false;
                    readLine();
                }
            }
            else
            {
                index = endsOfChoice.Pop();
                index++;
                endsOfOption.Pop();
            }
        }

        if (isCurrentLineEmpty(index) && isDialogueOn)
        {
            if (InputManager.Instance.GetInteractionPressed() || mustMoveForward)
            {
                mustMoveForward = false;
                endDialogue();
            }
        }
    }

    private bool isCurrentLineEmpty(int ind)
    {
        return currentLine(ind) == "";
    }

    private void readLine()
    {
        if (firstChar(index) == '*')
        {
            displayChoices();
        }
        else if (firstChar(index) == '/')
        {
            string[] parameters = currentLine(index).Replace("/", "").Split(";");
            parameters[1] = parameters[1].Replace("+", "").Replace("&", "").Replace("*", "");
            changeScale(parameters[0], int.Parse(parameters[1]));
            mustMoveForward = true;
        }
        else if (firstChar(index) == '^')
        {
            string[] parameters = currentLine(index).Replace("^", "").Split(";");
            parameters[1] = parameters[1].Replace("&", "").Replace("*", "");
            changeStage(int.Parse(parameters[0]), int.Parse(parameters[1]));
            mustMoveForward = true;
        }
        else outTextLine(index);
    }

    private void displayChoices()
    {
        string[] line = currentLine(index).Split(";");
        line[0] = line[0].Replace("*", "");
        Button[] choices = DialogueManager.Instance.ShowChoices(line.Length);

        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(true);
            choices[i].GetComponentInChildren<TextMeshProUGUI>().text = line[i];
            int capturedi = i;
            choices[i].onClick.AddListener(() => startChoices(line[capturedi]));
        }

        index++; // from * to first &
        endsOfChoice.Push(findTheEndChoice(index));
    }

    private void startChoices(string choice)
    {
        DialogueManager.Instance.HideChoices();
        reactToChoice(choice);
        readLine();
    }

    private void reactToChoice(string choice)
    {
        if (currentLine(index).Replace("&", "") == choice)
        {
            readChoice();
        }
        else
        {
            index = findTheEndOption(index);
            index++; // after option
            reactToChoice(choice);
        }
    }

    private void readChoice()
    {
        int endOfOption = findTheEndOption(index);
        endsOfOption.Push(endOfOption);
        index++;
    }

    private int findTheEndChoice(int ind)
    {
        int i = 0;
        while (i < 1)
        {
            ind++;
            if (firstChar(ind) == '*') i--;
            else if (lastChar(ind) == '*' || lastChar(ind) == '&')
            {
                for (int j = currentLine(ind).Length - 1; j > 0; j--)
                {
                    if (currentLine(ind).ToCharArray()[j] == '*')
                    {
                        i++;
                    }
                    else if (currentLine(ind).ToCharArray()[j] != '&')
                    {
                        break;
                    }
                }
            }
        }
        return ind;
    }

    private int findTheEndOption(int ind)
    {
        int i = 0;
        while (i < 1)
        {
            ind++;
            if (firstChar(ind) == '&') i--;
            else if (lastChar(ind) == '*' || lastChar(ind) == '&')
            {
                for (int j = currentLine(ind).Length - 1; j > 0; j--)
                {
                    if (currentLine(ind).ToCharArray()[j] == '&')
                    {
                        i++;
                    }
                    else if (currentLine(ind).ToCharArray()[j] != '*')
                    {
                        break;
                    }
                }
            }
        }
        return ind;
    }

    private string currentLine(int ind)
    {
        return text[ind].TrimStart('\t').TrimEnd();
    }

    private void changeStage(int questId, int result)
    {
        Debug.Log("id" + questId + " stage " + result);
        QuestManager.Instance.Quests[questId].SetCurrentStage(result);
        index++;
    }

    private void changeScale(string name, int delta)
    {
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
        index++;
    }

    private void outTextLine(int ind)
    {
        dialogueText.text = text[ind].Trim().Replace("*", "").Replace("&", "").Replace("#", "\n");
        if (dialogueText.text == "")
        {
            mustMoveForward = true;
        }
        index++;
    }

    private char firstChar(int ind)
    {
        return currentLine(ind).ToCharArray()[0];
    }

    private char lastChar(int ind)
    {
        return currentLine(ind).ToCharArray()[currentLine(ind).Length - 1];
    }

    public void startDialogue()
    {
        DialogueManager.Instance.Show();
        isDialogueOn = true;
        index = 0;
        if (TryGetComponent(out QuestIdentifier identifier))
        {
            reactToDialogueStart(identifier.chooseDialogue());
        }
        else
        {
            reactToDialogueStart("standard;normal");
        }
    }

    private void reactToDialogueStart(string dialogueId)
    {
        if (currentLine(index).Replace("%", "") == dialogueId)
        {
            index++;
            readLine();
        }
        else
        {
            index = findTheEndDialogue(index);
            index++;
            reactToDialogueStart(dialogueId);
        }
    }

    private int findTheEndDialogue(int ind)
    {
        while (!isCurrentLineEmpty(ind))
        {
            ind++;
        }
        return ind;
    }

    public void endDialogue()
    {
        DialogueManager.Instance.Hide();
        isDialogueOn = false;
        OnEndOfDialogue?.Invoke();
    }
}