using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Dialogue : MonoBehaviour
{
    public delegate void EndOfDialogue();
    public event EndOfDialogue OnEndOfDialogue;

    public delegate void ChoicesCreated();
    public event ChoicesCreated OnChoicesCreated;

    public delegate void ChoicePressed();
    public event ChoicePressed OnChoicePressed;

    public TextAsset story;

    private List<string> text;
    private int index;

    public bool isDialogueOn = false;
    private bool mustMoveForward = false;

    private TextMeshProUGUI dialogueText;

    private Stack<int> endsOfChoice = new();
    private Stack<int> endsOfOption = new();

    private const string choiceSign = "&";
    private const char choiceSignChar = '&';
    private const string pleasure = "Pleasure";
    private const string anxiety = "Anxiety";
    private const string realistic = "Realistic";

    public void Start()
    {
        dialogueText = DialogueManager.Instance.DialogueText.GetComponent<TextMeshProUGUI>();
        text = story.text.Split("\n").ToList();
        text = formatText(text);
    }

    private List<string> formatText(List<string> list)
    {
        for (int k = 0; k < list.Count; k++)
        {
            if (list[k].Contains(choiceSignChar) || list[k].Contains('*') || list[k].Contains('%') || list[k].Contains('/') || list[k].Contains('^'))
                continue;
            if (list[k] == "")
                continue;

            char[] chars = list[k].ToCharArray();
            list.Remove(list[k]);

            int j = 0;
            int lines = 0;
            string newLine = "";
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '#' || i == chars.Length - 1)
                {
                    int tempLines = Mathf.CeilToInt((i - j - 1) / DialogueManager.Instance.GetMaximumSymbolsInRow());
                    if (lines + tempLines <= DialogueManager.Instance.GetMaximumRows())
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
                        list.Insert(k, newLine);
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
            list.Insert(k, newLine);
            k++;
        }
        return list;
    }

    private void Update()
    {
        if (DialogueManager.Instance.IsChoiceActive() || index >= text.Count) return;

        if (!isLineEmpty(index) && isDialogueOn && endsOfChoice.Count == 0)
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

        if (isLineEmpty(index) && isDialogueOn)
        {
            if (InputManager.Instance.GetInteractionPressed() || mustMoveForward)
            {
                mustMoveForward = false;
                endDialogue();
            }
        }
    }

    private bool isLineEmpty(int ind)
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
            parameters[1] = parameters[1].Replace("+", "").Replace(choiceSign, "").Replace("*", "");
            changeScale(parameters[0], int.Parse(parameters[1]));
            mustMoveForward = true;

            if (!isLineEmpty(index) && isSpecial(firstChar(index))) readLine();
        }
        else if (firstChar(index) == '^')
        {
            string[] parameters = currentLine(index).Replace("^", "").Split(";");
            parameters[1] = parameters[1].Replace(choiceSign, "").Replace("*", "");
            changeStage(int.Parse(parameters[0]), int.Parse(parameters[1]));
            mustMoveForward = true;

            if (!isLineEmpty(index) && isSpecial(firstChar(index))) readLine();
        }
        else if (firstChar(index) == '@')
        {
            Diary.Instance.AddAchievement(text[index].Replace("@", "").Replace("*", "").Replace(choiceSign, "").Trim());
            index++;

            if (!isLineEmpty(index) && isSpecial(firstChar(index))) readLine();
        }
        else outTextLine(index);
    } 

    private bool isSpecial(char c)
    {
        return c == '/' || c == '^' || c == '@';
    }

    private void displayChoices()
    {
        string[] line = currentLine(index).Split(";");
        line[0] = line[0].Replace("*", "");
        List<Button> choices = DialogueManager.Instance.ShowChoices(line.Length);

        for (int i = 0; i < choices.Count; i++)
        {
            choices[i].gameObject.SetActive(true);
            choices[i].GetComponentInChildren<TextMeshProUGUI>().text = line[i];
            int capturedi = i;
            choices[i].onClick.AddListener(() => startChoices(line[capturedi]));
        }

        DialogueManager.Instance.RandomizeChoices();
        OnChoicesCreated?.Invoke();

        index++; // from * to first &
        endsOfChoice.Push(findTheEndChoice(index));
    }

    private void startChoices(string choice)
    {
        OnChoicePressed?.Invoke();
        DialogueManager.Instance.HideChoices();
        reactToChoice(choice);
        readLine();
    }

    private void reactToChoice(string choice)
    {
        if (currentLine(index).Replace(choiceSign, "") == choice)
        {
            int endOfOption = findTheEndOption(index);
            endsOfOption.Push(endOfOption);
            index++;
        }
        else
        {
            index = findTheEndOption(index);
            index++; // after option
            reactToChoice(choice);
        }
    }

    private int findTheEndChoice(int ind)
    {
        int i = 0;
        while (i < 1)
        {
            ind++;
            if (firstChar(ind) == '*') i--;
            else if (lastChar(ind) == '*' || lastChar(ind) == choiceSignChar)
            {
                for (int j = currentLine(ind).Length - 1; j > 0; j--)
                {
                    if (currentLine(ind).ToCharArray()[j] == '*')
                    {
                        i++;
                    }
                    else if (currentLine(ind).ToCharArray()[j] != choiceSignChar)
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
            if (firstChar(ind) == choiceSignChar) i--;
            else if (lastChar(ind) == '*' || lastChar(ind) == choiceSignChar)
            {
                for (int j = currentLine(ind).Length - 1; j > 0; j--)
                {
                    if (currentLine(ind).ToCharArray()[j] == choiceSignChar)
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
        QuestManager.Instance.SetStage(questId, result);
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
        dialogueText.text = text[ind].Trim().Replace("*", "").Replace(choiceSign, "").Replace("#", "\n");
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

    public void startDialogue(string questIdentifier) // for situations
    {
        isDialogueOn = true;
        index = 0;
        reactToDialogueStart(questIdentifier);
    }

    private void reactToDialogueStart(string dialogueId)
    {
        if (currentLine(index).Replace("%", "") == dialogueId)
        {
            DialogueManager.Instance.Show();
            index++;
            readLine();
        }
        else
        {
            index = findTheEndDialogue(index);
            index++;
            if (index < text.Count)
            {
                reactToDialogueStart(dialogueId);
            }
            else
            {
                Debug.LogWarning("Did not find a dialogue for " + gameObject.name);
            }
        }
    }

    private int findTheEndDialogue(int ind)
    {
        while (!isLineEmpty(ind))
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