using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private float distance;

    private void Start()
    {
        InputManager.Instance.OnInteractionPressed += OnInteraction;
    }

    private void OnInteraction()
    {
        if (Vector3.Distance(Globals.Instance.Player.transform.position, transform.position) <= distance && !Diary.Instance.IsDiaryOnScreen())
        {
            Diary.Instance.Show();
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnInteractionPressed -= OnInteraction;
    }
}