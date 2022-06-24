using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    private void Start()
    {
        QuestManager.Instance.Quests[1].SetCurrentStage(QuestManager.Instance.Quests[1].GetCurrentStage() + 1);
    }
}
