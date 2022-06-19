using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
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
        if (input != Vector2.zero)
        {
            nonZeroMoveDirection.x = input.x;
            nonZeroMoveDirection.y = input.y;
        }
    }

    private void FixedUpdate()
    {
        velocity = input * speed;
        Vector2 moveAmount = velocity * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + moveAmount);

        if (moveAmount.x < 0f) sprite.flipX = true;
        else if (moveAmount.x > 0f) sprite.flipX = false;
    }

    //Input system methods
    public void OnMovement(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}
