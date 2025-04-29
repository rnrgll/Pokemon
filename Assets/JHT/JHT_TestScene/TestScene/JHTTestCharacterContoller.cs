using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTTestCharacterContoller : MonoBehaviour
{
	[SerializeField] GameObject prefab;
	[SerializeField] Transform spawnPosition;
	private void Start()
	{
		
	}

	public void Spawn()
	{
		
		Instantiate(prefab, spawnPosition.position, Quaternion.identity);
		
	}
}
