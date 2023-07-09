using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cruiser : MonoBehaviour, IUnit
{
    //**    ---Components---    **//
    //  [[ set in editor ]] 
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private GameObject selectedIndicator;
    //  [[ set in Start() ]] 
    
    
    //**    ---Variables---    **//
    //  [[ balance control ]] 
    
    //  [[ internal work ]] 
    
    
    //**    ---Properties---    **//
    
    
    //**    ---Functions---    **//
    private void Start() {
        GameManager.Instance.AvailableUnits.Add((IUnit)this);
    }
    private void OnDisable() {
        GameManager.Instance.AvailableUnits.Remove((IUnit)this);
    }
    public void Command(Vector3 target) {
        agent.SetDestination(target);
    }

    public void Selected() {
        selectedIndicator.SetActive(true);
    }

    public void Deselected() {
        selectedIndicator.SetActive(false);
    }

    public Vector3 Position() {
        return transform.position;
    }
}
