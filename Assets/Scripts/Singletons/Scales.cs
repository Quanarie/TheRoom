using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scales : MonoBehaviour
{
    public Scales Instance { get; private set; }

    public int PleasureScale { get; private set; }
    public int FearScale { get; private set; }
    public int RealismScale { get; private set; }

    public void IncrementPleasure() => PleasureScale += 1;
    public void IncrementFear() => FearScale += 1;
    public void IncrementRealism() => RealismScale += 1;

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