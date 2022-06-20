using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private string[] lines;
    [SerializeField] private float speed;

    private TextMeshProUGUI text;
    private GameObject dialogueBox;

    private int index;

    private void Start()
    {
        text = Globals.Instance.DialogueText.GetComponent<TextMeshProUGUI>();
        dialogueBox = Globals.Instance.DialogueBox;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (text.text == lines[index])
            {
                nextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.text = string.Empty;
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
        index = 0;
        dialogueBox.SetActive(true);
        StartCoroutine(typeLine());

    }

    private void nextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(typeLine());
        }
        else
        {
            endDialogue();
        }
    }

    private void endDialogue()
    {
        StopAllCoroutines();
        dialogueBox.SetActive(false);
    }

    private IEnumerator typeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(speed);
        }
    }
}
