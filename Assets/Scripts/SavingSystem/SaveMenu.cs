using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    public void OnClicked(Button button)
    {
        PlayerPrefs.SetString("currentLoad", button.name + ".txt");
        PlayerPrefs.SetInt(button.name + ".txt", QuestsOnThisLevel.Instance.GetCurrentLevel());

        SavingSystem.Instance.Save(button.name + ".txt");
    }
}
