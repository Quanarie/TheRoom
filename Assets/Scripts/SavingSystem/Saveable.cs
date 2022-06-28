using System.Collections.Generic;
using UnityEngine;

public class Saveable : MonoBehaviour
{
    public string GetUniqueIdentifier() => gameObject.name;

    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        foreach (ISaveable saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }
        return state;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> dict = (Dictionary<string, object>)state;
        foreach (ISaveable saveable in GetComponents<ISaveable>())
        {
            string typeString = saveable.GetType().ToString();
            if (dict.ContainsKey(typeString))
            {
                saveable.RestoreState(dict[typeString]);
            }
        }
    }
}
