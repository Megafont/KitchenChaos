using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class LoaderCallback : MonoBehaviour
{
    private bool _IsFirstUpdate = true;


    private void Update()
    {
        if (_IsFirstUpdate)
        {
            _IsFirstUpdate = false;

            Loader.LoaderCallback();
        }
    }

}
