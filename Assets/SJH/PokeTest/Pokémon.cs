using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

	[Tooltip("배틀중일 때 스택 0 ~ 6")]
	public PokemonBattleStat pokemonBattleStack;

	[Tooltip("스킬 PP 관리")]
	public List<SkillData> skillDatas = new();

	#region 포켓몬 효과 관리 변수

	[Tooltip("분노 관리용")]
	public bool isAnger;
	public int angerStack;

	[Tooltip("구르기 관리용")]
	public bool isRollout;
	public int rolloutStack;

	[Tooltip("김밥말이 관리용")]
	public bool isBind;
	public int bindStack;

	[Tooltip("솔라빔 관리용")]
	public bool isCharge;

	[Tooltip("길동무 관리용")]
	public bool isDestinyBond;

	[Tooltip("리플렉터 관리용")]
	public bool isReflect;
	public int reflectCount;

	[Tooltip("빛의장막 관리용")]
	public bool isLightScreen;
	public int lightScreenCount;

	[Tooltip("저주 관리용")]
	public bool isCurse;

	[Tooltip("검은눈빛 관리용")]
	public bool isCantRun;

	[Tooltip("신비의부적 관리용")]
	public bool isSafeguard;
	public int safeguardCount;

	[Tooltip("꿰뚫어보기 관리용")]
	public bool isForesight;    // Defender

	[Tooltip("원한 관리용")]
	public string prevSkillName;

	[Tooltip("잠자기 관리용")]
	public int restCount;

	[Tooltip("상태이상 : 수면 관리용")]
	public int sleepCount;

	[Tooltip("상태이상 : 혼란 관리용")]
	public int confusionCount;

	#endregion

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
		condition = StatusCondition.Normal;

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
				// 포켓몬 레벨 이하면 추가
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

		// TODO : allSkills 의 List<string> 를 List<SkillData> 로 바꾸기
		skillDatas = new();
		foreach (string skillName in skills)
		{
			SkillS skills = Manager.Data.SkillSData.GetSkillDataByName(skillName);
			SkillData skillData = new SkillData(skills.name, skills.maxPP, skills.maxPP);
			skillDatas.Add(skillData);
		}
	}

	// 경험치 추가
	public void AddExp(int baseExp)
	{
		// 경험치 = (기본 경험치량 × 트레이너 보너스 × 레벨) / 7
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
				TryLearnSkill(newSkill, null );
			}
		}
	}
	

	
	#region LearSkillFlow

	public void TryLearnSkill(string newSkill, Action<bool> onFinish)
	{
		if (skills.Contains(newSkill))
		{
			Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
				.ShowMessage(
					ItemMessage.Get(ItemMessageKey.AlreadyKnow, pokeName, newSkill),()=>onFinish?.Invoke(false), true, true
						);
			return;
		}
		
		if (skills.Count < 4)
		{
			skills.Add(newSkill);
			ShowLearnSuccess(newSkill, onFinish);
			return;
		}

		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
			.ShowMessage(
				ItemMessage.Get(ItemMessageKey.LearnFail, pokeName,
					newSkill), //어느 기술을 잊게 할껀지 다이얼로그 띄우고 -> 콜백으로 선택지 창 띄우기
				() =>
				{
					Manager.UI.ShowConfirmPopup(
						onYes: () =>
						{
							Manager.UI.CloseAllPopUp();
							AskSkillToForget(newSkill, onFinish);
						},
						onNo: () =>
						{
							ShowCancelConfirmPopup(newSkill, onFinish);
						});
				}, true, true, true);

	}

	private void ShowCancelConfirmPopup(string newSkill, Action<bool> onFinish)
	{
		Manager.UI.CloseAllPopUp();
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
			.ShowMessage($"그렇다면...{newSkill}를(을)\n배우는 것을 그만두겠습니까?", () =>
			{
				Manager.UI.ShowConfirmPopup(
					onYes: () =>
					{
						onFinish?.Invoke(false); // 진짜 취소
					},
					onNo: () =>
					{
						Manager.UI.CloseAllPopUp();
						// 다시 가르칠지 묻는 창으로 복귀
						TryLearnSkill(newSkill, onFinish);
					});
			},true,true,true);
	}

	private void AskSkillToForget(string newSkill, Action<bool> onFinish)
	{
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
			.ShowMessage("어느 기술을\n잊게 하겠습니까?", () =>
			{
				ShowForgetChoicePopup(newSkill, onFinish);
			}, true, true, true);
	}

	//잊게할 기술 선택 팝업창 띄우는 메소드
	private void ShowForgetChoicePopup(string newSkill, Action<bool> onFinish)
	{
		var skillListUI = Manager.UI.ShowPopupUI<UI_SelectPopUp>("UI_SelectablePopUp");
		skillListUI.SetupOptions(new List<(string label, ISelectableAction action)>
		{
			($"{skills[0]}", new CustomAction(()=>ForgetAndLearn(0, newSkill, onFinish))),
			($"{skills[1]}", new CustomAction(()=>ForgetAndLearn(1, newSkill, onFinish))),
			($"{skills[2]}", new CustomAction(()=>ForgetAndLearn(2, newSkill, onFinish))),
			($"{skills[3]}", new CustomAction(()=>ForgetAndLearn(3, newSkill, onFinish)))
		}
		);
		skillListUI.OverrideCancelAction(new CustomAction(() =>
		{
			ShowCancelConfirmPopup(newSkill, onFinish);
		}));
		RectTransform boxRT = skillListUI.transform.GetChild(0).GetComponent<RectTransform>();
		Canvas canvas = boxRT.GetComponentInParent<Canvas>();
		Util.SetPositionFromBottomRight(boxRT, 0f, 0f);
		Util.SetRelativeVerticalOffset(boxRT, canvas, 0.34f);
	}
	
	//기술 잊게하기
	private void ForgetAndLearn(int idx, string newSkill, Action<bool> onFinish)
	{
		string oldSkill = skills[idx];
		if (Manager.Data.SkillSData.GetSkillDataByName(skills[idx]).isHM) // HM은 삭제 불가라면
		{
			Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp").ShowMessage("중요한 기술입니다\n잊게할 수 없습니다!", () =>
			{
				ShowForgetChoicePopup(newSkill, onFinish); // 다시 선택
			},true,true);
			return;
		}
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp").ShowMessage(ItemMessage.Get(ItemMessageKey.ForgetSkill,pokeName,oldSkill), () =>
		{
			skills[idx] = newSkill;
			ShowLearnSuccess(newSkill, onFinish);
		},true,true);
	}
	private void ShowLearnSuccess(string skill, Action<bool> onFinish)
	{
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
			.ShowMessage(ItemMessage.Get(ItemMessageKey.LearnSuccess, pokeName, skill),
				() =>
				{
					Manager.UI.CloseAllPopUp();
					onFinish?.Invoke(true);
				}, true, true);
	}
	
	
	#endregion

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
			condition = StatusCondition.Normal;
			return true;
		}

		return false;

	}

	public void TakeDamage(int damage)
	{
		hp -= damage;
		if (hp < 0)
		{
			hp = 0;
			isDead = true;
		}

		Debug.Log($"배틀로그 : {pokeName} 이/가 {damage} 대미지를 입었습니다. 현재체력 : {hp}");
	}

	public void TakeDamage(Pokémon attacker, Pokémon defender, SkillS attackSkill)
	{
		int ran = UnityEngine.Random.Range(0, 100);

		// 총대미지
		int damage = GetTotalDamage(attacker, defender, attackSkill);

		// 기술적중 체크
		// 명중률 = 기술 명중률 * (공격자 명중 스택 / 수비자 회피 스택)
		// int accu = (int)(attackSkill.accuracy * ((float)attacker.pokemonBattleStack.accuracy / defender.pokemonBattleStack.evasion));

		// 공격전
		switch (attackSkill.name)
		{
			case "분노":
				if (attacker.isAnger)
				{
					damage *= attacker.angerStack;
				}
				else if (!attacker.isAnger)
				{
					attacker.isAnger = true;
					attacker.angerStack = 1;
				}
				break;

			case "구르기":
				if (attacker.isRollout)
				{
					damage *= attacker.rolloutStack;
				}
				break;

			case "솔라빔":
				if (!attacker.isCharge)
				{
					attacker.isCharge = true;
					Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 기를 모으고 있다 {ran}");
					goto AfterAtack;
				}
				break;

			case "매그니튜드":
				attackSkill.damage = 0;
				int magnitudeRandom = UnityEngine.Random.Range(1, 8);

				switch (magnitudeRandom)
				{
					case 1: attackSkill.damage = 10; break;
					case 2: attackSkill.damage = 30; break;
					case 3: attackSkill.damage = 50; break;
					case 4: attackSkill.damage = 60; break;
					case 5: attackSkill.damage = 90; break;
					case 6: attackSkill.damage = 110; break;
					default: attackSkill.damage = 150; break;
				}
				damage = GetTotalDamage(attacker, defender, attackSkill);
				break;

			case "분노의앞니":
				if ((defender.pokeType1 == PokeType.Ghost) || (defender.pokeType2 == PokeType.Ghost))
				{
					// 고스트 타입이면 무효
					damage = 0;
				}
				else
				{
					// 고정피해라 여기서 대미지 계산
					damage = Mathf.Max(1, defender.hp / 2);
				}
				break;

			case "나이트헤드":
				if (defender.pokeType1 == PokeType.Normal || defender.pokeType2 == PokeType.Normal)
				{
					damage = 0;
				}
				else
				{
					damage = attacker.level;
				}
				break;

			case "따라하기":
				if (string.IsNullOrEmpty(defender.prevSkillName))
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 기술을 사용하지 않았다! 따라하기 실패. {ran}");
					break;
				}
				if (defender.prevSkillName == "따라하기")
				{
					Debug.Log($"배틀로그 : 따라하기는 따라할 수 없다! {ran}");
					break;
				}
				SkillS copySkill = Manager.Data.SkillSData.GetSkillDataByName(prevSkillName);
				if (copySkill == null)
				{
					Debug.Log($"배틀로그 : 따라할 스킬이 없다! {ran}");
				}

				copySkill.UseSkill(attacker, defender, copySkill);
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
		// 디펜더가 분노상태면 분노 스택 증가
		if (damage > 0 && defender.isAnger)
			defender.angerStack++;
		if (hp < 0)
		{
			hp = 0;
			isDead = true;
			// 길동무 같이주금
			if (isDestinyBond)
			{
				Debug.Log($"배틀로그 : {pokeName} 의 길동무! {attacker.pokeName} 도 기절했다 {ran}");
				attacker.hp = 0;
				attacker.isDead = true;
			}
			isCantRun = false;
		}
		// 맞고도 살았으면 길동무 해제
		isDestinyBond = false;

		// 대미지 계산 후 로그
		#region 대미지 계산 후 로그

		Debug.Log($"배틀로그 : {attacker.pokeName} 의 {attackSkill.name} 공격! 대미지 [{damage}] {ran}");
		float typeEffectiveness = TypesCalculator(attackSkill.type, defender);

		int effectiveness = (int)(typeEffectiveness * 100);
		string logMsg = "";
		switch (effectiveness)
		{
			case 0: 
				Debug.Log($"배틀로그 : 그러나 {defender.pokeName} 에게는 효과가 없었다...");
				logMsg = $"그러나 {defender.pokeName} 에게는 효과가 없었다...";
				break;
			case 25: 
				Debug.Log("배틀로그 : 그러나 효과는 미미했다");
				logMsg = "그러나 효과는 미미했다";
				break;
			case 50: 
				Debug.Log("배틀로그 : 효과는 조금 부족한 듯 하다");
				logMsg = "효과는 조금 부족한 듯 하다";
				break;
			case 100: /*효과는 보통일 경우 메시지 없음*/ break;
			case 200: Debug.Log("배틀로그 : 효과는 뛰어났다!");
				logMsg = "효과는 뛰어났다!";
				break;
			case 400: Debug.Log("배틀로그 : 효과는 굉장했다!!");
				logMsg = "효과는 굉장했다!!";
				break;
			default: Debug.Log($"배틀로그 : 버그 [{typeEffectiveness}]"); break;
		}
		if(!string.IsNullOrEmpty(logMsg))
			StartCoroutine(Manager.Dialog.ShowBattleMessage(logMsg));

		if (damage > 0)
			Debug.Log($"배틀로그 : {pokeName} 이/가 {damage} 대미지를 입었습니다. 현재체력 : [{hp}] {ran}");

		#endregion

		// 공격후
		switch (attackSkill.name)
		{
			case "분노":
				// 공격자 분노상태로 만들기
				attacker.isAnger = true;
				break;

			case "구르기":
				// 어태커가 구르기 상태면 스택 증가
				if (attacker.isRollout)
				{
					attacker.rolloutStack = Mathf.Min(attacker.rolloutStack * 2, 16); // 최대 16배
				}
				// 어태커가 첫 구르기면 true, 다음 구르기 대미지 2
				else
				{
					attacker.isRollout = true;
					attacker.rolloutStack = 2;
				}
				break;

			case "솔라빔":
				attacker.isCharge = false;
				break;

			default:
				break;
		}
	AfterAtack:
		// 마지막에 사용한 스킬 저장
		attacker.prevSkillName = attackSkill.name;
	}

	// Status 스킬 적용
	public void TakeEffect(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		int ran = UnityEngine.Random.Range(0, 100);
		switch (skill.name)
		{
			// 랭크 상승 공 > 방 > 특공 > 특방 > 스피드 > 명중 > 회피 > 급소
			#region 랭크 상승

			case "웅크리기":
			case "단단해지기":
				attacker.pokemonBattleStack.defense = Mathf.Min(6, attacker.pokemonBattleStack.defense + 1);
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 방어 1랭크 상승 {ran}");
				
				break;

			case "성장":
				attacker.pokemonBattleStack.speAttack = Mathf.Min(6, attacker.pokemonBattleStack.speAttack + 1);
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 특공 1랭크 상승 {ran}");
				break;

			case "망각술":
				attacker.pokemonBattleStack.speDefense = Mathf.Min(6, attacker.pokemonBattleStack.speDefense + 2);
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 특방 2랭크 상승 {ran}");
				break;

			case "고속이동":
				attacker.pokemonBattleStack.speed = Mathf.Min(6, attacker.pokemonBattleStack.speed + 2);
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 스피드 2랭크 상승 {ran}");
				break;

			case "기충전":
				attacker.pokemonBattleStack.critical = Mathf.Min(6, attacker.pokemonBattleStack.critical + 1);
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 급소율 1랭크 상승 {ran}");
				break;

			#endregion

			// 랭크 하락 공 > 방 > 특공 > 특방 > 스피드 > 명중 > 회피 > 급소
			#region 랭크 하락

			case "울음소리":
				defender.pokemonBattleStack.attack = Mathf.Max(-6, defender.pokemonBattleStack.attack - 1);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 공격 1랭크 하락 {ran}");
				break;

			case "싫은소리":
				defender.pokemonBattleStack.defense = Mathf.Max(-6, defender.pokemonBattleStack.defense - 2);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 방어 2랭크 하락 {ran}");
				break;

			case "꼬리흔들기":
			case "째려보기":
				defender.pokemonBattleStack.defense = Mathf.Max(-6, defender.pokemonBattleStack.defense - 1);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 방어 1랭크 하락 {ran}");
				break;

			case "사이코키네시스":
				defender.pokemonBattleStack.speDefense = Mathf.Max(-6, defender.pokemonBattleStack.speDefense - 1);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 특방 1랭크 하락 {ran}");
				break;

			case "겁나는얼굴":
				defender.pokemonBattleStack.speed = Mathf.Max(-6, defender.pokemonBattleStack.speed - 2);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 스피드 2랭크 하락 {ran}");
				break;

			case "실뿜기":
			case "휘감기":
				defender.pokemonBattleStack.speed = Mathf.Max(-6, defender.pokemonBattleStack.speed - 1);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 스피드 1랭크 하락 {ran}");
				break;

			case "모래뿌리기":
			case "연막":
			case "진흙뿌리기":
			case "플래시":
				defender.pokemonBattleStack.accuracy = Mathf.Max(-6, defender.pokemonBattleStack.accuracy - 1);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 명중 1랭크 하락 {ran}");
				break;

			case "달콤한향기":
				defender.pokemonBattleStack.evasion = Mathf.Max(-6, defender.pokemonBattleStack.evasion - 1);
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 회피율 1랭크 하락 {ran}");
				break;

			#endregion

			// 상태이상
			#region 상태이상

			case "최면술":
			case "수면가루":
				if (defender.isSafeguard)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 신비의부적 기술로 인해 수면 면역! {ran}");
				}
				else
				{
					defender.condition = StatusCondition.Sleep;
					defender.sleepCount = UnityEngine.Random.Range(1, 7);
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 잠들어 버렸다! {ran}");
				}
				break;

			case "독가루":
			case "독침":
				// 독 타입은 독 불가
				if (defender.pokeType1 == PokeType.Poison || defender.pokeType2 == PokeType.Poison)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 독 타입이라 독 면역! {ran}");
					break;
				}

				if (defender.isSafeguard)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 신비의부적 기술로 인해 독 면역! {ran}");
				}
				else
				{
					defender.condition = StatusCondition.Poison;
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 독 상태 {ran}");
				}
				break;

			case "이상한빛":
			case "초음파":
			case "염동력":
				if (defender.isSafeguard)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 신비의부적 기술로 인해 혼란 면역! {ran}");
				}
				else
				{
					defender.condition = StatusCondition.Confusion;
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 혼란 상태 {ran}");
				}
				break;

			case "저리가루":
			case "누르기":
			case "핥기":
				if (defender.isSafeguard)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 신비의부적 기술로 인해 마비 면역! {ran}");
				}
				else
				{
					defender.condition = StatusCondition.Paralysis;
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 마비 상태 {ran}");
				}
				break;

			case "불꽃세례":
			case "화염방사":
			case "화염자동차":
				// 동상은 확정해제
				defender.RestoreStatus(StatusCondition.Freeze);

				// 불꽃 타입은 화상 불가
				if (defender.pokeType1 == PokeType.Fire || defender.pokeType2 == PokeType.Fire)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 불꽃 타입이라 화상 면역! {ran}");
					break;
				}

				if (defender.isSafeguard)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 신비의부적 기술로 인해 화상 면역! {ran}");
				}
				else
				{
					defender.condition = StatusCondition.Burn;
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 화상 상태 {ran}");
				}
				break;

			case "필살앞니":
			case "물기":
				if (defender.isSafeguard)
				{
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 신비의부적 기술로 인해 풀죽음 면역! {ran}");
				}
				else
				{
					defender.condition = StatusCondition.Flinch;
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 풀죽음 상태 {ran}");
				}
				break;

			#endregion

			// 그외

			// 턴 관련
			#region 턴 관련

			case "리플렉터":
				attacker.isReflect = true;
				attacker.reflectCount = 6;
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 사용 {ran}");
				break;

			case "빛의장막":
				attacker.isLightScreen = true;
				attacker.lightScreenCount = 6;
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 사용 {ran}");
				break;

			case "신비의부적":
				attacker.isSafeguard = true;
				attacker.safeguardCount = 6;
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name}에 둘러싸였다. {ran}");
				break;

			case "잠자기":
				if (attacker.hp == attacker.maxHp)
				{
					Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 체력이 가득찬 상태라 {skill.name} 기술이 실패 {ran}");
				}
				else
				{
					int sleepHealAmount = attacker.Heal(maxHp);
					if (attacker.condition != StatusCondition.Faint)
						attacker.condition = StatusCondition.Sleep;
					restCount = 3; // 2로하면 턴종료가 바로되서 1턴되버림
					Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 상태이상과 체력을 {sleepHealAmount} 회복했다! {ran}");
				}
				break;

			case "김밥말이":
				int bindTurns = UnityEngine.Random.Range(3, 7); // 2~5턴
				defender.isBind = true;
				defender.bindStack = bindTurns;

				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {bindTurns}턴 동안 김밥말이에 감싸졌다! 교체 및 도망 불가!");
				break;


			#endregion

			case "길동무":
				attacker.isDestinyBond = true;
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 사용 {ran}");
				break;

			case "저주":
				if ((attacker.pokeType1 == PokeType.Ghost) || (attacker.pokeType2 == PokeType.Ghost))
				{
					if (!defender.isCurse)
					{
						// 고스트 타입이면 HP 절반 감소, 상대에 저주 상태 부여
						int lostHp = Mathf.Max(1, attacker.hp / 2);
						attacker.hp -= lostHp;
						defender.isCurse = true;
						Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {defender.pokeName} 에게 저주를 걸었다! HP {lostHp} 감소 {ran}");
					}
					else
					{
						Debug.Log($"배틀로그 : {defender.pokeName} 은/는 이미 저주 상태 {ran}");
					}

				}
				else
				{
					// 아니면 공격/방어 1 상승 스피드 1 하락
					attacker.pokemonBattleStack.attack = Mathf.Min(6, attacker.pokemonBattleStack.attack + 1);
					attacker.pokemonBattleStack.defense = Mathf.Min(6, attacker.pokemonBattleStack.defense + 1);
					attacker.pokemonBattleStack.speed = Mathf.Max(-6, attacker.pokemonBattleStack.speed - 1);
					Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 공격/방어 1랭크 상승, 스피드 1랭크 하락 {ran}");
				}
				break;

			case "검은눈빛":
			case "거미집":
				defender.isCantRun = true;
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술로 인해 도망갈 수 없게 됐다. {ran}");
				break;

			case "꿰뚫어보기":
				defender.isForesight = true;
				Debug.Log($"배틀로그 : {defender.pokeName} 은/는 {skill.name} 기술 맞음 {ran}");
				break;

			case "광합성":
				int healAmount = attacker.Heal(maxHp / 2);
				Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 체력을 {healAmount} 회복 {ran}");
				break;

			case "원한":
				if (!string.IsNullOrEmpty(prevSkillName))
					Debug.Log($"배틀로그 : {attacker.pokeName} 은/는 {skill.name} 기술로 {defender.pokeName}의 {defender.prevSkillName} 기술의 PP를 감소 {ran}");
				break;
		}
		// 마지막에 사용한 스킬 저장
		attacker.prevSkillName = skill.name;
		// 구르기나 분노 사용했다면 초기화
		if (attacker.isAnger)
		{
			attacker.isAnger = false;
			attacker.angerStack = 1;
		}
		if (attacker.isRollout)
		{
			attacker.isRollout = false;
			attacker.rolloutStack = 1;
		}
	}

	float TypesCalculator(PokeType attack, Pokémon defender)
	{
		PokeType defense1 = defender.pokeType1;
		PokeType defense2 = defender.pokeType2;
		float firstDamageRate = TypeCalculator(attack, defense1, defender.isForesight);
		float secondDamageRate = TypeCalculator(attack, defense2, defender.isForesight);

		return firstDamageRate * secondDamageRate;
	}

	float TypeCalculator(PokeType attack, PokeType defense, bool isForesight)
	{
		if (defense == PokeType.None) return 1f;

		if (attack == PokeType.Normal)
		{
			if (defense == PokeType.Rock || defense == PokeType.Steel) return 0.5f;
			if (defense == PokeType.Ghost) return isForesight ? 1f : 0f;
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
			if (defense == PokeType.Ghost) return isForesight ? 1f : 0f;
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

		bool isCritical = IsCritical(attacker.pokemonBattleStack.critical + (skill.name == "베어가르기" || skill.name == "잎날가르기" ? 1 : 0));

		// 물리 / 특수 체크
		bool isSpecial = skill.skillType == SkillType.Special;

		int attackStat = isSpecial
			? attacker.GetModifyStat(attacker.pokemonStat.speAttack, attacker.pokemonBattleStack.speAttack)
			: attacker.GetModifyStat(attacker.pokemonStat.attack, attacker.pokemonBattleStack.attack);
		int defenseStat = isSpecial
			? defender.GetModifyStat(attacker.pokemonStat.speDefense, attacker.pokemonBattleStack.speDefense)
			: defender.GetModifyStat(attacker.pokemonStat.defense, attacker.pokemonBattleStack.defense);

		// 화상 공격력 반감 / 급소면 무시
		if (attacker.condition == StatusCondition.Burn && !isSpecial)
		{
			attackStat /= 2;
		}

		// 리플렉터 / 빛의장막 체크 / 급소면 무시
		if (!isSpecial && defender.isReflect && !isCritical)
			defenseStat *= 2; // 물리 데미지 반감

		else if (isSpecial && defender.isLightScreen && !isCritical)
			defenseStat *= 2; // 특수 데미지 반감

		float damageRate = 1f;

		// 자속 체크
		if (skill.type == attacker.pokeType1 || skill.type == attacker.pokeType2)
			damageRate *= 1.5f;

		// 타입 체크
		damageRate *= TypesCalculator(skill.type, defender);
		if (damageRate == 0f)
			return 0;

		// 급소 체크
		if (isCritical)
			damageRate *= 1.5f;

		// 랜덤 난수 0.85 ~ 1
		damageRate *= UnityEngine.Random.Range(85, 101) / 100f;

		// 데미지 계산 공식
		float damage = (((((2f * level) / 5 + 2) * power * attackStat / defenseStat) / 50) + 2) * damageRate;

		// 최소 대미지 1
		return Mathf.Max(1, (int)damage);
	}

	public bool TryHit(Pokémon attacker, Pokémon defender, SkillS skill)
	{
		// 명중률이 0 ~ 100이 아닌 0~255로 사용 1바이트 때문인듯?
		// 0~255 원작
		// 0~100 직관적

		float attackerAcc = GetModifyAccEva(attacker.pokemonBattleStack.accuracy);
		float defenderEva = GetModifyAccEva(defender.pokemonBattleStack.evasion);

		float finalAccuracy = skill.accuracy * (attackerAcc / defenderEva);

		int ran = UnityEngine.Random.Range(0, 100);
		bool isHit = ran < finalAccuracy;
		if (!isHit)
		{
			Debug.Log($"배틀로그 :{attacker.pokeName} 의 {skill.name} 공격은 빗나갔다! [{ran}% >= {finalAccuracy}%]");
			StartCoroutine(Manager.Dialog.ShowBattleMessage($"{attacker.pokeName} 의 {skill.name} 공격은 빗나갔다!"));
		}
		return isHit;
	}

	public int GetModifyStat(int stat, int rank)
	{
		// 배틀중 스탯 = 원래 스탯 × (랭크 + 2) / 2
		rank = Mathf.Clamp(rank, -6, 6);
		if (rank >= 0)
			return stat * (rank + 2) / 2;
		else
			return stat * 2 / (Mathf.Abs(rank) + 2);
	}

	public float GetModifyAccEva(int rank)
	{
		// 명중 회피 전용
		rank = Mathf.Clamp(rank, -6, 6);
		if (rank >= 0)
			return (2f + rank) / 2f;
		else
			return 2f / (2f - rank);
	}

	public bool IsCritical(int rank)
	{
		// 1~4
		float ran = UnityEngine.Random.Range(0f, 1f);
		switch (rank)
		{
			case 1:
				return ran < 0.125f;   // 12.5%
			case 2:
				return ran < 0.25f;    // 25%
			case 3:
				return ran < 0.333f;   // 33.3%
			case 4:
				return ran < 0.5f;     // 50%
			default:
				return ran < 0.0625f;  // 6.25%
		}
	}


	public void TurnEnd()
	{
		// 김밥말이
		int ran = UnityEngine.Random.Range(0, 100);
		if (isBind)
		{
			int bindDamage = Mathf.Max(1, maxHp / 16); // 최소 1 대미지
			hp -= bindDamage;
			Debug.Log($"배틀로그 : {pokeName} 은/는 김밥말이로 인해 체력 {bindDamage} 감소 {ran}");

			bindStack--;
			Debug.Log($"배틀로그 : {pokeName}의 김밥말이 {bindStack}턴 남음 {ran}");
			if (bindStack <= 0)
			{
				isBind = false;
				Debug.Log($"배틀로그 : {pokeName}의 김밥말이 상태 종료 {ran}");
			}
		}

		// 저주
		if (isCurse)
		{
			int curseDamage = Mathf.Max(1, maxHp / 4);
			hp -= curseDamage;
			Debug.Log($"배틀로그 : {pokeName} 은/는 저주로 인해 체력 {curseDamage} 감소 {ran}");
		}

		// 리플렉터
		if (isReflect)
		{
			reflectCount--;
			Debug.Log($"배틀로그 : {pokeName}의 리플렉터 {reflectCount}턴 남음 {ran}");
			if (reflectCount <= 0)
			{
				isReflect = false;
				Debug.Log($"배틀로그 : {pokeName} 의 리플렉터 효과가 사라졌다! {ran}");
			}
		}
		// 빛의장막
		if (isLightScreen)
		{
			lightScreenCount--;
			Debug.Log($"배틀로그 : {pokeName}의 빛의장막 {lightScreenCount}턴 남음 {ran}");
			if (lightScreenCount <= 0)
			{
				isLightScreen = false;
				Debug.Log($"배틀로그 : {pokeName} 의 빛의장막 효과가 사라졌다! {ran}");
			}
		}
		// 신비의부적
		if (isSafeguard)
		{
			safeguardCount--;
			Debug.Log($"배틀로그 : {pokeName}의 신비의부적 {safeguardCount}턴 남음 {ran}");
			if (safeguardCount <= 0)
			{
				isSafeguard = false;
				Debug.Log($"배틀로그 : {pokeName} 의 신비의부적 효과가 사라졌다 {ran}");
			}
		}

		// 상태이상 체크
		switch (condition)
		{
			case StatusCondition.Sleep:
				if (restCount > 0)
				{
					restCount--;
					Debug.Log($"배틀로그 : {pokeName}의 잠자기! {restCount}턴 남음 {ran}");
					if (restCount <= 0)
					{
						condition = StatusCondition.Normal;
						Debug.Log($"배틀로그 : {pokeName} 은/는 잠에서 깨어났다 {ran}");
					}
				}

				else if (sleepCount > 0)
				{
					sleepCount--;
					Debug.Log($"배틀로그 : {pokeName} 은/는 쿨쿨 잠들어 있다 {restCount}턴 남음 {ran}");
					if (sleepCount <= 0)
					{
						condition = StatusCondition.Normal;
						Debug.Log($"배틀로그 : {pokeName} 은/는 잠에서 깨어났다 {ran}");
					}
				}
				break;
			case StatusCondition.Burn: // 1/8
				int burnDamage = Mathf.Max(1, maxHp / 8);
				hp -= burnDamage;
				Debug.Log($"배틀로그 : {pokeName} 은/는 화상 데미지를 입었다! [{burnDamage}] {ran}");
				break;
			case StatusCondition.Poison:
				int poisonDamage = Mathf.Max(1, maxHp / 8);
				hp -= poisonDamage;
				Debug.Log($"배틀로그 : {pokeName} 은/는 독에 의한 데미지를 입었다! [{poisonDamage}] {ran}");
				break;
		}

		// 턴 종료 후 사망 체크
		GetPokemonDeadCheck();
	}

	public bool GetPokemonDeadCheck()
	{
		int ran = UnityEngine.Random.Range(0, 100);
		if (hp <= 0)
		{
			hp = 0;
			isDead = true;
			condition = StatusCondition.Faint;
			Debug.Log($"배틀로그 : {pokeName} 은/는 쓰러졌다! {ran}");
			return true;
		}
		return false;
	}

	public bool CanActionCheck()
	{
		int ran = UnityEngine.Random.Range(0, 100);
		switch (condition)
		{
			case StatusCondition.Poison:
				{
					return true;
				}
			case StatusCondition.Burn:
				{
					return true;
				}
			case StatusCondition.Freeze:
				{
					// 해제되어도 공격불가
					if (UnityEngine.Random.Range(0, 1f) > 0.1f)
					{
						Debug.Log($"배틀로그 : {pokeName} 은/는 얼어버려서 움직일 수 없다! {ran}");
						return false;
					}
					else
					{
						Debug.Log($"배틀로그 : {pokeName} 은/는 얼음이 해제됐다! {ran}");
						condition = StatusCondition.Normal;
						return false;
					}
				}
			case StatusCondition.Sleep:
				{
					Debug.Log($"배틀로그 : {pokeName} 은/는 쿨쿨 잠들어 있다 {restCount}턴 남음 {ran}");
					return false;
				}
			case StatusCondition.Paralysis:
				{
					if (UnityEngine.Random.Range(0, 1f) > 0.25f)
					{
						Debug.Log($"배틀로그 : {pokeName} 은/는 몸이 저려서 움직일 수 없다! {ran}");
						return false;
					}
					return true;
				}
			case StatusCondition.Confusion:
				{
					confusionCount--;
					if (confusionCount <= 0)
					{
						Debug.Log($"배틀로그 : {pokeName}의 혼란이 풀렸다! {ran}");
						condition = StatusCondition.Normal;
					}
					else
					{
						// 50% 확률로 실패
						if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
						{
							int damage = GetConfusionDamage(); // 물리, 타입무시, 40 고정
							TakeDamage(damage);
							Debug.Log($"배틀로그 : {pokeName} 은/는 영문도 모른 채 자신을 공격했다! {ran}");
						}
					}
					return false;
				}
			case StatusCondition.Flinch:
				{
					// 한턴 날리고 행동 가능
					Debug.Log($"배틀로그 : {pokeName} 은/는 풀이 죽어 기술을 쓸 수 없다! {ran}");
					condition = StatusCondition.Normal;
					return false;
				}
			default:	// 노말
				return true;
		}
	}

	// 혼란 자신 공격 대미지 계산
	public int GetConfusionDamage()
	{
		int power = 40;

		// 물리
		int attackStat = GetModifyStat(pokemonStat.attack, pokemonBattleStack.attack);
		int defenseStat = GetModifyStat(pokemonStat.defense, pokemonBattleStack.defense);

		float damageRate = 1f;

		// 랜덤 난수 0.85 ~ 1
		damageRate *= UnityEngine.Random.Range(0.85f, 1f);

		// 데미지 계산 공식
		float damage = (((((2f * level) / 5 + 2) * power * attackStat / defenseStat) / 50) + 2) * damageRate;

		// 최소 대미지 1
		return Mathf.Max(1, (int)damage);
	}

	public void StackReset()
	{
		// 분노
		isAnger = false;
		angerStack = 0;
		// 구르기
		isRollout = false;
		rolloutStack = 0;
		// 김밥말이
		isBind = false;
		bindStack = 0;
		// 솔라빔
		isCharge = false;
		// 길동무
		isDestinyBond = false;
		// 리플렉터
		isReflect = false;
		reflectCount = 0;
		// 리플렉터
		isLightScreen = false;
		lightScreenCount = 0;
		// 저주
		isCurse = false;
		// 검은눈빛, 거미집
		isCantRun = false;
		// 신비의부적
		isSafeguard = false;
		safeguardCount = 0;
		// 꿰뚫어보기
		isForesight = false;
		// 원한, 따라하기
		prevSkillName = null;
	}

	public bool SkillPPCheck(string skillName)
	{
		for (int i = 0; i < skillDatas.Count; i++)
		{
			if (skillDatas[i].Name == skillName)
			{
				var data = skillDatas[i];
				
				// PP 체크
				if (data.CurPP <= 0)
					return false;

				// PP 감소
				data.DecreasePP();
				// 재할당
				skillDatas[i] = data;

				return true;
			}
		}
		Debug.Log($"{skillName} 은/는 없는 스킬 입니다.");
		return false;
	}
}
