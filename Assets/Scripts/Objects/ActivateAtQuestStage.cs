using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAtQuestStage : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int stage;

    private void OnEnable() // onenable goes after Awake in which quests are created
    {
        foreach(Quest quest in QuestManager.Instance.Quests)
        {
            if (quest.GetId() == id && quest.GetCurrentStage() == stage)
            {
                quest.SetCurrentStage(quest.GetCurrentStage() + 1);
            }
        }
    }
}
