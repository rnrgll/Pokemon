using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
	[SerializeField] GameObject playerPrefab;
	static bool isCreate;

	void Start()
	{
		if (isCreate)
			return;

		var player = Instantiate(playerPrefab);
		player.GetComponent<Player>().State = Define.PlayerState.Field;
		// CreatePlayer 오브젝트 위치에 플레이어 생성
		player.transform.position = gameObject.transform.position;
		isCreate = true;
		this.enabled = false;
		
		//테스트 코드
		
	}
}
