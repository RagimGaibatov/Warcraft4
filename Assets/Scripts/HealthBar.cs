using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour{
    public Transform ScaleTransform;
    public Transform Target;

    private Transform _CameraTransform;

    private void Start(){
        _CameraTransform = Camera.main.transform;
    }

    private void LateUpdate(){
        transform.position = Target.position + Vector3.up * 2f;
        transform.rotation = _CameraTransform.rotation;
    }

    public void SetUp(Transform target){
        Target = target;
    }

    public void SetHealth(int health, int maxHealth){
        float xScale = (float) health / maxHealth;
        xScale = Mathf.Clamp01(xScale);
        ScaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
}