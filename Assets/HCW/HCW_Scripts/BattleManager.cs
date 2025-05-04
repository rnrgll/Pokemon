using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

// 배틀 로직 및 흐름 제어
public class BattleManager : MonoBehaviour
{
	private string previousScene;        // 이전 씬 이름

	[Header("UI 관련")]
	// [SerializeField] private PlayerPokemonPos;       // 플레이어 포켓몬 위치
	// [SerializeField] private EnemyPokemonPos;        // 적 포켓몬 위치
	[SerializeField] private BattleUIController ui;  // UI 요소
	[SerializeField] private BattleHUD hud;          // hp 게이지·텍스트 제어
	
	private DialogManager dialogue => DialogManager.Instance;

	[Header("전투 관련")]
	private List<Pokémon> playerParty;    // 플레이어 포켓몬 리스트
	private List<Pokémon> enemyParty;     // 적 포켓몬 리스트
	private Pokémon playerPokemon;        // 플레이어 포켓몬
	
	//자동 반영을 위해 프로퍼티로 변경
	private Pokémon _enemyPokemon;
	private Pokémon enemyPokemon    // 적 포켓몬
	{
		get => _enemyPokemon;
		set
		{
			_enemyPokemon = value;
			Manager.Game.UpdateEnemyPokemon(value); //자동반영
		}
	}       

	
	private int currentEnemyIndex;        // 현재 적 포켓몬 인덱스
	private string selectedAction;        // 선택된 행동
	private string playerSelectedSkill;   // 선택된 스킬
	private string enemySelectedSkill;    // 적 포켓몬이 사용할 기술
	private const int MaxPartySize = 6;   // 최대 파티 크기

	[SerializeField] private bool isTrainer;                // 상대가 트레이너 인지
	[SerializeField] private string enemyTrainerName;    // 트레이너면 이름


	Coroutine battleCoroutine;
	WaitForSeconds battleDelay = new WaitForSeconds(1f);

	

	// TODO : 배틀매니저 싱글톤 풀고 awake나 start에서 처리하기


	private void Start()
	{
		previousScene = SceneManager.GetActiveScene().name; // 이전 씬 이름 저장

		// UI 이벤트 구독
		ui.OnActionSelected.AddListener(OnActionButton);
		ui.OnSkillSelected.AddListener(OnSkillButton);
		Debug.Log("배틀매니저 초기화");

		// 트레이너
		if (Manager.Poke.enemyParty.Count >= 1)
		{
			Debug.Log("트레이너 배틀 시작");
			//게임 데이터 설정
			//게임 매니저에 정보 업데이트
			Manager.Game.SetBattleState(true,false);
			StartBattle(Manager.Poke.party, Manager.Poke.enemyParty);
		}
		else
		{
			Debug.Log("야생 배틀 시작");
			Manager.Game.SetBattleState(true,true);
			StartBattle(Manager.Poke.party, Manager.Poke.enemyPokemon);
		}
	}
	private void Update()
	{
		dialogue.HandleUpdate();
	}


	private void OnDestroy()
	{
		// 구독 해제
		ui.OnActionSelected.RemoveListener(OnActionButton);
		ui.OnSkillSelected.RemoveListener(OnSkillButton);
	}
	private void OnBattleDialogClosed()
	{
		dialogue.CloseDialog -= OnBattleDialogClosed; // 대사창이 닫힐때 이벤트 해제

		hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 HUD 설정
		hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 HUD 설정

		ui.ShowActionMenu();
		battleCoroutine = StartCoroutine(BattleLoop()); // 배틀은 대화창이 닫힌 후 시작하게
	}

