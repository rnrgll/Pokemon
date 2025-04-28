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
		player.GetComponent<Player>().state = Define.PlayerState.Field;
		isCreate = true;
		this.enabled = false;
	}
}
