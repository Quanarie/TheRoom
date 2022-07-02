using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuContainer : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            if (pauseMenu.activeSelf) PauseGame();
            else StartGame();
        }
    }

    private void PauseGame()
    {
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
}
