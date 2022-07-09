using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDialogueTrigger : DialogueTrigger
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space) && !DialogueManager.Instance.IsDialogueOn())
        {
            subscribe();
            dialogue.startDialogue();
        }
    }

    private void addExitChoice()
    {
        Button choice = DialogueManager.Instance.AddChoice();
        choice.GetComponentInChildren<TextMeshProUGUI>().text = "Почекати";
        choice.onClick.AddListener(exitFromDialogue);
    }

    private void exitFromDialogue()
    {
        unSubscribe();
        dialogue.endDialogue();
    }

    private void startNextDialogue()
    {
        dialogue.startDialogue();
    }

    private void subscribe()
    {
        dialogue.OnEndOfDialogue += startNextDialogue;
        dialogue.OnChoicesCreated += addExitChoice;

    }

    private void unSubscribe()
    {
        dialogue.OnEndOfDialogue -= startNextDialogue;
        dialogue.OnChoicesCreated -= addExitChoice;
    }
}