using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.Object;


public class ObjectPool<T> where T : Component
{
	private T prefab;
	private Transform parent;
	private Stack<T> pool = new Stack<T>();
	public int num;//생성할 오브젝트 개수..? 최대 갯수도 정해야할듯
	
	//생성자
	public ObjectPool(T prefab, Transform parent)
	{
		this.prefab = prefab;
		this.parent = parent;
	}

	public void Init(int num)
	{
		for (int i = 0; i < num; i++)
		{
			T instance = Instantiate(prefab, parent);
			instance.gameObject.SetActive(false);

			var poolObject = instance.GetComponent<PoolObject<T>>();
			if (poolObject != null)
				poolObject.SetPool(this);

			pool.Push(instance);
		}
	}
	
	//Get
	 public T Get()
	 {
		 T instance;
	 	if (pool.Count <= 0)
	    {
		    instance = Instantiate(prefab, parent);
	    }
	    else
	    {
		    instance = pool.Pop();
	    }
	    instance.gameObject.SetActive(true);
	    return instance;
	    
	 }
	
	//Release
	public void Release(T poolObject)
	{
		poolObject.gameObject.SetActive(false);
		pool.Push(poolObject);
	}
	
	
	public void Clear()
	{
		foreach (var poolObject in pool)
		{
			Destroy(poolObject.gameObject);
		}
		pool.Clear();
	}
	
}
