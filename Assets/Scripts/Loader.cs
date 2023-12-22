using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class Loader
{
    public enum Scenes
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
        LobbyScene,
        CharacterSelectScene,
    }


    private static Scenes _TargetScene;



    public static void LoadScene(Scenes targetScene)
    {
        _TargetScene = targetScene;

        SceneManager.LoadScene(targetScene.ToString());
    }
       
    /// <summary>
    /// This function is just like LoadScene(), but for use in multiplayer games.
    /// </summary>
    /// <param name="targetScene"></param>
    public static void LoadSceneMultiplayer(Scenes targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(_TargetScene.ToString());
    }
}
