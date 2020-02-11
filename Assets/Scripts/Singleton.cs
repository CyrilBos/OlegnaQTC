using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    public static bool IsInitialized
    {
        get { return Instance != null; }
    }

    protected void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Trying to instantiate a second instance of a Singleton class");
        }
        else
        {
            Instance = (T) this;
        }
    }
    
    protected void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
