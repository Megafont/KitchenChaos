
using System.IO;

using UnityEngine;
using UnityEditor;


namespace PixagonalGames.MyTools.Editor.Utilities
{
        static class Assets
        {
            public static void ImportAsset(string asset, string subfolder,
                string rootFolder = "C:/Users/adam/AppData/Roaming/Unity/Asset Store-5.x")
            {
                AssetDatabase.ImportPackage(Path.Combine(rootFolder, subfolder, asset), false);
            }


        } // end class Assets

}