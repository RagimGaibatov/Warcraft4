using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour{
    public EnemyState CurrentEnemyState;

    public int Health;
    private int _maxHealth;

    public Building TargetBuilding;
    public Unit TargetUnit;

    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;

    public NavMeshAgent NavMeshAgent;

    public float AttackPeriod = 1f;
    private float _timer;

    public GameObject HealthBarPrefab;
    private HealthBar _healthBar;


    private void Start(){
        SetState(EnemyState.WalkToBuilding);

        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.SetUp(transform);
    }

    private void Update(){
        if (CurrentEnemyState == EnemyState.Idle){
            FindClosestBuidling();
            if (TargetBuilding){
                CurrentEnemyState = EnemyState.WalkToBuilding;
            }

            FindClosestUnit();
        }

        else if (CurrentEnemyState == EnemyState.WalkToBuilding){
            FindClosestUnit();
            if (TargetBuilding == null){
                SetState(EnemyState.Idle);
            }
        }

        else if (CurrentEnemyState == EnemyState.WalkToUnit){
            if (TargetUnit){
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToFollow + 1){
                    SetState(EnemyState.WalkToBuilding);
                }

                if (distance < DistanceToAttack){
                    SetState(EnemyState.Attack);
                }
            }
            else{
                CurrentEnemyState = EnemyState.WalkToBuilding;
            }
        }
        else if (CurrentEnemyState == EnemyState.Attack){
            if (TargetUnit){
                NavMeshAgent.SetDestination(TargetUnit.transform.position);

                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToAttack){
                    SetState(EnemyState.WalkToUnit);
                }

                _timer += Time.deltaTime;
                if (_timer > AttackPeriod){
                    _timer = 0;
                    // deal damage;
                    TargetUnit.TakeDamage(1);
                }
            }
            else{
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    public void SetState(EnemyState enemyState){
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle){
            //
        }

        else if (CurrentEnemyState == EnemyState.WalkToBuilding){
            FindClosestBuidling();
            if (TargetBuilding){
                NavMeshAgent.SetDestination(TargetBuilding.transform.position);
            }
            else{
                CurrentEnemyState = EnemyState.Idle;
            }
        }

        else if (CurrentEnemyState == EnemyState.WalkToUnit){
            //
        }
        else if (CurrentEnemyState == EnemyState.Attack){
            _timer = 0;
        }
    }

    public void FindClosestBuidling(){
        Building[] allBuildings = FindObjectsOfType<Building>();
        Building closestBuilding = null;

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < allBuildings.Length; i++){
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if (distance < minDistance){
                minDistance = distance;
                closestBuilding = allBuildings[i];
            }
        }

        TargetBuilding = closestBuilding;
    }

    public void FindClosestUnit(){
        Unit[] allUnits = FindObjectsOfType<Unit>();
        Unit closestUnit = null;

        float minDistance = Mathf.Infinity;

        for (int i = 0; i < allUnits.Length; i++){
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance){
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }

        if (minDistance < DistanceToFollow){
            TargetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
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
        if (_healthBar){
            Destroy(_healthBar.gameObject);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected(){
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
#endif
}