using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] private int id;
    [SerializeField] private int stage = 0;

    public Quest(int questId)
    {
        id = questId;
    }

    public void SetId(int value) => id = value;

    public int GetId() => id;

    public int GetCurrentStage() => stage;

    public void SetCurrentStage(int value) => stage = value;

    public bool IsQuestComplete() => stage == 100;
}
