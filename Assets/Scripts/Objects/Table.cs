using UnityEngine;

public class Table : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (InputManager.Instance.GetInteractionPressed())
            {
                if (Diary.Instance.IsDiaryOnScreen())
                {
                    Diary.Instance.Hide();
                }
                else
                {
                    Diary.Instance.Show();
                }
            }
        }
    }
}