
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;


namespace PixagonalGames.MyTools.Editor.Utilities
{
    static class Packages
    {
        static AddRequest Request;
        static Queue<string> PackagesToInstall = new();



        public static void InstallPackages(string[] packages)
        {
            foreach (var package in packages)
            {
                PackagesToInstall.Enqueue(package);
            }


            // Start the installation of the first package
            if (PackagesToInstall.Count > 0)
            {
                Request = Client.Add(PackagesToInstall.Dequeue());
                EditorApplication.update += Progress;
            }
        }

        static async void Progress()
        {
            if (Request.IsCompleted)
            {
                if (Request.Status == StatusCode.Success)
                    Debug.Log("Installed: " + Request.Result.packageId);
                else if (Request.Status >= StatusCode.Failure)
                    Debug.Log(Request.Error.message);

                EditorApplication.update -= Progress;


                // If there are more packages to install, start the next one
                if (PackagesToInstall.Count > 0)
                {
                    // Add delay before next package install
                    await Task.Delay(1000);

                    Request = Client.Add(PackagesToInstall.Dequeue());
                    EditorApplication.update += Progress;
                }
            }
        }


    } // end class Packages

}