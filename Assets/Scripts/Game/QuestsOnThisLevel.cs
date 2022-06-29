using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestsOnThisLevel : MonoBehaviour
{
    [SerializeField] private int[] quests;
    [SerializeField] private TwoDArray[] stages;

    public int[] GetQuests() => quests;
    public TwoDArray[] GetStages() => stages;

    public int GetCurrentLevel()
    {
        string name = SceneManager.GetActiveScene().name;
        string lastLetter = name[name.Length - 1].ToString();
        int level = int.Parse(lastLetter);
        return level;
    }

    public static QuestsOnThisLevel Instance { get; private set; }

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
}

[System.Serializable]
public class TwoDArray
{
    public int[] array;
}
