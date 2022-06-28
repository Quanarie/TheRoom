using System;
using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour, ISaveable
{
    public static Diary Instance { get; private set; }

    [SerializeField] private int maxAchievementsOnPage = 9;
    [SerializeField] private int maxSymbolsInRow = 32;
    [SerializeField] private Vector3 startPosStatusLeft = new Vector3(-45f, 312f, 0f);
    [SerializeField] private Vector3 startPosStatusRight = new Vector3(527f, 312f, 0f);
    [SerializeField] private float deltaY = 35.6f;
    [SerializeField] private float deltaYTitle = 53;
    [SerializeField] private string titleFormatingStart = "<size=150%><b><align=left>";
    [SerializeField] private string titleFormatingEnd = "</size></b></align=right>";

    private List<List<DiaryAchievement>> pages = new();

    private int currentPage = 0;

    private List<DiaryAchievement> achievements = new();
    private List<List<GameObject>> statuses = new();

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
        Globals.Instance.DiaryUI.SetActive(false);
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
        if (currentPage + 2 < pages.Count)
        {
            DestroyStatuses();

            currentPage += 2;
            displayPages(currentPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage - 2 >= 0)
        {
            DestroyStatuses();

            currentPage -= 2;
            displayPages(currentPage);
        }
    }

    private void ClearTextes()
    {
        Globals.Instance.DiaryTextRight.text = "";
        Globals.Instance.DiaryTextLeft.text = "";
    }

    private void ClearText(int pageIndex)
    {
        if (pageIndex % 2 == 0)
            Globals.Instance.DiaryTextLeft.text = "";
        else
            Globals.Instance.DiaryTextRight.text = "";
    }

    public void Show()
    {
        if (QuestManager.Instance.Quests[3].GetCurrentStage() == 0) return; // unactive before discovering
        if (DialogueManager.Instance.IsDialogueOn()) return;

        Globals.Instance.DiaryUI.SetActive(true);

        pages.Clear();
        ClearTextes();
        currentPage = 0;

        int achievementOnCurrentPage = 0;
        int current = 0;
        pages.Add(new List<DiaryAchievement>());
        statuses.Add(new List<GameObject>());

        for (int i = 1; ; i++) // level
        {
            bool isThereNextLevelAchievement = false;
            for (int j = 0; j < achievements.Count; j++)
            {
                if (achievements[j].level == i)
                {
                    if (achievementOnCurrentPage < maxAchievementsOnPage)
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
                else if (achievements[j].level == i + 1)
                {
                    isThereNextLevelAchievement = true;
                }
            }
            achievementOnCurrentPage = 0;

            if (isThereNextLevelAchievement)
            {
                current++;
                pages.Add(new List<DiaryAchievement>());
                statuses.Add(new List<GameObject>());
            }
            else
            {
                break;
            }
        }
        displayPages(currentPage);
    }

    public void DisplayDescription(int pageIndex, int row)
    {
        if (pageIndex % 2 == 0)
        {
            Globals.Instance.DiaryTextLeft.text = "";
        }
        else
        {
            Globals.Instance.DiaryTextRight.text = "";
        }

        for (int i = 0; i < pages[pageIndex].Count; i++)
        {
            if (i == row)
            {
                if (pageIndex % 2 == 0)
                {
                    Globals.Instance.DiaryTextLeft.text += titleFormatingStart + pages[pageIndex][i].name + titleFormatingEnd + "\n";
                    Globals.Instance.DiaryTextLeft.text += pages[pageIndex][i].description + "\n";
                }
                else
                {
                    Globals.Instance.DiaryTextRight.text += titleFormatingStart + pages[pageIndex][i].name + titleFormatingEnd + "\n";
                    Globals.Instance.DiaryTextRight.text += pages[pageIndex][i].description + "\n";
                }
            }
            else
            {
                if (pageIndex % 2 == 0)
                    Globals.Instance.DiaryTextLeft.text += titleFormatingStart + pages[pageIndex][i].name + titleFormatingEnd + "\n";
                else
                    Globals.Instance.DiaryTextRight.text += titleFormatingStart + pages[pageIndex][i].name + titleFormatingEnd + "\n";
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
                newPosition = new Vector3(startPosStatusLeft.x, startPosStatusLeft.y - indexInList * deltaYTitle - addedLines * deltaY, startPosStatusLeft.z); ;
            }
            else
            {
                newPosition = new Vector3(startPosStatusRight.x, startPosStatusRight.y - indexInList * deltaYTitle - addedLines * deltaY, startPosStatusRight.z);
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
                Globals.Instance.DiaryTextLeft.text += titleFormatingStart + pages[pageIndex][i].name + titleFormatingEnd + "\n";
            else
                Globals.Instance.DiaryTextRight.text += titleFormatingStart + pages[pageIndex][i].name + titleFormatingEnd + "\n";
        }

        for (int i = 0; i < statuses[pageIndex].Count; i++)
        {
            Vector3 newPosition = Vector3.zero;
            int indexInList = statuses[pageIndex][i].GetComponent<StatusButton>().indexInList;
            if (pageIndex % 2 == 0)
            {
                newPosition = new Vector3(startPosStatusLeft.x, startPosStatusLeft.y - indexInList * deltaYTitle, startPosStatusLeft.z);
            }
            else
            {
                newPosition = new Vector3(startPosStatusRight.x, startPosStatusRight.y - indexInList * deltaYTitle, startPosStatusRight.z);
            }

            statuses[pageIndex][i].GetComponent<RectTransform>().localPosition = newPosition;
        }
    }

    private void displayPages(int index)
    {
        ClearTextes();

        for (int i = 0; i < pages[index].Count; i++)
        {
            Globals.Instance.DiaryTextLeft.text += titleFormatingStart + pages[index][i].name + titleFormatingEnd + "\n";
            createStatusButton(index, i);
        }

        if (index + 1 < pages.Count)
        {
            for (int i = 0; i < pages[index + 1].Count; i++)
            {
                Globals.Instance.DiaryTextRight.text += titleFormatingStart + pages[index + 1][i].name + titleFormatingEnd + "\n";
                createStatusButton(index + 1, i);
            }
        }
    }

    private void createStatusButton(int index, int row)
    {
        GameObject statusButton = null;
        if (pages[index][row].status == AchievementStatus.Completed)
        {
            statusButton = Instantiate(Globals.Instance.StatusButtonTick, Globals.Instance.Canvas.transform);
        }
        else if (pages[index][row].status == AchievementStatus.Failed)
        {
            statusButton = Instantiate(Globals.Instance.StatusButtonCross, Globals.Instance.Canvas.transform);
        }

        if (statusButton)
        {
            if (index % 2 == 0)
            {
                statusButton.GetComponent<RectTransform>().localPosition = new Vector3(startPosStatusLeft.x, startPosStatusLeft.y - row * deltaYTitle, startPosStatusLeft.z);
            }
            else
            {
                statusButton.GetComponent<RectTransform>().localPosition = new Vector3(startPosStatusRight.x, startPosStatusRight.y - row * deltaYTitle, startPosStatusRight.z);
            }
            statusButton.GetComponent<StatusButton>().pageIndex = index;
            statusButton.GetComponent<StatusButton>().indexInList = row;
            statuses[index].Add(statusButton);
        }
    }

    public void Hide()
    {
        Globals.Instance.DiaryUI.SetActive(false);
        DestroyStatuses();
    }

    private void DestroyStatuses()
    {
        if (currentPage < statuses.Count)
        {
            for (int i = 0; i < statuses[currentPage].Count; i++)
            {
                Destroy(statuses[currentPage][i]);
            }
            statuses[currentPage].Clear();
        }
        
        if (currentPage + 1 < statuses.Count)
        {
            for (int i = 0; i < statuses[currentPage + 1].Count; i++)
            {
                Destroy(statuses[currentPage + 1][i]);
            }
            statuses[currentPage + 1].Clear();
        }
    }

    public bool IsDiaryOnScreen() => Globals.Instance.DiaryUI.activeSelf;

    public object CaptureState()
    {
        return achievements;
    }

    public void RestoreState(object state)
    {
        achievements = (List<DiaryAchievement>)state;
    }
}

public enum AchievementStatus 
{
    Inprocess,
    Completed,
    Failed,
    Denied
}