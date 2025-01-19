using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; } //This is a singleton pattern. It allows us to access the Player instance from anywhere in the game.


    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    [SerializeField] LayerMask countersLayerMask;

    private KitchenObject kitchenObject;
    [SerializeField] private GameObject kitchenObjectHoldPoint;


    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;

    private void Awake() //DO INITIALIZATION ON AWAKE, REFERENCES ON START
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance in the scene");
        }
        Instance = this;
    }


    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }



    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(GameManager.Instance.IsGamePlaying() == false)
        {
            return;
        }
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying() == false)
        {
            return;
        }
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }



    private void HandleInteraction()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        //Seperate the logic
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float interactDistance = 2f;

        if (moveDir == Vector3.zero)
        {
            moveDir = lastInteractDir;
        }
        else
        {
            lastInteractDir = moveDir;
        }
        //out is a keyword that allows us to return multiple values from a function. In this case, we're returning a RaycastHit object that contains information about the object that was hit by the raycast.
        if (Physics.Raycast(transform.position, moveDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
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
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        //Seperate the logic
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);


        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        if (!canMove)
        {
            //cannot move towards moveDir
            //Attempt to move in the x direction
            Vector3 xDir = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, xDir, moveDistance);
            if (canMove)
            {
                moveDir = xDir;
            }
            else
            {
                //cannot move in the x direction  
                //Attempt to move in the z direction
                Vector3 zDir = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, zDir, moveDistance);
                if (canMove)
                {
                    moveDir = zDir;
                }
                else
                {
                    //cannot move in any direction
                }
            }

        }



        if (canMove)
        {
            transform.position += moveDir * moveDistance;//deltaTime is the time between frames. By multiplying the moveDir by deltaTime, we ensure that the player moves at the same speed regardless of the frame rate.
        }


        isWalking = moveDir != Vector3.zero;


        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        //Slerp is a function that smoothly interpolates between two vectors. In this case, we're using it to smoothly rotate the player to face the direction they're moving in.
        //Interpolate means to 
    }
    public bool IsWalking()
    {
        return isWalking;
    }


    private void SetSelectedCounter(BaseCounter baseCounter)
    {
        this.selectedCounter = baseCounter;
        OnSelectedCounterChanged?.Invoke(this,
        new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = baseCounter
        });
    }

    public GameObject GetKitchenObjectHoldPoint()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

}
