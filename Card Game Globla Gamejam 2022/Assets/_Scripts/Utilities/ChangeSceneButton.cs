using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    public void ChangeScene(string sceneName) {
        SceneSystem.Instance.LoadScene(sceneName);
    }

    public void ExitGame(){
        SceneSystem.Instance.ExitGame();
    }
}
