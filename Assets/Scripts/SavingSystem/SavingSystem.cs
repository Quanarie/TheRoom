using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public static SavingSystem Instance { get; private set; }

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

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        string path = getPathToTheFile();
        using FileStream stream = File.Open(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, CaptureState());
    }

    public void Load()
    {
        string path = getPathToTheFile();
        try
        {
            FileStream stream = File.Open(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            RestoreState(formatter.Deserialize(stream));
        }
        catch { }
    }

    private object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        foreach (Saveable saveable in FindObjectsOfType<Saveable>())
        {
            state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
        }
        return state;
    }

    private void RestoreState(object state)
    {
        Dictionary<string, object> dict = (Dictionary<string, object>)state;
        foreach (Saveable saveable in FindObjectsOfType<Saveable>())
        {
            saveable.RestoreState(dict[saveable.GetUniqueIdentifier()]);
        }
    }

    private string getPathToTheFile()
    {
        return Path.Combine(Application.persistentDataPath, "data.txt");
    }
}
