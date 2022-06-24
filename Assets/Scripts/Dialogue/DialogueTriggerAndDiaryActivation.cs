using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerAndDiaryActivation : DialogueTriggerOnInteract
{
    protected override void Start()
    {
        base.Start();
        dialogue.OnEndOfDialogue += destroyCurrentDialogueAndActivateNext;
        Diary.Instance.gameObject.SetActive(false);
    }

    private void destroyCurrentDialogueAndActivateNext()
    {
        if (GetComponent<QuestIdentifier>().isCompletedAll())
        {
            Diary.Instance.gameObject.SetActive(true);
            Diary.Instance.Show();
            Destroy(dialogue);
            Destroy(this);
        }
    }
}
