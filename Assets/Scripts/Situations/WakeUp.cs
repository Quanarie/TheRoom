using UnityEngine;
using UnityEngine.UI;

public class WakeUp : DialogueTrigger
{
    protected override void Start()
    {
        base.Start();
        dialogue.Start();
        dialogue.startDialogue();
        dialogue.OnEndOfDialogue += endSituation;

        Globals.Instance.Canvas.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
    }

    private void endSituation()
    {
        Destroy(Globals.Instance.Canvas.GetComponent<Image>());
    }
}