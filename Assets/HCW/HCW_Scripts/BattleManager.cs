using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
using Random = UnityEngine.Random;

// 배틀 로직 및 흐름 제어
public class BattleManager : MonoBehaviour
{
	[SerializeField] private GameObject introPrefab;
	[SerializeField] private RectTransform battleSceneUI;
	[SerializeField] private string battleScene;

	[Header("UI 관련")]
	[SerializeField] private Transform playerPokemonPos;       // 플레이어 포켓몬 위치
	[SerializeField] private Transform enemyPokemonPos;        // 적 포켓몬 위치
	private Vector3 pOriginScale;
	private Vector3 eOriginScale;
	[SerializeField] private BattleUIController ui;  // UI 요소
	[SerializeField] private BattleHUD hud;          // hp 게이지·텍스트 제어
	[SerializeField] private PokemonSelect pokemonSelect;

	private DialogManager Dialog => DialogManager.Instance;

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
	int currentTurn; // 턴


	
	#region start - 배틀 시작 전 초기화 및 데이터 설정
	
	private void Start()
	{
		
		battleScene = SceneManager.GetActiveScene().name; //현재 씬 이름 저장
		//origin 위치 저장
		pOriginScale = playerPokemonPos.localScale;
		eOriginScale = enemyPokemonPos.localScale;
		// UI 이벤트 구독
		ui.OnActionSelected.AddListener(OnActionButton);
		ui.OnSkillSelected.AddListener(OnSkillButton);
		Debug.Log("배틀매니저 초기화");

		currentTurn = 1;

		// Intro 프리팹 생성 & 보여주기
		var introObject = Instantiate(introPrefab, battleSceneUI);
		var intro = introObject.GetComponent<BattleIntro>();
		intro.battleScene = battleScene;

		void OnComplete()
		{
			// 트레이너
			if (Manager.Poke.enemyData.TrainerPartyData.Count >= 1)
			{
				Debug.Log("트레이너 배틀 시작");
				//게임 데이터 설정
				//게임 매니저에 정보 업데이트
				Manager.Game.SetBattleState(true, false);

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
				Manager.Game.SetBattleState(true, true);
				StartBattle(Manager.Poke.party, Manager.Poke.enemyPokemon);
			}
			intro.OnIntroComplete -= OnComplete;
			Destroy(introObject);
			introObject.transform.parent.gameObject.SetActive(false);
		}
		intro.OnIntroComplete += OnComplete;

		if (Manager.Poke.enemyParty.Count >= 1)
			intro.PlayTrainerIntro();
		else
			intro.PlayWildIntro();
	}
	#endregion

	#region 파괴 및 다이얼르그 종료 시 처리?
	private void OnDestroy()
	{
		// 구독 해제
		ui.OnActionSelected.RemoveListener(OnActionButton);
		ui.OnSkillSelected.RemoveListener(OnSkillButton);
	}
	private void OnBattleDialogClosed()
	{
		Dialog.CloseDialog -= OnBattleDialogClosed; // 대사창이 닫힐때 이벤트 해제

		hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 HUD 설정
		hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 HUD 설정

		ui.ShowActionMenu(playerPokemon);
		Debug.Log("배틀 다이얼로그 종료");
		battleCoroutine = StartCoroutine(BattleLoop()); // 배틀은 대화창이 닫힌 후 시작하게
	}
	
	#endregion


	#region 배틀 시작 - 트레이너 대결
	
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


		//포켓몬 프리팹 스폰
		SpawnPokemonAtStart(playerPokemon, playerPokemonPos, true);
		SpawnPokemonAtStart(enemyPokemon,enemyPokemonPos,false,() => hud.SpawnStatePanel(false));
		
		hud.SetPlayerHUD(playerPokemon);   // 플레이어 포켓몬 HUD 설정
		hud.SetEnemyHUD(enemyPokemon);     // 적 포켓몬 HUD 설정

		hud.InitPlayerExp(playerPokemon);//플레이어 포켓몬 exp 설정


		var lines = new List<string> { $"{enemyTrainerName} 이(가) 승부를 걸어왔다!" };

