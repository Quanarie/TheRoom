using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerAndDiaryActivation : DialogueTriggerOnInteract
{
    protected override void Start()
    {
        base.Start();
        dialogue.OnEndOfDialogue += Diary.Instance.Show;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (InputManager.Instance.GetInteractionPressed())
        {
            if (identifier.chooseDialogue().Split(";")[0] == "standard")
            {
                if (collision.CompareTag("Player") && !Diary.Instance.IsDiaryOnScreen())
                {
                    Diary.Instance.Show();
                }
            }
            else
            {
                if (collision.CompareTag("Player") && !DialogueManager.Instance.IsDialogueOn())
                {
                    dialogue.startDialogue();
                }
            }
        }
    }
}
