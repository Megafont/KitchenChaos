using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[SelectionBase]
public class Player : MonoBehaviour
{
    [SerializeField] [Min(0f)] private float _MoveSpeed = 7f;
    [SerializeField] private GameInput _GameInput;


    private bool _IsWalking;


    private void Update()
    {
        Vector2 inputVector = _GameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDir * _MoveSpeed * Time.deltaTime;

        _IsWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }

    public bool IsWalking() { return _IsWalking; }
}
