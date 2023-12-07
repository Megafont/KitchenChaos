
using System.IO;

using UnityEngine;


namespace PixagonalGames.MyTools.Editor.Utilities
{
    static class Folders
    {
        public static void CreateDefaultProjectFolders(string root, params string[] folders)
        {
            string fullPath = Path.Combine(Application.dataPath, root);


            foreach (string folder in folders)
            {
                string path = Path.Combine(fullPath, folder);

                if (!Directory.Exists(path))                    
                {
                    Directory.CreateDirectory(path);
                }
            } // end foreach
        }


    }

}