using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private string[] questLines;
    [SerializeField] private string[] regularLines;
    [SerializeField] private float speed;

    private TextMeshProUGUI text;
    private GameObject dialogueBox;

    private int index;
    private bool isPlayerInside;
    private bool isPrintingQuestLines;

    private IQuestElement questElement;

    private void Start()
    {
        text = Globals.Instance.DialogueText.GetComponent<TextMeshProUGUI>();
        dialogueBox = Globals.Instance.DialogueBox;

        questElement = GetComponent<IQuestElement>();
    }

    private void Update()
    {
        if (isPrintingQuestLines) printLines(questLines);
        else printLines(regularLines);
    }

    private void printLines(string[] lines)
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPlayerInside)
        {
            if (text.text == lines[index])
            {
                nextLine(lines);
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.Space) && !isPlayerInside) // isPlayerInside check so it does not call startDialogue() multiple times while inside
        {
            text.text = string.Empty;
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
        index = 0;
        if (questElement.StageBegin())
        {
            dialogueBox.SetActive(true);
            isPrintingQuestLines = true;
            StartCoroutine(typeLine(questLines));
        }
        else
        {
            dialogueBox.SetActive(true);
            isPrintingQuestLines = false;
            StartCoroutine(typeLine(regularLines));
        }
    }

    private void nextLine(string[] lines)
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(typeLine(lines));
        }
        else 
        {
            endStage();
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

    private IEnumerator typeLine(string[] lines)
    {
        foreach(char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(speed);
        }
    }
}
