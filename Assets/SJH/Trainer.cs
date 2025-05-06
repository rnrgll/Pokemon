using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TrainerPokemon
{
	public string PokeName;
	public int PokeLevel;
}

[System.Serializable]
public class TrainerData
{
	public int TrainerId;
	public string Name;
	public bool IsFight;
	public int Money;
	public List<TrainerPokemon> TrainerPartyData;
}

public class Trainer : MonoBehaviour, IInteractable
{
	// TrainerData
	[SerializeField] public int trainerId;
	[SerializeField] public bool isFight;

	// 이동
	[SerializeField] public Vector2 exitPos;

	[SerializeField] Animator anim;
	[SerializeField] public bool isTrainerMove;
	[SerializeField] public Vector2 currentDirection;
	[SerializeField] public float moveDuration;
	Coroutine moveCoroutine;

	void Start()
	{
		// 시작할 때 등록된 트레이너아이디면 isFight 설정
		isFight = Manager.Event.TrainerIsFight(trainerId);
		anim = GetComponent<Animator>();

		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
		anim.SetBool("npcMoving", false);
	}

	void Update()
	{
		if (isTrainerMove && moveCoroutine == null)
		{
			moveCoroutine = StartCoroutine(TrainerMove());
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("Player"))
		{
			Debug.Log("시야에 플레이어 들어옴!");

			if (isFight)
				return;

			exitPos = (Vector2)Manager.Game.Player.transform.position - currentDirection * 2;
			isTrainerMove = true;

			Debug.Log("배틀 시작");
			//BattleStart();
		}
	}

	public void Interact(Vector2 position)
	{
		Debug.Log("트레이너 배틀 체크");
		if (isFight)
		{
			Debug.Log("이미 이긴 트레이너 입니다.");
			return;
		}

		Debug.Log("배틀 시작");
		BattleStart();
	}

	public void BattleStart()
	{
		// 포켓몬 매니저 enemyParty에 파티 넣으면 배틀씬에서 트레이너 배틀로 인식
		Manager.Poke.enemyData = Manager.Event.GetTrainerDataById(trainerId);

		// 플레이어 스탑
		var player = Manager.Game.Player;
		player.GetComponent<Player>().StopMoving();

		// 씬전환 전 정보 저장
		player.PrevSceneName = SceneManager.GetActiveScene().name;

		Debug.Log("트레이너 배틀 시작");

		// 씬전환
		player.CurSceneName = "BattleScene_UIFix";
		SceneManager.LoadScene("BattleScene_UIFix");
	}

	IEnumerator TrainerMove()
	{
		// 도착위치 = 플레이어 위치 - 엔피시방향 * 2
		Vector3 startPos = transform.position;
		Vector3 targetPos = startPos + (Vector3)(currentDirection * 2);
		currentDirection = (targetPos - startPos).normalized;

		bool isEnd = false;
		if (currentDirection.x == 0)	//상하
		{
			if (Mathf.Approximately(startPos.y, exitPos.y))
				isEnd = true;
		}
		else if (currentDirection.y == 0)	// 좌우
		{
			if (Mathf.Approximately(startPos.x, exitPos.x))
				isEnd = true;
		}

		Debug.Log(isEnd);
		if (isEnd)
		{
			// 플레이어를 체크하려면 레이를 좀 낮게
			Vector2 rayStartPos = (Vector2)startPos + Vector2.down * 0.3f;

			// 이동 전에 Raycast 검사

			RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartPos + currentDirection * 1.1f, currentDirection, 0.5f);
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider != null && hit.transform.gameObject.CompareTag("Player"))
				{
					isTrainerMove = false;
					anim.SetBool("npcMoving", false);
					FindPlayer();
					moveCoroutine = null;
					Debug.Log("플레이어 감지");
					yield break;
				}
			}
		}

		if (!isEnd)
		{
			// 이동 시작
			anim.SetFloat("x", currentDirection.x);
			anim.SetFloat("y", currentDirection.y);
			anim.SetBool("npcMoving", true);
			float time = 0;
			while (time < moveDuration)
			{
				time += Time.deltaTime;
				float percent = time / moveDuration;
				transform.position = Vector2.Lerp(startPos, targetPos, percent);
				yield return null;
			}
			transform.position = targetPos;

			// 도착 후 코루틴 null
			moveCoroutine = null;
		}
	}

	void FindPlayer()
	{
		Manager.Game.Player.state = Define.PlayerState.Dialog;

		// TODO : 대사 출력
		Debug.Log("트레이너 앞에 플레이어 발견 배틀시작");
		//BattleStart();
		
	}
}

