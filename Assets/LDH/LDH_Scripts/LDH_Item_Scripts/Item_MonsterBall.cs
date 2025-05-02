using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/MonsterBall")]
public class Item_MonsterBall : ItemBase
{
	[SerializeField] private float catchRate;
	public float CatchRate => catchRate;
	public override bool Use(Pokémon target, InGameContext inGameContext)
	{
		if (!inGameContext.IsWildBattle)
		{
			inGameContext.NotifyMessage?.Invoke(ItemMessage.Get(ItemMessageKey.CanNotUseBall));
			
			//트레이너가 볼을 쳐내는 애니메이션 적용
			//아이템 개수 줄어듬 -> 사용은 됨!
			return true;
		}

		// TODO: 포획 시도 로직은 이후 BattleManager와 연동
		inGameContext.NotifyMessage?.Invoke($"{itemName}을(를) 던졌다!");
		return true;
	}
}