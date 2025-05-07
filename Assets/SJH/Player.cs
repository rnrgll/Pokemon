using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using static Define;

public class Player : MonoBehaviour
{
	public PlayerState prevState;
	[SerializeField] Define.PlayerState state;
	[Tooltip("플레이어 상태")]
	[SerializeField] public Define.PlayerState State {
		get => state;
		set
		{
			if (state != value)
			{
				Debug.Log($"플레이어 상태 변경: {state} > {value}");
				prevState = state;
				state = value;
			}
		}
	}
	[Tooltip("플레이어 방향")]
	[SerializeField] public Vector2 currentDirection = Vector2.down; // 처음 방향은 아래
	// 플레이어 이동 코루틴
	public Coroutine moveCoroutine;
	// 플레이어 점프 코루틴
	Coroutine jumpCoroutine;
	// 점프 시간
	WaitForSeconds jumpTime;
	// 플레이어 Z 키 코루틴
	public Coroutine zInput;

	[Tooltip("이동 거리 (기본 2)")]
	[SerializeField] public int moveValue = 2;
	[Tooltip("이동 시간 (기본 0.3)")]
	[SerializeField] public float moveDuration = 0.3f;
	[Tooltip("플레이어 이동상태")]
	[SerializeField] bool isMoving = false;
	// 플레이어 방향키입력 뗐을 때 트리거
	bool isIdle = false;
	[Tooltip("플레이어 씬 변경 상태")]
	[SerializeField] public bool isSceneChange;

	// 플레이어 점프중 체크
	bool isJump = false;
	[Tooltip("플레이어 그림자 오브젝트")]
	[SerializeField] GameObject shadow;

	[Tooltip("플레이어 풀 오브젝트")]
	[SerializeField] GameObject grassEffect;

	public static event Action OnGrassEntered;

	[SerializeField] public Queue<Vector3> prevPosQueue = new Queue<Vector3>();

	[SerializeField] string curSceneName;
	public string CurSceneName
	{
		get => curSceneName;
		set
		{
			Debug.Log("씬 이름 변경 / 사운드 이벤트 실행");
			curSceneName = value;
			OnSceneChangeEvent?.Invoke(curSceneName);
		}
	}
	[SerializeField] public event Action<string> OnSceneChangeEvent;

	[SerializeField] private string prevSceneName;
	public string PrevSceneName
	{
		get => prevSceneName;
		set => prevSceneName = value;
	}


	Animator anim;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		anim = GetComponent<Animator>();
		// Z 키 코루틴
		zInput = StartCoroutine(ZInput());
		// 점프 시간
		jumpTime = new WaitForSeconds(0.03f);

