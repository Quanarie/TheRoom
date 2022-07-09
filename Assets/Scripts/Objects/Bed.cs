using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Bed : MonoBehaviour
{
    [SerializeField] private float distance;

    private void Start()
    {
        InputManager.Instance.OnInteractionPressed += OnInteraction;
    }

    private void OnInteraction()
    {
        if (Vector3.Distance(Globals.Instance.Player.transform.position, transform.position) <= distance && !DialogueManager.Instance.IsDialogueOn())
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
            List<Button> choices = DialogueManager.Instance.ShowChoices(2);
            choices[0].GetComponentInChildren<TextMeshProUGUI>().text = "Відпочити";
            choices[1].GetComponentInChildren<TextMeshProUGUI>().text = "Почекати";

            choices[0].onClick.AddListener(() => nextLevel());
            choices[1].onClick.AddListener(() => hide());
        }
        else
        {
            List<Button> choices = DialogueManager.Instance.ShowChoices(1);
            choices[0].GetComponentInChildren<TextMeshProUGUI>().text = "Почекати";

            choices[0].onClick.AddListener(() => hide());
        }
    }

    private void nextLevel()
    {
        Globals.Instance.GameManager.nextLevel();
    }

    private bool canSleep()
    {
        int[] quests = QuestsOnThisLevel.Instance.GetQuests();
        TwoDArray[] stages = QuestsOnThisLevel.Instance.GetStages();
        for (int i = 0; i < quests.Length; i++)
        {
            bool isThereOneCorrectStage = false;
            for (int j = 0; j < stages[i].array.Length; j++)
            {
                if (QuestManager.Instance.GetStage(quests[i]) == stages[i].array[j])
                {
                    isThereOneCorrectStage = true;
                    break;
                }
            }
            if (isThereOneCorrectStage == false)
            {
                return false;
            }
        }
        return true;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnInteractionPressed -= OnInteraction;
    }
}
