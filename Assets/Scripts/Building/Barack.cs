using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Barack : Building{
    public Transform spawn;

    public void CreateUnit(GameObject unitPrefab){
        GameObject newUnit = Instantiate(unitPrefab, spawn.position, Quaternion.identity);
        Vector3 position = spawn.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 0f));
        newUnit.GetComponent<Unit>().WhenClickOnGround(position);
    }
}