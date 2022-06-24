using UnityEngine;

public class SituationNearObject : DialogueTrigger
{
    protected override void Start()
    {
        base.Start();
        dialogue.OnEndOfDialogue += destroy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !DialogueManager.Instance.IsDialogueOn())
        {
            dialogue.startDialogue();
        }
    }

    private void destroy()
    {
        Destroy(gameObject);
    }
}
