using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDialogueTrigger : DialogueTrigger
{
    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputManager.Instance.GetInteractionPressed() && !DialogueManager.Instance.IsDialogueOn())
        {
            subscribe();
            dialogue.startDialogue();
        }
    }

    private void tryDisableButtons()
    {
        if (LevelData.Instance.GetEnergy() == 0)
        {
            List<Button> choices = DialogueManager.Instance.GetChoices();
            foreach (Button choice in choices)
            {
                choice.onClick.RemoveAllListeners();
            }
        }
        addExitChoice();
    }

    private void onChoiceChosen()
    {
        LevelData.Instance.ChangeEnergy(-1);
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
        dialogue.OnChoicesCreated += tryDisableButtons;
        dialogue.OnChoicePressed += onChoiceChosen;
    }

    private void unSubscribe()
    {
        dialogue.OnEndOfDialogue -= startNextDialogue;
        dialogue.OnChoicesCreated -= tryDisableButtons;
        dialogue.OnChoicePressed -= onChoiceChosen;
    }
}