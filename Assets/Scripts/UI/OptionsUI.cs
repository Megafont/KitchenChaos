using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance;


    [SerializeField] private Button _SoundEffectsButton;
    [SerializeField] private Button _MusicButton;
    [SerializeField] private Button _CloseButton;
    
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _SoundEffectsText;
    [SerializeField] private TextMeshProUGUI _MusicText;

    [Space(20)]
    [SerializeField] private Button _MoveUpButton;
    [SerializeField] private Button _MoveDownButton;
    [SerializeField] private Button _MoveLeftButton;
    [SerializeField] private Button _MoveRightButton;
    [SerializeField] private Button _InteractButton;
    [SerializeField] private Button _InteractAltButton;
    [SerializeField] private Button _PauseButton;
    
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _MoveUpText;
    [SerializeField] private TextMeshProUGUI _MoveDownText;
    [SerializeField] private TextMeshProUGUI _MoveLeftText;
    [SerializeField] private TextMeshProUGUI _MoveRightText;
    [SerializeField] private TextMeshProUGUI _InteractText;
    [SerializeField] private TextMeshProUGUI _InteractAltText;
    [SerializeField] private TextMeshProUGUI _PauseText;

    [Space(20)]
    [SerializeField] private Button _GamepadInteractButton;
    [SerializeField] private Button _GamepadInteractAltButton;
    [SerializeField] private Button _GamepadPauseButton;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI _GamepadInteractText;
    [SerializeField] private TextMeshProUGUI _GamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI _GamepadPauseText;

    [Space(10)]
    [SerializeField] private Transform _PressToRebindKeyTransform;


    private Action _OnCloseButtonAction;



    private void Awake()
    {
        Instance = this;

        _SoundEffectsButton.onClick.AddListener(OnSoundEffectsClicked);
        _MusicButton.onClick.AddListener(OnMusicClicked);
        _CloseButton.onClick.AddListener(OnCloseClicked);

        _MoveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Up); });
        _MoveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Down); });
        _MoveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Left); });
        _MoveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Move_Right); });
        _InteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Interact); });
        _InteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Interact_Alt); });
        _PauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Pause); });

        _GamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Interact); });
        _GamepadInteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Interact_Alt); });
        _GamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Bindings.Pause); });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;    
        
        UpdateVisuals();

        HidePressToRebindKey();
        Hide();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void OnSoundEffectsClicked()
    {
        SoundManager.Instance.ChangeVolume();
        UpdateVisuals();
    }

    private void OnMusicClicked()
    {
        MusicManager.Instance.ChangeVolume();
        UpdateVisuals();
    }

    public void OnCloseClicked()
    {
        Hide();
        _OnCloseButtonAction();
    }

    public void Show(Action onCloseButtonAction)
    {
        _OnCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        _SoundEffectsButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisuals()
    {
        _SoundEffectsText.text = $"Sound Effects: {(int) (SoundManager.Instance.GetVolume() * 10f)}";
        _MusicText.text = $"Music: {(int) (MusicManager.Instance.GetVolume() * 10f)}";

        _MoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Up);
        _MoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Down);
        _MoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Left);
        _MoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Right);
        _InteractText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact);
        _InteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Interact_Alt);
        _PauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);

        _GamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Interact);
        _GamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Interact_Alt);
        _GamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Pause);

    }

    private void ShowPressToRebindKey()
    {
        _PressToRebindKeyTransform.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        _PressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Bindings binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisuals();
        });
    }
}
