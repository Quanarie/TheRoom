using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AchievementPaper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Vector3 position; 
    private Image image;
    private GameObject statusImage;

    private void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        Diary.Instance.OnAchievementAdded += OnAchievementAdded;
    }

    private void OnAchievementAdded(string name, AchievementStatus status)
    {
        image.enabled = true;

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
        image.enabled = false;
        text.text = "";
        if (statusImage)
        {
            Destroy(statusImage);
        }
    }
}
