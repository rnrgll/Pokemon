using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PokeType
{
	None = 0,
	Water,
	Fire,
	Grass,
	Wind,
	Ice,
	Poison,
	Ground
}

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
	
	[SerializeField] public PokemonStatS baseStat;   // 종족값
	[SerializeField] public PokemonIVS iv;           // 개체값
													//public PokemonStat ev;			// 노력치
	[SerializeField] public PokemonStatS pokemonStat;    // 현재수치
	public PokeType pokeType1;
	public PokeType pokeType2;

	public bool isDead;

	public List<SkillS> skills = new List<SkillS>();

	public event Action OnDie;
	public event Action OnLevelUp;

	public PokemonS(int _id, string _name, int _level, PokemonStatS _baseStat, PokemonIVS _iv, PokeType _pokeType1, PokeType _pokeType2)
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

	public void GeNextExp(int amount)
	{
		curExp += amount;
		while (curExp >= nextExp)
		{
			curExp -= nextExp;
			OnLevelUp?.Invoke();
		}

	}

	public void TakeStatus(PokemonS attacker, int amount)
	{

	}

	public void TakeDamage(PokemonS attacker, int amount)
	{
		if (isDead) return;

		float realDamage = 0;

		if (!WinType(attacker))
		{
			realDamage *= 1.3f;
		}

		amount += (int)realDamage;

		Debug.Log($"{amount}만큼 데미지를 받습니다");
		hp = Mathf.Max(0, hp - amount);
		Debug.Log($"현재 체력 : {hp}");

		if (hp == 0)
		{
			OnDie?.Invoke();
		}
	}

	public void Die()
	{
		isDead = true;
		Debug.Log($"{gameObject.name}이 사망했습니다");
		Destroy(this.gameObject);
		if (PokemonManagerS.Get.party.Count > 0)
		{
			Debug.Log("다음 포켓몬을 꺼내겠습니다");
		}
		else
		{
			Debug.Log("포켓몬이 없습니다");
			//battle씬에서 나감
		}
	}	

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
		hp += Mathf.Min(maxHp, hp + amount);
		return hp;
	}

	public bool WinType(PokemonS attacker)
	{
		if (this.pokeType1 == PokeType.None)
			return false;
		else if (this.pokeType1 == PokeType.Water)
		{
			if (attacker.pokeType1 == PokeType.Grass ||
				attacker.pokeType1 == PokeType.Ice ||
				attacker.pokeType1 == PokeType.Ground)
			{
				return true;
			}
		}
		else if (pokeType1 == PokeType.Fire)
		{
			if (attacker.pokeType1 == PokeType.Water ||
				attacker.pokeType1 == PokeType.Ice ||
				attacker.pokeType1 == PokeType.Ground)
			{
				return true;
			}
		}
		else if (pokeType1 == PokeType.Ice)
		{
			if (attacker.pokeType1 == PokeType.Grass ||
				attacker.pokeType1 == PokeType.Ground ||
				attacker.pokeType1 == PokeType.Wind)
			{
				return true;
			}
		}
		else if (pokeType1 == PokeType.Wind)
		{
			if (attacker.pokeType1 == PokeType.Fire ||
				attacker.pokeType1 == PokeType.Water ||
				attacker.pokeType1 == PokeType.Poison)
			{
				return true;
			}
		}
		else if (pokeType1 == PokeType.Poison)
		{
			if (attacker.pokeType1 == PokeType.Fire ||
				attacker.pokeType1 == PokeType.Water ||
				attacker.pokeType1 == PokeType.Ice)
			{
				return true;
			}
		}
		else if (pokeType1 == PokeType.Ground)
		{
			if (attacker.pokeType1 == PokeType.Wind ||
				attacker.pokeType1 == PokeType.Water ||
				attacker.pokeType1 == PokeType.Poison)
			{
				return true;
			}
		}
		return false;
	}

}
