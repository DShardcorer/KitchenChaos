using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //No kitchen object on the counter
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //There is a kitchen object on the counter
            if (player.HasKitchenObject())
            {
                //Player is holding something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a plate
                    plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;//Casting
                    if (plateKitchenObject.TryAddIngredients(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }


                }
                else
                {
                    //Player is holding something that is not a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //There is a plate on the counter
                        plateKitchenObject = GetKitchenObject() as PlateKitchenObject;//Casting
                        if (plateKitchenObject.TryAddIngredients(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                //Player has no kitchen object
                GetKitchenObject().SetKitchenObjectParent(player);
            }

        }
    }



}
