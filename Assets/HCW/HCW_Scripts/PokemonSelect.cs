using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokemonSelect : MonoBehaviour
{
	public GameObject selectPanel;
	public Transform content;
	public PokemonEntry entryPrefab;
	public UI_PokemonSlot pokemonSloPrefb;
	public UI_PokemonSlot stopSlotPrefab;
	
	private System.Action<Pokémon> onChoose;
	private System.Action onCancel;
	
	//커서 인덱스 관리
	private int curIdx = 0;
	//포켓몬 슬롯 리스트
	private List<UI_PokemonSlot> partyList = new();
	
	
	//현재 전투 중인 포켓몬
	private Pokémon currentPokemon;
	private bool haveToChoose;
	private bool isUIActive = false; // UI 활성화 상태 체크
	
	private void OnEnable()
	{
		//커서 초기화
		curIdx = 0;
		partyList[curIdx].Select();
		
		Manager.UI.OnAllUIClosed += OnUIClosed; // UI가 모두 닫힐 때 이벤트 연결
	}
	

	private void OnDisable()
	{
		Manager.UI.OnAllUIClosed -= OnUIClosed;
	}
	
	private void OnUIClosed()
	{
		isUIActive = false; // UI가 닫힐 때 입력 비활성화
	}

	private void Update()
	{
		Debug.Log(Manager.UI.IsAnyUIOpen);
		if (isUIActive) return; // UI 활성화 중면 입력 차단
		
		
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			MoveCursor(-1);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			MoveCursor(1);
		}
		else if (Input.GetKeyDown(KeyCode.Z))
		{
			SelectSlot();
		}		
		
		else if (Input.GetKeyDown(KeyCode.X))
		{
			//취소
			if (haveToChoose)
			{
				return;
			}
			SelectQuit();
		}
	}

	private void OnEntrySelected(Pokémon p)
	{
		selectPanel.SetActive(false);
		onChoose?.Invoke(p);
	}


	#region 슬롯 선택 / 닫기

	private void SelectSlot()
	{
		//Debug.Log(Manager.UI.IsAnyUIOpen);
		if (curIdx == partyList.Count-1) //그만두다를 선택한 경우
		{
			SelectQuit();
			return;
		}
		
		if (partyList[curIdx].Pokemon.isDead || partyList[curIdx].Pokemon.hp <= 0)
		{
			return;
		}
		if (partyList[curIdx].Pokemon == currentPokemon)
		{
			return;
		}
	
		
		OnEntrySelected(partyList[curIdx].Pokemon);
	}

	private void SelectQuit()
	{
		Debug.Log("그만두다 선택");
		selectPanel.SetActive(false);
		if(onCancel==null) Debug.Log("onCancle is null");
		onCancel?.Invoke();
	}


	#endregion

	
	
	//커서 입력 컨트롤
	private void MoveCursor(int direction)
	{
		int nextIdx = curIdx + direction;
		if (nextIdx < 0) nextIdx = partyList.Count-1; //그만두다 슬롯까지 파티리스트에 넣었으므로 -1해줘야함
		else if (nextIdx >= partyList.Count) nextIdx = 0;
		
		//이전 커서 선택 해제, 다음 커서 선택으로 업데이트
		partyList[curIdx].Deselect();
		curIdx = nextIdx;
		partyList[curIdx].Select();

	}
	
	
	//포켓몬 선택 창 open & 파티에 있는 포켓몬으로 슬롯 동적 생성
	public void Show(bool haveToChoose, List<Pokémon> party, Pokémon playerPokemon, System.Action<Pokémon> onChooseCallback, System.Action onCancelCallback)
	{
		isUIActive = false;
		
		//현재 포켓몬
		currentPokemon = playerPokemon;
		this.haveToChoose = haveToChoose;
		
		//슬롯 타입 설정하기 (PartySlotType.Skill 만 아니면 됨)
		Manager.Game.SetSlotType(UI_PokemonParty.PartySlotType.Menu);
		
		onChoose = onChooseCallback;
		onCancel = onCancelCallback;

		
		// 기존항목 제거 및 리스트 초기화
		foreach (Transform t in content) Destroy(t.gameObject);
		partyList.Clear();

		// 파티 수 만큼 슬롯 생성
		foreach (var p in party)
		{
			//var entry = Instantiate(entryPrefab, content);
			var entry = Instantiate(pokemonSloPrefb, content); //프리팹 교체
			entry.transform.SetAsLastSibling();
			entry.SetData(p, null);
			
			//리스트에 추가
			partyList.Add(entry);
			
			// entry.Setup(p, OnEntrySelected);
		}
		
		//마지막에 '그만두다' 슬롯 추가하기 -> 강제 선택인 경우 생성하고 리스트에 넣지말기
		if (!haveToChoose)
		{
			var stopSlot = Instantiate(stopSlotPrefab, content);
			stopSlot.transform.SetAsLastSibling();
			//그만두다 슬롯도 리스트에 포함
			partyList.Add(stopSlot);
		}
	
		
		
		
		// if (content.childCount > 0)
		// 	UnityEngine.EventSystems.EventSystem
		// 		.current.SetSelectedGameObject(content.GetChild(0).gameObject);
		selectPanel.SetActive(true);
	}
	
}

