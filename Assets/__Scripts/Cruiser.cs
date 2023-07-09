using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Cruiser : MonoBehaviour, IUnit
{
    //**    ---Components---    **//
    //  [[ set in editor ]] 
    [SerializeField] private NavMeshAgent agent;
    private Klaud Klaud;
    [SerializeField] private GameObject selectedIndicator;
    //  [[ set in Start() ]] 
        
    
    //**    ---Variables---    **//
    //  [[ balance control ]] 
    public float viewRange;
    //  [[ internal work ]] 
    private bool chasing = false;
    
    //**    ---Properties---    **//
    
    
    //**    ---Functions---    **//
    private void Update() {
        if ((Klaud.transform.position - transform.position).magnitude < viewRange && !chasing) {
            Klaud.Found();
            chasing = true;
        }else if ((Klaud.transform.position - transform.position).magnitude >= viewRange && chasing) {
            Klaud.ViewBroken();
            chasing = false;
        }
    }

    private void Start() {
        GameManager.Instance.AvailableUnits.Add((IUnit)this);
        Klaud = GameObject.FindGameObjectWithTag("Klaud").GetComponent<Klaud>();
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
