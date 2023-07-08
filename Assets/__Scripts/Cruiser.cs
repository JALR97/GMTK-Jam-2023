using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cruiser : MonoBehaviour, IUnit
{
    //**    ---Components---    **//
    //  [[ set in editor ]] 
    [SerializeField] private NavMeshAgent agent;
    //  [[ set in Start() ]] 
    
    
    //**    ---Variables---    **//
    //  [[ balance control ]] 
    
    //  [[ internal work ]] 
    
    
    //**    ---Properties---    **//
    
    
    //**    ---Functions---    **//
    public void Command(Vector3 target) {
        agent.SetDestination(target);
    }

    public void Selected() {
        //Show unit is selected
    }

    public void Deselected() {
        //Show unit is not selected
    }
}
