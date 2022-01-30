using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string SceneName;
    public void LoadAScene(){
        SceneSystem.Instance.LoadScene(SceneName);
    }
}
