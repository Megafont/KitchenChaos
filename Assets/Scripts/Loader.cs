using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public static class Loader
{
    public enum Scenes
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }


    private static Scenes _TargetScene;



    public static void LoadScene(Scenes targetScene)
    {
        _TargetScene = targetScene;

        SceneManager.LoadScene(Scenes.LoadingScene.ToString());
    }
       
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(_TargetScene.ToString());
    }
}
