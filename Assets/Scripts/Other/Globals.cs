using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
