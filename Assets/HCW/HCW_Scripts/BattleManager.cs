using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

// 배틀 로직 및 흐름 제어
public class BattleManager : MonoBehaviour
{
	private string previousScene;        // 이전 씬 이름

	[Header("UI 관련")]
	[SerializeField] private Transform playerPokemonPos;       // 플레이어 포켓몬 위치
	[SerializeField] private Transform enemyPokemonPos;        // 적 포켓몬 위치
	[SerializeField] private BattleUIController ui;  // UI 요소
	[SerializeField] private BattleHUD hud;          // hp 게이지·텍스트 제어
	[SerializeField] private PokemonSelect pokemonSelect;

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
	[SerializeField] int winMoney;	// 승리보상


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
			//게임 데이터 설정
			//게임 매니저에 정보 업데이트
			Manager.Game.SetBattleState(true,false);

			TrainerData trainerData = Manager.Poke.enemyData;

			// TODO : EnemyData로 파티 생성하기
			List<Pokémon> enemyPartyData = new();

			foreach (var data in trainerData.TrainerPartyData)
			{
				if (string.IsNullOrEmpty(data.PokeName) || data.PokeLevel <= 0)
					continue;
				Pokémon poke = Manager.Poke.AddEnemyPokemon(data.PokeName, data.PokeLevel);
				enemyPartyData.Add(poke);
			}
			// 포켓몬 지정
			enemyParty = enemyPartyData;
			// 이름지정
			enemyTrainerName = trainerData.Name;
			// 상금지정
			winMoney = trainerData.Money;

			StartBattle(Manager.Poke.party, enemyParty);
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

		ui.ShowActionMenu(playerPokemon);
		Debug.Log("배틀 다이얼로그 종료");
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

		var lines = new List<string> { $"{enemyTrainerName} 이(가) 승부를 걸어왔다!" };

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

		var lines = new List<string> { $"앗! 야생의 {enemyPokemon.pokeName}이(가) 튀어나왔다!", "테스트" };

