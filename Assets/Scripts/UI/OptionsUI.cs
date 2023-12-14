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
    [SerializeField] private TextMeshProUGUI _SoundEffectsText;
    [SerializeField] private TextMeshProUGUI _MusicText;



    private void Awake()
    {
        Instance = this;

        _SoundEffectsButton.onClick.AddListener(OnSoundEffectsClicked);
        _MusicButton.onClick.AddListener(OnMusicClicked);
        _CloseButton.onClick.AddListener(OnCloseClicked);
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;    
        
        UpdateVisuals();

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
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisuals()
    {
        _SoundEffectsText.text = $"Sound Effects: {(int) (SoundManager.Instance.GetVolume() * 10f)}";
        _MusicText.text = $"Music: {(int) (MusicManager.Instance.GetVolume() * 10f)}";
    }
}
