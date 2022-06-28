using UnityEngine;
using UnityEngine.UI;

public class WakeUp : DialogueTrigger
{
    [SerializeField] private int id;
    [SerializeField] private int stage;

    protected override void Start()
    {
        if (QuestManager.Instance.Quests[id].GetCurrentStage() != stage) Destroy(gameObject);
        else
        {
            base.Start();
            dialogue.Start();
            dialogue.startDialogue();
            dialogue.OnEndOfDialogue += endSituation;

            Globals.Instance.Canvas.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);

        }
    }

    private void endSituation()
    {
        Destroy(Globals.Instance.Canvas.GetComponent<Image>());
    }
}