using System.Collections.Generic;
using UnityEngine;

public class SituationManager : MonoBehaviour, ISaveable
{
    public static SituationManager Instance { get; private set; }

    [SerializeField] private TextAsset story;
    [SerializeField] private Situation[] Situations;

    private Dictionary<string, GameObject> dict = new();

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

    private void Update()
    {
        foreach(Situation s in Situations)
        {
            if (PlayerPrefs.GetInt("currentLevel") < s.lvl) continue;

            bool scales = s.CheckScales();
            bool distance = s.CheckDistance();
            bool quests = s.CheckQuests();

            if (scales && quests && !s.isDone && !distance)
            {
                if (!dict.ContainsKey(s.name))
                {
                    dict[s.name] = Instantiate(Globals.Instance.Exclamation, new Vector3(s.place.x,
                    s.place.y + 1f, s.place.z), Quaternion.identity, transform);
                }
            }
            else if (dict.ContainsKey(s.name) && dict[s.name] != null)
            {
                Destroy(dict[s.name]);
            }

            if (scales && distance && quests && !s.isDone && !TryGetComponent(out Dialogue _))
            {
                Dialogue dialogue = gameObject.AddComponent<Dialogue>();
                dialogue.story = story;
                dialogue.Start();
                dialogue.startDialogue("situation;" + s.name);

                if (s.isOneTime == true)
                {
                    dialogue.OnEndOfDialogue += () => s.isDone = true;
                    dialogue.OnEndOfDialogue += () => Destroy(dialogue);
                }
            }
        }
    }

    public object CaptureState()
    {
        return Situations;
    }

    public void RestoreState(object state)
    {
        Situations = (Situation[])state;

    }
}
