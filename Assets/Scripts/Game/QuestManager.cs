using UnityEngine;

public class QuestManager : MonoBehaviour, ISaveable
{
    public static QuestManager Instance { get; private set; }

    public delegate void QuestStageChanged();
    public event QuestStageChanged OnQuestStageChanged;

    public Quest[] Quests;

    public int GetStage(int index) => Quests[index].GetCurrentStage();
    public void SetStage(int index, int stage)
    {
        Quests[index].SetCurrentStage(stage);
        OnQuestStageChanged?.Invoke();
    }
    public void SetStageQuite(int index, int stage)
    {
        Quests[index].SetCurrentStage(stage);
    }
    public void ChangeStage(int index, int delta)
    {
        Quests[index].ChangeCurrentStage(delta);
        OnQuestStageChanged?.Invoke();
    }

    private const int questsQuantity = 50;
    private bool wasLoaded = false;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (!wasLoaded)
        {
            Quests = new Quest[questsQuantity];
            for (int i = 0; i < questsQuantity; i++)
            {
                Quests[i] = new Quest(i);
            }
        }
    }

    public object CaptureState()
    {
        return Quests;
    }

    public void RestoreState(object state)
    {
        Quests = (Quest[])state;
        wasLoaded = true;
    }
}
