using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
	[SerializeField] GameObject playerPrefab;
	static bool isCreate;

	void Start()
	{
		if (isCreate)
			return;
		Instantiate(playerPrefab);
		isCreate = true;
		this.enabled = false;
	}
}
