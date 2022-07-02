using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;

    private const float timeToFade = 1f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && InputManager.Instance.GetInteractionPressed())
        {
            Globals.Instance.GameManager.StopAllCoroutines();
            StartCoroutine(fadeOut());
        }
    }

    private IEnumerator fadeOut()
    {
        Globals.Instance.isTransitioningDoor = true;
        while (!Mathf.Approximately(Globals.Instance.Fader.alpha, 1f))
        {
            Globals.Instance.Fader.alpha += Time.deltaTime / timeToFade;
            yield return null;
        }
        Globals.Instance.isTransitioningDoor = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
