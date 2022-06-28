using UnityEngine;

public class QuestManager : MonoBehaviour, ISaveable
{
    public static QuestManager Instance { get; private set; }

    public Quest[] Quests;

    private const int questsQuantity = 50;

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

        Quests = new Quest[questsQuantity];
        for (int i = 0; i < questsQuantity; i++)
        {
            Quests[i] = new Quest(i);
        }
    }

    public object CaptureState()
    {
        return Quests;
    }

    public void RestoreState(object state)
    {
        Quests = (Quest[])state;
    }
}
