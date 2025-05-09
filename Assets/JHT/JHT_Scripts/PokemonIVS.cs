using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonIVS : MonoBehaviour
{
	public int hp;
	public int attack;
	public int defense;
	public int speAttack;
	public int speDefense;
	public int speed;

	public PokemonIVS(int hp, int attack, int defense, int speAttack, int speDefense, int speed)
	{
		this.hp = hp;
		this.attack = attack;
		this.defense = defense;
		this.speAttack = speAttack;
		this.speDefense = speDefense;
		this.speed = speed;
	}

	public static PokemonIVS GetRandomIV()
	{
		// 개체값 랜덤 반환
		return new PokemonIVS
			(
				UnityEngine.Random.Range(0, 32),  // 체력
				UnityEngine.Random.Range(0, 32),  // 공격
				UnityEngine.Random.Range(0, 32),  // 방어
				UnityEngine.Random.Range(0, 32),  // 특공
				UnityEngine.Random.Range(0, 32),  // 특방
				UnityEngine.Random.Range(0, 32)   // 스핏
			);
	}
}
