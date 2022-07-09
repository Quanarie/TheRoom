using UnityEngine;

public class DialogueTriggerAndDiaryActivation : DialogueTriggerOnInteract
{
    protected override void Start()
    {
        base.Start();
        dialogue.OnEndOfDialogue += Diary.Instance.Show;
    }

    protected override void OnInteraction()
    {
        if (!DialogueManager.Instance.IsDialogueOn() && Vector3.Distance(Globals.Instance.Player.transform.position, transform.position) <= distance)
        {
            if (dialogue.isThereAQuestDialogue() != -1)
            {
                if (!DialogueManager.Instance.IsDialogueOn())
                {
                    dialogue.startDialogue();
                    InputManager.Instance.GetInteractionPressed();
                }
            }
            else
            {
                if (!Diary.Instance.IsDiaryOnScreen())
                {
                    Diary.Instance.Show();
                }
            }
        }
    }
}
