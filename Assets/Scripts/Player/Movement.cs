using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour, ISaveable
{
    [SerializeField] private float speed;

    private new Rigidbody2D rigidbody;
    private SpriteRenderer sprite;

    private Vector2 input;
    private Vector2 velocity;
    private Vector2 nonZeroMoveDirection;

    public Vector2 GetMoveDirection()
    {
        return new Vector2(velocity.normalized.x, velocity.normalized.y);
    }

    public Vector2 GetNonZeroMoveDirection() => nonZeroMoveDirection;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!canMove()) return;

        input = InputManager.Instance.PlayerInput;
        if (input != Vector2.zero)
        {
            nonZeroMoveDirection.x = input.x;
            nonZeroMoveDirection.y = input.y;
        }
    }

    private bool canMove()
    {
        if (DialogueManager.Instance.IsDialogueOn()) return false;
        if (Diary.Instance.IsDiaryOnScreen()) return false;
        if (Globals.Instance.isTransitioningDoor) return false;

        return true;
    }

    private void FixedUpdate()
    {
        if (!canMove())
        {
            velocity = Vector2.zero;
            return;
        }

        velocity = input * speed;
        Vector2 moveAmount = velocity * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + moveAmount);

        if (moveAmount.x < 0f) sprite.flipX = true;
        else if (moveAmount.x > 0f) sprite.flipX = false;
    }

    public object CaptureState()
    {
        return new float[3] { transform.position.x, transform.position.y, transform.position.z };
    }

    public void RestoreState(object state)
    {
        float[] coordinates = (float[])state;
        transform.position = new Vector3(coordinates[0], coordinates[1], coordinates[2]);
    }
}