		Dialog.CloseDialog += OnBattleDialogClosed;
		Dialog.StartDialogue(new Dialog(lines));
		
	}
	#endregion

	#region 배틀 시작 - 야생 포켓몬
	public void StartBattle(List<Pokémon> party, Pokémon enemy)
	{
		isTrainer = false; // 상대가 트레이너가 아닐경우
		playerParty = party?.Take(MaxPartySize).ToList() ?? new List<Pokémon>();// 파티의 최대 크기 설정 및 초기화

		playerPokemon = Manager.Poke.GetFirtstPokemon(); // 파티의 첫번째 포켓몬
		enemyPokemon = enemy; // 적 포켓몬 설정

		SpawnPokemonAtStart(playerPokemon, playerPokemonPos, true,() => hud.SpawnStatePanel(true));
		SpawnPokemonAtStart(enemyPokemon,enemyPokemonPos,false,() => hud.SpawnStatePanel(false));
		
		
		hud.SetPlayerHUD(playerPokemon);   // 플레이어 포켓몬 HUD 설정
		hud.SetEnemyHUD(enemyPokemon);     // 적 포켓몬 HUD 설정
		hud.InitPlayerExp(playerPokemon);//플레이어 포켓몬 exp 설정



		var lines = new List<string> { $"앗! 야생의 {enemyPokemon.pokeName}이(가) 튀어나왔다!" };

		Dialog.CloseDialog += OnBattleDialogClosed;
		Dialog.StartDialogue(new Dialog(lines));
	}
	#endregion
	
	
	//액션, 스킬 선택시 호출되는 메소드?
	private void OnActionButton(string action) => selectedAction = action;
	private void OnSkillButton(int idx) => playerSelectedSkill = playerPokemon.skills[idx];
	
	#region 배틀 루프
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
				
				yield return StartCoroutine(PokemonSwitch(true));
				{
					hud.SetPlayerHUD(playerPokemon, false);
					hud.SetEnemyHUD(enemyPokemon, false);

					yield return new WaitForSeconds(1f);
					continue;
				}
			}

			// 적 포켓몬 교체 체크
			if (enemyPokemon.hp <= 0 || enemyPokemon.isDead)
			{
				//yield return StartCoroutine(PlayFaintAnimation(enemyPokemonPos));
				
				// 포켓몬 경험치 + 해줘야함
				// 경험치 = (기본 경험치량 × 트레이너 보너스 × 레벨) / 7
				int totalExp = (int)((enemyPokemon.baseExp * (isTrainer == true ? 1.5f : 1f) * enemyPokemon.level) / 7);
				yield return StartCoroutine(
					Manager.Dialog.ShowBattleMessage($"배틀로그 : {playerPokemon.pokeName} 은/는 {totalExp} 경험치를 얻었다!"));

				Debug.Log($"배틀로그 : {playerPokemon.pokeName} 은/는 {totalExp} 경험치를 얻었다!");
				yield return StartCoroutine(AnimateGainExp(totalExp));

				// 트레이너 다음 포켓몬 교체
				if (isTrainer)
				{
					currentEnemyIndex++; // 다음 포켓몬
					if (currentEnemyIndex < enemyParty.Count)
					{
						enemyPokemon = enemyParty[currentEnemyIndex];
						Debug.Log($"배틀로그 : 상대는 {enemyPokemon.pokeName}을/를 꺼냈다");
						yield return StartCoroutine(PlaySwitchAnimation(null, enemyPokemon.pokeName,enemyPokemonPos, false));
						yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"배틀로그 : 상대는 {enemyPokemon.pokeName}을/를 꺼냈다"));
						hud.SetEnemyHUD(enemyPokemon, false);

						yield return battleDelay;
						continue;
					}
					else
					{
						// 상대 전멸
						//EndBattle("Win");
						StartCoroutine(EndBattleCoroutine("Win"));
						yield break;
					}
				}
				// 야생
				else
				{
					
					StartCoroutine(EndBattleCoroutine("Win"));
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
			//action 생성해서 처리하는게 똑같은 로직이라 밖으로 뺐음
			var actions = new List<BattleAction>(); // 적과 플레이어의 행동을 리스트에 추가
			actions.Clear();
			
			switch (selectedAction)
			{
				case "Fight":
					{
						//기술 선택을 취소할 수도 있음
						//기술 선택창을 열었다고 액션 체크를 하면 안됨
						//기술 선택창과 액션 체크를 분리시켜야 함

						actions.Add(new BattleAction(playerPokemon, enemyPokemon, playerSelectedSkill));
						actions.Add(new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill));

						// 속도에 따라 정렬
						actions.Sort((a, b) =>
						{
							// 우선도가 없고 선공기는 전광석화 뿐이니 단순하게
							bool aIsQuickAttack = a.Skill == "전광석화";
							bool bIsQuickAttack = b.Skill == "전광석화";

							if (aIsQuickAttack && !bIsQuickAttack)
								return -1; // a가 먼저
							if (!aIsQuickAttack && bIsQuickAttack)
								return 1; // b가 먼저

							// 스피드에 랭크 계산
							int speedA = a.Attacker.GetModifyStat(a.Attacker.pokemonStat.speed,
								a.Attacker.pokemonBattleStack.speed);
							int speedB = b.Attacker.GetModifyStat(a.Attacker.pokemonStat.speed,
								a.Attacker.pokemonBattleStack.speed);

							if (a.Attacker.condition == StatusCondition.Paralysis)
								speedA = speedA / 4;
							if (b.Attacker.condition == StatusCondition.Paralysis)
								speedB = speedB / 4;

							Debug.Log(
								$"배틀로그 {currentTurn}턴 : [{a.Attacker.pokeName}의 스피드 : {speedA}] VS [{b.Attacker.pokeName}의 스피드 : {speedB}]");
							if (speedA != speedB)
								return speedB.CompareTo(speedA);

							// 속도 같으면 랜덤
							return Random.Range(0, 2) == 0 ? -1 : 1;
						});

						// foreach (var act in actions)
						// {
						// 	yield return battleDelay;
						// 	if (act.Attacker.hp <= 0)
						// 	{
						// 		Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} 은/는 기절 행동불가");
						// 		continue;
						// 	}
						//
						// 	Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} ! {act.Skill} !");
						// 	yield return StartCoroutine(
						// 		Manager.Dialog.ShowBattleMessage($"{act.Attacker.pokeName} ! {act.Skill} !"));
						//
						// 	// 상태이상체크
						// 	if (act.Attacker.CanActionCheck())
						// 	{
						// 		// TODO : PP 체크
						// 		yield return StartCoroutine(Motion(act)); // 데미지랑 딜레이 실행 모션 코루틴으로 이동
						// 	}
						//
						// 	//매 액션마다 피격자의 hp 갱신해주기
						// 	if (act.Attacker == playerPokemon)
						// 		hud.SetEnemyHUD(enemyPokemon); // 적 피격
						// 	else
						// 		hud.SetPlayerHUD(playerPokemon); // 플레이어 피격
						//
						// 	isAction = true;
						//
						// }

						// yield return battleDelay;

						isAction = true;
						break;
					}
				case "Pokemon":
					{
						bool isSwitched = false;
						yield return StartCoroutine(PokemonSwitch(false,value => isSwitched = value));
						//포켓몬 교체를 취소한 경우 턴 종료 아님 -> 다시 액션 선택을 기다려야함
						//PokemonSwitch bool 반환으로 변경
						if (!isSwitched)
						{
							selectedAction = null;
							continue;
						}

						

						// TODO : 상대 포켓몬의 PP 체크

						//액션 설정
						var act = new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill);
						actions.Add(act);
						//
						// //5. 액션에 대햔 다이얼로그 출력
						// Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} ! {act.Skill} !");
						// yield return StartCoroutine(
						// 	Manager.Dialog.ShowBattleMessage($"{act.Attacker.pokeName} ! {act.Skill} !"));
						//
						// if (act.Attacker.CanActionCheck())
						// {
						// 	// TODO : PP 체크
						// 	yield return StartCoroutine(Motion(act)); // 데미지랑 딜레이 실행 모션 코루틴으로 이동
						// }
						//
						// //6. 수치 반영
						// hud.SetPlayerHUD(playerPokemon);
						// hud.SetEnemyHUD(enemyPokemon);
						//
						// isAction = true;
						//
						// yield return new WaitForSeconds(1f);
						isAction = true;
						break;
					}
				case "Bag":
					{
						//1. 인벤토리 창을 띄운다.
						Manager.UI.ShowLinkedUI<UI_Bag>("UI_Bag");

						//2. 대기 - 대기를 끝내는 조건 : 가방 UI가 닫혀서 UI 매니저가 관리하는 UI가 없는 경우 = 아무 ui도 열려있지 않은 경우
						yield return new WaitUntil(() => !Manager.UI.IsAnyUIOpen);

						//3. 턴 인지 확인하는 체크
						//플레이어가 도구를 사용한 경우 턴 종료 
						if (Manager.Game.IsInBattle && Manager.Game.IsItemUsed)
						{
							//1.잠시 대기 후
							yield return new WaitForSeconds(0.5f);

							//2. hp 업데이트 (도구 사용했을 가능성)
							hud.SetPlayerHUD(playerPokemon, false);
							hud.SetEnemyHUD(enemyPokemon, false);

							//3. 액션 설정 (상대만 공격하면 되니까 리스트에 넣지 않는다, 정렬 필요 없음)
							var act = new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill);
							actions.Add(act);

							// //4. fight와 동일하게 조건 체크 후 액션 처리
							// if (act.Attacker.hp <= 0)
							// {
							// 	Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} 은/는 기절 행동불가");
							// 	continue;
							// }
							//
							// //5. 액션에 대햔 다이얼로그 출력
							// Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} ! {act.Skill} !");
							// yield return StartCoroutine(
							// 	Manager.Dialog.ShowBattleMessage($"{act.Attacker.pokeName} ! {act.Skill} !"));
							//
							// // 6. 상태이상체크
							// if (act.Attacker.CanActionCheck())
							// {
							// 	// TODO : PP 체크
							// 	// TODO : 상태이상으로 공격 못하는 로그 반영하기
							// 	yield return StartCoroutine(Motion(act)); // 데미지랑 딜레이 실행 모션 코루틴으로 이동
							// }
							//
							// hud.SetPlayerHUD(playerPokemon);
							// hud.SetEnemyHUD(enemyPokemon);
							//
							isAction = true;

						}
						else
						{
							continue;
						}

						break;
					}
				case "Run":
					{
						// 도망못가게함
						if (playerPokemon.isCantRun || playerPokemon.isBind)
						{
							// 검은눈빛, 거미집, 김밥말이
							Debug.Log($"배틀로그 {currentTurn}턴 : {playerPokemon.pokeName} 은/는 도망칠 수 없다!");
							yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"{playerPokemon.pokeName} 은/는 도망칠 수 없다!"));
							isAction = true;
						}
						else if (isTrainer)
						{
							
							yield return StartCoroutine(Manager.Dialog.ShowBattleMessage("안돼! 승부도중에 상대에게 등을 보일 수 없어!"));
							
						}
						else
						{
							StartCoroutine(EndBattleCoroutine("Run"));
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

			if (isAction)
			{
				//액션 선택인 경우 실행

				foreach (var act in actions)
				{
					yield return battleDelay;
					if (act.Attacker.hp <= 0)
					{
						Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} 은/는 기절 행동불가");
						continue;
					}

					Debug.Log($"배틀로그 {currentTurn}턴 : {act.Attacker.pokeName} ! {act.Skill} !");
					yield return StartCoroutine(
						Manager.Dialog.ShowBattleMessage($"{act.Attacker.pokeName} ! {act.Skill} !"));

					// 상태이상체크
					if (act.Attacker.CanActionCheck())
					{
						// TODO : PP 체크
						yield return StartCoroutine(Motion(act)); // 데미지랑 딜레이 실행 모션 코루틴으로 이동
					}

					//매 액션마다 피격자의 hp 갱신해주기
					if (act.Attacker == playerPokemon)
						hud.SetEnemyHUD(enemyPokemon); // 적 피격
					else
						hud.SetPlayerHUD(playerPokemon); // 플레이어 피격
					
					//피격자가 hp가 0이하인경우 기절 및 배틀에서 나오게 처리
					if (act.Target.hp <= 0)
					{
						Transform target;
						if (act.Target == playerPokemon) target = playerPokemonPos;
						else target = enemyPokemonPos;
						yield return StartCoroutine(PlayFaintAnimation(target));
					}
				}
				Debug.Log($"배틀로그 {currentTurn}턴 : {currentTurn} 턴 종료");
				// 턴카운트 증가
				currentTurn++;
				
				
				// 각 포켓몬 턴종료 액션 실행
				playerPokemon.TurnEnd();
				enemyPokemon.TurnEnd();
				
				hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 체력바 업데이트
				hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 체력바 업데이트
				
				if(playerPokemon.hp<=0)
					yield return StartCoroutine(PlayFaintAnimation(playerPokemonPos));
				else if (enemyPokemon.hp <= 0)
					yield return StartCoroutine(PlayFaintAnimation(enemyPokemonPos));
				
			}

			// 플레이어 포켓몬 체크
			if (!Manager.Poke.AlivePokemonCheck())
			{
				Debug.Log($"배틀로그 {currentTurn}턴 : 플레이어 전멸");
				yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"더 이상 싸울 수 있는 포켓몬이 없다!"));
				
				StartCoroutine(EndBattleCoroutine("Lose"));
				yield break;
			}

			// 야생 포켓몬 체크
			if (!isTrainer && enemyPokemon.hp <= 0)
			{
				Debug.Log($"배틀로그 {currentTurn}턴 : 야생 포켓몬 쓰러짐");
				yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"{enemyPokemon.pokeName}은 쓰러졌다!"));

				int totalExp = (int)((enemyPokemon.baseExp * enemyPokemon.level) / 7);
				Debug.Log($"{playerPokemon.pokeName}은/는 {totalExp} 경험치를 얻었다!");
				yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"{playerPokemon.pokeName}은/는 {totalExp} 경험치를 얻었다!"));
				
				//playerPokemon.AddExp(totalExp); -> 아래 코루틴에서 애니메이션 실행하면서 플레이어한테 추가시켜줌
				yield return StartCoroutine(AnimateGainExp(totalExp));
				
				StartCoroutine(EndBattleCoroutine("Win"));
				yield break;
			}
		}
		Debug.Log($"배틀로그 {currentTurn}턴 : 배틀종료");
	}
    #endregion
    
    
	//포켓몬 교체
	private IEnumerator PokemonSwitch(bool haveToChoose, Action<bool> callback=null)
	{
		Pokémon chosen = null;
		bool cancelled = false;

		//포켓몬 선택 팝업창 열기
		pokemonSelect.Show(haveToChoose, playerParty, playerPokemon, p => chosen = p, () => cancelled = true);
		
		yield return new WaitUntil(() => chosen != null || cancelled);

		if (cancelled) // 취소시 메뉴 다시 열기
		{
			Debug.LogWarning("포켓몬 선택 취소");
		}
		else
		{
			if (playerPokemon.hp <= 0 || playerPokemon.isDead)
			{
				yield return StartCoroutine(PlayEnterBattleAnimation(chosen.pokeName,playerPokemonPos,true));

			}
			else
			{
				//교체 애니메이션 실행
				yield return StartCoroutine(PlaySwitchAnimation(playerPokemon.pokeName, chosen.pokeName, playerPokemonPos, true));
			}
			
			playerPokemon = chosen; // 선택 하면 교체
			hud.SetPlayerHUD(playerPokemon, false);
		}
		selectedAction = null;
		callback?.Invoke(!cancelled);
	}

	// 행동 선택 후 행동 처리
	private void ExecuteAction(BattleAction action)
	{
		var skill = Manager.Data.SkillSData.GetSkillDataByName(action.Skill);
		skill.UseSkill(action.Attacker, action.Target, skill);
	}
	

    //배틀 종료
	private IEnumerator EndBattleCoroutine(string reason)
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
					yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"골드는 상금으로 {winMoney}원을 손에 넣었다!"));
					Manager.Data.PlayerData.AddMoney(winMoney);
					Manager.Poke.enemyData.IsFight = true;
				}
				break;
			case "Lose":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 패배");
					yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"{Manager.Data.PlayerData.PlayerName}는(은) 눈 앞이 캄캄해졌다"));

					// TODO : 마지막 회복 위치로 이동해야할듯 우선은 이전씬으로만
				}
				break;
			case "Run":
				{
					Debug.Log($"배틀로그 {currentTurn}턴 : 성공적으로 도망쳤다!");
					yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"{Manager.Data.PlayerData.PlayerName}는(은) 성공적으로 도망쳤다!"));
				}
				break;
		}

		// 변수 초기화
		isTrainer = false;
		setting.allowSceneActivation = true;
		Manager.Game.Player.State = PlayerState.Field;
		//게임 데이터 업데이트
		Manager.Game.EndBattle();

	}



	#region 경험치 증가 코루틴(연속 레벨업시 순차적으로 증가 처리)

	//경험치 증가 코루틴
	public IEnumerator AnimateGainExp(int totalExp)
	{
		while (totalExp > 0)
		{
			int curExp = playerPokemon.curExp;
			int nextLevelExp = playerPokemon.GetExpByLevel(playerPokemon.level + 1);
			int curLevelExp = playerPokemon.GetExpByLevel(playerPokemon.level);
			int requiredToLevelUp = nextLevelExp - curLevelExp; //레벨업까지 현재 레벨에서 올려야하는 경험치 총량

			int remainToLevelUp = playerPokemon.nextExp;

			int expThisRound = Mathf.Min(totalExp, remainToLevelUp);
			totalExp -= expThisRound;

			// 경험치 증가 및 슬라이더 반영
			int fromExp = curExp;
			int toExp = fromExp + expThisRound;

			yield return StartCoroutine(hud.PlayerExpBar.AnimateExpBar(fromExp - curLevelExp, toExp - curLevelExp, requiredToLevelUp));


			// 레벨업 처리
			if (expThisRound == remainToLevelUp)
			{
				playerPokemon.AddExp(expThisRound); //증가할 exp를 넣어주면 알아서 레벨업이랑 계산해서 처리함
				hud.SetPlayerHUD(playerPokemon, false);
				//yield return ShowLevelUpDialog(); // 레벨업 안내 문구

				continue;
			}

			playerPokemon.AddExp(expThisRound);
			// 레벨업이 아니면 종료
			break;
		}
	}

	#endregion
	
	
	#region 포켓몬 스프라이트 교체 / 배틀 중 애니메이션 / 공격 모션
	
	//1. 배틀 시작시 스프라이트 생성 및 hp 바 애니메이션
	public void SpawnPokemonAtStart(Pokémon p, Transform pokemonTransform, bool isPlayer, Action onComplete = null)
	{
		SpriteRenderer sr = pokemonTransform.GetComponent<SpriteRenderer>();
		sr.color = new Color(1, 1, 1, 0); //투명하게 설정

		//플레이어 포켓몬이면 왼-> 타겟 위치로 등장
		Vector2 targetPos = pokemonTransform.position;
		Vector2 entryOffset = isPlayer ? Vector2.left * 10f : Vector2.right * 10f;
		Vector2 startPosition = targetPos + entryOffset;


		float playTime = 1f;

		pokemonTransform.position = startPosition;

		Sequence seq = DOTween.Sequence();
		seq.AppendCallback(() => SetPokemonSprite(p.pokeName, sr, isPlayer));
		seq.Append(pokemonTransform.DOMove(targetPos, playTime).SetEase(Ease.OutQuad));
		seq.Join(sr.DOFade(1f, playTime)); //동시 처리

		//상대 포켓몬이면 오 -> 타겟 위치로 등장

		//끝나고 패널 등장
		seq.OnComplete(() => onComplete?.Invoke());

	}


	//2. 포켓몬 교체시 애니메이션
	//포켓몬 볼로 돌아오기 - 가운데를 중심으로 크기 줄어듬
	public IEnumerator PlayReturnAnimation(string prePokeName, Transform pokemonTransform, bool needMsg)
	{
		if(needMsg)
			yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"{prePokeName} 이젠 됐어 돌아와!"));
		
		
		SpriteRenderer sr = pokemonTransform.GetComponent<SpriteRenderer>();
		
		Sequence seq = DOTween.Sequence();
		seq.Append(pokemonTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack));
		if (sr != null)
			seq.Join(sr.DOFade(0f, 0.5f));

		yield return seq.WaitForCompletion();
	}


	//포켓몬 배틀로 나가기 - 가운데를 중심으로 크기 증가
	public IEnumerator PlayEnterBattleAnimation(string newPokeName, Transform pokemonTransform ,bool isPlayer, Action onComplete = null)
	{
		
		SpriteRenderer sr = pokemonTransform.GetComponent<SpriteRenderer>();
		SetPokemonSprite(newPokeName, sr, isPlayer);
		
		float duration = 0.3f;
		
		if(isPlayer)
			yield return StartCoroutine(Manager.Dialog.ShowBattleMessage($"가랏! {newPokeName}!"));
		
		// 초기 상태: 작게 시작
		pokemonTransform.localScale = Vector3.zero;
		Vector3 targetScale = isPlayer ? pOriginScale : eOriginScale;
		//투명하게 시작 처리
		if (sr != null)
			sr.color = new Color(1f, 1f, 1f, 0f);
		
		
		// 애니메이션 시퀀스
		Sequence seq = DOTween.Sequence();
		seq.Append(pokemonTransform.DOScale(targetScale, duration).SetEase(Ease.OutBack));
		if (sr != null)
			seq.Join(sr.DOFade(1f, duration));

		yield return seq.WaitForCompletion();
		
		onComplete?.Invoke();
	}

	//포켓몬 교체 애니메이션(나가고 들어오는거 전체)
	public IEnumerator  PlaySwitchAnimation(string prePokeName, string newPokeName,
		Transform pokemonTransform, bool isPlayer, float delay = 0.2f, Action onComplete = null)
	{
		// 1. 작아지며 퇴장
		yield return StartCoroutine(PlayReturnAnimation(prePokeName, pokemonTransform, isPlayer));

		// 2. 잠깐 대기
		yield return new WaitForSeconds(delay);

		// 3. 스프라이트 교체
		SpriteRenderer sr = pokemonTransform.GetComponent<SpriteRenderer>();
		SetPokemonSprite(newPokeName, sr, isPlayer);

		// 4. 커지며 등장
		yield return StartCoroutine(PlayEnterBattleAnimation(newPokeName, pokemonTransform, isPlayer));

		onComplete?.Invoke();

	}
	
	
	//스프라이트 불러오기
	public void SetPokemonSprite(string pokeName, SpriteRenderer spriteRenderer, bool isPlayerPokemon)
	{
		Debug.Log("스프라이트 불러오기");

		if (isPlayerPokemon) spriteRenderer.sprite = Manager.Data.SJH_PokemonData.GetBattleBackSprite(pokeName);
		else spriteRenderer.sprite = Manager.Data.SJH_PokemonData.GetBattleFrontSprite(pokeName);
	}

	private IEnumerator PlayFaintAnimation(Transform target)
	{
		Vector2 startPos = target.localPosition;
		Vector2 targetPos = startPos + Vector2.down * 4;
		
		float playTime = 1f;
		Debug.Log(startPos);
		Debug.Log(targetPos);
		Sequence seq = DOTween.Sequence();
		seq.Append(target.DOMove(targetPos, playTime).SetEase(Ease.Linear)); // 아래로 이동

		yield return seq.WaitForCompletion(); // 아래로 이동 완료 대기
    
		// PlayReturnAnimation을 직접 실행하고 대기
		yield return StartCoroutine(PlayReturnAnimation(null, target, false));
	}
	
	
	
	//공격, 피격 모션
	private IEnumerator Motion(BattleAction action)
	{
		// 공격 모션
		var atkPos = action.Attacker == playerPokemon ? playerPokemonPos : enemyPokemonPos;
		var atkAnim = atkPos.GetComponent<Animator>();
		atkAnim.SetTrigger("DoAttack");

		yield return new WaitForSeconds(0.5f);

		ExecuteAction(action);
		// hud.SetPlayerHUD(playerPokemon);
		// hud.SetEnemyHUD(enemyPokemon);

		//피격 모션
		var tgtPos = action.Target == playerPokemon ? playerPokemonPos : enemyPokemonPos;
		var tgtAnim = tgtPos.GetComponent<Animator>();
		tgtAnim.SetTrigger("DoBlink");
		yield return new WaitForSeconds(0.5f);
	}


	
	#endregion

}