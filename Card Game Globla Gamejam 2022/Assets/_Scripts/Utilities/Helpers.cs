using UnityEngine;

/// <summary>
/// Una clase estatica con metodos utiles de ayuda
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Destruye todos los objetos hijos de este transform
    /// Se usa asi:
    /// <code>
    /// transform.DestroyChildren();
    /// </code>
    /// </summary>
    public static void DestroyChildren(this Transform t){
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
}
