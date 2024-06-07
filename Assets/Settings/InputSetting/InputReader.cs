using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    public Vector2 MoveInput;
    public bool IsPerformed;

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        IsPerformed = context.performed;
    }

    public void StopMove()
    {
        MoveInput = Vector2.zero;
    }
}
