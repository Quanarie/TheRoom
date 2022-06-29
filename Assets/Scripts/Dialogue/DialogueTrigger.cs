using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset story;

    protected Dialogue dialogue;

    protected QuestIdentifier identifier;
    protected GameObject exclamationPoint;

    protected virtual void Start()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.story = story;
        TryGetComponent(out identifier);
    }

    private void Update()
    {
        if (identifier != null)
            tryShowExclamation();
    }

    private void tryShowExclamation()
    {
        if (identifier.chooseDialogue().Split(";")[0] == "quest")
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


    public Dialogue GetDialogue() => dialogue;
}
