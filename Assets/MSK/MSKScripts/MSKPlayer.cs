using System;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSKPlayer : MonoBehaviour
{
	[SerializeField] Vector2 currentDirection = Vector2.down; // 처음 방향은 아래
	[SerializeField] Vector3 currentPos;
	[SerializeField] Scene currenScene;
	Coroutine moveCoroutine;
	WaitForSeconds moveDelay;

	[Tooltip("이동 거리 (기본 2)")]
	[SerializeField] int moveValue = 2;
	[Tooltip("이동 시간 (기본 0.3)")]
	[SerializeField] float moveDuration = 0.3f;
	bool isMoving = false;
	bool isIdle = false;

	Animator anim;

	//	이동확인용 레이어
	public LayerMask interactableLayer;
	public LayerMask objectsLayer;
    void PCRaycast()
    {
		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 2f))
		{
			Debug.Log(hitInfo.collider.gameObject.name);
		}
		else
        {
            Debug.Log("감지 없음");
        }

    }
    void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		PCRaycast();
		// 방향키 떼면 Idle 설정하고 이동이 끝나면 isIdle에 따라 바꾸기
		if (Input.GetKeyUp(KeyCode.UpArrow) ||
			Input.GetKeyUp(KeyCode.DownArrow) ||
			Input.GetKeyUp(KeyCode.LeftArrow) ||
			Input.GetKeyUp(KeyCode.RightArrow))
		{
			isIdle = true;
			moveCoroutine = null;
		}

		if (!isMoving)
		{
			Vector2 inputDir = Vector2.zero;

			if (Input.GetKey(KeyCode.UpArrow)) inputDir = Vector2.up;
			else if (Input.GetKey(KeyCode.DownArrow)) inputDir = Vector2.down;
			else if (Input.GetKey(KeyCode.LeftArrow)) inputDir = Vector2.left;
			else if (Input.GetKey(KeyCode.RightArrow)) inputDir = Vector2.right;

			if (inputDir != Vector2.zero)
			{
				// 방향 변경
				anim.SetFloat("x", inputDir.x);
				anim.SetFloat("y", inputDir.y);

				// 방향이 같으면 이동 시작
				if (inputDir == currentDirection)
				{
					if (IsWalkable(inputDir))
					{
                        moveCoroutine = StartCoroutine(Move(inputDir));
                    
					}
                }
				// 방향만 바꾸고 대기
				else
				{
					currentDirection = inputDir;
				}
			}
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
		while (time < moveDuration)
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


	//	플레이어 진행 방향에 레이어를 확인
	//	인터랙트, 오브젝트 레이어가 존재할 시 이동 불가능
	private bool IsWalkable(Vector3 targetPos) 
	{
		if (Physics2D.OverlapCircle(targetPos, 1f, objectsLayer | interactableLayer) != null) 
		{ 
			return false;
		}
		return true;
	}
}
