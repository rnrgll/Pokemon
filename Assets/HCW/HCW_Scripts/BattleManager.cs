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

	[SerializeField] private bool istraner;                // 상대가 트레이너 인지
	[SerializeField] private string enemyTrainerName;    // 트레이너면 이름


	Coroutine battleCoroutine;


	

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
		istraner = true; // 상대가 트레이너일경우
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
		istraner = false; // 상대가 트레이너가 아닐경우
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

		while (playerPokemon.hp > 0 && currentEnemyIndex < enemyParty.Count)
		{
			// 적 포켓몬 교체 체크
			if (enemyPokemon.hp <= 0)
			{
				// 포켓몬 경험치 + 해줘야함
				Debug.Log($"{playerPokemon.pokeName}가 경험치를 얻었다");

				if (istraner)
				{
					currentEnemyIndex++; // 다음 포켓몬
					if (currentEnemyIndex < enemyParty.Count)
					{
						enemyPokemon = enemyParty[currentEnemyIndex];
						Debug.Log($"상대는 {enemyPokemon.pokeName}을/를 꺼냈다");
						yield return new WaitForSeconds(1f);
						continue;
					}

				}
				yield return new WaitForSeconds(1f);
				break;
			}


			// 행동 선택 대기
			selectedAction = null;
			yield return new WaitUntil(() => selectedAction != null);
			ui.HideActionMenu();

			// 전투 수행
			if (selectedAction == "Fight")
			{
				playerSelectedSkill = null;
				ui.ShowSkillSelection(playerPokemon);
				yield return new WaitUntil(() => playerSelectedSkill != null); // 기술 선택할때까지 대기
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
					if (act.Attacker.hp <= 0) continue;
					ExecuteAction(act);
					yield return new WaitForSeconds(1f);
				}

				hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 체력바 업데이트
				hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 체력바 업데이트

				yield return new WaitForSeconds(1f);
			}
			else // Fight가 아닌 선택지 추가 필요
			{
				Debug.Log($"플레이어 액션: {selectedAction}");
				yield return new WaitForSeconds(1f);
			}
		}
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
		// int dmg = 
		Debug.Log($"{tgt.pokeName} hp: {tgt.hp}");
	}

	// 배틀 종료 처리
	private void EndBattle()
	{
		if (playerPokemon.hp <= 0)
			Debug.Log("게임 오버: 플레이어 전멸");
		else
		{
			Debug.Log("승리: 모든 적 포켓몬 격파");
			// 트레이너 배틀일 경우 돈 + 경험치
			// 경험치 및 보상, 이전 씬으로 다시 이동 구현 필요
			SceneManager.LoadScene(previousScene); // 이전 씬으로 이동
			// 변수 초기화
			istraner = false;
			// 코루틴 초기화
			battleCoroutine = null;
			StopCoroutine(battleCoroutine);
			
		}
		Destroy(Manager.Poke.enemyPokemon);
	}
}