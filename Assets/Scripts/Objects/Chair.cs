using UnityEngine;

public class Chair : MonoBehaviour
{
    private void Start()
    {
        SavingSystem.Instance.OnLevelLoaded += increaseStage;
    }

    private void increaseStage()
    {
        QuestManager.Instance.Quests[1].SetCurrentStage(QuestManager.Instance.Quests[1].GetCurrentStage() + 1); // chair quest
    }
}
