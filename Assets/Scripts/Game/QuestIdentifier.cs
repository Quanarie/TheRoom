using UnityEngine;

public class QuestIdentifier : MonoBehaviour
{
    [SerializeField] private string[] id;
    [SerializeField] private string[] stage;

    private const string orSign = "|";
    private const string andSign = "&";

    public string chooseDialogue()
    {
        for (int i = 0; i < id.Length; i++)
        {
            string[] ids = id[i].Split(orSign);
            string[] stages = stage[i].Split(orSign);

            for (int j = 0; j < ids.Length; j++)
            {
                if (checkQuests(ids[j].Replace("(", "").Replace(")", ""), stages[j].Replace("(", "").Replace(")", "")))
                {
                    return "quest;" + id[i] + ";" + stage[i];
                }
            }
        }
        return "standard;normal";
    }

    private bool checkQuests(string ids, string stages)
    {
        string[] eachId = ids.Split(andSign);
        string[] eachStage = stages.Split(andSign);

        for (int i = 0; i < eachId.Length; i++)
        {
            if (QuestManager.Instance.GetStage(int.Parse(eachId[i])) != int.Parse(eachStage[i]))
            {
                return false;
            }
        }
        return true;
    }

    /*if (QuestManager.Instance.GetStage(id[i]) == stage[i])
            {
                return "quest;" + id[i] + ";" + stage[i];
            }*/
}
