using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// 배틀 로직 및 흐름 제어
public class BattleManager : MonoBehaviour
{
	private string previousScene;        // 이전 씬 이름

	[Header("UI 관련")]
	// [SerializeField] private PlayerPokemonPos;       // 플레이어 포켓몬 위치
	// [SerializeField] private EnemyPokemonPos;        // 적 포켓몬 위치
	[SerializeField] private BattleUIController ui;  // UI 요소
	[SerializeField] private BattleHUD hud;          // hp 게이지·텍스트 제어
	[SerializeField] private PokemonSelect pokemonSelect;

	private DialogManager dialogue => DialogManager.Instance;

	[Header("전투 관련")]
	private List<Pokémon> playerParty;    // 플레이어 포켓몬 리스트
	private List<Pokémon> enemyParty;     // 적 포켓몬 리스트
	private Pokémon playerPokemon;        // 플레이어 포켓몬
	private Pokémon enemyPokemon;         // 적 포켓몬
	private int currentEnemyIndex;        // 현재 적 포켓몬 인덱스
	private string selectedAction;        // 선택된 행동
	private string playerSelectedSkill;   // 선택된 스킬
	private string enemySelectedSkill;    // 적 포켓몬이 사용할 기술
	private const int MaxPartySize = 6;   // 최대 파티 크기

	[SerializeField] private bool isTrainer;                // 상대가 트레이너 인지
	[SerializeField] private string enemyTrainerName;    // 트레이너면 이름


	Coroutine battleCoroutine;
	WaitForSeconds battleDelay = new WaitForSeconds(1f);

	// 턴
	int currentTurn;

