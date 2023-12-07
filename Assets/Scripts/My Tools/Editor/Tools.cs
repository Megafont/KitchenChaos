using UnityEngine;
using UnityEditor;

using PixagonalGames.MyTools.Editor.Utilities;


namespace PixagonalGames.MyTools.Editor
{
    /// <summary>
    /// This namespace is a collection of Unity Editor tools.
    /// </summary>
    /// <remarks>
    /// Some of the code in here is from git-amend's videos, such as this 3D platformer series:
    /// https://www.youtube.com/watch?v=--_CH5DYz0M&list=PLnJJ5frTPwRNdyRAD4oBtG1eUVBuj2h1O
    /// </remarks>
    public static class Setup
    {

        // The two extra parameters in this attribute make it add a separator bar under this command in the menu.
        [MenuItem("Tools/Setup/Create Default Project Folders", false, 0)]
        public static void CreateDefaultProjectFolders()
        {
            Folders.CreateDefaultProjectFolders("_Project",
                                                "Animation", "Audio", "Materials", "Prefabs", "Scriptable Objects", "Scenes", "Scripts", "Settings");

            AssetDatabase.Refresh();
        }


        [MenuItem("Tools/Setup/Import My Favorite Assets")]
        public static void ImportMyFavoriteAssets()
        {
            Assets.ImportAsset("DOTween HOTween v2.unitypackage", "Demigiant/ScriptingAnimation");
        }

        [MenuItem("Tools/Setup/Install Netcode for GameObjects")]
        public static void InstallNetcodeForGameObjects()
        {
            Packages.InstallPackages(new[] 
            {
                "com.unity.multiplayer.tools",
                "com.unity.netcode.gameobjects"
            });
        }

        [MenuItem("Tools/Setup/Install Unity AI Navigation")]
        public static void InstallUnityAINavigation()
        {
            Packages.InstallPackages(new[] 
            {
                "com.unity.ai.navigation"
            });
        }

        [MenuItem("Tools/Setup/Install My Favorite Open Source")]
        public static void InstallOpenSource()
        {
            Packages.InstallPackages(new[]
            {
                "git+https://github.com/KyleBanks/scene-ref-attribute",
                "git+https://github.com/starikcetin/Eflatun.SceneReference.git#3.1.1"
            });

        }


    } // end class Setup

}