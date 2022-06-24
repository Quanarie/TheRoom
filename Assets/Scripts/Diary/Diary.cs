using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Diary : MonoBehaviour
{
    public static Diary Instance { get; private set; }

    [SerializeField] private GameObject DiaryUI;
    [SerializeField] private TextMeshProUGUI diaryTextLeft;
    [SerializeField] private TextMeshProUGUI diaryTextRight;
    [SerializeField] private int maxSymbolsInRow;
    [SerializeField] private int maxRows;

    private List<string> leftPages = new();
    private List<string> rightPages = new();

    private int currentPage = 0;

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
        DiaryUI.SetActive(false);
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

    public void NextPage()
    {
        if (currentPage + 1 < leftPages.Count)
        {
            currentPage++;
            displayPages(currentPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage - 1 >= 0)
        {
            currentPage--;
            displayPages(currentPage);
        }
    }

    public void Show()
    {
        DiaryUI.SetActive(true);

        string fullText = "";
        foreach (DiaryAchievement achievement in achievements)
        {
            fullText += achievement.name + "\n" + achievement.description + "\n";
        }

        List<string> pages = formatText(fullText);
        leftPages.Clear();
        rightPages.Clear();
        currentPage = 0;
        
        for (int i = 0; i < pages.Count; i++)
        {
            if (i % 2 == 0) // left page
            {
                leftPages.Add(pages[i]);
            }
            else
            {
                rightPages.Add(pages[i]);
            }
        }

        displayPages(currentPage); // level - 1 indstead of 0
    }

    private List<string> formatText(string text)
    {
        List<string> list = new();
        int j = 0;
        int lines = 0;
        string newLine = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '.' || i == text.Length - 1)
            {
                int tempLines;
                if (text[i] == '\n') // it counts \n as one symbol, however it is a quanity of symbols from i to the end of row
                {
                    int tempI = maxSymbolsInRow * Mathf.FloorToInt(((float)i / maxSymbolsInRow));
                    int delta = maxSymbolsInRow - (i - tempI);
                    tempLines = Mathf.CeilToInt((float)(i - j - 1 + delta) / maxSymbolsInRow);
                }
                else
                {
                    tempLines = Mathf.CeilToInt((float)(i - j - 1) / maxSymbolsInRow);
                }
                if (lines + tempLines <= maxRows)
                {
                    lines += tempLines;
                    for (; j < i; j++)
                    {
                        newLine += text[j];
                    }
                }
                else
                {
                    lines = tempLines;
                    list.Add(newLine.Trim());
                    newLine = "";
                    j++;
                    for (; j < i; j++)
                    {
                        newLine += text[j];
                    }
                }
            }
        }
        list.Add(newLine);

        return list;
    }

    private void displayPages(int index)
    {
        diaryTextLeft.text = leftPages[index];
        if (index < rightPages.Count)
        {
            diaryTextRight.text = rightPages[index];
        }
        else
        {
            diaryTextRight.text = "";
        }
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