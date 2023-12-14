using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }


    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;


    private PlayerInputActions _PlayerInputActions;



    private void Awake()
    {
        Instance = this;

        _PlayerInputActions = new PlayerInputActions();
        _PlayerInputActions.Player.Enable();

        _PlayerInputActions.Player.Interact.performed += Interact_performed;
        _PlayerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        _PlayerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        _PlayerInputActions.Player.Interact.performed -= Interact_performed;
        _PlayerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        _PlayerInputActions.Player.Pause.performed -= Pause_performed;

        _PlayerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _PlayerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
