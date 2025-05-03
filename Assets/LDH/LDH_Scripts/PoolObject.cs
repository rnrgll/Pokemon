using UnityEngine;

public class PoolObject<T> : MonoBehaviour where T : Component
{
	public ObjectPool<T> pool; //release 될 pool
	

	public void SetPool(ObjectPool<T> pool)
	{
		this.pool = pool;
	}

	public void ReturnToPool()
	{
		if (pool != null)
			pool.Release(this as T);
		else
			Destroy(gameObject);
	}
}
