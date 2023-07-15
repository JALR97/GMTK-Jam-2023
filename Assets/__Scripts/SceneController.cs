using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    //**    ---Functions---    **//
    //Singleton behavior
    // private void Awake() {
    //     if (Instance == null) {
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else {
    //         Destroy(gameObject);
    //     }
    // }
    
    public static void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}