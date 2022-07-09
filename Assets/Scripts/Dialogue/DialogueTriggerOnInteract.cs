using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerOnInteract : DialogueTrigger
{
    protected override void OnInteraction()
    {
        if (Vector3.Distance(Globals.Instance.Player.transform.position, transform.position) <= distance && !DialogueManager.Instance.IsDialogueOn())
        {
            dialogue.startDialogue();
            InputManager.Instance.GetInteractionPressed(); // so it doesnt skip the first line
        }
    }
}   
