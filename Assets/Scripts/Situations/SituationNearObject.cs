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
        if (collision.CompareTag("Player") && !dialogue.isDialogueOn)
        {
            dialogue.startDialogue();
        }
    }

    private void destroy()
    {
        Destroy(gameObject);
    }
}
