using UnityEngine;

public class SparedObjects : MonoBehaviour
{
    public static SparedObjects Instance { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            SavingSystem.Instance.Load(PlayerPrefs.GetString("currentLoad", "Autoload.txt"));
            DontDestroyOnLoad(gameObject);
        }
    }
}
