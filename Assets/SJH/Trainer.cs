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
	[SerializeField] BoxCollider2D coll;

	[SerializeField] public bool isTrainerMove;
	[SerializeField] public Vector2 currentDirection;
	[SerializeField] public float moveDuration;
	[SerializeField] public bool isTrainerTurn;

	Coroutine moveCoroutine;
	Coroutine findCoroutine;
	Coroutine turnCoroutine;

	// 대사
	[SerializeField] Dialog dialog;

	// 풀이펙트
	[SerializeField] GameObject grassEffect;

	// 레이용
	[SerializeField] float findDistance = 4f;
	[SerializeField] bool isFind;

	void Start()
	{
		// 시작할 때 등록된 트레이너아이디면 isFight 설정
		isFight = Manager.Event.TrainerIsFight(trainerId);
		anim = GetComponent<Animator>();
		coll = GetComponent<BoxCollider2D>();

		// 풀 이펙트 설정
		grassEffect = transform.GetChild(0).gameObject;
		grassEffect.SetActive(false);

		// 첫 방향 설정
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

		if (isTrainerTurn && turnCoroutine == null)
		{
			turnCoroutine = StartCoroutine(TrainerTurn());
		}

		if (!isFind && !isFight && !isTrainerMove && findCoroutine == null)
		{
			findCoroutine = StartCoroutine(FindPlayer());
		}
	}

	//void OnTriggerEnter2D(Collider2D collision)
	//{
	//	if (collision.transform.CompareTag("Player"))
	//	{
	//		Debug.Log("시야에 플레이어 들어옴!");

	//		if (isFight)
	//			return;

	//		if (findCoroutine == null)
	//		{
	//			findCoroutine = StartCoroutine(FindPlayer());
	//		}
	//	}
	//}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Grass") && collision.IsTouching(coll))
		{
			if (grassEffect != null)
				grassEffect.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Grass"))
		{
			if (grassEffect != null)
				grassEffect.SetActive(false);
		}
	}

	IEnumerator FindPlayer()
	{
		if (isTrainerMove)
		{
			findCoroutine = null;
			yield break;
		}
		// TODO : 레이 쏘기
		Vector2 rayStartPos = (Vector2)transform.position;

		if (currentDirection.x != 0)
			rayStartPos += Vector2.down * 0.3f;
		RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartPos, currentDirection, findDistance);

		isFind = false;

		foreach (RaycastHit2D hit in hits)
		{
			// 플레이어를 찾았을 때
			if (hit.transform.CompareTag("Player"))
			{
				isTrainerTurn = false;
				isFind = true;
				break;
			}
		}

		if (!isFind)
		{
			findCoroutine = null;
			yield break;
		}

		// 플레이어 이동 제한
		var player = Manager.Game.Player;
		player.State = Define.PlayerState.Dialog;

		// 이동 할 위치 저장
		yield return new WaitUntil(() =>
		{
			Vector2Int pos = Vector2Int.RoundToInt(player.transform.position);
			return pos.x % 2 == 0 && pos.y % 2 == 0;
		});

		// 플레이어 이동 종료
		player.StopMoving();

		// 위치 저장
		exitPos = Vector2Int.RoundToInt(player.transform.position) - Vector2Int.RoundToInt(currentDirection * 2);

		isTrainerMove = true;
		findCoroutine = null;
	}

	public void Interact(Vector2 position)
	{
		Debug.Log("트레이너 배틀 체크");

		// 플레이어 방향으로 변경
		var player = Manager.Game.Player;
		player.State = Define.PlayerState.Dialog;

		Vector2 dir = currentDirection;

		if (player.currentDirection == Vector2.up)
			dir = Vector2.down;
		else if (player.currentDirection == Vector2.down)
			dir = Vector2.up;
		else if (player.currentDirection == Vector2.left)
			dir = Vector2.right;
		else
			dir = Vector2.left;

		anim.SetFloat("x", dir.x);
		anim.SetFloat("y", dir.y);

		if (isFight)
		{
			Debug.Log("이미 이긴 트레이너 입니다.");
			player.State = Define.PlayerState.Field;
			return;
		}

		if (Manager.Dialog.isTyping == false)
		{
			Manager.Dialog.npcState = Define.NpcState.Talking;
			Manager.Dialog.StartDialogue(dialog);
			Debug.Log("배틀 시작");
			BattleStart();
		}
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

		// TODO : 배틀 기능 끝나면 주석 해제하기
		// 씬전환
		//player.CurSceneName = "BattleScene_UIFix";
		//SceneManager.LoadScene("BattleScene_UIFix");
	}

	IEnumerator TrainerMove()
	{
		// 도착위치 = 플레이어 위치 - 엔피시방향 * 2
		Vector3 startPos = transform.position;
		Vector3 targetPos = startPos + (Vector3)(currentDirection * 2);
		currentDirection = (targetPos - startPos).normalized;
		//Debug.Log($"시작위치 : {startPos} 도착위치 : {exitPos} 방향 : {currentDirection}");

		bool isEnd = false;
		if (currentDirection.x == 0)    //상하
		{
			if (Mathf.Approximately(startPos.y, exitPos.y))
				isEnd = true;
		}
		else if (currentDirection.y == 0)   // 좌우
		{
			if (Mathf.Approximately(startPos.x, exitPos.x))
				isEnd = true;
		}

		if (isEnd)
		{
			// 플레이어를 체크하려면 레이를 좀 낮게
			Vector2 rayStartPos = (Vector2)transform.position;
			if (currentDirection.x != 0)
				rayStartPos += Vector2.down * 0.3f;
			// 이동 전에 Raycast 검사
			RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartPos + currentDirection * 1.1f, currentDirection, 1f);
			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider != null && hit.transform.gameObject.CompareTag("Player"))
				{
					isTrainerMove = false;
					anim.SetBool("npcMoving", false);
					BattleCheck();
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

	IEnumerator TrainerTurn()
	{
		Debug.Log("엔피시 회전");

		int ran = UnityEngine.Random.Range(0, 4);
		Vector2 dir;
		switch (ran)
		{
			case 0: dir = Vector2.up; break;
			case 1: dir = Vector2.down; break;
			case 2: dir = Vector2.left; break;
			default: dir = Vector2.right; break;
		}

		currentDirection = dir;

		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);

		yield return new WaitForSeconds(2f);

		turnCoroutine = null;
	}

	void BattleCheck()
	{
		// 위에서 지정
		//Manager.Game.Player.state = Define.PlayerState.Dialog;

		// 플레이어 방향을 트레이너 방향으로 바꾸기
		var player = Manager.Game.Player;
		Vector2 dir = -currentDirection;
		player.AnimChange(dir);

		// 대사 출력
		if (Manager.Dialog.isTyping == false)
		{
			Manager.Dialog.npcState = Define.NpcState.Talking;
			Manager.Dialog.StartDialogue(dialog);
			Debug.Log("배틀 시작");
			BattleStart();
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Vector3 rayStart = transform.position;
		if (currentDirection.x != 0)
			rayStart += Vector3.down * 0.3f;
		Gizmos.DrawLine(rayStart, rayStart + (Vector3)(currentDirection * findDistance));
	}
}

