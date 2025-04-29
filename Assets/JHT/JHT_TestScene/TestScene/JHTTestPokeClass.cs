using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestPoke", menuName = "TestPokeData/PokeData")]
public class JHTTestPokeClass : ScriptableObject
{
	public JHTCharType type;
    public bool isMyPoke;
	public int id;
	public Sprite icon;
    public int damage;
    public int level;
	//public int exp;
	public int hp;
	[SerializeField] GameObject prefab;
	//public int prefabIndex;
	//public GameObject[] levelPrefab;
	//public GameObject curLevelPrefab = null;

	
}