		CurSceneName = SceneManager.GetActiveScene().name;
	}

	private void OnEnable()
	{
		if(Manager.Game.Player==null)
			Manager.Game.SetPlayer(this);
	}

	private void OnDisable()
	{
		if(Manager.Game!=null && Manager.Game.Player!=null)
			Manager.Game.ReleasePlayer();
	}

	void Start()
	{
		// 그림자 / 풀 이펙트 비활성화
		if (shadow != null)
			shadow.SetActive(false);
		if (grassEffect != null)
			grassEffect.SetActive(false);
    
		Manager.UI.OnAllUIClosed += OnAllUIClosed;
	}
	
	private void OnDestroy()
	{
		//이벤트 구독 해제
		if(Manager.UI!=null)
			Manager.UI.OnAllUIClosed -= OnAllUIClosed;
	}

	void Update()
	{
		if (State == Define.PlayerState.SceneChange) // 씬이동중
			return;

		switch (State)
		{
			case Define.PlayerState.Field:          // 필드
				MoveState();
				break;
			case Define.PlayerState.Battle:         // 배틀중
				break;
			case Define.PlayerState.UI:             // UI활성화중
				break;
			case Define.PlayerState.Menu:           // Menu 활성화중
				break;
			case Define.PlayerState.Dialog:         //	대화 활성화중
				Debug.Log("현재 State = Dialog");
				break;
		}
	}

	public void AnimChange()
	{
		anim.SetFloat("x", currentDirection.x);
		anim.SetFloat("y", currentDirection.y);
	}
	public void AnimChange(Vector2 direction)
	{
		anim.SetFloat("x", direction.x);
		anim.SetFloat("y", direction.y);
	}

	void MoveState()
	{
		if (isJump)
			return;
		// Idle 설정
		if (Input.GetKeyUp(KeyCode.UpArrow) ||
			Input.GetKeyUp(KeyCode.DownArrow) ||
			Input.GetKeyUp(KeyCode.LeftArrow) ||
			Input.GetKeyUp(KeyCode.RightArrow))
		{
			isIdle = true;
		}

		if (isMoving)
			return;
		
		//메뉴 키 입력
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if (!Manager.UI.IsAnyUIOpen)
			{
				Manager.UI.ShowLinkedUI<UI_Menu>("UI_Menu");
				State = Define.PlayerState.Menu; // 플레이어 상태를 UI로 전환
			}
			return;
		}

		Vector2 inputDir = Vector2.zero;
		if (Input.GetKey(KeyCode.UpArrow)) inputDir = Vector2.up;
		else if (Input.GetKey(KeyCode.DownArrow)) inputDir = Vector2.down;
		else if (Input.GetKey(KeyCode.LeftArrow)) inputDir = Vector2.left;
		else if (Input.GetKey(KeyCode.RightArrow)) inputDir = Vector2.right;

		if (inputDir == Vector2.zero)
			return;

		currentDirection = inputDir;

		// 방향 변경
		AnimChange(inputDir);

		// 레이캐스트 벽 체크
		Debug.DrawRay((Vector2)transform.position + inputDir * 1.1f, inputDir, Color.green);
		RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)transform.position + inputDir * 1.1f, inputDir, 1f);
		foreach (var hit in hits)
		{
			string tag = hit.transform.gameObject.tag;
			// Debug.Log($"앞에 : [{hit.transform.gameObject.name}]");

			// NPC의 시야 콜라이더는 지나갈 수 있게
			if (tag == "NPC" && hit.collider.isTrigger)
				continue;

			switch (tag)
			{
				case "Wall":
				case "NPC":
					StopMoving();
					return;
				case "SlopeLeft":
				case "SlopeRight":
				case "SlopeDown":
					bool isCanJump =
						(hit.transform.gameObject.tag == "SlopeLeft" && inputDir == Vector2.left) ||
						(hit.transform.gameObject.tag == "SlopeRight" && inputDir == Vector2.right) ||
						(hit.transform.gameObject.tag == "SlopeDown" && inputDir == Vector2.down);

					if (isCanJump)
					{
						if (jumpCoroutine == null)
						{
							if (moveCoroutine != null)
								StopCoroutine(moveCoroutine);

							jumpCoroutine = StartCoroutine(Jump(currentDirection));
						}
					}
					else
					{
						StopMoving();
					}
					return;
			}
		}

		// 같은 방향이면 이동 시작
		if (inputDir == currentDirection)
		{
			moveCoroutine = StartCoroutine(Move(inputDir));
		}
		// 방향만 전환
		else
		{
			currentDirection = inputDir;
		}
	}

	IEnumerator Move(Vector2 direction)
	{
		// 1 이동 = x or y 2 변화
		// 바로 2를 이동하지않고 이동시간에 걸쳐서 이동
		isMoving = true;
		isIdle = false;
		anim.SetBool("isMoving", isMoving);

		Vector2 startPos = transform.position;
		Vector2 endPos = startPos + (direction * moveValue);

		float time = 0;
		while (time < moveDuration && isMoving)
		{
			time += Time.deltaTime;
			float percent = time / moveDuration;
			transform.position = Vector2.Lerp(startPos, endPos, percent);
			yield return null;
		}
		transform.position = endPos;

		isMoving = false;
		if (isIdle)
		{
			anim.SetBool("isMoving", false);
			isIdle = false;
		}

		// 타일체크
		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, 0);
		foreach (var hit in hits)
		{
			if (hit.collider.CompareTag("Grass"))
			{
				//Debug.Log("플레이어는 수풀에 있음 이벤트 실행");
				OnGrassEntered?.Invoke();
			}
		}
		prevPosQueue.Enqueue(startPos);
		//Debug.Log($"플레이어 위치 저장 : {startPos}");
	}

	public void PlayerMove(Vector2 direction)
	{
		if (moveCoroutine == null)
		{
			moveCoroutine = StartCoroutine(Move(direction));
		}
	}
	public void StopMoving()
	{
		isMoving = false;
		anim.SetBool("isMoving", false);
		moveCoroutine = null;
	}

	//void OnDrawGizmos()
	//{
	//	// 플레이어 이동방향 기즈모
	//	Gizmos.color = Color.magenta;
	//	Gizmos.DrawLine((Vector2)transform.position + Vector2.up * 1.1f, (Vector2)transform.position + Vector2.up * 1.1f + Vector2.up);
	//	Gizmos.DrawLine((Vector2)transform.position + Vector2.down * 1.1f, (Vector2)transform.position + Vector2.down * 1.1f + Vector2.down);
	//	Gizmos.DrawLine((Vector2)transform.position + Vector2.right * 1.1f, (Vector2)transform.position + Vector2.right * 1.1f + Vector2.right);
	//	Gizmos.DrawLine((Vector2)transform.position + Vector2.left * 1.1f, (Vector2)transform.position + Vector2.left * 1.1f + Vector2.left);
	//}

	IEnumerator ZInput()
	{
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.Z))
			{
				switch (State)
				{
					case PlayerState.Field:
						// 필드 상호작용
						Debug.DrawRay((Vector2)transform.position + currentDirection * 1.1f, currentDirection, Color.red);
						RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)transform.position + currentDirection * 1.1f, currentDirection, 1f);
						foreach (var hit in hits)
						{
							var check = hit.transform.GetComponent<IInteractable>();
							check?.Interact(transform.position);
						}
						break;
					case PlayerState.Battle:
						// 배틀 대화 넘기기
						break;
					case PlayerState.UI:
						// UI
						break;
					case PlayerState.Menu:
						break;
					case PlayerState.Dialog:
						DialogManager.Instance.HandleUpdate();
						// 대화
						break;
				}
			}
			yield return null;
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Grass"))
		{
			if (grassEffect != null && !grassEffect.activeSelf)
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

	IEnumerator Jump(Vector2 direction)
	{
		isJump = true;
		float jumpHeight = 2;
		float jumpDistance = 4;


		Vector3 startPos = new Vector3(
			Mathf.Round(transform.position.x),
			direction == Vector2.up ? Mathf.Round(transform.position.y - 1) : (int)transform.position.y,
			0);

		Vector3 endPos = startPos + ((Vector3)direction * jumpDistance);

		// 그림자 활성화
		if (shadow != null)
		{
			shadow.SetActive(true);
			shadow.transform.SetParent(null);   // 부모 분리
			shadow.transform.position = startPos;
		}

		//Debug.Log($"이전 위치 : {transform.position}");
		//Debug.Log($"시작 위치 : {startPos}");
		//Debug.Log($"도착 위치 : {endPos}");

		// 네방향 구분
		switch (direction)
		{
			case var v when v == Vector2.up:
				{
					float shadowXPos = startPos.x;
					for (int i = 3; i >= 0; i--)
					{
						float yPos = startPos.y + (0.16f * i);
						transform.position = new Vector3(startPos.x, yPos, 0);
						//Debug.Log($"{i} : {transform.position}");
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(shadowXPos, transform.position.y - (0.1f * i));
						}
						yield return jumpTime;
					}
					for (int i = 1; i <= 7; i++)
					{
						float yPos = startPos.y + (jumpDistance / 10 * i);
						transform.position = new Vector3(startPos.x, yPos, 0);
						//Debug.Log($"{i} : {transform.position}");
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(shadowXPos, transform.position.y);
						}
						if (i == 5)
							StopMoving();
						yield return jumpTime;
					}
					break;
				}
			case var v when v == Vector2.down:
				{
					float shadowXPos = startPos.x;
					for (int i = 3; i >= 0; i--)
					{
						float yPos = startPos.y + (0.16f * i);
						transform.position = new Vector3(startPos.x, yPos, 0);
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(shadowXPos, transform.position.y - 1);
						}
						yield return jumpTime;
					}
					for (int i = 1; i <= 7; i++)
					{
						float yPos = startPos.y - (jumpDistance / 10 * i);
						transform.position = new Vector3(startPos.x, yPos, 0);
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(shadowXPos, Mathf.Max(endPos.y, transform.position.y - 1f));
						}
						if (i == 6)
							StopMoving();
						yield return jumpTime;
					}
					break;
				}
			case var v when v == Vector2.left:
				{
					float shadowYPos = startPos.y;
					for (int i = 1; i <= 5; i++)
					{
						float playerYPos = startPos.y + (jumpHeight / 10 * i);
						transform.position = new Vector3(startPos.x - (jumpDistance / 10 * i), playerYPos, 0);
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(transform.position.x, shadowYPos);
						}
						yield return jumpTime;
					}
					for (int i = 6; i <= 10; i++)
					{
						float playerYPos = (startPos.y + jumpHeight / 2) - (jumpHeight / 10 * (i - 5));
						transform.position = new Vector3(startPos.x - (jumpDistance / 10 * i), playerYPos, 0);
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(transform.position.x, shadowYPos);
						}
						if (i == 8)
							StopMoving();
						yield return jumpTime;
					}
				}
				break;
			case var v when v == Vector2.right:
				{
					float shadowStartY = startPos.y;
					for (int i = 1; i <= 5; i++)
					{
						float playerYPos = startPos.y + (jumpHeight / 10 * i);
						transform.position = new Vector3(startPos.x + (jumpDistance / 10 * i), playerYPos, 0);
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(transform.position.x, shadowStartY, 0);
						}
						yield return jumpTime;
					}
					for (int i = 6; i <= 10; i++)
					{
						float playerYPos = (startPos.y + jumpHeight / 2) - (jumpHeight / 10 * (i - 5));
						transform.position = new Vector3(startPos.x + (jumpDistance / 10 * i), playerYPos, 0);
						if (shadow != null)
						{
							shadow.transform.position = new Vector3(transform.position.x, shadowStartY, 0);
						}
						if (i == 8)
							StopMoving();
						yield return jumpTime;
					}
				}
				break;
		}

		// 점프 후 위치 설정
		transform.position = endPos;
		isJump = false;
		jumpCoroutine = null;

		// 그림자 비활성화
		if (shadow != null)
		{
			shadow.SetActive(false);
			shadow.transform.SetParent(transform);	// 플레이어를 부모로 다시 설정
		}

		yield return new WaitForSeconds(0.5f);
	}
	#region UI
	void OnAllUIClosed()
	{
		//만약 이전 상태가 필드가 아니라 배틀 상태였다면..?? -> 추후 로직에 따라 수정 필요
		if(State==Define.PlayerState.Menu)
			State = Define.PlayerState.Field;
		else if (Manager.Game.IsInBattle)
		{
			State = Define.PlayerState.Battle;
		}
		else
		{
			//todo: 수정필요(대사 중 팝업 띄울떼 고랴)
			State = Define.PlayerState.Field; //다이얼로그..일때.. 고려해함(수정 필요)
		}
	}
	#endregion	

	
}
