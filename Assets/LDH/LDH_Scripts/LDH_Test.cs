using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDH_Test : MonoBehaviour
{
	[SerializeField] private Pokémon _pokemon;
	[SerializeField] private int startHp;
	[SerializeField] private int maxHp;
	public InGameContext inGameContext;
	
	public bool isInBattle;

	public Define.StatusCondition condition;
	
	[Header("Test Items")]
	[SerializeField] private Item_Heal healItem;
	[SerializeField] private Item_KeyItem keyItem;
	
	
    // Start is called before the first frame update
    void Start()
    {
	    _pokemon.Init(1, 3);
	    Debug.Log(_pokemon.hp);
	    _pokemon.maxHp = maxHp;
	    _pokemon.hp = startHp;

	    inGameContext = new InGameContext { IsInBattle = isInBattle, NotifyMessage = msg => Debug.Log(msg) };

	    _pokemon.condition = condition;
	    
	    Debug.Log(_pokemon.condition);
	    
    }

    public void Heal()
    {
	    Debug.Log($"대상이 필요? :  {healItem.RequiresTarget()}");
	    
	    Debug.Log($"지금 사용가능? :  {healItem.CanUseNow(inGameContext)}");
	    
	    healItem.Use(_pokemon,inGameContext);
    }

    public void UseItem(ItemBase item)
    {
	    Debug.Log($"대상이 필요? :  {item.RequiresTarget()}");
	    
	    Debug.Log($"지금 사용가능? :  {item.CanUseNow(inGameContext)}");
	    item.Use(_pokemon,inGameContext);
	    
	    // item.Use(null,inGameContext);
    }

    public void AddBadge(int idx)
    {
	    Manager.Data.LdhPlayerData.GetBadge(idx);
    }
}
