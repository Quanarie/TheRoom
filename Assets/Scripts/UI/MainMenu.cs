using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if (PlayerPrefs.GetString("currentLoad") == "")
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt(PlayerPrefs.GetString("currentLoad"), 1));
    }

    public void OnNewGameButtonClicked()
    {
        PlayerPrefs.SetString("currentLoad", "");
        SceneManager.LoadScene(1);
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
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        PlayerPrefs.DeleteAll();
    }
}
