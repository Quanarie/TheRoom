using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOptionsButtonClicked()
    {

    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
