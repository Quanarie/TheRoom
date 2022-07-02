using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Bed : MonoBehaviour
{
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
            List<Button> choices = DialogueManager.Instance.ShowChoices(2);
            choices[0].GetComponentInChildren<TextMeshProUGUI>().text = "Відпочити";
            choices[1].GetComponentInChildren<TextMeshProUGUI>().text = "Почекати";

            choices[0].onClick.AddListener(() => Globals.Instance.GameManager.nextLevel());
            choices[1].onClick.AddListener(() => hide());
        }
        else
        {
            List<Button> choices = DialogueManager.Instance.ShowChoices(1);
            choices[0].GetComponentInChildren<TextMeshProUGUI>().text = "Почекати";

            choices[0].onClick.AddListener(() => hide());
        }
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
}
