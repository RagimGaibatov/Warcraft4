using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building{
    private Resources _resources;
    [SerializeField] private float timeToEarn = 8f;
    [SerializeField] private int gold = 5;
    private float _time;

    public override void Start(){
        base.Select();
        _resources = FindObjectOfType<Resources>();
    }


    private void Update(){
        _time += Time.deltaTime;
        if (_time >= timeToEarn){
            _resources.Money += gold;
            _time = 0;
        }
    }
}