		dialogue.CloseDialog += OnBattleDialogClosed;
		dialogue.StartDialogue(new Dialog(lines));
	}

	private IEnumerator BattleLoop()
	{
		// ui.ShowActionMenu(); // 행동 선택 UI 표시
		// selectedAction = null;
		// yield return new WaitUntil(() => selectedAction != null); // 행동 선택 대기

		//while ((playerPokemon.hp > 0) && ((isTrainer && currentEnemyIndex < enemyParty.Count) || (!isTrainer && enemyPokemon.hp > 0)))
		while ((Manager.Poke.AlivePokemonCheck()) && ((isTrainer && currentEnemyIndex < enemyParty.Count) || (!isTrainer && enemyPokemon.hp > 0)))
		{
			bool isAction = false;
			Manager.Game.SetIsItemUsed(false); //아이템 사용 여부 플래그 초기화
			
			Debug.Log($"배틀로그 {currentTurn}턴 : [{playerPokemon.pokeName} {playerPokemon.hp} / {playerPokemon.maxHp}] VS [{enemyPokemon.pokeName} {enemyPokemon.hp} / {enemyPokemon.maxHp}]");
			
			// 내 포켓몬 교체 체크
			if (playerPokemon.hp <= 0 || playerPokemon.isDead)
			{
				Debug.Log($"배틀로그 {currentTurn}턴 : 교체할 포켓몬을 선택해주세요.");

				yield return StartCoroutine(PokemonSwitch());
				{
					hud.SetPlayerHUD(playerPokemon);
					hud.SetEnemyHUD(enemyPokemon);

					yield return new WaitForSeconds(1f);
					continue;
				}
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

						hud.SetEnemyHUD(enemyPokemon);

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
			// else
			// {
			// 	// 상대 포켓몬이 살아있으면 다시 ui 활성화
			// 	ui.ShowActionMenu(); // 행동 선택 UI 표시
			// }

			// 행동 선택 대기
			ui.ShowActionMenu(playerPokemon);
			selectedAction = null;
			//Debug.Log($"행동 선택대기중");
			yield return new WaitUntil(() => selectedAction != null);
			//Debug.Log($"행동 {selectedAction} 선택!");
			// ui.HideActionMenu();

			// 적 포켓몬 행동 선택 AI
			int idx = Random.Range(0, enemyPokemon.skills.Count);
			enemySelectedSkill = enemyPokemon.skills[idx];
			Debug.Log("enemy 스킬 선택");

			// 전투 수행
			switch (selectedAction)
			{
				case "Fight":
					// playerSelectedSkill = null;
					// ui.ShowSkillSelection(playerPokemon);
					// yield return new WaitUntil(() => playerSelectedSkill != null); // 기술 선택할때까지 대기
					//ui.HideSkillSelection();
					
					//기술 선택을 취소할 수도 있음
					//기술 선택창을 열었다고 액션 체크를 하면 안됨
					//기술 선택창과 액션 체크를 분리시켜야 함
					
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

						if (a.Attacker.condition == StatusCondition.Paralysis)
							speedA = speedA / 4;
						if (b.Attacker.condition == StatusCondition.Paralysis)
							speedB = speedB / 4;

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

						// 상태이상체크
						if (act.Attacker.CanActionCheck())
						{
							// TODO : PP 체크
							yield return StartCoroutine(Motion(act)); // 데미지랑 딜레이 실행 모션 코루틴으로 이동
						}
						isAction = true;
						
					}

					hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 체력바 업데이트
					hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 체력바 업데이트

					yield return battleDelay;
					break;

				case "Pokemon":
					yield return StartCoroutine(PokemonSwitch());
					{
						// TODO : 상대 포켓몬의 PP 체크
						ExecuteAction(new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill));

						hud.SetPlayerHUD(playerPokemon);
						hud.SetEnemyHUD(enemyPokemon);

						isAction = true;

						yield return new WaitForSeconds(1f);
					}
					break;

				case "Bag":
					//1. 인벤토리 창을 띄운다.
					Manager.UI.ShowLinkedUI<UI_Bag>("UI_Bag");
					
					//2. 대기
					//대기를 끝내는 조건 : 가방 UI가 닫혀서 UI 매니저가 관리하는 UI가 없는 경우 = 아무 ui도 열려있지 않은 경우
					yield return new WaitUntil(() => !Manager.UI.IsAnyUIOpen);
					
					//3. 턴 인지 확인하는 체크
					//플레이어가 도구를 사용한 경우 턴 종료 
					//todo : 플레이어 도구 사용 여부를 체크할 변수가 필요 && 전투 중인경우 도구를 사용하면 아예 ui를 다 닫아야함
					if (Manager.Game.IsInBattle && Manager.Game.IsItemUsed)
					{
						isAction = true;
					}
					break;

				case "Run":
					{
						// 도망못가게함
						if (playerPokemon.isCantRun || playerPokemon.isBind)
						{
							// 검은눈빛, 거미집, 김밥말이
							Debug.Log($"배틀로그 {currentTurn}턴 : {playerPokemon.pokeName} 은/는 도망칠 수 없다!");
						}
						else if (isTrainer)
						{
							Debug.Log("안돼! 승부도중에 상대에게 등을 보일 수 없어!");
						}
						else
						{
							EndBattle("Run");
							yield break;
						}
					}
					break;
					
				default:
					Debug.LogWarning($"지정하지 않은 액션 선택");
					break;
			}

			// TODO : 턴종료 조건 추가하기
			// 도망을 실패해도 턴이 증가함
			//2. 가방
			if (isAction)
			{
				Debug.Log($"배틀로그 {currentTurn}턴 : {currentTurn} 턴 종료");
				// 턴카운트 증가
				currentTurn++;
				// 각 포켓몬 턴종료 액션 실행
				playerPokemon.TurnEnd();
				enemyPokemon.TurnEnd();

				hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 체력바 업데이트
				hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 체력바 업데이트
			}

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
			ui.ShowActionMenu(playerPokemon);
			yield break;
		}

		playerPokemon = chosen; // 선택 하면 교체
		hud.SetPlayerHUD(playerPokemon);

		selectedAction = null;
	}
	private IEnumerator Motion(BattleAction action)
	{
		// 공격 모션
		var atkPos = action.Attacker == playerPokemon ? playerPokemonPos : enemyPokemonPos;
		var atkAnim = atkPos.GetComponent<Animator>();
		atkAnim.SetTrigger("DoAttack");

		yield return new WaitForSeconds(0.5f);

		ExecuteAction(action);
		hud.SetPlayerHUD(playerPokemon);
		hud.SetEnemyHUD(enemyPokemon);

		//피격 모션
		var tgtPos = action.Target == playerPokemon ? playerPokemonPos : enemyPokemonPos;
		var tgtAnim = tgtPos.GetComponent<Animator>();
		tgtAnim.SetTrigger("DoBlink");
		yield return new WaitForSeconds(0.5f);
	}

	// 행동 선택 후 행동 처리
	private void ExecuteAction(BattleAction action)
	{
		var skill = Manager.Data.SkillSData.GetSkillDataByName(action.Skill);
		skill.UseSkill(action.Attacker, action.Target, skill);
	}


	// 배틀 종료 처리
	private void EndBattle(string reason)
	{
		// 공통적으로 실행
		Debug.Log($"배틀로그 {currentTurn}턴 : 배틀 종료 - {reason}");

		// 코루틴 초기화
		if (battleCoroutine != null)
		{
			StopCoroutine(battleCoroutine);
			battleCoroutine = null;
		}

		// 상대 포켓몬 초기화
		if (isTrainer)
		{
			foreach (var poke in enemyParty)
			{
				Destroy(poke.gameObject);
			}
		}
		Destroy(enemyPokemon.gameObject);

		// 내 포켓몬 상태 초기화
		Manager.Poke.ClearPartyState();

		var setting = SceneManager.LoadSceneAsync(Manager.Game.Player.PrevSceneName); // 이전 씬으로 이동
		setting.allowSceneActivation = false;

		switch (reason)
		{
			case "Win":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 승리");

					// TODO : 배틀에서 이기고 다시 배틀할 수 없게 해야함
					Manager.Event.TrainerWin(Manager.Poke.enemyData.TrainerId);
					Debug.Log($"골드는 상금으로 {winMoney}원을 손에 넣었다!");
					Manager.Data.PlayerData.AddMoney(winMoney);
					Manager.Poke.enemyData.IsFight = true;
				}
				break;
			case "Lose":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 패배");

					// TODO : 마지막 회복 위치로 이동해야할듯 우선은 이전씬으로만
				}
				break;
			case "Run":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 성공적으로 도망쳤다!");
				}
				break;
		}

		// 변수 초기화
		isTrainer = false;
		setting.allowSceneActivation = true;
		//게임 데이터 업데이트
		Manager.Game.EndBattle();
	}
}