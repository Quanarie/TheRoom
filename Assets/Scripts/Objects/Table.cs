using UnityEngine;

public class Table : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (InputManager.Instance.GetInteractionPressed())
        {
            if (collision.CompareTag("Player") && !Diary.Instance.IsDiaryOnScreen())
            {
                Diary.Instance.Show();
            }
        }
    }
}