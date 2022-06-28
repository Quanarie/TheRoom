using UnityEngine;

public class SituationNearObject : DialogueTrigger
{
    [SerializeField] private int id;
    [SerializeField] private int stage;

    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (QuestManager.Instance.Quests[id].GetCurrentStage() != stage) Destroy(gameObject);
        else
        {
            if (collision.CompareTag("Player") && !DialogueManager.Instance.IsDialogueOn())
            {
                dialogue.startDialogue();
            }
        }
    }
}
