using UnityEngine;

public class Stage : MonoBehaviour, IQuestElement
{
    [SerializeField] private int id;
    [SerializeField] private int startStage;
    [SerializeField] private int nextStage;

    public bool StageBegin()
    {
        int currentStage = QuestManager.Instance.Quests[id].GetCurrentStage();
        if (currentStage == startStage)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StageComplete()
    {
        int currentStage = QuestManager.Instance.Quests[id].GetCurrentStage();
        if (currentStage == startStage)
        {
            QuestManager.Instance.Quests[id].SetCurrentStage(nextStage);
        }
    }
}
