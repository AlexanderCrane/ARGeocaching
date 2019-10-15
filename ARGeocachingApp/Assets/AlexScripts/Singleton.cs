using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For any class that needs to be a singleton, you can have it inherit from this
// instead of Monobehavior (insert the class name in for <T>)
public class Singleton<T> : MonoBehaviour where T: MonoBehaviour{

    private T instance;

    public T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            return instance;
        }
    }

	
}
