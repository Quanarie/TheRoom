using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    public void OnClicked(Button button)
    {
        PlayerPrefs.SetString("currentLoad", button.name + ".txt");
        SceneManager.LoadScene(PlayerPrefs.GetInt(button.name + ".txt", 1));
    }
}
