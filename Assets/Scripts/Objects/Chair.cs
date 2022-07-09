using UnityEngine.SceneManagement;
using UnityEngine;

public class Chair : DialogueTrigger
{
    protected override void Start()
    {
        base.Start();
        QuestManager.Instance.SetStage(1, PlayerPrefs.GetInt("currentLevel"));
    }

    protected override void OnInteraction()
    {
        if (Vector3.Distance(Globals.Instance.Player.transform.position, transform.position) <= distance && !DialogueManager.Instance.IsDialogueOn())
        {
            dialogue.startDialogue();
            InputManager.Instance.GetInteractionPressed(); // so it doesnt skip the first line
        }
    }
}
