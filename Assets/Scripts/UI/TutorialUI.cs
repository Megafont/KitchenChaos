using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;


public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _KeyMoveUpText;
    [SerializeField] private TextMeshProUGUI _KeyMoveDownText;
    [SerializeField] private TextMeshProUGUI _KeyMoveLeftText;
    [SerializeField] private TextMeshProUGUI _KeyMoveRightText;
    [SerializeField] private TextMeshProUGUI _KeyInteractText;
    [SerializeField] private TextMeshProUGUI _KeyInteractAltText;
    [SerializeField] private TextMeshProUGUI _KeyPauseText;
    
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _KeyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI _KeyGamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI _KeyGamepadPauseText;


    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisuals();

        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        _KeyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Up);
        _KeyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Down);
        _KeyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Left);
        _KeyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Right);
        _KeyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact);
        _KeyInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact_Alt);
        _KeyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);

        _KeyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Interact);
        _KeyGamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Interact_Alt);
        _KeyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
