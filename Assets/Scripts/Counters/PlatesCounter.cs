using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public EventHandler OnPlateSpawned;
    public EventHandler OnPlateRemoved;
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer >= spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;
            if (GameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {

                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }

    }

    public override void Interact(Player player){
        if(!player.HasKitchenObject()){
            //Player has no kitchen object
            if(platesSpawnedAmount>0){
                platesSpawnedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
