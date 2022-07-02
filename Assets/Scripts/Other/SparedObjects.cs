using UnityEngine;

public class SparedObjects : MonoBehaviour
{
    public static SparedObjects Instance { get; private set; }

    private void OnEnable()
    {
        if (Instance)
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
