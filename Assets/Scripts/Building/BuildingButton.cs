using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour{
    public BuildingPlacer BuildingPlacer;
    public GameObject BuildingPrefab;

    [SerializeField] private Text priceText;

    private void Start(){
        priceText.text = BuildingPrefab.GetComponent<Building>().price.ToString();
    }

    public void TryBuy(){
        int price = BuildingPrefab.GetComponent<Building>().price;
        if (FindObjectOfType<Resources>().Money >= price){
            BuildingPlacer.CreateBuidling(BuildingPrefab);
        }
    }
}