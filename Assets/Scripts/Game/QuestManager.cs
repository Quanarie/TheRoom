using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public Quest[] Quests = new Quest[200];

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
    }

    public void InitializeQuest(Quest quest)
    {
        Quests[quest.GetId()] = quest;
    }
}
