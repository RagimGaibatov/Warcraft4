using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour{
    public float cellSize = 1f;

    public Camera RayCastCamera;
    private Plane _plane;

    public Building CurrentBuilding;

    public Dictionary<Vector2Int, Building> BuildingDictionary = new Dictionary<Vector2Int, Building>();


    private void Start(){
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update(){
        if (CurrentBuilding == null){
            return;
        }

        Ray ray = RayCastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / cellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0f, z) * cellSize;

        if (CheckAllow(x, z, CurrentBuilding)){
            CurrentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0)){
                int price = CurrentBuilding.GetComponent<Building>().price;
                if (FindObjectOfType<Resources>().Money >= price){
                    InstallBuilding(x, z, CurrentBuilding);
                    FindObjectOfType<Resources>().Money -= price;
                    CurrentBuilding = null;
                }
            }
        }
        else{
            CurrentBuilding.DisplayUnacceptablePosition();
        }
    }

    bool CheckAllow(int xPosition, int zPosition, Building building){
        for (int x = 0; x < building.xSize; x++){
            for (int z = 0; z < building.zSize; z++){
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (BuildingDictionary.ContainsKey(coordinate)){
                    return false;
                }
            }
        }

        return true;
    }

    void InstallBuilding(int xPosition, int zPosition, Building building){
        for (int x = 0; x < building.xSize; x++){
            for (int z = 0; z < building.zSize; z++){
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);

                BuildingDictionary.Add(coordinate, CurrentBuilding);
            }
        }
    }

    public void CreateBuidling(GameObject buildingPrefab){
        GameObject newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding.GetComponent<Building>();
    }
}