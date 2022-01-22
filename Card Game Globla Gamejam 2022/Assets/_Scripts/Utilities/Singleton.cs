using UnityEngine;


/// <summary>
/// Una clase de tipo estatica que cuando detecta un nuevo objeto con esta clase
/// borra el viejo y se queda con el nuevo, como una especie de reset
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;

    protected virtual void OnApplicationQuit(){
        Instance = null;
        Destroy(gameObject);
    }
}
/// <summary>
/// Un singleton real que en una escena solo deja que exista uno solo de este objeto. 
/// </summary>
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        base.Awake();
    }
}

/// <summary>
/// Un singleton real que ademas es persistente durante todas las escenas. 
/// </summary>
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour{
    protected override void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}
