using UnityEngine;

public class Quest : MonoBehaviour, IQuestElement
{
    [SerializeField] private int id;
    [SerializeField] private int numberOfStages;

    public int GetId() => id;
    public int GetNumberOfStages() => numberOfStages;

    private bool[] stages;

    private void Start()
    {
        stages = new bool[numberOfStages];

        QuestManager.Instance.InitializeQuest(this);
    }

    public int GetCurrentStage()
    {
        int stage = 0;
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i] == true) stage++;
        }
        return stage;
    }

    public void MarkStageComplete(int stage)
    {
        stages[stage] = true;
    }

    public bool IsQuestComplete()
    {
        bool isComplete = true;
        for (int i = 0; i < stages.Length; i++)
        {
            if (stages[i] == false)
            {
                isComplete = false;
                break;
            }
        }
        return isComplete;
    }

    public bool Begin()
    {
        return !stages[0];
    }

    public void Complete()
    {
        stages[0] = true;
    }
}
