using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour{
    public int Money;

    [SerializeField] private Text moneyText;

    private void Update(){
        moneyText.text = "Money: " + Money;
    }
}