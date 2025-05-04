using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BagUseFlow
{
	private readonly UI_Bag _bag;
	private InventorySlot _slot;
	private ItemBase _item;
	private InGameContext _context;

	public BagUseFlow(UI_Bag bag)
	{
		_bag = bag;
	}

	public void Start(InventorySlot slot)
	{
		_slot = slot;
		_item = Manager.Data.ItemDatabase.GetItemData(slot.ItemName);
		StartItemUseFlow();
	}
	
	private void StartItemUseFlow()
	{
		_context = InGameContextFactory.CreateFromGameManager();

		// 1. 플레이어 대상: context.Target == null
		if (_item.TargetType == Define.ItemTarget.Player)
		{
			UseOnPlayer();
		}
		// 2. 기술머신/비전머신
		else if (_item.Category == Define.ItemCategory.TM_HM)
		{
			ShowSkillMachineFlow();
		}
		// 3. 몬스터볼
		else if (_item.Category==Define.ItemCategory.Ball)
		{
			//야생 포켓몬인지 체크해서 넘기기
			UseMonsterBall();
		}
		//중요한 물건
		else if (_item.Category == Define.ItemCategory.KeyItem)
		{
			UseKeyItem();		
		}
		// 4. 그 외 포켓몬 대상 아이템
		else
		{
			ShowPokemonSelectFlow();
		}
	}
	
	
	private void UseOnPlayer()
	{
		_bag.SetDescription($"{_item.ItemName}를(을) 사용했다!");
		_context.SetMessage(ShowMultiLineNotifyMsg);
		bool success = _item.Use(null, _context);
		UseResult(success);
	}

	private void UseMonsterBall()
	{
		//todo: 몬스터볼 배틀에서 수정하기
		//타갯 포켓몬
		bool success = _item.Use(Manager.Game.EnemyPokemon, _context);
		UseResult(success);
		Debug.Log("몬스터볼 사용 함");
		//ShowResultMessage(success);
	}

	private void UseKeyItem()
	{
		_context.SetMessage(ShowMultiLineNotifyMsg);
		bool success = _item.Use(null, _context);
		UseResult(success);
	}
	
	private void ShowSkillMachineFlow()
	{
		// TODO: 기술 설명 출력 → 예/아니오 팝업 → 포켓몬 선택 UI → 사용 처리
		
		//1.기술 설명 출력
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp").ShowMessage(ItemMessage.Get(ItemMessageKey.TMHMBeforeUse,_item.ItemName),
			() =>
			{
				//2. 가르치겠냐는 메시지
				_bag.SetDescription($"{_item.ItemName}를(을)\n포켓몬에게 가르치겠습니까?");
				//2. 예/아니오 팝업 -> //3.포켓몬 선택 및 사용처리 콜백 연결
				_bag.PopupManager.ShowConfirmPopup(ShowPokemonSelectFlow, 
					()=>
					{
						_bag.Refresh();
					});
			},true,true);
	}
	
	

	private void ShowPokemonSelectFlow()
	{
		
		//context 설정
		_context.SetMessage((msg) =>
		{
			Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
				.ShowMessage(msg,
					() =>
					{
						Manager.UI.UndoLinkedUI();
						_bag.Refresh();
					});
		});

		
		OpenPokePartyUI();

	}

	private void OpenPokePartyUI()
	{
		var partyUI = Manager.UI.ShowLinkedUI<UI_PokemonParty>("UI_PokemonParty", false);
		partyUI.onPokemonSelected
			= (poke, slot) =>
			{
				_context.PokemonSlot = slot;
				bool success = _item.Use(poke, _context);
				partyUI.Refresh();
				UseResult(success);
			};
		partyUI.gameObject.SetActive(true);
	}
	
	
	private void UseResult(bool success)
	{
		_bag.Refresh();
		if (!success) return;

		// 소모 가능한 아이템인지 검사
		if (_item.IsConsumable)
		{
			Manager.Data.PlayerData.Inventory.RemoveItem(_slot, 1);
		}
	}

	private void ShowMultiLineNotifyMsg(string msg)
	{
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
			.ShowMessage(msg);
		
	}
	
	
	
}

