using System;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T s_instance;
    public static T Instance { get; }
    
    public static bool IsInitialized
    {
        get { return s_instance != null; }
    }

    protected void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogError("Trying to instantiate a second instance of a Singleton class");
        }
        else
        {
            s_instance = (T) this;
        }
    }
    
    protected void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}
