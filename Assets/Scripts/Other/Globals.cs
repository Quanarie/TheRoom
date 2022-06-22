using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    public GameObject Player;

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
