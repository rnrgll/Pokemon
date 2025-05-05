using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokemonSelect : MonoBehaviour
{
	public GameObject selectPanel;
	public Transform content;
	public PokemonEntry entryPrefab;

	private System.Action<Pokémon> onChoose;
	private System.Action onCancel;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
		{
			selectPanel.SetActive(false);
			onCancel?.Invoke();
		}
	}

	private void OnEntrySelected(Pokémon p)
	{
		selectPanel.SetActive(false);
		onChoose?.Invoke(p);
	}

	public void Show(List<Pokémon> party, System.Action<Pokémon> onChooseCallback, System.Action onCancelCallback)
	{
		onChoose = onChooseCallback;
		onCancel = onCancelCallback;

		// 기존항목 제거
		foreach (Transform t in content) Destroy(t.gameObject);

		// 파티 수 만큼 슬롯 생성
		foreach (var p in party)
		{
			var entry = Instantiate(entryPrefab, content);
			entry.Setup(p, OnEntrySelected);
		}
		if (content.childCount > 0)
			UnityEngine.EventSystems.EventSystem
				.current.SetSelectedGameObject(content.GetChild(0).gameObject);

		selectPanel.SetActive(true);
	}

}

