using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scales : MonoBehaviour
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
}