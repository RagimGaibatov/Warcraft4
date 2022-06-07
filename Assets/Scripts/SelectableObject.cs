using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableObject : MonoBehaviour{
    public GameObject selectionIndicator;

    public virtual void Start(){
        selectionIndicator.SetActive(false);
    }

    public virtual void OnHover(){
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnUnhover(){
        transform.localScale = Vector3.one;
    }

    public virtual void Select(){
        selectionIndicator.SetActive(true);
    }

    public virtual void Unselect(){
        selectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point){
    }
}