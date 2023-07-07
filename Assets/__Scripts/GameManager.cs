using System;
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
        SwitchState(GameState.MainMenu);
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
}
