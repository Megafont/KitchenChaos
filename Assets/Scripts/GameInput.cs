using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    PlayerInputActions _PlayerInputActions;



    private void Awake()
    {
        _PlayerInputActions = new PlayerInputActions();
        _PlayerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _PlayerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        Debug.Log(inputVector);
        return inputVector;
    }
}
