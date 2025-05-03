using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class UI_PokemonParty : UI_Linked
{
	
	//인덱스
	private static int curCursorIdx = 0;
	private int preCursorIdx = 0;
	
	//포켓몬 파티
	private List<Pokémon> party;
	private List<UI_PokemonSlot> slotList = new();

	//인스펙터 창에서 직접 할당
	[Header("UI 오브젝트 참조")] 
	[SerializeField]
	private Transform pokemonSlotRoot;
	[SerializeField] private TMP_Text msgText;
	[SerializeField] private UI_PokemonSlot uiPokemonSlotPrefab;
	[SerializeField] private UI_PokemonSlot stopSlotPrefab;
	private UI_PokemonSlot stopSlotInstance; // "그만두다" 슬롯 인스턴스
	

	private void Awake()
	{
		if (pokemonSlotRoot.childCount!=0)
		{
			foreach (Transform child in pokemonSlotRoot)
			{
				Destroy(child.gameObject);
			}
		}
		
		uiPokemonSlotPrefab = Resources.Load<UI_PokemonSlot>("UI_Prefabs/Component/UI_PokemonSlot");
		
		stopSlotPrefab = Resources.Load<UI_PokemonSlot>("UI_Prefabs/Component/UI_PokemonSlot_Quit");
		InitSlots();
		
	}

	private void OnEnable()
	{
		Refresh(); // 진입 시 전체 UI 초기화
	}

	protected override void Init()
	{
		base.Init();

	}
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow)) MoveCursor(-1);
		else if (Input.GetKeyDown(KeyCode.DownArrow)) MoveCursor(1);
	}
	
	
	private void InitSlots()
	{
		slotList.Clear();

		for (int i = 0; i < 6; i++)
		{
			UI_PokemonSlot slot = Instantiate(uiPokemonSlotPrefab, pokemonSlotRoot);
			slot.gameObject.SetActive(false);
			slotList.Add(slot);
		}

		// "그만두다" 슬롯 따로 생성
		if (stopSlotInstance == null)
		{
			stopSlotInstance = Instantiate(stopSlotPrefab, pokemonSlotRoot);
		}
		stopSlotInstance.transform.SetAsLastSibling();
	}
	
	private void Refresh()
	{
		RefreshPartyData();
		RefreshUI();
		
	}
	


	//파티 데이터 갱신
	void RefreshPartyData()
	{
		party = Manager.Poke.party;
	}

	//ui 갱신
	void RefreshUI()
	{
		// 포켓몬 슬롯에 데이터 세팅
		for (int i = 0; i < slotList.Count; i++)
		{
			if (i < party.Count)
			{
				slotList[i].gameObject.SetActive(true);
				slotList[i].SetData(party[i]);
			}
			else
			{
				slotList[i].gameObject.SetActive(false);
			}
		}

		stopSlotInstance.gameObject.SetActive(true);
		stopSlotInstance.transform.SetAsLastSibling();

		UpdateCursor();
		msgText.text = "포켓몬을 골라 주십시오";
	}

	
	//커서 이동
	private void MoveCursor(int direction)
	{
		if (curCursorIdx + direction < 0 || curCursorIdx + direction > party.Count)
		{
			return;
		}
		preCursorIdx = curCursorIdx;
		curCursorIdx += direction;
		//int max = party.Count;
		//int next = Mathf.Clamp(curCursorIdx + direction, 0, max);
		
		//curCursorIdx = next;
		
		UpdateCursor();
		

	}
	
	//커서 업데이트
	void UpdateCursor()
	{
		//Debug.Log($"커서 업데이트 : {preCursorIdx} => {curCursorIdx}");
		if (preCursorIdx < party.Count)
			slotList[preCursorIdx].Deselect();
		else
			stopSlotInstance.Deselect();

		if (curCursorIdx < party.Count)
			slotList[curCursorIdx].Select();
		else
			stopSlotInstance.Select();
	
	}

	public override void OnSelect()
	{
		if (curCursorIdx == slotList.Count - 1)
		{
			//그만두다 메뉴
			CloseSelf();
			return;
		}
		
		//화살표 바꾸기
		slotList[curCursorIdx].ChangeArrow(true);
		//메시지창 비우기
		msgText.text = "";
		//Manager.UI.ShowPopupUI<>()
	}


	public override void OnCancle()
	{
		base.OnCancle();
		Debug.Log("UI_Pokemon: 닫힘 처리됨");
	}

}
