using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject{
    public NavMeshAgent NavMeshAgent;
    public int price;
    public int Health = 15;
    private int _maxHealth;

    public GameObject HealthBarPrefab;
    private HealthBar _healthBar;

    public override void Start(){
        base.Start();
        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.SetUp(transform);
    }

    public override void WhenClickOnGround(Vector3 point){
        NavMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue){
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0){
            //die
            Destroy(gameObject);
        }
    }

    private void OnDestroy(){
        FindObjectOfType<Management>().Unselect(this);
        if (_healthBar){
            Destroy(_healthBar.gameObject);
        }
    }
}