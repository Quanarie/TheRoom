using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset story;

    protected Dialogue dialogue;

    protected virtual void Start()
    {
        dialogue = gameObject.AddComponent<Dialogue>();
        dialogue.story = story;
    }
}
