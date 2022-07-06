using UnityEngine;

[System.Serializable]
public class Situation
{
    public string name;
    public int lvl;
    [SerializeField] private int pleasureNeeded;
    [SerializeField] private int anxietyNeeded;
    [SerializeField] private int realisticNeeded;
    public bool isOneTime;
    [SerializeField] private int[] quests;
    [SerializeField] private TwoDArray[] stages;
    public SerializableVector3 place;
    [SerializeField] private float distanceToThis;

    [HideInInspector] public bool isDone = false;

    public bool CheckScales()
    {
        if (pleasureNeeded != -1 && Scales.Instance.PleasureScale < pleasureNeeded) return false;
        if (anxietyNeeded != -1 && Scales.Instance.AnxietyScale < anxietyNeeded) return false;
        if (realisticNeeded != -1 && Scales.Instance.RealisticScale < realisticNeeded) return false;

        return true;
    }

    public bool CheckDistance()
    {
        if (distanceToThis == -1) return true;
        else if (Vector3.Distance(new Vector3(place.x, place.y, place.z),
            Globals.Instance.Player.transform.position) <= distanceToThis) return true;

        return false;
    }

    public bool CheckQuests()
    {
        if (quests.Length == 0) return true;

        for (int i = 0; i < quests.Length; i++)
        {
            for (int j = 0; j < stages[i].array.Length; j++)
            {
                if (QuestManager.Instance.GetStage(quests[i]) == stages[i].array[j])
                {
                    return true;
                }
            }
        }
        return false;
    }
}

[System.Serializable]
public class SerializableVector3
{
    public float x;
    public float y;
    public float z;
}