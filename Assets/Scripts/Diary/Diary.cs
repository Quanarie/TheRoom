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
    [SerializeField] private GameObject statusButtonTick;
    [SerializeField] private GameObject statusButtonCross;
    [SerializeField] private int maxAchievementsOnPage;
    [SerializeField] private int maxSymbolsInRow;
    [SerializeField] private int maxSymbolsInDescription;

    private List<List<DiaryAchievement>> pages = new();

    private int currentPage = 0;

    private List<DiaryAchievement> achievements = new();
    private List<List<GameObject>> statuses = new();

    private Vector3 startPosStatusLeft = new Vector3(-45f, 323f, 0f);
    private Vector3 startPosStatusRight = new Vector3(527f, 323f, 0f);
    private const float deltaY = 35.44f;

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

    private void Update()
    {
        if (InputManager.Instance.GetDiaryCallerPressed())
        {
            if (IsDiaryOnScreen())
            {
                Hide();
            }
            else
            {
                Show();
            }

        }
        else if (IsDiaryOnScreen() && InputManager.Instance.GetInteractionPressed()) Hide();
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
        if (currentPage + 2 < pages.Count && pages[currentPage + 2].Count != 0)
        {
            DestroyStatuses();
            statuses[currentPage].Clear();

            currentPage += 2;
            displayPages(currentPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage - 2 >= 0)
        {
            DestroyStatuses();
            statuses[currentPage].Clear();

            currentPage -= 2;
            displayPages(currentPage);
        }
    }

    private void ClearTextes()
    {
        diaryTextRight.text = "";
        diaryTextLeft.text = "";
    }

    private void ClearText(int pageIndex)
    {
        if (pageIndex % 2 == 0)
            diaryTextLeft.text = "";
        else
            diaryTextRight.text = "";
    }

    public void Show()
    {
        if (DialogueManager.Instance.IsDialogueOn()) return;

        DiaryUI.SetActive(true);

        pages.Clear();
        ClearTextes();

        int achievementOnCurrentPage = 0;
        int current = 0;
        pages.Add(new List<DiaryAchievement>());
        statuses.Add(new List<GameObject>());

        for (int i = 1; ; i++) // level
        {
            bool isEmpty = true;
            for (int j = 0; j < achievements.Count; j++)
            {
                if (achievements[j].level == i)
                {
                    isEmpty = false;
                    if (achievementOnCurrentPage + 1 < maxAchievementsOnPage)
                    {
                        achievementOnCurrentPage++;
                    }
                    else
                    {
                        achievementOnCurrentPage = 1;
                        current++;
                        pages.Add(new List<DiaryAchievement>());
                        statuses.Add(new List<GameObject>());
                    }
                    pages[current].Add(achievements[j]);
                }
            }
            if (isEmpty) break;
            current++;
            pages.Add(new List<DiaryAchievement>());
            statuses.Add(new List<GameObject>());
        }

        if (achievements.Count > 0)
        {
            displayPages(0);
        }
    }

    public void DisplayDescription(int pageIndex, int row)
    {
        if (pageIndex % 2 == 0)
        {
            diaryTextLeft.text = "";
        }
        else
        {
            diaryTextRight.text = "";
        }

        for (int i = 0; i < pages[pageIndex].Count; i++)
        {
            if (i == row)
            {
                if (pageIndex % 2 == 0)
                {
                    diaryTextLeft.text += pages[pageIndex][i].name + "\n";
                    diaryTextLeft.text += pages[pageIndex][i].description + "\n";
                }
                else
                {
                    diaryTextRight.text += pages[pageIndex][i].name + "\n";
                    diaryTextRight.text += pages[pageIndex][i].description + "\n";
                }
            }
            else
            {
                if (pageIndex % 2 == 0)
                    diaryTextLeft.text += pages[pageIndex][i].name + "\n";
                else
                    diaryTextRight.text += pages[pageIndex][i].name + "\n";
            }
        }

        for (int i = 0; i < statuses[pageIndex].Count; i++)
        {
            if (statuses[pageIndex][i].GetComponent<StatusButton>().indexInList != row)
            {
                statuses[pageIndex][i].GetComponent<StatusButton>().isOpened = false;
            }

            Vector3 newPosition;
            int indexInList = statuses[pageIndex][i].GetComponent<StatusButton>().indexInList;
            int addedLines = 0;
            if (statuses[pageIndex][i].GetComponent<StatusButton>().indexInList > row)
            {
                addedLines = Mathf.CeilToInt((float)pages[pageIndex][row].description.Length / maxSymbolsInRow);
            }
          
            if (pageIndex % 2 == 0)
            {
                newPosition = new Vector3(startPosStatusLeft.x, startPosStatusLeft.y - (indexInList + addedLines) * deltaY, startPosStatusLeft.z); ;
            }
            else
            {
                newPosition = new Vector3(startPosStatusRight.x, startPosStatusRight.y - (indexInList + addedLines) * deltaY, startPosStatusRight.z);
            }

            statuses[pageIndex][i].GetComponent<RectTransform>().localPosition = newPosition;
        }
    }

    public void HideDescription(int pageIndex)
    {
        ClearText(pageIndex);

        for (int i = 0; i < pages[pageIndex].Count; i++)
        {
            if (pageIndex % 2 == 0)
                diaryTextLeft.text += pages[pageIndex][i].name + "\n";
            else
                diaryTextRight.text += pages[pageIndex][i].name + "\n";
        }

        for (int i = 0; i < statuses[pageIndex].Count; i++)
        {
            Vector3 newPosition = Vector3.zero;
            int indexInList = statuses[pageIndex][i].GetComponent<StatusButton>().indexInList;
            if (pageIndex % 2 == 0)
            {
                newPosition = new Vector3(startPosStatusLeft.x, startPosStatusLeft.y - indexInList * deltaY, startPosStatusLeft.z);
            }
            else
            {
                newPosition = new Vector3(startPosStatusRight.x, startPosStatusRight.y - indexInList * deltaY, startPosStatusRight.z);
            }

            statuses[pageIndex][i].GetComponent<RectTransform>().localPosition = newPosition;
        }
    }

    private void displayPages(int index)
    {
        ClearTextes();

        for (int i = 0; i < pages[index].Count; i++)
        {
            diaryTextLeft.text += pages[index][i].name + "\n";
            createStatusButton(index, i);
        }

        if (index + 1 < pages.Count)
        {
            for (int i = 0; i < pages[index + 1].Count; i++)
            {
                diaryTextRight.text += pages[index + 1][i].name + "\n";
                createStatusButton(index + 1, i);
            }
        }
    }

    private void createStatusButton(int index, int row)
    {
        GameObject statusButton = null;
        if (pages[index][row].status == AchievementStatus.Completed)
        {
            statusButton = Instantiate(statusButtonTick, Globals.Instance.Canvas.transform);
        }
        else if (pages[index][row].status == AchievementStatus.Failed)
        {
            statusButton = Instantiate(statusButtonCross, Globals.Instance.Canvas.transform);
        }

        if (statusButton)
        {
            if (index % 2 == 0)
            {
                statusButton.GetComponent<RectTransform>().localPosition = new Vector3(startPosStatusLeft.x, startPosStatusLeft.y - row * deltaY, startPosStatusLeft.z);
            }
            else
            {
                statusButton.GetComponent<RectTransform>().localPosition = new Vector3(startPosStatusRight.x, startPosStatusRight.y - row * deltaY, startPosStatusRight.z);
            }
            statusButton.GetComponent<StatusButton>().pageIndex = index;
            statusButton.GetComponent<StatusButton>().indexInList = row;
            statuses[index].Add(statusButton);
        }
    }

    public void Hide()
    {
        DiaryUI.SetActive(false);
        DestroyStatuses();
    }

    private void DestroyStatuses()
    {
        for (int i = 0; i < statuses[currentPage].Count; i++)
        {
            Destroy(statuses[currentPage][i]);
        }

        for (int i = 0; i < statuses[currentPage + 1].Count; i++)
        {
            Destroy(statuses[currentPage + 1][i]);
        }
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