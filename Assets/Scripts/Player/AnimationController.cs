using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    private Movement playerMovement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<Movement>();
    }

    private void Update()
    {
        animator.SetFloat("horizontalMovement", Mathf.Abs(playerMovement.GetMoveDirection().x));
        animator.SetFloat("verticalMovement", playerMovement.GetMoveDirection().y);
        animator.SetFloat("horizontalNonZeroMovement", Mathf.Abs(playerMovement.GetNonZeroMoveDirection().x));
        animator.SetFloat("verticalNonZeroMovement", playerMovement.GetNonZeroMoveDirection().y);
    }
}
