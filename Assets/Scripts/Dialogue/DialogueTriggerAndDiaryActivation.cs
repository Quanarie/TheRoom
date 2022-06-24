using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerAndDiaryActivation : DialogueTriggerOnInteract
{
    [SerializeField] private GameObject nextObject;

    protected override void Start()
    {
        base.Start();
        nextObject.SetActive(false);
        dialogue.OnEndOfDialogue += destroyCurrentDialogueAndActivateNext;
    }

    private void destroyCurrentDialogueAndActivateNext()
    {
        if (GetComponent<QuestIdentifier>().isCompletedAll())
        {
            nextObject.SetActive(true);
            Diary.Instance.Show();
            Destroy(dialogue);
            Destroy(this);
        }
    }
}