	private void Start()
	{
		previousScene = SceneManager.GetActiveScene().name; // 이전 씬 이름 저장

		// UI 이벤트 구독
		ui.OnActionSelected.AddListener(OnActionButton);
		ui.OnSkillSelected.AddListener(OnSkillButton);
		Debug.Log("배틀매니저 초기화");

		// 현재턴 지정
		currentTurn = 1;
		// 트레이너
		if (Manager.Poke.enemyParty.Count >= 1)
		{
			Debug.Log("트레이너 배틀 시작");
			StartBattle(Manager.Poke.party, Manager.Poke.enemyParty);
		}
		else
		{
			Debug.Log("야생 배틀 시작");
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

		// 내 포켓몬 배틀스탯 초기화
		Manager.Poke.PartyBattleStatInit();

		if (playerParty.Count == 0 || enemyParty.Count == 0)
		{
			Debug.LogError("플레이어 또는 적 포켓몬 파티가 비어있습니다!");
			return;
		}

		playerPokemon = Manager.Poke.GetFirtstPokemon(); // 파티의 첫번째 포켓몬
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

		playerPokemon = Manager.Poke.GetFirtstPokemon(); // 파티의 첫번째 포켓몬
		enemyPokemon = enemy; // 적 포켓몬 설정

		//hud.SetPlayerHUD(playerPokemon);   // 플레이어 포켓몬 HUD 설정
		//hud.SetEnemyHUD(enemyPokemon);     // 적 포켓몬 HUD 설정

		var lines = new List<string> { $"야생의 {enemyPokemon.pokeName}이(가) 나타났다!", "테스트" };

		dialogue.CloseDialog += OnBattleDialogClosed;
		dialogue.StartDialogue(new Dialog(lines));
	}

	private IEnumerator BattleLoop()
	{
		ui.ShowActionMenu(); // 행동 선택 UI 표시
		selectedAction = null;
		yield return new WaitUntil(() => selectedAction != null); // 행동 선택 대기

		//while ((playerPokemon.hp > 0) && ((isTrainer && currentEnemyIndex < enemyParty.Count) || (!isTrainer && enemyPokemon.hp > 0)))
		while ((Manager.Poke.AlivePokemonCheck()) && ((isTrainer && currentEnemyIndex < enemyParty.Count) || (!isTrainer && enemyPokemon.hp > 0)))
		{
			Debug.Log($"배틀로그 {currentTurn}턴 : [{playerPokemon.pokeName} {playerPokemon.hp} / {playerPokemon.maxHp}] VS [{enemyPokemon.pokeName} {enemyPokemon.hp} / {enemyPokemon.maxHp}]");
			// 내 포켓몬 교체 체크
			if (playerPokemon.hp <= 0 || playerPokemon.isDead)
			{
				Debug.Log($"배틀로그 : 교체할 포켓몬을 선택해주세요.");

				// TODO : 교체 UI 활성화

				// 행동 선택 대기
				selectedAction = null;
				//Debug.Log($"행동 선택대기중");
				yield return new WaitUntil(() => selectedAction != null);
				//Debug.Log($"행동 {selectedAction} 선택!");
				ui.HideActionMenu();


			}

			// 적 포켓몬 교체 체크
			if (enemyPokemon.hp <= 0 || enemyPokemon.isDead)
			{
				// 포켓몬 경험치 + 해줘야함
				// 경험치 = (기본 경험치량 × 트레이너 보너스 × 레벨) / 7
				int totalExp = (int)((enemyPokemon.baseExp * (isTrainer == true ? 1.5f : 1f) * enemyPokemon.level) / 7);
				Debug.Log($"배틀로그 : {playerPokemon.pokeName} 은/는 {totalExp} 경험치를 얻었다!");
				playerPokemon.AddExp(totalExp);

				// 트레이너 다음 포켓몬 교체
				if (isTrainer)
				{
					currentEnemyIndex++; // 다음 포켓몬
					if (currentEnemyIndex < enemyParty.Count)
					{
						enemyPokemon = enemyParty[currentEnemyIndex];
						Debug.Log($"배틀로그 : 상대는 {enemyPokemon.pokeName}을/를 꺼냈다");
						yield return battleDelay;
						continue;
					}
					else
					{
						// 상대 전멸
						EndBattle("Win");
						yield break;
					}
				}
				// 야생
				else
				{
					EndBattle("Win");
					yield break;
				}
			}
			else
			{
				// 상대 포켓몬이 살아있으면 다시 ui 활성화
				ui.ShowActionMenu(); // 행동 선택 UI 표시
			}

			// 행동 선택 대기
			selectedAction = null;
			//Debug.Log($"행동 선택대기중");
			yield return new WaitUntil(() => selectedAction != null);
			//Debug.Log($"행동 {selectedAction} 선택!");
			ui.HideActionMenu();

			// 적 포켓몬 행동 선택
			// TODO : 적 포켓몬 기술 선택 AI
			int idx = Random.Range(0, enemyPokemon.skills.Count);
			enemySelectedSkill = enemyPokemon.skills[idx];

			// 전투 수행
			switch (selectedAction)
			{
				case "Fight":
					playerSelectedSkill = null;
					ui.ShowSkillSelection(playerPokemon);
					yield return new WaitUntil(() => playerSelectedSkill != null); // 기술 선택할때까지 대기
					ui.HideSkillSelection();

					var actions = new List<BattleAction> // 적과 플레이어의 행동을 리스트에 추가
                    {
					    new BattleAction(playerPokemon, enemyPokemon, playerSelectedSkill),
					    new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill)
					};

					// 속도에 따라 정렬
					actions.Sort((a, b) =>
					{
						// 우선도가 없고 선공기는 전광석화 뿐이니 단순하게
						bool aIsQuickAttack = a.Skill == "전광석화";
						bool bIsQuickAttack = b.Skill == "전광석화";

						if (aIsQuickAttack && !bIsQuickAttack)
							return -1; // a가 먼저
						if (!aIsQuickAttack && bIsQuickAttack)
							return 1;  // b가 먼저

						// 스피드에 랭크 계산
						int speedA = a.Attacker.GetModifyStat(a.Attacker.pokemonStat.speed, a.Attacker.pokemonBattleStack.speed);
						int speedB = b.Attacker.GetModifyStat(a.Attacker.pokemonStat.speed, a.Attacker.pokemonBattleStack.speed);

						Debug.Log($"배틀로그 {currentTurn}턴 : [{a.Attacker.pokeName}의 스피드 : {speedA}] VS [{b.Attacker.pokeName}의 스피드 : {speedB}]");
						if (speedA != speedB)
							return speedB.CompareTo(speedA);

						// 속도 같으면 랜덤
						return Random.Range(0, 2) == 0 ? -1 : 1;
					});

					foreach (var act in actions)
					{
						if (act.Attacker.hp <= 0)
						{
							Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} 은/는 기절 행동불가");
							continue;
						}

						Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} ! {act.Skill} !");
						ExecuteAction(act);
						yield return battleDelay;
					}

					hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 체력바 업데이트
					hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 체력바 업데이트

					yield return battleDelay;
					break;

				case "Pokemon":
					yield return StartCoroutine(PokemonSwitch());
					{
						ExecuteAction(new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill));

						hud.SetPlayerHUD(playerPokemon);
						hud.SetEnemyHUD(enemyPokemon);

						yield return new WaitForSeconds(1f);
					}
					break;

				case "Bag":
					// TODO : 가방 
					break;

