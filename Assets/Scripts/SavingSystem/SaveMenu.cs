using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    public void OnClicked(Button button)
    {
        PlayerPrefs.SetString("currentLoad", button.name + ".txt");
        PlayerPrefs.SetInt(button.name + ".txt", UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt(button.name + ".level", PlayerPrefs.GetInt("currentLevel"));

        SavingSystem.Instance.Save(button.name + ".txt");
    }
}
