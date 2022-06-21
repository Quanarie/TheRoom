using UnityEngine;

public class Stage : MonoBehaviour, IQuestElement
{
    [SerializeField] private int id;
    [SerializeField] private int stageNumber;

    public bool Begin()
    {
        int currentStage = QuestManager.Instance.Quests[id].GetCurrentStage();
        if (currentStage == stageNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Complete()
    {
        int currentStage = QuestManager.Instance.Quests[id].GetCurrentStage();
        if (currentStage == stageNumber)
        {
            QuestManager.Instance.Quests[id].MarkStageComplete(stageNumber);
        }
    }
}
