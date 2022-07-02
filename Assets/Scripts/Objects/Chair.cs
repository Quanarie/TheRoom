using UnityEngine.SceneManagement;
using UnityEngine;

public class Chair : DialogueTrigger
{
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputManager.Instance.GetInteractionPressed() && !DialogueManager.Instance.IsDialogueOn())
        {
            if (GetComponent<QuestIdentifier>().chooseDialogue().Split(";")[0] == "quest")
            {
                dialogue.startDialogue();
            }
            else
            {
                dialogue.startDialogue("quest;1;" + PlayerPrefs.GetInt("currentLevel").ToString());
            }
        }
    }
}
