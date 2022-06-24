using UnityEngine;

public class DiaryAchievement
{
    public int level;
    public string name;
    public string description;
    public AchievementStatus status;

    public DiaryAchievement(int lvl, string achName)
    {
        level = lvl;
        name = achName;
    }
}