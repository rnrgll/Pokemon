using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

public class NewSceneChanger : MonoBehaviour
{
	[Tooltip("도착할 씬 이름")]
	[SerializeField] string exitSceneName;
	[Tooltip("이동할 방향 조건")]
	[SerializeField] Vector2 exitDirection;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Player player = collision.GetComponent<Player>();

			// 도착 씬 로드
			if (!SceneManager.GetSceneByName(exitSceneName).isLoaded)
			{
				SceneManager.LoadScene(exitSceneName, LoadSceneMode.Additive);
				// 이동할 씬 저장
				player.CurSceneName = SceneManager.GetActiveScene().name;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Player player = collision.GetComponent<Player>();

			// 콜라이더를 나갈 때 방향이 같으면 이전 씬 언로드
			if (player != null && player.currentDirection == exitDirection && SceneManager.GetSceneByName(exitSceneName).isLoaded)
			{
				if (player.CurSceneName != exitSceneName)
				{
					SceneManager.UnloadSceneAsync(player.CurSceneName);
				}
				player.CurSceneName = SceneManager.GetActiveScene().name;
			}
			else if (player != null && player.currentDirection != exitDirection)
			{
				if (player.CurSceneName != exitSceneName && SceneManager.GetSceneByName(exitSceneName).isLoaded)
				{
					SceneManager.UnloadSceneAsync(exitSceneName);
				}
			}
		}
	}
}
