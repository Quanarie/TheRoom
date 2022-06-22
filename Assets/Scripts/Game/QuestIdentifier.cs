using UnityEngine;

public class QuestIdentifier : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int stage;

    public bool canHaveDialogue() => QuestManager.Instance.Quests[id].GetCurrentStage() == stage;
}
