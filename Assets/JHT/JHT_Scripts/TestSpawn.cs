using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
	public GameObject myObject;
	public GameObject enemyObject;
	public Transform myPos;
	public Transform enemyPos;

	private void Start()
	{
		if (myObject == null || enemyObject == null) return;
		Instantiate(myObject, myPos.position, Quaternion.identity);
		Instantiate(enemyObject, enemyPos.position, Quaternion.identity);
	}
}
