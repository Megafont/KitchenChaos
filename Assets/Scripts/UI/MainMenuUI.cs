using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _PlayButton;
    [SerializeField] private Button _QuitButton;



    private void Awake()
    {
        _PlayButton.onClick.AddListener(PlayClicked);
        _QuitButton.onClick.AddListener(QuitClicked);
    }

    private void PlayClicked()
    {
        Loader.LoadScene(Loader.Scenes.GameScene);
    }

    private void QuitClicked()
    {
        Application.Quit();
    }
}