				case "Run":
					{
						EndBattle("Run");
						yield break;
					}
				default:
					Debug.LogWarning($"지정하지 않은 액션 선택");
					break;
			}
			Debug.Log($"배틀로그 {currentTurn}턴 : {currentTurn} 턴 종료");
			// 턴카운트 증가
			currentTurn++;
			// 각 포켓몬 턴종료 액션 실행
			playerPokemon.TurnEnd();
			enemyPokemon.TurnEnd();

			hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 체력바 업데이트
			hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 체력바 업데이트

			// 플레이어 포켓몬 체크
			if (!Manager.Poke.AlivePokemonCheck())
			{
				Debug.Log($"배틀로그 {currentTurn}턴 : 플레이어 전멸");
				EndBattle("Lose");
				yield break;
			}

			// 야생 포켓몬 체크
			if (!isTrainer && enemyPokemon.hp <= 0)
			{
				int totalExp = (int)((enemyPokemon.baseExp * enemyPokemon.level) / 7);
				Debug.Log($"{playerPokemon.pokeName} 은/는 {totalExp} 경험치를 얻었다!");
				playerPokemon.AddExp(totalExp);

				Debug.Log($"배틀로그 {currentTurn}턴 : 야생 포켓몬 쓰러짐");
				EndBattle("Win");
				yield break;
			}
		}
		Debug.Log($"배틀로그 {currentTurn}턴 : 배틀종료");
	}

	private void OnActionButton(string action) => selectedAction = action;
	private void OnSkillButton(int idx) => playerSelectedSkill = playerPokemon.skills[idx];

	private IEnumerator PokemonSwitch()
	{
		Pokémon chosen = null;
		bool cancelled = false;

		pokemonSelect.Show(playerParty, p => chosen = p, () => cancelled = true);

		yield return new WaitUntil(() => chosen != null || cancelled);

		if (cancelled) // 취소시 메뉴 다시 열기
		{
			selectedAction = null;
			ui.ShowActionMenu();
			yield break;
		}

		playerPokemon = chosen; // 선택 하면 교체
		hud.SetPlayerHUD(playerPokemon);

		selectedAction = null;
	}

	// 행동 선택 후 행동 처리
	private void ExecuteAction(BattleAction action)
	{
		//Debug.Log($"{action.Attacker.pokeName} 사용 {action.Skill}");
		//Attack(action.Attacker, action.Target, action.Skill);
		var skill = Manager.Data.SkillSData.GetSkillDataByName(action.Skill);
		skill.UseSkill(action.Attacker, action.Target, skill);
	}


	// 배틀 종료 처리
	private void EndBattle(string Reason)
	{
		switch (Reason)
		{
			case "Win":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 승리: 모든 적 포켓몬 격파");
					// 트레이너 배틀일 경우 돈 + 경험치
					// 경험치 및 보상, 이전 씬으로 다시 이동 구현 필요
					var setting = SceneManager.LoadSceneAsync(Manager.Encounter.prevSceneName); // 이전 씬으로 이동
					setting.allowSceneActivation = false;

					// 변수 초기화
					isTrainer = false;
					// 코루틴 초기화
					StopCoroutine(battleCoroutine);
					battleCoroutine = null;

					//// 경험치 계산
					//int totalExp = (int)((enemyPokemon.baseExp * (isTrainer == true ? 1.5f : 1f) * enemyPokemon.level) / 7);
					//playerPokemon.AddExp(totalExp);
					//Debug.Log($"배틀로그 {currentTurn}턴 : {playerPokemon.pokeName} 은/는 {totalExp} 경험치를 얻었다!");

					// 상대 포켓몬 파괴
					Destroy(Manager.Poke.enemyPokemon);
					Destroy(enemyPokemon);

					// 씬활성화
					setting.allowSceneActivation = true;
				}
				break;
			case "Lose":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 게임 오버: 플레이어 전멸");
					Destroy(Manager.Poke.enemyPokemon);

					// TODO : 마지막 회복 위치로 이동해야할듯 우선은 이전씬으로만
					var setting = SceneManager.LoadSceneAsync(Manager.Encounter.prevSceneName); // 이전 씬으로 이동
					setting.allowSceneActivation = false;

					// 변수 초기화
					isTrainer = false;
					// 코루틴 초기화
					StopCoroutine(battleCoroutine);
					battleCoroutine = null;
					Destroy(Manager.Poke.enemyPokemon);

					setting.allowSceneActivation = true;
				}
				break;
			case "Run":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 성공적으로 도망쳤다!");

					var setting = SceneManager.LoadSceneAsync(Manager.Encounter.prevSceneName); // 이전 씬으로 이동
					setting.allowSceneActivation = false;

					// 변수 초기화
					isTrainer = false;
					// 코루틴 초기화
					StopCoroutine(battleCoroutine);
					battleCoroutine = null;
					Destroy(Manager.Poke.enemyPokemon);

					setting.allowSceneActivation = true;
				}
				break;
		}
	}
}