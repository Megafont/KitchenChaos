using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


[SelectionBase]
public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }



    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter SelectedCounter;
    }



    [SerializeField] [Min(0f)] private float _MoveSpeed = 7f;
    [SerializeField] private GameInput _GameInput;
    [SerializeField] private LayerMask _CountersLayerMask;
    [SerializeField] private Transform _KitchenObjectHoldPoint;


    private bool _IsWalking;
    private Vector3 _LastInputDir;
    private ClearCounter _SelectedCounter;
    private KitchenObject _KitchenObject;



    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("There is more than one Player instance!");

        Instance = this;
    }

    private void Start()
    {
        _GameInput.OnInteractAction += GameInput_OnInteractAction;    
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (_SelectedCounter != null)
            _SelectedCounter.Interact(this);
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _GameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
            _LastInputDir = moveDir;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _LastInputDir, out RaycastHit raycastHit, interactDistance, _CountersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter
                if (clearCounter != _SelectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }                
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

    }

    private void HandleMovement()
    {
        Vector2 inputVector = _GameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = _MoveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Cannot move towards moveDir.

            // Attempt only X movement.
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on X.

                // Attempt only Z movement.
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on Z.
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction.
                }
            }
        }


        if (canMove)
            transform.position += moveDir * _MoveSpeed * Time.deltaTime;


        _IsWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(ClearCounter clearCounter)
    {
        _SelectedCounter = clearCounter;


        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = _SelectedCounter,
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return _KitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _KitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return _KitchenObject;
    }

    public void ClearKitchenObject()
    {
        _KitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _KitchenObject != null;
    }


    public bool IsWalking() { return _IsWalking; }

}
