using UnityEngine;

public class Situation : MonoBehaviour, ISaveable
{
    [SerializeField] private int pleasureNeeded;
    [SerializeField] private int anxietyNeeded;
    [SerializeField] private int realisticNeeded;
    [SerializeField] private bool isOneTime;
    [SerializeField] private int[] quests;
    [SerializeField] private TwoDArray[] stages;
    [SerializeField] private float distanceToThis;
    [SerializeField] private TextAsset story;

    private Dialogue dialogue;
    private GameObject exclamationPoint;
    private bool isDone = false;

    private void Update()
    {
        bool scales = CheckScales();
        bool distance = CheckDistance();
        bool quests = CheckQuests();

        if (scales && quests && !isDone && !distance)
        {
            if (exclamationPoint == null)
            {
               exclamationPoint = Instantiate(Globals.Instance.Exclamation, new Vector3(transform.position.x,
               transform.position.y + 1f, transform.position.z), Quaternion.identity, transform);
            }
        }

        if (scales && distance && quests && !isDone)
        {
            dialogue = gameObject.AddComponent<Dialogue>();
            dialogue.story = story;
            dialogue.Start();
            dialogue.startDialogue("situation;" + gameObject.name);
            dialogue.OnEndOfDialogue += destroyExclamation;

            if (isOneTime == true)
            {
                isDone = true;
            }
        }
    }

    private void destroyExclamation()
    {
        Destroy(exclamationPoint);
    }

    private bool CheckScales()
    {
        if (pleasureNeeded != -1 && Scales.Instance.PleasureScale < pleasureNeeded) return false;
        if (anxietyNeeded != -1 && Scales.Instance.AnxietyScale < anxietyNeeded) return false;
        if (realisticNeeded != -1 && Scales.Instance.RealisticScale < realisticNeeded) return false;

        return true;
    }

    private bool CheckDistance()
    {
        if (distanceToThis == -1) return true;
        else if (Vector3.Distance(transform.position,
            Globals.Instance.Player.transform.position) <= distanceToThis) return true;

        return false;
    }

    private bool CheckQuests()
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

    public object CaptureState()
    {
        return isDone;
    }

    public void RestoreState(object state)
    {
        isDone = (bool)state;
    }
}