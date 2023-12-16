using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";


    public static GameInput Instance { get; private set; }


    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;


    public enum Bindings
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alt,
        Pause,
        Gamepad_Interact,
        Gamepad_Interact_Alt,
        Gamepad_Pause,
    }


    private PlayerInputActions _PlayerInputActions;

    

    private void Awake()
    {
        Instance = this;

        _PlayerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            _PlayerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

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

    public string GetBindingText(Bindings binding)
    {
        switch (binding)
        {
            default:

            case Bindings.Move_Up:
                return _PlayerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Bindings.Move_Down:
                return _PlayerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Bindings.Move_Left:
                return _PlayerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Bindings.Move_Right:
                return _PlayerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Bindings.Interact:
                return _PlayerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Bindings.Interact_Alt:
                return _PlayerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Bindings.Pause:
                return _PlayerInputActions.Player.Pause.bindings[0].ToDisplayString();
            
            case Bindings.Gamepad_Interact:
                return _PlayerInputActions.Player.Interact.bindings[1].ToDisplayString();
            case Bindings.Gamepad_Interact_Alt:
                return _PlayerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Bindings.Gamepad_Pause:
                return _PlayerInputActions.Player.Pause.bindings[1].ToDisplayString();

        } // end switch
    }

    public void RebindBinding(Bindings binding, Action onActionRebound)
    {
        // Disable the action map first.
        _PlayerInputActions.Player.Disable();


        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:

            case Bindings.Move_Up:
                inputAction = _PlayerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Bindings.Move_Down:
                inputAction = _PlayerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Bindings.Move_Left:
                inputAction = _PlayerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Bindings.Move_Right:
                inputAction = _PlayerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Bindings.Interact:
                inputAction = _PlayerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Bindings.Interact_Alt:
                inputAction = _PlayerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Bindings.Pause:
                inputAction = _PlayerInputActions.Player.Pause;
                bindingIndex = 0;
                break;

            case Bindings.Gamepad_Interact:
                inputAction = _PlayerInputActions.Player.Interact;
                bindingIndex = 1;
                break;
            case Bindings.Gamepad_Interact_Alt:
                inputAction = _PlayerInputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Bindings.Gamepad_Pause:
                inputAction = _PlayerInputActions.Player.Pause;
                bindingIndex = 1;
                break;

        } // end switch


        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => 
            {  
                callback.Dispose();
                _PlayerInputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, _PlayerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save(); // Save manually in case Unity crashes or something.

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();

    }

    public void ResetAllBindings()
    {
        _PlayerInputActions.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(PLAYER_PREFS_BINDINGS);
        PlayerPrefs.Save();

        OnBindingRebind?.Invoke(this, EventArgs.Empty);
    }
}
