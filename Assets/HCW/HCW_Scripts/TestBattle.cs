using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBattle : MonoBehaviour
{
	// void Start()
	// {
	// 	// 스킬 생성
	// 	var fireball = new Skill("Fireball", 25);
	// 	var tackle = new Skill("Tackle", 10);
	// 
	// 	// 플레이어 포켓몬
	// 	var pikachu = new Pokémon("Pikachu", maxHp: 100, speed: 30, skills: new List<Skill> { fireball, tackle });
	// 		
	// 	
	// 
	// 	// 상대 포켓몬
	// 	var eevee = new Pokémon("Eevee", maxHp: 80, speed: 25, skills: new List<Skill> { fireball, tackle });
	// 
	// 	// BattleManager 찾아서 배틀 시작
	// 	var bm = FindObjectOfType<BattleManager>();
	// 	if (bm != null)
	// 	{
	// 		bm.StartBattle(new List<Pokemon> { pikachu }, new List<Pokemon> { eevee });
	// 	}
	// 	else
	// 	{
	// 		Debug.LogError("씬에 BattleManager가 없습니다!");
	// 	}
	// }
}
