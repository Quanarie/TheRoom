using UnityEngine;

public class QuestsOnThisLevel : MonoBehaviour
{
    [SerializeField] private int[] levels;
    [SerializeField] private int[] stages;

    public int[] GetLevels() => levels;
    public int[] GetStages() => stages;

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
