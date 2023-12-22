using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;


[SelectionBase]
public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPlayerPickedUpSomething;

    public static void ResetStaticData() 
    { 
        OnAnyPlayerSpawned = null;
        OnAnyPlayerPickedUpSomething = null;
    }

    public static Player LocalInstance { get; private set; }


    public event EventHandler OnPickedUpSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }



    [SerializeField] [Min(0f)] private float _MoveSpeed = 7f;
    [SerializeField] private LayerMask _CountersLayerMask;
    [SerializeField] private LayerMask _CollisionsLayerMask;
    [SerializeField] private Transform _KitchenObjectHoldPoint;
    [SerializeField] private List<Vector3> _SpawnPositionList;


    private bool _IsWalking;
    private Vector3 _LastInputDir;
    private BaseCounter _SelectedCounter;
    private KitchenObject _KitchenObject;



    private void Awake()
    {

    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += _GameInput_OnInteractAlternateAction;
    }

    /// <summary>
    /// This is basically like Awake() or Start(), but for NetworkObjects.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            LocalInstance = this;

        transform.position = _SpawnPositionList[(int) OwnerClientId];

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        // We only need to listen to this event if this is the server.
        if (IsServer)
        {
            NetworkManager.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
        }
    }

    private void Update()
    {
        // Run the update code only if this object is the local player.
        if (!IsOwner)
        {
            return;
        }


        HandleMovement();
        HandleInteractions();
    }

    private void _GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
            return;

        if (_SelectedCounter != null)
            _SelectedCounter.InteractAlternate(this);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
            return;

        if (_SelectedCounter != null)
            _SelectedCounter.Interact(this);
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
            _LastInputDir = moveDir;

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _LastInputDir, out RaycastHit raycastHit, interactDistance, _CountersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter
                if (baseCounter != _SelectedCounter)
                {
                    SetSelectedCounter(baseCounter);
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
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = _MoveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, _CollisionsLayerMask);

        if (!canMove)
        {
            // Cannot move towards moveDir.

            // Attempt only X movement.
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, _CollisionsLayerMask);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on X.

                // Attempt only Z movement.
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, _CollisionsLayerMask);

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

    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        _SelectedCounter = baseCounter;


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

        if (kitchenObject != null)
        {
            OnPickedUpSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPlayerPickedUpSomething?.Invoke(this, EventArgs.Empty);
        }
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

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }


    public bool IsWalking() { return _IsWalking; }

}
