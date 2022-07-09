using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuContainer : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject saveMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (saveMenu.activeSelf)
            {
                saveMenu.SetActive(false);
                StartGame();
            }
            else
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);

                if (pauseMenu.activeSelf) PauseGame();
                else StartGame();
            }
        }
    }

    private void PauseGame()
    {
        saveMenu.SetActive(false);
        Globals.Instance.Player.GetComponent<Animator>().enabled = false;
        Time.timeScale = 0f;
    }

    private void StartGame()
    {
        Globals.Instance.Player.GetComponent<Animator>().enabled = true;
        Time.timeScale = 1f;
    }

    public void OnMenuButtonClicked()
    {
        StartGame();
        Destroy(SparedObjects.Instance.gameObject);
        SceneManager.LoadScene(0);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnSaveClicked(Button button)
    {
        PlayerPrefs.SetString("currentLoad", button.name + ".txt");
        PlayerPrefs.SetInt(button.name + ".txt", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt(button.name + ".level", PlayerPrefs.GetInt("currentLevel"));

        SavingSystem.Instance.Save(button.name + ".txt");

        pauseMenu.SetActive(true);
        saveMenu.SetActive(false);
    }
}
