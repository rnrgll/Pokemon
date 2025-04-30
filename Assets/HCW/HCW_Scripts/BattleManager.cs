using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 배틀 로직 및 흐름 제어
public class BattleManager : MonoBehaviour
{
    private const int MaxPartySize = 6;              // 최대 파티 크기
	// [SerializeField] private PlayerPokemonPos;       // 플레이어 포켓몬 위치
	// [SerializeField] private EnemyPokemonPos;        // 적 포켓몬 위치
	[SerializeField] private BattleUIController ui;  // UI 요소
	[SerializeField] private BattleHUD hud;          // HP 게이지·텍스트 제어

	private List<Pokemon> playerParty;    // 플레이어 포켓몬 리스트
    private Pokemon playerPokemon;        // 플레이어 포켓몬

    private List<Pokemon> enemyParty;     // 적 포켓몬 리스트
    private int currentEnemyIndex;        // 현재 적 포켓몬 인덱스
    private Pokemon enemyPokemon;         // 적 포켓몬

    private string selectedAction;        // 선택된 행동
    private Skill playerSelectedSkill;    // 선택된 스킬
    private Skill enemySelectedSkill;     // 적 포켓몬이 사용할 기술


    private void Awake()
    { 
	// UI 이벤트 구독
       ui.OnActionSelected.AddListener(OnActionButton);
       ui.OnSkillSelected.AddListener(OnSkillButton);
    }
    
    private void OnDestroy()
    {
	// 구독 해제
       ui.OnActionSelected.RemoveListener(OnActionButton);
       ui.OnSkillSelected.RemoveListener(OnSkillButton);
    }

    // 배틀 시작: 플레이어/적 파티 초기화 및 첫 포켓몬 설정
    public void StartBattle(List<Pokemon> party, List<Pokemon> enemies)
    {
       playerParty = party?.Take(MaxPartySize).ToList() ?? new List<Pokemon>();// 파티의 최대 크기 설정 및 초기화
	enemyParty = enemies?.ToList() ?? new List<Pokemon>(); // 적 포켓몬 리스트 초기화

	if (playerParty.Count == 0 || enemyParty.Count == 0)
        {
            Debug.LogError("플레이어 또는 적 포켓몬 파티가 비어있습니다!");
            return;
        }
    
        playerPokemon = playerParty[0]; // 파티의 첫번째 포켓몬
        enemyPokemon = enemyParty[0]; // 적의 첫번째 포켓몬

	 hud.SetPlayerHUD(playerPokemon); // 플레이어 포켓몬 HUD 설정
	 hud.SetEnemyHUD(enemyPokemon);   // 적 포켓몬 HUD 설정

	 Debug.Log($"배틀 시작 : {playerPokemon.Name} VS {enemyPokemon.Name}");
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        while (playerPokemon.HP > 0 && currentEnemyIndex < enemyParty.Count)
        {
            // 적 포켓몬 교체 체크
            if (enemyPokemon.HP <= 0)
            {
                currentEnemyIndex++; // 다음 포켓몬
                if (currentEnemyIndex < enemyParty.Count)
                {
                    enemyPokemon = enemyParty[currentEnemyIndex];
                    Debug.Log($"상대는 {enemyPokemon.Name}을/를 꺼냈다");
                    yield return new WaitForSeconds(1f);
                    continue;
                }
                break;
            }
    
            // 행동 선택
            selectedAction = null;
            yield return new WaitUntil(() => selectedAction != null);
    
            // 전투 수행
            if (selectedAction == "Fight")
            {
                playerSelectedSkill = null;
			 ui.ShowSkillSelection(playerPokemon);
			 yield return new WaitUntil(() => playerSelectedSkill != null); // 기술 선택할때까지 대기
			 ui.HideSkillSelection();

			 enemySelectedSkill = EnemyChooseAction();

                var actions = new List<BattleAction> // 적과 플레이어의 행동을 리스트에 추가
                {
                    new BattleAction(playerPokemon, enemyPokemon, playerSelectedSkill),
                    new BattleAction(enemyPokemon, playerPokemon, enemySelectedSkill)
                };
                
                actions.Sort((a, b) => b.Attacker.Speed.CompareTo(a.Attacker.Speed)); // 속도에 따라 정렬

			foreach (var act in actions) ///
                {
                    if (act.Attacker.HP <= 0) continue;
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
    private void OnSkillButton(int idx) => playerSelectedSkill = playerPokemon.Skills[idx];

    private Skill EnemyChooseAction()
    {
        int idx = Random.Range(0, enemyPokemon.Skills.Count);
        return enemyPokemon.Skills[idx];
    }
    
    private void ExecuteAction(BattleAction action)
    {
        Debug.Log($"{action.Attacker.Name} 사용 {action.Skill.Name}");
        Attack(action.Attacker, action.Target, action.Skill);
    }
    
    private void Attack(Pokemon atk, Pokemon tgt, Skill skl)
    {
        tgt.HP = Mathf.Max(0, tgt.HP - skl.Power);
        Debug.Log($"{tgt.Name} HP: {tgt.HP}");
    }
    
    private void EndBattle()
    {
        if (playerPokemon.HP <= 0)
            Debug.Log("게임 오버: 플레이어 전멸");
        else
		{
			Debug.Log("승리: 모든 적 포켓몬 격파");
			// 경험치 및 보상, 이전 씬으로 다시 이동 구현 필요
		}
            
    }
}