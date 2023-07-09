using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Klaud : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform navNodes;
    [SerializeField] private KlaudState State;
    [SerializeField] private GameObject Model;
    private float timerStart = 0;
    private int goalNode = -1;
    public bool outOfView = true;

    private int copsInRange = 0;
    private int copsEngaging = 0;

    public float timeToHide = 2;
    public float timeToRoam = 5;
    public float chillTime = 1;
    public float hiddenSpeed = 2.5f;
    public float roamSpeed = 3;
    public float runSpeed = 3.5f;
    public float graceViewTime = 2;
    public enum KlaudState {
        ROAM,
        HIDE,
        RUN,
        ENGAGE
    }

    public void Found() {
        copsInRange += 1;
        Debug.Log($"Found. {copsInRange}");
        if (copsInRange == 1) {
            agent.speed = runSpeed;
            State = KlaudState.RUN;
            outOfView = false;
            Model.SetActive(true);
        }
    }

    public void ViewBroken() {
        copsInRange -= 1;
        Debug.Log($"Broken. {copsInRange}");
        if (copsInRange == 0) {
            timerStart = Time.deltaTime;
        }
    }
    
    public void CopEng() {
        copsEngaging += 1;
    }

    public void CopDiseng() {
        copsEngaging -= 1;
    }
    

    private void Update() {
        Evaluate();
    }
    public void Evaluate(){
        switch (State) {
            case KlaudState.ROAM:
                
                break;
            case KlaudState.HIDE:
                if (Time.time >= timerStart + timeToRoam) {
                    State = KlaudState.ROAM;
                    MoveRNode(-1);
                }
                break;
            case KlaudState.RUN:
                if (copsInRange == 0 && Time.time >= timerStart + graceViewTime) {
                    outOfView = true;
                    Model.SetActive(false);
                }
                //away from cops
                if (outOfView && Time.time >= timerStart + timeToHide) {
                    State = KlaudState.HIDE;
                    agent.speed = hiddenSpeed;
                    MoveRNode(-1);
                }
                break;
            case KlaudState.ENGAGE:
                
                break;
        }
    }

    private void Start() {
        MoveRNode(-1);
        
    }

    private void MoveRNode(int prevId) {
        while (prevId == goalNode){
            goalNode = Random.Range(0, navNodes.childCount);    
        }
        agent.destination = navNodes.GetChild(goalNode).transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Node") && navNodes.GetChild(goalNode) == other.transform) {
            MoveRNode(goalNode);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
