using UnityEngine;

public class SituationNearObject : DialogueTrigger
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (identifier.chooseDialogue().Split(";")[0] == "standard") Destroy(gameObject);
        else
        {
            if (collision.CompareTag("Player") && !DialogueManager.Instance.IsDialogueOn())
            {
                dialogue.startDialogue();
            }
        }
    }
}
