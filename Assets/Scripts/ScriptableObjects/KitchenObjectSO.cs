using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{

    [SerializeField]private GameObject prefab;
    [SerializeField]private Sprite sprite;
    [SerializeField]private string objectName;


    public GameObject Prefab
    {
        get => prefab;
        set => prefab = value;
    }

    public Sprite Sprite
    {
        get => sprite;
        set => sprite = value;
    }

    public string ObjectName
    {
        get => objectName;
        set => objectName = value;
    }
     
}
