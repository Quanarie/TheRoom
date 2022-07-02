using UnityEngine;
using TMPro;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    public GameObject Player;
    public CanvasGroup Fader;
    public GameObject Canvas;
    public GameObject DiaryUI;
    public TextMeshProUGUI DiaryTextLeft;
    public TextMeshProUGUI DiaryTextRight;
    public GameObject StatusButtonTick;
    public GameObject StatusButtonCross;
    public GameObject Exclamation;

    public bool isTransitioningDoor = false;

    public GameManager GameManager { get; private set; }

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
        GameManager = GetComponent<GameManager>();
        Fader.alpha = 1;
    }
}
