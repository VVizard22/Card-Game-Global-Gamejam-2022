using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Un sistema singleton que persiste durante todo el proyecto al que llamaremos cada vez que 
/// querramos manejar algo de cambio de escenas.
/// </summary>
public class SceneSystem : Singleton<SceneSystem>
{

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    private float _target;
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);}
        /*
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);
        _target = 0;
        _progressBar.fillAmount = 0;

        do{
            //for testing purposes
            await System.Threading.Tasks.Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);

        await System.Threading.Tasks.Task.Delay(1000);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }*/

    public void ExitGame(){
        Application.Quit();
    }

    void Update()
    {
        //_progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }
}
