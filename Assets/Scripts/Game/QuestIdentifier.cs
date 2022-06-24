using UnityEngine;

public class QuestIdentifier : MonoBehaviour
{
    [SerializeField] private int[] id;
    [SerializeField] private int[] stage;

    public string chooseDialogue()
    {
        for (int i = 0; i < id.Length; i++)
        {
            if (QuestManager.Instance.Quests[id[i]].GetCurrentStage() == stage[i])
            {
                return "quest;" + id[i] + ";" + stage[i];
            }
        }
        return "standard;normal";
    }

    public bool isCompletedAll()
    {
        for (int i = 0; i < id.Length; i++)
        {
            if (QuestManager.Instance.Quests[id[i]].GetCurrentStage() <= stage[i])
            {
                return false;
            }
        }
        return true;
    }
}
