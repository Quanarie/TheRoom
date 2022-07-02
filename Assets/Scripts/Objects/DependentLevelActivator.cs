using UnityEngine;

public class DependentLevelActivator : MonoBehaviour
{
    [SerializeField] private int level;

    private void Awake()
    {
        if (level == PlayerPrefs.GetInt("currentLevel"))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
