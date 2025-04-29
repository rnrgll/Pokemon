using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTTestCharacterContoller : MonoBehaviour
{
	[SerializeField] JHTTestPokeClass pokeClass;
	[SerializeField] Transform spawnPosition;
	private void Start()
	{
		pokeClass.curPrefab = pokeClass.levelPrefab[pokeClass.prefabIndex];
	}

	public void Spawn()
	{
		
		Instantiate(pokeClass.curPrefab, spawnPosition.position, Quaternion.identity);
		
	}
}
