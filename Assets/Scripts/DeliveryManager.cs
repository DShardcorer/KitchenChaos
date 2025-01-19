using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;




    public static DeliveryManager Instance { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;

    private int maxWaitingRecipeCount = 4;

    private int deliveredRecipeCount = 0;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }
    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (GameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < maxWaitingRecipeCount)
            {

                RecipeSO recipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(recipeSO);
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

public void DeliverPlate(PlateKitchenObject plateKitchenObject)
{
    // Get the set of kitchen objects from the delivered plate
    HashSet<KitchenObjectSO> plateKitchenObjectSet = new HashSet<KitchenObjectSO>(plateKitchenObject.GetKitchenObjectSOList());

    foreach (RecipeSO waitingRecipeSO in waitingRecipeSOList)
    {
        // Get the set of kitchen objects for the current recipe
        HashSet<KitchenObjectSO> waitingRecipeSOSet = new HashSet<KitchenObjectSO>(waitingRecipeSO.kitchenObjectSOList);

        // Check if the plate matches the recipe (same number of items and exact items)
        if (waitingRecipeSOSet.SetEquals(plateKitchenObjectSet))
        {
            // Recipe success: invoke events, remove recipe, destroy plate, and log success
            OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
            OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            waitingRecipeSOList.Remove(waitingRecipeSO);
            plateKitchenObject.DestroySelf();
            deliveredRecipeCount++;
            Debug.Log("Recipe Delivered Successfully");
            return; // Exit after a successful match
        }
    }

    // If no matches are found, it's a recipe failure
    OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    plateKitchenObject.DestroySelf();
    Debug.Log("Recipe Failed");
}


    public List<RecipeSO> GetWaitingRecipeSOList(){
        return waitingRecipeSOList;
    }

    public int GetDeliveredRecipeCount(){
        return deliveredRecipeCount;
    }


}
