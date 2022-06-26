using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;

public class Bed : MonoBehaviour
{
    public delegate void NextLevelTransition();
    public static event NextLevelTransition OnNextLevelTransition;

    [SerializeField] private float timeToFade = 2f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputManager.Instance.GetInteractionPressed() && !DialogueManager.Instance.IsDialogueOn())
        {
            show();
        }
    }

    private void hide()
    {
        DialogueManager.Instance.Hide();
        DialogueManager.Instance.HideChoices();
    }

    private void show()
    {
        DialogueManager.Instance.Show();
        DialogueManager.Instance.DialogueText.GetComponent<TextMeshProUGUI>().text = "Це вже кінець? Немає більше нічого вартого уваги? Нічого, важливого чи цікавого?";
        if (canSleep())
        {
            Button[] choices = DialogueManager.Instance.ShowChoices(2);
            choices[0].GetComponentInChildren<TextMeshProUGUI>().text = "Відпочити";
            choices[1].GetComponentInChildren<TextMeshProUGUI>().text = "Почекати";

            choices[0].onClick.AddListener(() => nextLevel());
            choices[1].onClick.AddListener(() => hide());
        }
        else
        {
            Button[] choices = DialogueManager.Instance.ShowChoices(1);
            choices[0].GetComponentInChildren<TextMeshProUGUI>().text = "Почекати";

            choices[0].onClick.AddListener(() => hide());
        }
    }

    private bool canSleep()
    {
        bool everyQuestOnThisLevleIsDone = true;
        int[] lvls = QuestsOnThisLevel.Instance.GetLevels();
        for (int i = 0; i < lvls.Length; i++)
        {
            if (QuestManager.Instance.Quests[lvls[i]].GetCurrentStage() != QuestsOnThisLevel.Instance.GetStages()[i])
            {
                everyQuestOnThisLevleIsDone = false;
                break;
            }
        }
        return everyQuestOnThisLevleIsDone;
    }

    private void nextLevel()
    {
        DialogueManager.Instance.DialogueText.GetComponent<TextMeshProUGUI>().text = "Я втомився. Здається, я зробив все, що міг. Я заслужив на відпочинок.";
        OnNextLevelTransition?.Invoke();
        StartCoroutine(hideScreen());
    }

    private IEnumerator hideScreen()
    {
        yield return new WaitForSeconds(timeToFade);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
