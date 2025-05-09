using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class UI_PokemonParty : UI_Linked
{
	public enum PartySlotType
	{
		Menu,
		Item,
		Skill
	}
	
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
	

	
	//순서바꾸기 기능 변수
	private int changeFromIdx = -1;
	private int changeToIdx = -1;
	private bool isChangingOrder = false;

	private PartySlotType _slotType => Manager.Game.SlotType;
	
	//콜백용
	public Action<Pokémon, UI_PokemonSlot> onPokemonSelected;
	//스킬 가르치기 시 필요한 데이터 밖에서 주입
	private Item_SkillMachine skillMachineItem = null;

	public void SetSkillMachine(Item_SkillMachine item)
	{
		skillMachineItem = item;
	}

	
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
	
	
	public override void HandleInput(Define.UIInputType inputType)
	{
		if (!isChangingOrder)
		{
			switch (inputType)
			{
				case Define.UIInputType.Up:
					MoveCursor(-1);
					break;
				case Define.UIInputType.Down:
					MoveCursor(1);
					break;
				case Define.UIInputType.Select:
					OnSelect();
					break;
				case Define.UIInputType.Cancel:
					OnCancel();
					break;
			}
		}
		else
		{
			switch (inputType)
			{
				case Define.UIInputType.Up:
					MoveChangeTargetCursor(-1);
					break;
				case Define.UIInputType.Down:
					MoveChangeTargetCursor(1);
					break;
				case Define.UIInputType.Select:
					ConfirmOrderChange();
					break;
				case Define.UIInputType.Cancel:
					QuitChangeOrder();
					break;
			}
		}
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
	
	public void Refresh()
	{
		RefreshPartyData();
		RefreshUI();
		
	}
	


	//파티 데이터 갱신
	void RefreshPartyData() => party = Manager.Poke.party;

	//ui 갱신
	void RefreshUI()
	{
		// 포켓몬 슬롯에 데이터 세팅
		for (int i = 0; i < slotList.Count; i++)
		{
			if (i < party.Count)
			{
				slotList[i].gameObject.SetActive(true);
				slotList[i].SetData(party[i], skillMachineItem);
			}
			else
			{
				slotList[i].gameObject.SetActive(false);
			}
		}

		stopSlotInstance.gameObject.SetActive(true);
		stopSlotInstance.transform.SetAsLastSibling();

		UpdateCursor();
		switch (_slotType)
		{
			case (PartySlotType)0:
				msgText.text = "포켓몬을 골라 주십시오";
				break;
			case (PartySlotType)1:
				msgText.text = "어느 포켓몬에 사용하시겠습니까?";
				break;
			case (PartySlotType)2:
				msgText.text = "어느 포켓몬에게 가르치겠습니까?";
				break;
		}
		
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
		if (curCursorIdx == party.Count)
		{
			CloseSelf();
			return;
		}
		
		//화살표 바꾸기
		slotList[curCursorIdx].ChangeArrow(true);

	
		if (onPokemonSelected != null)
		{
			onPokemonSelected.Invoke(party[curCursorIdx], slotList[curCursorIdx]); // 등록된 동작 호출
			return;
		}
	
		var popupUI = Manager.UI.ShowPopupUI<UI_SelectPopUp>("UI_PokemonPopUp_1");
		popupUI.SetupOptions(popupUI.ButtonParent,
			new List<(string, ISelectableAction)>
			{
				("강한정도를 보다", new CustomAction(ShowPokemonInfo)),
				("순서바꾸기", new CustomAction(ChangeOrder)),
				("사용할 수 있는 기술",  new CustomAction(ShowSkillInfo) ),
				("돌아가다", new CustomAction(popupUI.OnCancel)),
			});
	}


	public override void OnCancel()
	{
		base.OnCancel();
		Debug.Log("UI_Pokemon: 닫힘 처리됨");
	}

	#region 포켓몬 정보 조회

	private void ShowPokemonInfo()
	{
		var linkedUI = Manager.UI.ShowLinkedUI<UI_PokemonInfo>("UI_PokemonInfo", false);

		linkedUI.SetPokemonInfo(party[curCursorIdx]);
		linkedUI.Open();
	}

	#endregion

	#region 포켓몬 스킬 정보 조회
	private void ShowSkillInfo()
	{
		var linkedUI = Manager.UI.ShowLinkedUI<UI_PokemonSkillInfo>("UI_PokemonSkillInfo", false);
		
		linkedUI.SetPartyIdx(curCursorIdx);
		Debug.Log(curCursorIdx);
		linkedUI.Open();
	}
	

	#endregion
	#region 순서바꾸기 기능

	public void ChangeOrder()
	{
		isChangingOrder = true;
		changeFromIdx = curCursorIdx;
		changeToIdx = curCursorIdx;

		// 시작 화살표를 빈 화살표로 설정
		// slotList[changeFromIdx].ChangeArrow(false);
		// slotList[changeFromIdx].Select();
		
		slotList[changeToIdx].ChangeArrow(true);
		slotList[changeToIdx].Select();
		Debug.Log(changeToIdx);
		msgText.text = "어디로 이동하겠습니까?";
	}
	
	private void MoveChangeTargetCursor(int direction)
	{
		int nextIdx = changeToIdx + direction;
		if (nextIdx < 0) nextIdx = party.Count - 1;
		else if (nextIdx >= party.Count) nextIdx = 0;

		if (changeToIdx == changeFromIdx)
		{
			slotList[changeToIdx].ChangeArrow(false); //이전 인덱스는 빈 화살표로
			slotList[changeToIdx].Select();
		}
		else
		{
			slotList[changeToIdx].ChangeArrow(true);
			slotList[changeToIdx].Deselect();
		}
		changeToIdx = nextIdx;
		Debug.Log(changeToIdx);
		slotList[changeToIdx].ChangeArrow(true);
		slotList[changeToIdx].Select();
	}
	private void ConfirmOrderChange()
	{
		if (changeFromIdx == changeToIdx)
		{
			QuitChangeOrder();
			return;
		}
		slotList[changeToIdx].ChangeArrow(false);

		// 스왑
		(party[changeFromIdx], party[changeToIdx]) = (party[changeToIdx], party[changeFromIdx]);

		
		slotList[changeToIdx].Deselect();
		slotList[changeFromIdx].Deselect();
		
		// 애니메이션 전용 코루틴 시작
		StartCoroutine(AnimateSlotSwap(changeFromIdx, changeToIdx));

		curCursorIdx = changeToIdx;
		changeToIdx = changeFromIdx;
		QuitChangeOrder();
	}

	private void QuitChangeOrder()
	{
		slotList[changeToIdx].ChangeArrow(true);
		slotList[changeToIdx].Deselect();
		slotList[curCursorIdx].ChangeArrow(true);
		
		UpdateCursor();
		isChangingOrder = false;
		msgText.text = "포켓몬을 골라 주십시오";
		
	}
	
	private IEnumerator AnimateSlotSwap(int fromIdx, int toIdx)
	{
		UI_PokemonSlot fromSlot = slotList[fromIdx];
		UI_PokemonSlot toSlot = slotList[toIdx];

		// 왼쪽으로 사라지는 연출
		yield return StartCoroutine(fromSlot.PlayExitLeftAnimation());
		yield return StartCoroutine(toSlot.PlayExitLeftAnimation());

		// 데이터 반영
		RefreshUI();

		// 새로 등장
		yield return StartCoroutine(slotList[fromIdx].PlayEnterLeftAnimation());
		yield return StartCoroutine(slotList[toIdx].PlayEnterLeftAnimation());

	
		msgText.text = "포켓몬을 골라 주십시오";
	}

	#endregion

}
