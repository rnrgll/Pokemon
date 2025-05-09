using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static void CreateInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                //ToDo : 임시 인스턴스화 (추후 수정 예정)
                T prefab = Resources.Load<T>($"Managers_Prefabs/{typeof(T).Name}");
                _instance = Instantiate(prefab);
            }
            DontDestroyOnLoad(_instance.gameObject);
        }
        
    }

    public static void ReleaseInstance()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }

    public static T GetInstance()
    {
	    return _instance;
    }


    protected virtual void Awake()
    {
	    //이미 생성된게 있으면 자기 자신 파괴
	    if (_instance != null && _instance != this)
	    {
		    Debug.Log(name);
		    Destroy(gameObject);
	    }
	    
	    Init();
	    
	    
    }

    protected virtual void Init()
    {
	    
    }
}
