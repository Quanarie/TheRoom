using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour, ISaveable
{
    public static LevelData Instance { get; private set; }

    [SerializeField] private int maxEnergy = 5;
    public int energy;

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

    public int GetEnergy() => energy;
    public void ChangeEnergy(int delta)
    {
        energy += delta;
        energy = Mathf.Clamp(energy, 0, maxEnergy);
    }

    private void Start()
    {
        energy = maxEnergy;
    }

    public object CaptureState()
    {
        return energy;
    }

    public void RestoreState(object state)
    {
        energy = (int)state;
    }
}
