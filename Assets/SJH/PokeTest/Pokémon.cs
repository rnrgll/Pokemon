using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Pokémon : MonoBehaviour
{
	[Tooltip("도감 번호")]
	public int id;
	[Tooltip("포켓몬 이름")]
	public string pokeName;
	[Tooltip("포켓몬 레벨")]
	public int level;
	[Tooltip("포켓몬 현재 체력")]
	public int hp;
	[Tooltip("포켓몬 최대 체력")]
	public int maxHp;
	[Tooltip("포켓몬 누적 경험치")]
	public int curExp;
	[Tooltip("포켓몬 다음 경험치")]
	public int nextExp;
	[Tooltip("포켓몬 종족값")]
	[SerializeField] public PokemonStat baseStat;
	[Tooltip("포켓몬 개체값")]
	[SerializeField] public PokemonIV iv;
	//public PokemonStat ev;	// 노력치
	[Tooltip("포켓몬 총 스탯")]
	[SerializeField] public PokemonStat pokemonStat;
	[Tooltip("포켓몬 타입 1")]
	public Define.PokeType pokeType1;
	[Tooltip("포켓몬 타입 2")]
	public Define.PokeType pokeType2;
	[Tooltip("포켓몬 경험치 타입")]
	public Define.ExpType expType;
	[Tooltip("다음 진화 레벨")]
	public int evolveLevel;
	[Tooltip("다음 진화 이름")]
	public string evolveName;
	[Tooltip("포켓몬 기술")]
	public List<string> skills;
	[Tooltip("죽었으면 true / 살았으면 false")]
	public bool isDead;
	[Tooltip("기본 경험치")]
	public int baseExp;

	[Tooltip("상태")] public StatusCondition condition;

	[Tooltip("분노 관리용")]
	public bool isAnger;
	public int angerStack;

	[Tooltip("구르기 관리용")]
	public bool isRollout;
	public int rolloutStack;

	[Tooltip("배틀중일 때 스택 0 ~ 6")]
	public PokemonBattleStat pokemonBattleStack;


	// 생성자는 사용할 수 없으니 Init함수를 실행해서 데이터 할당
	public void Init(int _id, int _level)
	{
		// 데이터 매니저에서 고정데이터 받아오기
		SJH_PokemonData data = Manager.Data.SJH_PokemonData.GetPokemonData(_id);
		SetData(data, _level);
	}
	public void Init(string _name, int _level)
	{
		SJH_PokemonData data = Manager.Data.SJH_PokemonData.GetPokemonData(_name);
		SetData(data, _level);
	}

	private void SetData(SJH_PokemonData data, int _level)
	{
		DontDestroyOnLoad(gameObject);
		// 고정데이터 Id, 이름, 종족값, 타입
		id = data.Id;
		pokeName = data.Name;
		baseStat = data.BaseStat;
		pokeType1 = data.PokeType1;
		pokeType2 = data.PokeType2;
		expType = data.ExpType;
		evolveLevel = data.EvolveLevel;
		evolveName = data.EvolveName;
		baseExp = data.BaseExp;

		// 개별데이터 hp exp iv stat
		level = _level;
		curExp = level == 1 ? 0 : GetExpByLevel(level);   // 1이면 0 아니면 레벨에 맞는 누적 경험치
		nextExp = GetExpByLevel(level + 1) - curExp;      // 다음 레벨까지 필요한 경험치
		isDead = false;
		iv = PokemonIV.GetRandomIV();
		pokemonStat = GetStat();
		hp = pokemonStat.hp;
		maxHp = hp;

		// 스킬 랜덤
		SetSkills(data);

		//상태 정상으로 설정
		condition = StatusCondition.None;

		// 배틀용 스택 0으로 초기화
		pokemonBattleStack = new PokemonBattleStat(0);
	}

	// 개체값 종족값 레벨을 계산해서 기본 스탯 반환
	private PokemonStat GetStat()
	{
		return new PokemonStat(
			hp: ((baseStat.hp * 2 + iv.hp) * level) / 100 + level + 10,
			attack: ((baseStat.attack * 2 + iv.attack) * level) / 100 + 5,
			defense: ((baseStat.defense * 2 + iv.defense) * level) / 100 + 5,
			speAttack: ((baseStat.speAttack * 2 + iv.speAttack) * level) / 100 + 5,
			speDefense: ((baseStat.speDefense * 2 + iv.speDefense) * level) / 100 + 5,
			speed: ((baseStat.speed * 2 + iv.speed) * level) / 100 + 5
		);
	}

	// 경험치 타입에 따라 다음 경험치 반환
	public int GetNextExp()
	{
		int curTotal = GetExpByLevel(level);
		int nextTotal = GetExpByLevel(level + 1);
		return nextTotal - curTotal;
	}

	public int GetExpByLevel(int level)
	{
		switch (expType)
		{
			// 빠른 800,000			EXP = 4 * Level³ / 5
			case Define.ExpType.Fast: return (int)(4f * Mathf.Pow(level, 3) / 5f);
			// 약간 빠름 1,000,000	EXP = Level³
			case Define.ExpType.MediumFast: return (int)(Mathf.Pow(level, 3));
			// 약간 느림 1,059,860	EXP = (6/5) * Level³ - 15 * Level² + 100 * Level - 140
			case Define.ExpType.MediumSlow: return (int)((6f / 5f) * Mathf.Pow(level, 3) - 15 * Mathf.Pow(level, 2) + 100 * level - 140);
			// 약간 빠름 1,000,000	EXP = Level³
			default: return (int)(Mathf.Pow(level, 3));
		}
	}

	// 포켓몬이 생성될 때 스킬 부여
	public void SetSkills(SJH_PokemonData data)
	{
		skills = new List<string>();

		int defaultSkillCount = data.DefaultSkill.Count;
		int learnableSkillCount = 0;

		List<string> learnableSkills = new List<string>();

		// 현재 레벨 이하 배울 수 있는 스킬 리스트
		if (data.SkillDic != null)
		{
			foreach (int skillLevel in data.SkillDic.Keys)
			{
				if (skillLevel <= level)
				{
					string skill = data.SkillDic[skillLevel];
					if (!learnableSkills.Contains(skill))
					{
						learnableSkillCount++;
						learnableSkills.Add(skill);
					}
				}
			}
		}

		// 기본 스킬 + 배울 수 있는 스킬 리스트
		List<string> allSkills = new List<string>();

		// 기본 스킬 추가
		if (data.DefaultSkill != null)
			allSkills.AddRange(data.DefaultSkill);

		// 배울 스킬 추가 (중복체크)
		foreach (string skill in learnableSkills)
		{
			if (!allSkills.Contains(skill))
				allSkills.Add(skill);
		}

		// 배울 스킬이 5개 이상이면 랜덤
		if (allSkills.Count > 4)
		{
			// Fisher-Yates Shuffle
			System.Random ran = new System.Random();
			int n = allSkills.Count;
			while (n > 1)
			{
				n--;
				int k = ran.Next(n + 1);
				(allSkills[k], allSkills[n]) = (allSkills[n], allSkills[k]);
			}

			skills = allSkills.GetRange(0, 4);
		}
		else
		{
			skills = allSkills;
		}
	}

	// 경험치 추가
	public void AddExp(int baseExp)
	{
		// 경험치 = (기본 경험치량 × 트레이너 보너스 × 레벨) / 7
		// TODO : 경험치를 주는쪽에서 계산할지 받는쪽에서 계산할지
		Debug.Log($"{pokeName} : {baseExp} 경험치를 얻었습니다!");
		curExp += baseExp;

		// 누적 경험치가 초과하면 레벨업 반복
		while (curExp >= GetExpByLevel(level + 1))
		{
			Debug.Log($"{pokeName} :  레벨업! {level} → {level + 1}");
			level++;
			// 1. 기술체크
			CheckLearnableSkill();
			// 2. 진화체크
			CheckEvolution();
		}

		// 레벨업 후 경험치 계산
		nextExp = GetExpByLevel(level + 1) - curExp;

		// 재정산 전 최대체력
		int prevMaxHp = maxHp;

		// 스탯 재정산
		pokemonStat = GetStat();

		// 체력 재정산
		maxHp = pokemonStat.hp;

		// 체력 상승분만큼 회복
		int hpDiff = maxHp - prevMaxHp;
		hp += hpDiff;

		// 최대 체력을 초과하지 않게 클램프
		hp = Mathf.Min(hp, maxHp);
	}

	// 레벨업시 배울 수 있는 스킬 체크
	void CheckLearnableSkill()
	{
		var data = Manager.Data.SJH_PokemonData.GetPokemonData(pokeName);

		if (data.SkillDic == null)
			return;

		if (data.SkillDic.TryGetValue(level, out string newSkill))
		{
			if (!skills.Contains(newSkill))
			{
				//메소드로 분리했습니다.(이도현)
				TryLearnSkill(newSkill);
			}
		}
	}

	public bool TryLearnSkill(string newSkill)
	{
		if (skills.Count >= 4)
		{
			Debug.Log("보유스킬이 4개입니다. 스킬 배울 수 없음");
			return false;
			//todo:가지고 있는 기술 지우기
			//지우려고 하는 스킬이 비전머신이냐 여부에 따라 결정
			//지우기 실패시 return false
		}

		//지우기 성공 후 새스킬 배우기
		skills.Add(newSkill);
		Debug.Log($"{pokeName}은/는 {newSkill}을/를 배웠다!");
		//Manager.UI.ShowPopupUI<UI_PopUp>("UI_Dialogue");
		//todo:ui 띄우기
		return true;

	}


	void CheckEvolution()
	{
		// TODO : 진화할지 말지 플레이어가 결정
		// 우선은 강제 진화
		if (evolveLevel <= 0 || evolveName == null)
			return;
		if (level >= evolveLevel)
		{
			Debug.Log($"{pokeName}이/가 {evolveName}으로 진화합니다!");

			// 진화전 데이터
			int hpGap = maxHp - hp;
			string prevName = pokeName;

			// 데이터 교체
			var evolveData = Manager.Data.SJH_PokemonData.GetPokemonData(evolveName);
			pokeName = evolveData.Name;
			baseStat = evolveData.BaseStat;
			pokeType1 = evolveData.PokeType1;
			pokeType2 = evolveData.PokeType2;
			expType = evolveData.ExpType;
			evolveLevel = evolveData.EvolveLevel;
			evolveName = evolveData.EvolveName;

			// 진화하면서 스탯 갱신
			pokemonStat = GetStat();
			maxHp = pokemonStat.hp;
			hp = maxHp - hpGap;
			if (hp < 1)
				hp = 1;

			nextExp = GetExpByLevel(level + 1) - curExp;

			Debug.Log($"진화 완료 {prevName} → {pokeName}");
		}
	}

	/// <summary>
	/// HP를 지정된 양 만틈 회복하고, 실제 회복한 값을 반환
	/// </summary>
	public int Heal(int value)
	{
		if (value <= 0) //오류
			throw new ArgumentOutOfRangeException(nameof(value), "회복량은 양수여야 합니다.");
		int hpGap = maxHp - hp;

		//실제 회복량
		int actualHeal = Mathf.Min(value, hpGap);
		hp += actualHeal;

		return actualHeal;
	}

	public bool RestoreStatus(Define.StatusCondition targetCondition)
	{
		if (condition == targetCondition)
		{
			condition = StatusCondition.None;
			return true;
		}

		return false;

	}

	public void TakeDamage(int damage)
	{
		// TODO : 대미지 입음
		hp -= damage;
		if (hp < 0)
		{
			hp = 0;
			isDead = true;
		}

		Debug.Log($"{pokeName} 이/가 {damage} 대미지를 입어 체력이 {hp}이/가 되었습니다.");
	}

	public void TakeDamage(Pokémon attacker, Pokémon defender, SkillS attackSkill)
	{
		// TODO : 대미지 입음

		// 총대미지
		int damage = GetTotalDamage(attacker, defender, attackSkill);

		// 기술적중 체크
		// TODO : 배틀 중 명중감소 당했을 때 생각
		// 명중률 = 기술 명중률 * (공격자 명중 스택 / 수비자 회피 스택)
		// int accu = (int)(attackSkill.accuracy * ((float)attacker.pokemonBattleStack.accuracy / defender.pokemonBattleStack.evasion));

		// 공격전
		switch (attackSkill.name)
		{
			case "분노":
				if (attacker.isAnger)
					damage *= attacker.angerStack;
				break;

			case "구르기":
				if (attacker.isRollout)
					damage *= attacker.rolloutStack;
				break;

			default:
				// 분노 초기화
				attacker.isAnger = false;
				attacker.angerStack = 1; // 1로 유지
				// 구르기 초기화
				attacker.isRollout = false;
				attacker.rolloutStack = 1;
				break;
		}

		// 대미지 연산
		hp -= damage;
		if (hp < 0)
		{
			hp = 0;
			isDead = true;
		}

		// PP감소
		attackSkill.curPP--;

		Debug.Log($"{pokeName} 이/가 {damage} 대미지를 입어 체력이 {hp}이/가 되었습니다.");

		// 공격후
		switch (attackSkill.name)
		{
			case "분노":
				// 디펜더가 분노상태면 분노 스택 증가
				if (defender.isAnger)
					defender.angerStack++;
				break;

			case "구르기":
				// 어태커가 구르기 상태면 스택 증가
				if (attacker.isRollout)
				{
					attacker.rolloutStack *= 2;
				}
				// 어태커가 첫 구르기면 true, 다음 구르기 대미지 2
				else
				{
					attacker.isRollout = true;
					attacker.rolloutStack = 2;
				}
				break;

			default:
				break;
		}
	}

	float TypesCalculator(PokeType attack, PokeType defense1, PokeType defense2)
	{
		float firstDamageRate = TypeCalculator(attack, defense1);
		float secondDamageRate = TypeCalculator(attack, defense2);

		return firstDamageRate * secondDamageRate;
	}

	float TypeCalculator(PokeType attack, PokeType defense)
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

	public int GetTotalDamage(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		int level = attacker.level;
		int power = (int)skill.damage;
		bool isSpecial = skill.skillType == SkillType.Special;

		int attackStat = isSpecial ? attacker.pokemonStat.speAttack : attacker.pokemonStat.attack;
		int defenseStat = isSpecial ? defender.pokemonStat.speDefense : defender.pokemonStat.defense;

		float damageRate = 1f;

		// 자속 체크
		if (skill.type == attacker.pokeType1 || skill.type == attacker.pokeType2)
			damageRate *= 1.5f;

		// 타입 체크
		damageRate *= TypesCalculator(skill.type, defender.pokeType1, defender.pokeType2);

		// 랜덤 난수 0.85 ~ 1
		damageRate *= UnityEngine.Random.Range(85, 101) / 100f;

		// 데미지 계산 공식
		float damage = (((((2f * level) / 5 + 2) * power * attackStat / defenseStat) / 50) + 2) * damageRate;

		// 최소 대미지 1
		return Mathf.Max(1, (int)damage);
	}

	public bool TryHit(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		int ran = UnityEngine.Random.Range(0, 100);
		bool isHit = ran < skill.accuracy;
		return isHit;
	}
}
