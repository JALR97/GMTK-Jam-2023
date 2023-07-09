using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Subscribing to Events:
/*
    private void Awake() {
        GameManager.OnGameStateChange += StateChange;
    }
    
    private void OnDestroy() {
        GameManager.OnGameStateChange -= StateChange;
    }

    private void StateChange(GameManager.GameState newState) {
        //Do something depending on the newState
        ;
    }
*/

public class GameManager : MonoBehaviour {
    ///Singleton class to manage the game on a high level
    public static GameManager Instance;
    public GameState State;

    public static event Action<GameState> OnGameStateChange; 
    public enum GameState {
        MainMenu,
        Game,
        Paused,
        GameOver
    }
    //**    ---Components---    **//
    

    //**    ---Variables---    **//
    //  [[ balance control ]] 
    
    //  [[ internal work ]] 
    public HashSet<IUnit> SelectedUnits = new HashSet<IUnit>();
    public List<IUnit> AvailableUnits = new List<IUnit>();
    
    //**    ---Properties---    **//
    

    //**    ---Functions---    **//
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        SwitchState(GameState.Game);
    }  
    
    public void SwitchState(GameState newState) {
        State = newState;
        switch (newState) {
            case GameState.MainMenu:
                break;
            case GameState.Game:
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
        }
        OnGameStateChange?.Invoke(newState);
    }

    public void Select(IUnit unit) {
        SelectedUnits.Add(unit);
        unit.Selected();
    }
    public void Deselect(IUnit unit) {
        SelectedUnits.Remove(unit);
        unit.Deselected();
    }
    public void DeselectAll(){
        foreach (var unit in SelectedUnits) {
            unit.Deselected();
        }
        SelectedUnits.Clear();
    }
    public void MoveCommand(Vector3 movePosition) {
        if (State != GameState.Game) {
            return;
        }
        Debug.Log("Exec Moves");
        foreach (var unit in SelectedUnits) {
            Debug.Log("one unit Move");
            unit.Command(movePosition);
        }
    }

}

