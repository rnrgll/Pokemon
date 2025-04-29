using System;
using System.Collections;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class Player : MonoBehaviour
{
	[SerializeField] public Define.PlayerState state;
	[SerializeField] public Vector2 currentDirection = Vector2.down; // 처음 방향은 아래
	public Coroutine moveCoroutine;
	public Coroutine zInput;

	[Tooltip("이동 거리 (기본 2)")]
	[SerializeField] int moveValue = 2;
	[Tooltip("이동 시간 (기본 0.3)")]
	[SerializeField] float moveDuration = 0.3f;
	[SerializeField] bool isMoving = false;
	bool isIdle = false;
	[SerializeField] public bool isSceneChange;

	Animator anim;

	void Awake()
	{
		Debug.Log("PlayerAwake");
		DontDestroyOnLoad(gameObject);
		anim = GetComponent<Animator>();
		zInput = StartCoroutine(ZInput());
		Debug.Log(zInput);
	}

	private void Start()
	{
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
		if (state == Define.PlayerState.SceneChange) // 씬이동중
			return;

		switch (state)
		{
			case Define.PlayerState.Field:          // 필드
				MoveState();
				break;
			case Define.PlayerState.Battle:			// 배틀중
				break;
			case Define.PlayerState.UI:				// UI활성화중
				break;
			case Define.PlayerState.Menu:			// Menu 활성화중
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
				state = Define.PlayerState.Menu; // 플레이어 상태를 UI로 전환
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
		RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + inputDir * 1.1f, inputDir, 1f);
		if (hit)
		{
			switch (hit.transform.gameObject.tag)
			{
				case "Wall":
				case "NPC":
					StopMoving();
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
				switch (state)
				{
					case PlayerState.Field:
						// 필드 상호작용
						Debug.DrawRay((Vector2)transform.position + currentDirection * 1.1f, currentDirection, Color.red);
						RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + currentDirection * 1.1f, currentDirection, 1f);
						if (hit)
						{
							var check = hit.transform.GetComponent<IInteractable>();
							check?.Interact();
						}
						break;
					case PlayerState.Battle:
						// 배틀 대화 넘기기
						break;
					case PlayerState.UI:
						// UI
						break;
					case PlayerState.Menu:
						Manager.UI.OnUISelect();
						break;
				}
			}
			yield return null;
		}
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Slope")
		{
			// TODO : 언덕 점프
			
		}
	}
	
	#region UI
	void OnAllUIClosed()
	{
		//만약 이전 상태가 필드가 아니라 배틀 상태였다면..?? -> 추후 로직에 따라 수정 필요
		if(state==Define.PlayerState.Menu)
			state = Define.PlayerState.Field;
	}
	#endregion	

}
