using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel", 1));
    }

    public void OnOptionsButtonClicked()
    {

    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnResetButtonClicked()
    {
        FileStream data = File.Open(Path.Combine(Application.persistentDataPath, "data.txt"), FileMode.Truncate);
        data.Close();
        PlayerPrefs.DeleteAll();
    }
}
