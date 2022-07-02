using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestsOnThisLevel : MonoBehaviour
{
    [SerializeField] private TwoDArray[] quests;
    [SerializeField] private ThreeDArray[] stages;

    public int[] GetQuests() => quests[PlayerPrefs.GetInt("currentLevel")].array;
    public TwoDArray[] GetStages() => stages[PlayerPrefs.GetInt("currentLevel")].array;

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

[System.Serializable]
public class ThreeDArray
{
    public TwoDArray[] array;
}
