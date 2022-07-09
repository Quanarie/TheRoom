using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset story;
    [SerializeField] protected float distance;

    protected Dialogue dialogue;

    protected GameObject exclamationPoint;

    protected virtual void Start()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.story = story;
        dialogue.Start();
        QuestManager.Instance.OnQuestStageChanged += tryShowExclamation;
        tryShowExclamation();
        InputManager.Instance.OnInteractionPressed += OnInteraction;
    }

    protected virtual void tryShowExclamation()
    {
        if (dialogue.isThereAQuestDialogue() != -1)
        {
            if (exclamationPoint == null)
            {
                exclamationPoint = Instantiate(Globals.Instance.Exclamation, new Vector3(transform.position.x,
                transform.position.y + 1f, transform.position.z), Quaternion.identity, transform);
            }
        }
        else if (exclamationPoint != null)
        {
            Destroy(exclamationPoint);
        }
    }

    protected virtual void OnInteraction() { }

    private void OnDestroy()
    {
        InputManager.Instance.OnInteractionPressed -= OnInteraction;
        QuestManager.Instance.OnQuestStageChanged -= tryShowExclamation;
    }

    public Dialogue GetDialogue() => dialogue;
}
