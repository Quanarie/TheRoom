using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    public GameObject Player;
    public GameObject DialogueBox;
    public GameObject DialogueText;


    private void Start()
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
