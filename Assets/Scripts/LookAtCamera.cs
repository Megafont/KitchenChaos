using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }


    [SerializeField] private Mode mode;



    private void LateUpdate()
    {
        // This is in LateUpdate() because you should update a canvas' look at
        // direction after the object it is attached to has already moved in this frame.        
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;


        }
    }
}
