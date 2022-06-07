using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject{
    public int price;
    public int xSize = 3;
    public int zSize = 3;

    private Color _startColor;
    public Renderer Renderer;

    public GameObject MenuObject;

    public override void Start(){
        base.Start();
        MenuObject.SetActive(false);
    }

    private void Awake(){
        _startColor = Renderer.material.color;
    }

    public override void Select(){
        base.Select();
        MenuObject.SetActive(true);
    }

    public override void Unselect(){
        base.Unselect();
        MenuObject.SetActive(false);
    }


    private void OnDrawGizmos(){
        float cellSize = FindObjectOfType<BuildingPlacer>().cellSize;

        for (int x = 0; x < xSize; x++){
            for (int z = 0; z < zSize; z++){
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize,
                    new Vector3(1f, 0, 1f) * cellSize);
            }
        }
    }

    public void DisplayUnacceptablePosition(){
        Renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition(){
        Renderer.material.color = _startColor;
    }
}