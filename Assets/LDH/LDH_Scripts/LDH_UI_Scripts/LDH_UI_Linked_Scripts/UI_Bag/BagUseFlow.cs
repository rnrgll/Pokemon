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
		else if (_bag.CurrentCategory == Define.ItemCategory.TM_HM)
		{
			ShowSkillMachineFlow();
		}
		// 3. 몬스터볼
		else if (_item is Item_MonsterBall)
		{
			//야생 포켓몬인지 체크해서 넘기기
			UseMonsterBall();
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
		_context.SetMessage(msg => 
			Manager.UI.ShowPopupUI<UI_MultiLinePopUp>("UI_MultiLinePopUp").ShowMessage(msg, _bag.Refresh));
		bool success = _item.Use(null, _context);
		if (success)
		{
			Manager.Data.PlayerData.Inventory.RemoveItem(_slot, 1);
		}

		// ShowResultMessage(success);
	}

	private void UseMonsterBall()
	{
		//타갯 포켓몬
		bool success = _item.Use(Manager.Game.EnemyPokemon, _context);
		if (success)
			Manager.Data.PlayerData.Inventory.RemoveItem(_slot, 1);
		Debug.Log("몬스터볼 사용 함");
		//ShowResultMessage(success);
	}

	
	private void ShowSkillMachineFlow()
	{
		// TODO: 기술 설명 출력 → 예/아니오 팝업 → 포켓몬 선택 UI → 사용 처리
	}
	
	private void ShowPokemonSelectFlow()
	{
		// TODO: 포켓몬 선택 UI → 포켓몬 넘겨서 사용 처리 → 성공 여부 메시지
	}
	private void ShowResultMessage(bool success)
	{
		
		//여기는 상황에 따라 다르기 때문에 내가 다르게 구현해야함
		
		// if (success)
		// 	_bag.PopupManager.ShowMultiLineMessage(new() { "아이템을 사용했습니다." }, _bag.Refresh);
		// else
		// 	_bag.PopupManager.ShowMultiLineMessage(new() { "사용할 수 없습니다." }, _bag.Refresh);
	}
	


}

