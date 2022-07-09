using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int sceneToLoad;
    [SerializeField] private float distance;

    private const float timeToFade = 1f;

    private void Start()
    {
        InputManager.Instance.OnInteractionPressed += OnInteraction;
    }

    private void OnInteraction()
    {
        if (Vector3.Distance(Globals.Instance.Player.transform.position, transform.position) <= distance)
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

    private void OnDestroy()
    {
        InputManager.Instance.OnInteractionPressed -= OnInteraction;
    }
}
