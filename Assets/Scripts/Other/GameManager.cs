using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeToFade = 2f;

    private bool isTransitioning = false;
    private int levelOnLoaded = 1;

    private void Start()
    {
        SceneManager.sceneLoaded += StartFadingIn;
        levelOnLoaded = PlayerPrefs.GetInt("currentLevel");
        StartCoroutine(fadeIn());
    }

    private void StartFadingIn(Scene s, LoadSceneMode l)
    {
        if (levelOnLoaded == PlayerPrefs.GetInt("currentLevel"))
        {
            Globals.Instance.Player.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
        }
        else
        {
            levelOnLoaded = PlayerPrefs.GetInt("currentLevel");
        }
        StartCoroutine(fadeIn());
    }

    public void nextLevel()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        DialogueManager.Instance.DialogueText.GetComponent<TextMeshProUGUI>().text = "Я втомився. Здається, я зробив все, що міг. Я заслужив на відпочинок.";

        PlayerPrefs.SetString("currentLoad", "AutoLoad.txt");
        PlayerPrefs.SetInt("AutoLoad.txt", 1); // first room
        PlayerPrefs.SetInt("AutoLoad.level", PlayerPrefs.GetInt("currentLevel") + 1);
        SavingSystem.Instance.Save("AutoLoad.txt");
        StopAllCoroutines();
        StartCoroutine(hideScreen());
    }

    private IEnumerator hideScreen()
    {
        yield return fadeOut();
        DialogueManager.Instance.Hide();
        DialogueManager.Instance.HideChoices();
        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel") + 1);
        SceneManager.LoadScene(1); // first room
    }

    private IEnumerator fadeOut()
    {
        while (!Mathf.Approximately(Globals.Instance.Fader.alpha, 1f))
        {
            Globals.Instance.Fader.alpha += Time.deltaTime / timeToFade;
            yield return null;
        }
    }

    private IEnumerator fadeIn()
    {
        while (!Mathf.Approximately(Globals.Instance.Fader.alpha, 0))
        {
            Globals.Instance.Fader.alpha -= Time.deltaTime / timeToFade;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= StartFadingIn;
    }
}
