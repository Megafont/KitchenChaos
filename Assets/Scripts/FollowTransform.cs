using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class FollowTransform : MonoBehaviour
{
    private Transform _TargetTransform;



    private void LateUpdate()
    {
        if (_TargetTransform == null)
            return;

        transform.position = _TargetTransform.position;
        transform.rotation = _TargetTransform.rotation;
    }
    public void SetTargetTransform(Transform targetTransform)
    {
        _TargetTransform = targetTransform;
    }
}
