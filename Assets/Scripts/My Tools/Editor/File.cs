using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

using PixagonalGames.MyTools.Editor.Utilities;


namespace PixagonalGames.MyTools.Editor
{    
    /// <summary>
    /// This namespace is a collection of Unity Editor tools.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class File
    {
        [MenuItem("File/Save Scene and Project")]
        public static void SaveSceneAndProject()
        {
            EditorApplication.ExecuteMenuItem("File/Save");
            EditorApplication.ExecuteMenuItem("File/Save Project");

            Debug.Log($"Saved scene \"{SceneManager.GetActiveScene().name}\" and the whole project.");
        }

    } // end class Setup

}