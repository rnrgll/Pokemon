using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	[Tooltip("도착할 씬 이름")]
	[SerializeField] string exitSceneName;
	[Tooltip("도착할 씬 위치")]
	[SerializeField] Vector2 exitPos;
	[Tooltip("씬체인저 타입")]
	[SerializeField] Define.PortalType portalType;
	[SerializeField] bool isPlayerIn;
	[Tooltip("씬 변경후 방향")]
	[SerializeField] Vector2 keyDirection;
	Coroutine sceneCoroutine;
	[SerializeField] bool isChange;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (portalType == Define.PortalType.Stair && !isChange)
			{
				// 플레이어 이동
				//SceneChange(gameObject.name, collision.transform.gameObject);
				sceneCoroutine = StartCoroutine(Change(collision.gameObject));
			}
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (portalType == Define.PortalType.Foothold)
		{
			isPlayerIn = true;
		}
		if (collision.CompareTag("Player"))
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if ((portalType == Define.PortalType.Foothold) && (isPlayerIn) && (!isChange) && (transform.localPosition == player.transform.position))
			{

				// 방향키 입력 직접 체크
				Vector2 inputDir = Vector2.zero;
				if (Input.GetKey(KeyCode.UpArrow)) inputDir = Vector2.up;
				else if (Input.GetKey(KeyCode.DownArrow)) inputDir = Vector2.down;
				else if (Input.GetKey(KeyCode.LeftArrow)) inputDir = Vector2.left;
				else if (Input.GetKey(KeyCode.RightArrow)) inputDir = Vector2.right;

				if (inputDir == keyDirection)
				{
					// 플레이어 이동
					//SceneChange(gameObject.name, player.gameObject);
					sceneCoroutine = StartCoroutine(Change(player.gameObject));
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		isPlayerIn = false;
	}

	IEnumerator Change(GameObject player)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(exitSceneName);
		asyncLoad.allowSceneActivation = false;
		// 잔류이동 지우기
		Player pc = player.GetComponent<Player>();
		isChange = true;
		pc.state = Define.PlayerState.SceneChange;
		player.transform.position = new Vector3(exitPos.x, exitPos.y);
		pc.StopMoving();
		pc.currentDirection = keyDirection;
		pc.AnimChange();

		pc.CurSceneName = exitSceneName;

		while (!asyncLoad.isDone)
		{
			if (asyncLoad.progress >= 0.9f)
			{
				//Debug.Log(gameObject.name);
				player.transform.position = exitPos;
				yield return new WaitForSeconds(0.1f);
				asyncLoad.allowSceneActivation = true;

				// 상태 초기화
				isChange = false;
				pc.state = Define.PlayerState.Field;
				sceneCoroutine = null;
				//Debug.Log("state init");

				break;  // 루프 탈출
			}
			yield return null;
		}
	}


	public void Change(string nextSceneName, Vector2 nextPos)
	{
		exitSceneName = nextSceneName;
		exitPos = nextPos;
		isChange = false;
		
		Player player = FindObjectOfType<Player>();

		StartCoroutine(Change(player.gameObject));
	}
}
