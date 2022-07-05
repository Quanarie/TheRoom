using UnityEngine;

public class Scales : MonoBehaviour, ISaveable
{
    public static Scales Instance { get; private set; }

    public int PleasureScale;
    public int AnxietyScale;
    public int RealisticScale;

    public void AddPleasure(int value) => PleasureScale += value;
    public void AddAnxiety(int value) => AnxietyScale += value;
    public void AddRealistic(int value) => RealisticScale += value;

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

    public object CaptureState()
    {
        int[] scales = new int[3]
        {
            PleasureScale,
            AnxietyScale,
            RealisticScale
    };
        return scales;
    }

    public void RestoreState(object state)
    {
        PleasureScale = ((int[])state)[0];
        AnxietyScale = ((int[])state)[1];
        RealisticScale = ((int[])state)[2];
    }
}