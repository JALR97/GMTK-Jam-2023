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
    private bool captured = false;

    private int copsInRange = 0;
    private int copsEngaging = 0;

    public float timeToHide = 2;
    public float timeToRoam = 5;
    public float chillTime = 1;
    public float hiddenSpeed = 2.5f;
    public float roamSpeed = 3;
    public float runSpeed = 3.5f;
    public float graceViewTime = 2;
    public float timeToCatch = 5;
    public enum KlaudState {
        ROAM,
        HIDE,
        RUN,
        ENGAGE,
        DONE
    }

    public void Found() {
        copsInRange += 1;
        if (copsInRange == 1) {
            agent.speed = runSpeed;
            State = KlaudState.RUN;
            timerStart = Time.time;
            outOfView = false;
            Model.SetActive(true);
        }
    }

    public void ViewBroken() {
        copsInRange -= 1;
        if (copsInRange == 0) {
            timerStart = Time.time;
        }
    }
    
    public void CopEng() {
        copsEngaging += 1;
    }

    public void CopDiseng() {
        copsEngaging -= 1;
    }
    

    private void Update() {
        if (!captured) {
            Evaluate();
        }
        else {
            agent.speed = 0;
        }
    }
    public void Evaluate(){
        switch (State) {
            case KlaudState.ROAM:
                
                break;
            case KlaudState.HIDE:
                if (Time.time >= timerStart + timeToRoam) {
                    State = KlaudState.ROAM;
                    MoveRNode(-1);
                    agent.speed = roamSpeed;
                }
                break;
            case KlaudState.RUN:
                if (outOfView && Time.time >= timerStart + timeToHide) {
                    State = KlaudState.HIDE;
                    agent.speed = hiddenSpeed;
                    MoveRNode(-1);
                    timerStart = Time.time;
                }

                if (!outOfView && copsInRange > 0) {
                    Debug.Log($"acab {Time.time - timerStart}");
                }

                if (!outOfView && copsInRange == 0 && Time.time >= timerStart + graceViewTime) {
                    outOfView = true;
                    Model.SetActive(false);
                    timerStart = Time.time;
                }
                else if (!outOfView && copsInRange > 0 && Time.time >= timerStart + timeToCatch) {
                    Debug.Log("Jhonny 5O");
                    GameManager.Instance.Win();
                    State = KlaudState.DONE;
                    captured = true;
                }
                 
                //away from cops
                
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
