using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null; //Clear all event listeners
    }
    [SerializeField] private GameObject counterTopPoint;

    private KitchenObject kitchenObject;
    private bool isSelectedCounter = false;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == this)
        {
            isSelectedCounter = true;
        }
        else
        {
            isSelectedCounter = false;
        }
    }
    public virtual void Interact(Player player)
    {
        //This should not ever be triggered. 
        //This method should be overridden by the child class

        Debug.LogError("BaseCounter.Interact() called. This should not happen. This method should be overridden by the child class");

    }

    public virtual void InteractAlternate(Player player)
    {
        //This should not ever be triggered. 
        //This method should be overridden by the child class

        Debug.LogError("BaseCounter.InteractAlternate() called. This should not happen. This method should be overridden by the child class");

    }

    public GameObject GetKitchenObjectHoldPoint()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
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
