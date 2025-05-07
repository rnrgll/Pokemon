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

	private UI_PokemonParty.PartySlotType _slotType;

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
			_slotType = UI_PokemonParty.PartySlotType.Skill;
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
			_slotType = UI_PokemonParty.PartySlotType.Item;
			ShowPokemon_ItemFlow();
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

		// 확률 X = ((3 × MaxHP - 2 × HP) × Rate × Ball) / (3 × MaxHP)
		var enemyPokemon = Manager.Game.EnemyPokemon;
		int maxHp = enemyPokemon.maxHp;
		int curHp = enemyPokemon.hp;
		int rate = ((3 * maxHp - 2 * curHp) * 255 * 1) / (3 * maxHp);

		// 확정성공
		if (rate >= 255)
		{
			Debug.Log($"{enemyPokemon.pokeName} 포획 성공! {rate}");
		}
		else
		{
			// 0~254 중 rate 이상이 나올 확률
			int rand = UnityEngine.Random.Range(0, 256); // 0~255
			if (rand < rate)
			{
				Debug.Log($"{enemyPokemon.pokeName} 포획 성공! ({rand} < {rate})");
			}
			else
			{
				Debug.Log($"{enemyPokemon.pokeName} 포획 실패 ({rand} >= {rate})");
			}
		}

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
				Manager.UI.ShowConfirmPopup(ShowPokemon_TMFlow, 
					()=>
					{
						_bag.Refresh();
					});
			},true,true);
	}
	
	

	private void ShowPokemon_ItemFlow()
	{
		
		//context 설정
		_context.SetMessage((msg) =>
		{
			Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
				.ShowMessage(msg,
					() =>
					{
						Manager.UI.UndoLinkedUI(); //포켓몬 파티 창 닫기
						_bag.Refresh();
						if(Manager.Game.IsInBattle && Manager.Game.IsItemUsed) //배틀중에 아이템 사용한 경우 가방 창까지 닫아야함
							Manager.UI.UndoLinkedUI(); //가방 창 닫기
					},true,true);
		});

		
		OpenPokePartByType(_slotType);

	}
	private void ShowPokemon_TMFlow()
	{
		//context 설정
		_context.Callback = () =>
		{
			Manager.UI.UndoLinkedUI();
			//기술머신만 여기서 result로 아이템 개수 갱신 처리
			UseResult(_context.Result);
		};
		
		OpenPokePartByType(_slotType);

	}

	private void OpenPokePartByType(UI_PokemonParty.PartySlotType slotType)
	{
		Manager.Game.SetSlotType(slotType);
		var partyUI = Manager.UI.ShowLinkedUI<UI_PokemonParty>("UI_PokemonParty", false);
		if(slotType==UI_PokemonParty.PartySlotType.Skill)
			partyUI.SetSkillMachine((Item_SkillMachine)_item);
		else
			partyUI.SetSkillMachine(null);
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
		if (success)
		{
			// 소모 가능한 아이템인지 검사
			if (_item.IsConsumable)
			{
				Manager.Data.PlayerData.Inventory.RemoveItem(_slot, 1);
			}
			
			//배틀 중이라면 아이템 사용했음을 저장해두가
			if (Manager.Game.IsInBattle)
			{
				Manager.Game.SetIsItemUsed(success); //아이템 사용함!
				return;
			}
		}

		_bag.Refresh();
	}

	private void ShowMultiLineNotifyMsg(string msg)
	{
		Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp")
			.ShowMessage(msg);
		
	}
	
	
	
}

