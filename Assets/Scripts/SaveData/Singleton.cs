using System;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : Component
{
    #region Private Data
    private static T _instance;
    //always a reference data type is passed to Lock - object (allocate memory for it)
    private static object _lock = new object();
    #endregion


    public static T Instance
    {
        get
        {
            #region То, что находится внутри конструкции lock
            //bool lockTaken = false;
            //Monitor.Enter(_syncRoot, ref lockTaken);
            //Monitor.Enter(_syncRoot);
            //Monitor.Wait(_syncRoot, 1000);
            //try
            //{

            //}
            //finally
            //{
            //    Monitor.Exit(_syncRoot);
            //} 
            #endregion
            
            lock (_lock)
            {                
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    //if we find more than one such type of object data - Exept
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopening the scene might fix it.");
                        return _instance;
                    }
                    //if you did not find an object on the scene
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        //add the same data type to our GameObject
                        _instance = singleton.AddComponent<T>();
                        //give the name
                        singleton.name = String.Format("{0} {1}", "(singleton) ", typeof(T));
                        //mark it "not deleted"
                        DontDestroyOnLoad(singleton);
                        //say we created this singleton
                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                                  " is needed in the scene, so '" + singleton +
                                  "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                                  _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }


    #region Unity Methods
    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
}