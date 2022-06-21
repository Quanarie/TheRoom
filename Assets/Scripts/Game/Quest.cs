using UnityEngine;

public class Quest : MonoBehaviour, IQuestElement
{
    [SerializeField] private int id;
    [SerializeField] private int nextStage;

    public int GetId() => id;

    private int stage = 0;

    private void Start()
    {
        QuestManager.Instance.InitializeQuest(this);
    }

    public int GetCurrentStage() => stage;

    public void SetCurrentStage(int value)
    {
        stage = value;
    }

    public bool IsQuestComplete() => stage == 100; //quest completed

    public bool StageBegin()
    {
        if (stage == 0 || stage == 1) // 0 - not started, 1 - just started(0 stages done)
        {
            stage = 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StageComplete() // 0 stage completed (not the whole quest)
    {
        if (stage == 1)
        {
            stage = nextStage;
        }
    }
}
