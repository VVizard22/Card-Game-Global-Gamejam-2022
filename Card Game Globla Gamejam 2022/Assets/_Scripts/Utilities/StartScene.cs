using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] string SceneName;
    void Start(){
        SceneSystem.Instance.LoadScene(SceneName);
    }
}
