using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    public void OnClicked(Button button)
    {
        PlayerPrefs.SetString("currentLoad", button.name + ".txt");
        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt(button.name + ".level", 1));
        SceneManager.LoadScene(PlayerPrefs.GetInt(button.name + ".txt", 1));
    }
}