	// 배틀 시작: 플레이어/적 파티 초기화 및 첫 포켓몬 설정
	public void StartBattle(List<Pokémon> party, List<Pokémon> enemies)
	{
		isTrainer = true; // 상대가 트레이너일경우
		
		playerParty = party?.Take(MaxPartySize).ToList() ?? new List<Pokémon>();// 파티의 최대 크기 설정 및 초기화
		enemyParty = enemies?.ToList() ?? new List<Pokémon>(); // 적 포켓몬 리스트 초기화

		if (playerParty.Count == 0 || enemyParty.Count == 0)
		{
			Debug.LogError("플레이어 또는 적 포켓몬 파티가 비어있습니다!");
			return;
		}

		playerPokemon = playerParty[0]; // 파티의 첫번째 포켓몬
		enemyPokemon = enemyParty[0];   // 적의 첫번째 포켓몬
		


		hud.SetPlayerHUD(playerPokemon);   // 플레이어 포켓몬 HUD 설정
		hud.SetEnemyHUD(enemyPokemon);     // 적 포켓몬 HUD 설정

		var lines = new List<string> { $"체육관 관장 {enemyTrainerName}이(가) 나타났다!" };

		dialogue.CloseDialog += OnBattleDialogClosed;
		dialogue.StartDialogue(new Dialog(lines));
	}

	public void StartBattle(List<Pokémon> party, Pokémon enemy)
	{
		isTrainer = false; // 상대가 트레이너가 아닐경우
		playerParty = party?.Take(MaxPartySize).ToList() ?? new List<Pokémon>();// 파티의 최대 크기 설정 및 초기화

		playerPokemon = playerParty[0]; // 파티의 첫번째 포켓몬
		enemyPokemon = enemy; // 적 포켓몬 설정


		
		hud.SetPlayerHUD(playerPokemon);   // 플레이어 포켓몬 HUD 설정
		hud.SetEnemyHUD(enemyPokemon);     // 적 포켓몬 HUD 설정

		var lines = new List<string> { $"야생의 {enemyPokemon.pokeName}이(가) 나타났다!" };
		dialogue.CloseDialog += OnBattleDialogClosed;
		dialogue.StartDialogue(new Dialog(lines));
	}

