using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private GameObject counterTopPoint;
    [SerializeField] private GameObject plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }


    private void Start()
    {

        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }



    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        GameObject plateVisualGameObject = Instantiate(plateVisualPrefab, counterTopPoint.transform);

        float offsetY = 0.1f;
        plateVisualGameObject.transform.localPosition = new Vector3(0, offsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateVisualGameObject);
    }
    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        if (plateVisualGameObjectList.Count > 0)
        {
            GameObject plateVisualGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
            plateVisualGameObjectList.RemoveAt(plateVisualGameObjectList.Count - 1);
            Destroy(plateVisualGameObject);
        }
    }
}
