using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Vector2 PlayerInput { get; private set; }

    public void OnMovement(InputAction.CallbackContext context)
    {
        PlayerInput = context.ReadValue<Vector2>();
    }

    private bool isInteractionPressed;

    public bool GetInteractionPressed()
    {
        bool result = isInteractionPressed;
        isInteractionPressed = false;
        return result;
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isInteractionPressed = true;
        }
        else if (context.canceled)
        {
            isInteractionPressed = false;
        }
    }
}
