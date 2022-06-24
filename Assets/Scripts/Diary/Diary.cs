using System;
using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour
{
    public static Diary Instance { get; private set; }

    [SerializeField] private GameObject DiaryUI;

    private List<DiaryAchievement> achievements = new();

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

    public void AddAchievement(string newLine)
    {
        string[] parts = newLine.Split(";");

        if (parts[1] == "new")
        {
            DiaryAchievement newAchievement = new DiaryAchievement(int.Parse(parts[0]), parts[2]);
            achievements.Add(newAchievement);
        }
        else if (parts[1] == "changeDescription")
        {
            int index = findAchievement(int.Parse(parts[0]), parts[2]);
            achievements[index].description = parts[3];
        }
        else if (parts[1] == "changeStatus")
        {
            int index = findAchievement(int.Parse(parts[0]), parts[2]);
            achievements[index].status = Enum.Parse<AchievementStatus>(parts[3]);
        }
    }

    private int findAchievement(int level, string name)
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i].name == name && achievements[i].level == level)
            {
                return i;
            }
        }

        return -1;
    }

    public void Show()
    {
        DiaryUI.SetActive(true);
    }

    public void Hide()
    {
        DiaryUI.SetActive(false);
    }

    public bool IsDiaryOnScreen() => DiaryUI.activeSelf;
}

public enum AchievementStatus 
{
    Inprocess,
    Completed,
    Failed,
    Denied
}   