using UnityEngine;
using TMPro;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    public GameObject Player;
    public GameObject Background;
    public GameObject Canvas;
    public GameObject DiaryUI;
    public TextMeshProUGUI DiaryTextLeft;
    public TextMeshProUGUI DiaryTextRight;
    public GameObject StatusButtonTick;
    public GameObject StatusButtonCross;

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

        Background.SetActive(false);
    }
}
