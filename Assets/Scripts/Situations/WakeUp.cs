using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : DialogueTrigger
{
    protected override void Start()
    {
        base.Start();
        dialogue.Start();
        dialogue.startDialogue();
        dialogue.OnChoicePressed += setBGNotActive;
        Globals.Instance.Background.SetActive(true);
    }

    private void setBGNotActive()
    {
        Globals.Instance.Background.SetActive(false);
    }
}
