using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AchievementPaper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Vector3 position; 
    private GameObject statusImage;

    private void Start()
    {
        GetComponent<Image>().enabled = false;
        Diary.Instance.OnAchievementAdded += OnAchievementAdded;
    }

    private void OnAchievementAdded(string name, AchievementStatus status)
    {
        GetComponent<Image>().enabled = true;

        if (statusImage)
        {
            Destroy(statusImage);
        }

        StartCoroutine(hidePaper());

        if (status == AchievementStatus.Denied)
        {
            text.text = "<s>" + name + "</s>";
            return;
        }
        else
        {
            text.text = name;
        }


        if (status == AchievementStatus.Inprocess)
        {
            return;
        }
        else if (status == AchievementStatus.Completed)
        {
            statusImage = Instantiate(Globals.Instance.StatusButtonTick, transform);
        }
        else if (status == AchievementStatus.Failed)
        {
            statusImage = Instantiate(Globals.Instance.StatusButtonCross, transform);
        }
        statusImage.GetComponent<Button>().enabled = false;
        statusImage.GetComponent<RectTransform>().localPosition = position;
    }

    private IEnumerator hidePaper()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Image>().enabled = false;
        text.text = "";
        if (statusImage)
        {
            Destroy(statusImage);
        }
    }

    private void OnDestroy()
    {
        Diary.Instance.OnAchievementAdded -= OnAchievementAdded;
    }
}