	private IEnumerator BattleLoop()
	{
		ui.ShowActionMenu(); // 행동 선택 UI 표시
		selectedAction = null;
		yield return new WaitUntil(() => selectedAction != null); // 행동 선택 대기

		//while ((playerPokemon.hp > 0) && currentEnemyIndex < enemyParty.Count)
		while ((playerPokemon.hp > 0) && ((isTrainer && currentEnemyIndex < enemyParty.Count) || (!isTrainer && enemyPokemon.hp > 0)))
		{
			Debug.Log($"배틀로그 : 배틀 진행중 [{playerPokemon.pokeName} {playerPokemon.hp} / {playerPokemon.maxHp}] VS [{enemyPokemon.pokeName} {enemyPokemon.hp} / {enemyPokemon.maxHp}]");
			// 적 포켓몬 교체 체크
			if (enemyPokemon.hp <= 0)
			{
				// 포켓몬 경험치 + 해줘야함
				// 경험치 = (기본 경험치량 × 트레이너 보너스 × 레벨) / 7
				int totalExp = (int)((enemyPokemon.baseExp * (isTrainer == true ? 1.5f : 1f) * enemyPokemon.level) / 7);
				Debug.Log($"{playerPokemon.pokeName}가 {totalExp} 경험치를 얻었다");
				playerPokemon.AddExp(totalExp);

				// 트레이너
				if (isTrainer)
				{
					currentEnemyIndex++; // 다음 포켓몬
					if (currentEnemyIndex < enemyParty.Count)
					{
						enemyPokemon = enemyParty[currentEnemyIndex];
						Debug.Log($"상대는 {enemyPokemon.pokeName}을/를 꺼냈다");
						yield return battleDelay;
						continue;
					}

				}
				// 야생
				yield return battleDelay;
				Debug.Log("Break");
				break;
			}
			else
			{
				// 상대 포켓몬이 살아있으면 다시 ui 활성화
				ui.ShowActionMenu(); // 행동 선택 UI 표시
			}


			// 행동 선택 대기
			selectedAction = null;
			Debug.Log($"배틀로그 : 행동 선택대기중");
			yield return new WaitUntil(() => selectedAction != null);
			Debug.Log($"배틀로그 : 행동 {selectedAction} 선택!");
			ui.HideActionMenu();

			// 전투 수행
			if (selectedAction == "Fight")
			{
				playerSelectedSkill = null;
				ui.ShowSkillSelection(playerPokemon);
				Debug.Log($"배틀로그 : Fight! 기술 선택대기중");
				yield return new WaitUntil(() => playerSelectedSkill != null); // 기술 선택할때까지 대기
				Debug.Log($"배틀로그 : Fight! {playerSelectedSkill} 선택!");
				ui.HideSkillSelection();

				// 적 포켓몬 행동 선택
				int idx = Random.Range(0, enemyPokemon.skills.Count);
				enemySelectedSkill = enemyPokemon.skills[idx];

				var actions = new List<BattleAction> // 적과 플레이어의 행동을 리스트에 추가
                {
					new BattleAction(playerPokemon, enemyPokemon, playerSelectedSkill),
					new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill)
				};

				// 속도에 따라 정렬
				actions.Sort((a, b) => b.Attacker.pokemonStat.speed.CompareTo(a.Attacker.pokemonStat.speed));

				foreach (var act in actions) ///
				{
					if (act.Attacker.hp <= 0)
					{
						Debug.Log($"{act.Attacker.pokeName} 은/는 기절 행동불가");
						continue;
					}

					Debug.Log($"{act.Attacker.pokeName} 이/가 {act.Skill} 사용!");
					ExecuteAction(act);
					Debug.Log($"{act.Attacker.pokeName} 이/가 {act.Skill} 사용완료!");
					yield return battleDelay;
				}

				hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 체력바 업데이트
				hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 체력바 업데이트

				yield return battleDelay;
			}
			else // Fight가 아닌 선택지 추가 필요
			{
				Debug.Log($"플레이어 액션: {selectedAction}");
				yield return battleDelay;
			}
		}
		Debug.Log("배틀로그 : 배틀종료");
		EndBattle();
	}

	private void OnActionButton(string action) => selectedAction = action;
	private void OnSkillButton(int idx) => playerSelectedSkill = playerPokemon.skills[idx];

	// 행동 선택 후 행동 처리
	private void ExecuteAction(BattleAction action)
	{
		Debug.Log($"{action.Attacker.pokeName} 사용 {action.Skill}");
		Attack(action.Attacker, action.Target, action.Skill);
	}

	// 공격처리 추후 계산기 따로만들던가 여기 추가
	private void Attack(Pokémon atk, Pokémon tgt, string skl)
	{
		// TODO : 디펜더의 함수를 사용하지 말고 스킬 함수의 UseSkill 사용으로 대미지 구현하기
		var skill = Manager.Data.SkillSData.GetSkillDataByName(skl);
		int damage = GetTotalDamage(atk, tgt, skill);
		tgt.TakeDamage(damage);
	}

	// 배틀 종료 처리
	private void EndBattle()
	{
		if (playerPokemon.hp <= 0)
		{
			Debug.Log("게임 오버: 플레이어 전멸");
			Destroy(Manager.Poke.enemyPokemon);
		}
		else
		{
			Debug.Log("승리: 모든 적 포켓몬 격파");
			// 트레이너 배틀일 경우 돈 + 경험치
			// 경험치 및 보상, 이전 씬으로 다시 이동 구현 필요
			var setting = SceneManager.LoadSceneAsync(Manager.Encounter.prevSceneName); // 이전 씬으로 이동
			setting.allowSceneActivation = false;

			// 변수 초기화
			isTrainer = false;
			// 코루틴 초기화
			StopCoroutine(battleCoroutine);
			battleCoroutine = null;
			Destroy(Manager.Poke.enemyPokemon);

			int totalExp = (int)((enemyPokemon.baseExp * (isTrainer == true ? 1.5f : 1f) * enemyPokemon.level) / 7);
			Debug.Log($"{playerPokemon.pokeName}가 {totalExp} 경험치를 얻었다");
			playerPokemon.AddExp(totalExp);

			// 위치지정
			//Debug.Log($"플레이어 {Manager.Encounter.prevPosition} 으로 이동");
			//Manager.Game.Player.transform.position = Manager.Encounter.prevPosition;

			setting.allowSceneActivation = true;
			
		}
		
		//게임 데이터 업데이트
		Manager.Game.EndBattle();
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
		damageRate *= Random.Range(85, 101) / 100f;

		// 데미지 계산 공식
		float damage = (((((2f * level) / 5 + 2) * power * attackStat / defenseStat) / 50) + 2) * damageRate;

		// 최소 대미지 1
		return Mathf.Max(1, (int)damage);
	}
}