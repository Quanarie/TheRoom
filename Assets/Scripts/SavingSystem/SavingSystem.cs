using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public delegate void Loaded();
    public event Loaded OnLoaded;

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

    public void Save(string fileName)
    {
        string path = getPathToTheFile(fileName);
        using FileStream stream = File.Open(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, CaptureState());
    }

    public void Load(string fileName)
    {
        string path = getPathToTheFile(fileName);
        if (File.Exists(path))
        {
            using FileStream stream = File.Open(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            if (stream.Length != 0)
            {
                RestoreState(formatter.Deserialize(stream));
                OnLoaded?.Invoke();
            }
        }
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

    private string getPathToTheFile(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
