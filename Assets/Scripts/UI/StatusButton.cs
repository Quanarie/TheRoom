using UnityEngine;

public class StatusButton : MonoBehaviour
{
    public int pageIndex;
    public int indexInList;
    public bool isOpened;

    public void OnClick()
    {
        isOpened = !isOpened;

        if (isOpened)
        {
            Diary.Instance.DisplayDescription(pageIndex, indexInList);
        }
        else
        {
            Diary.Instance.HideDescription(pageIndex);
        }
    }
}
