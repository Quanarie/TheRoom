using UnityEngine;

public class DependentLevelActivator : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private bool isMore;

    private void Awake()
    {
        int currLvl = PlayerPrefs.GetInt("currentLevel");
        if (level == currLvl || (isMore && level < currLvl))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
