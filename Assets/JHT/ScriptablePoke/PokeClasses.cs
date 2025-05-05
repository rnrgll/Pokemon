using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokeClasses", menuName = "PokeData/PokeData")]
public class PokeClasses : ScriptableObject
{
	public int id;
	public string pokeName;
	public PokeClassType type;
	public Sprite icon;
	public float power;
	public int maxExp;
	public int maxHp;
	public bool isMine;
	//[SerializeField] List<PokeSkills> skill;

	//private void Start()
	//{
	//	skill = new List<PokeSkills>();
	//}
}
public enum PokeClassType
{
	fire,
	elec,
	fly,
	ground
}
