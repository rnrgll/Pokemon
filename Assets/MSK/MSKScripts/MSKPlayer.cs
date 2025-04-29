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

	//public event Action OnEncountered;


    void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
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
                        moveCoroutine = StartCoroutine(Move(inputDir));
                }
				// 방향만 바꾸고 대기
				else
				{
					currentDirection = inputDir;
				}
			}
		}
    }
    void Interact()
    {
        var facingDit = new Vector3(anim.GetFloat("x"), anim.GetFloat("y"));
        var interactPos = transform.position + facingDit;
		Debug.Log(interactPos);
		
		var collider = Physics2D.OverlapCircle(interactPos, 0.3f, LayerMask.GetMask("ObjectLayer") | LayerMask.GetMask("InteractableLayer"));
		if (collider != null)
		{
			collider.GetComponent<IInteractable>()?.Interact();
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

}
