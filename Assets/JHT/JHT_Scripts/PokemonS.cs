using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class PokemonS : MonoBehaviour
{
	public int id;
	public string pokeName;
	public int level;
	public int hp;
	public int maxHp;
	public int curExp;
	public int nextExp;

	public Sprite front;
	public Sprite back;
	public Animator animator;
	
	public PokemonStatS baseStat;   // 종족값
	public PokemonIVS iv;           // 개체값
									//public PokemonStat ev;			// 노력치
	public PokemonStatS pokemonStat;    // 현재수치
	public PokeType pokeType1;
	public PokeType pokeType2;

	public bool isDead;

	public List<SkillS> skills;

	public event Action OnDie;
	public event Action OnLevelUp;

	//인스턴스할 포켓몬 능력치
	public PokemonS(int _id, string _name, int _level, PokemonStatS _baseStat, PokemonIVS _iv,
		PokeType _pokeType1, PokeType _pokeType2,List<SkillS> _skills)
	{
		id = _id;
		pokeName = _name;
		level = _level;
		baseStat = _baseStat;
		iv = _iv;
		pokeType1 = _pokeType1;
		pokeType2 = _pokeType2;

		pokemonStat = GetStat();
		curExp = 0;
		nextExp = 999;
		hp = maxHp;
		this.skills = _skills;

		isDead = false;
	}

	private void OnEnable()
	{
		OnDie += Die;
		OnLevelUp += UpdateLevel;
	}
	private void OnDisable()
	{
		OnDie -= Die;
		OnLevelUp -= UpdateLevel;
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	// 개체값 종족값 레벨을 계산해서 기본 스탯 반환
	private PokemonStatS GetStat()
	{
		return new PokemonStatS(
			hp: ((baseStat.hp * 2 + iv.hp) * level) / 100 + level + 10,
			attack: ((baseStat.attack * 2 + iv.attack) * level) / 100 + 5,
			defense: ((baseStat.defense * 2 + iv.defense) * level) / 100 + 5,
			speAttack: ((baseStat.speAttack * 2 + iv.speAttack) * level) / 100 + 5,
			speDefense: ((baseStat.speDefense * 2 + iv.speDefense) * level) / 100 + 5,
			speed: ((baseStat.speed * 2 + iv.speed) * level) / 100 + 5
		);
	}

	//다음레벨 경험치 -> 추후 진화이벤트, 스킬 추가이벤트 넣어야됨
	public int GetNextExp(int level)
	{
		if (0 <= level && level < 5)
			return level * level * 2;
		else if (5 <= level && level < 10) 
			return level * level * 4;
		else if (10 <= level && level < 20)
			return level * level * 8;
		else
			return level * level * 8;
	}

	//다음레벨 경험치
	public void GeNextExp(int amount)
	{
		curExp += amount;
		while (curExp >= nextExp)
		{
			curExp -= nextExp;
			OnLevelUp?.Invoke();
		}

	}

	//상태 데미지(defence조절)
	public void TakeStat(PokemonS attacker, PokemonS defender, SkillS skill)
	{
		if (defender == null || isDead) return;
		skill.damage += TypeSumCalculator(attacker, defender);
		pokemonStat.defense = Mathf.Max(0, (int)(pokemonStat.defense - skill.damage));

	}

	public void TakeMyStat(PokemonS attacker, SkillS skill)
	{
		if (attacker == null || isDead) return;
		int basicStat = pokemonStat.defense;
		pokemonStat.defense += (int)skill.damage;

		//if(//게임이 끝났을 때)
		//{
		//	pokemonStat.defense = basicStat;
		//}
	}

	//일반 데미지
	public void TakeDamage(PokemonS attacker,PokemonS defender, SkillS skill)
	{
		if (defender == null || isDead) return;

		skill.damage += TypeSumCalculator(attacker,defender);

		Debug.Log($"{skill.damage}만큼 데미지를 받습니다");
		pokemonStat.hp = Mathf.Max(0, (int)(pokemonStat.hp - skill.damage));
		Debug.Log($"현재 체력 : {pokemonStat.hp}");

		if (hp == 0)
		{
			OnDie?.Invoke();
		}
	}

	//체력0 이벤트
	public void Die()
	{
		isDead = true;
		Debug.Log($"{gameObject.name}이 사망했습니다");
		//Destroy(this.gameObject);
		//if (PokemonManagerS.Get.party.Contains(해당 포켓몬이 존재 할경우))
		//{
		//	PokemonManagerS.Get.party.Remove(해당 포켓몬 리스트에서 삭제); //Or RemoveAt인덱스 삭제
		//}

		//if (PokemonManagerS.Get.party.Count > 0)
		//{
		//	Debug.Log("다음 포켓몬을 꺼내겠습니다");
		//}
		//else
		//{
		//	Debug.Log("다음 포켓몬이 없습니다");
		//	//battle씬에서 나감
		//}
	}	

	//레벨 이벤트
	public void UpdateLevel()
	{
		if (level <= 20)
		{
			level++;

			int oldMaxHp = maxHp;
			pokemonStat = GetStat(); 

			int newHp = maxHp - oldMaxHp;
			Heal(newHp);
		}
		else
		{
			Debug.Log("최대레벨 입니다");
			curExp = 0;
		}
	}

	public int Heal(int amount)
	{
		pokemonStat.hp += Mathf.Min(maxHp, pokemonStat.hp + amount);
		return pokemonStat.hp;
	}

	//타입별 계산 합
	#region Type Damage Sum

	public float TypeSumCalculator(PokemonS attack, PokemonS defense)
	{
		float firstDamageRate = TypeCalculator(attack.pokeType1, defense.pokeType1);
		float secondDamageRate = TypeCalculator(attack.pokeType2, defense.pokeType2);

		return firstDamageRate * secondDamageRate;
	}

	#endregion

	//포켓몬 타입별 damage계산
	#region Type Damage
	public float TypeCalculator(PokeType attack, PokeType defense)
	{

		if (defense == PokeType.None) return 1f;

		if (attack == PokeType.Normal)
		{
			if (defense == PokeType.Rock || defense == PokeType.Steel) return 0.5f;
			if (defense == PokeType.Ghost) return 0.0f;
		}
		else if (attack == PokeType.Fire)
		{
			if (defense == PokeType.Grass || defense == PokeType.Ice || defense == PokeType.Bug || defense == PokeType.Steel) return 2f;
			if (defense == PokeType.Fire || defense == PokeType.Water || defense == PokeType.Rock || defense == PokeType.Dragon) return 0.5f;
		}
		else if (attack == PokeType.Water)
		{
			if (defense == PokeType.Fire || defense == PokeType.Ground || defense == PokeType.Rock) return 2f;
			if (defense == PokeType.Water || defense == PokeType.Grass || defense == PokeType.Dragon) return 0.5f;
		}
		else if (attack == PokeType.Electric)
		{
			if (defense == PokeType.Water || defense == PokeType.Flying) return 2f;
			if (defense == PokeType.Electric || defense == PokeType.Grass || defense == PokeType.Dragon) return 0.5f;
		}
		else if (attack == PokeType.Grass)
		{
			if (defense == PokeType.Water || defense == PokeType.Ground || defense == PokeType.Rock) return 2f;
			if (defense == PokeType.Fire || defense == PokeType.Grass || defense == PokeType.Poison || defense == PokeType.Flying || defense == PokeType.Bug || defense == PokeType.Dragon || defense == PokeType.Steel) return 0.5f;
		}
		else if (attack == PokeType.Ice)
		{
			if (defense == PokeType.Grass || defense == PokeType.Ground || defense == PokeType.Flying || defense == PokeType.Dragon) return 2f;
			if (defense == PokeType.Fire || defense == PokeType.Water || defense == PokeType.Ice || defense == PokeType.Steel) return 0.5f;
		}
		else if (attack == PokeType.Fighting)
		{
			if (defense == PokeType.Normal || defense == PokeType.Ice || defense == PokeType.Rock || defense == PokeType.Dark || defense == PokeType.Steel) return 2f;
			if (defense == PokeType.Poison || defense == PokeType.Flying || defense == PokeType.Psychic || defense == PokeType.Bug) return 0.5f;
			if (defense == PokeType.Ghost) return 0f;
		}
		else if (attack == PokeType.Poison)
		{
			if (defense == PokeType.Grass) return 2f;
			if (defense == PokeType.Poison || defense == PokeType.Ground || defense == PokeType.Rock || defense == PokeType.Ghost) return 0.5f;
			if (defense == PokeType.Steel) return 0f;
		}
		else if (attack == PokeType.Ground)
		{
			if (defense == PokeType.Fire || defense == PokeType.Electric || defense == PokeType.Poison || defense == PokeType.Rock || defense == PokeType.Steel) return 2f;
			if (defense == PokeType.Grass || defense == PokeType.Bug) return 0.5f;
			if (defense == PokeType.Flying) return 0f;
		}
		else if (attack == PokeType.Flying)
		{
			if (defense == PokeType.Grass || defense == PokeType.Fighting || defense == PokeType.Bug) return 2f;
			if (defense == PokeType.Electric || defense == PokeType.Rock || defense == PokeType.Steel) return 0.5f;
		}
		else if (attack == PokeType.Psychic)
		{
			if (defense == PokeType.Fighting || defense == PokeType.Poison) return 2f;
			if (defense == PokeType.Psychic || defense == PokeType.Steel) return 0.5f;
			if (defense == PokeType.Dark) return 0f;
		}
		else if (attack == PokeType.Bug)
		{
			if (defense == PokeType.Grass || defense == PokeType.Psychic || defense == PokeType.Dark) return 2f;
			if (defense == PokeType.Fire || defense == PokeType.Fighting || defense == PokeType.Poison || defense == PokeType.Flying || defense == PokeType.Ghost || defense == PokeType.Steel) return 0.5f;
		}
		else if (attack == PokeType.Rock)
		{
			if (defense == PokeType.Fire || defense == PokeType.Ice || defense == PokeType.Flying || defense == PokeType.Bug) return 2f;
			if (defense == PokeType.Fighting || defense == PokeType.Ground || defense == PokeType.Steel) return 0.5f;
		}
		else if (attack == PokeType.Ghost)
		{
			if (defense == PokeType.Psychic || defense == PokeType.Ghost) return 2f;
			if (defense == PokeType.Dark || defense == PokeType.Steel) return 0.5f;
			if (defense == PokeType.Normal) return 0f;
		}
		else if (attack == PokeType.Dragon)
		{
			if (defense == PokeType.Dragon) return 2f;
			if (defense == PokeType.Steel) return 0.5f;
		}
		else if (attack == PokeType.Dark)
		{
			if (defense == PokeType.Psychic || defense == PokeType.Ghost) return 2f;
			if (defense == PokeType.Fighting || defense == PokeType.Dark || defense == PokeType.Steel) return 0.5f;
		}
		else if (attack == PokeType.Steel)
		{
			if (defense == PokeType.Ice || defense == PokeType.Rock) return 2f;
			if (defense == PokeType.Fire || defense == PokeType.Water || defense == PokeType.Electric || defense == PokeType.Steel) return 0.5f;
		}

		return 1f;
	}
	#endregion
}
