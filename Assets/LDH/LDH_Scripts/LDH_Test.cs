using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDH_Test : MonoBehaviour
{
	private Pokemon _pokemon;
	[SerializeField] private int startHp;
	public InGameContext inGameContext;
	
	public bool isInBattle;
	
	
	[Header("Test Items")]
	[SerializeField] private Item_Heal healItem;
	[SerializeField] private Item_KeyItem keyItem;
	
	
    // Start is called before the first frame update
    void Start()
    {
	    _pokemon = new Pokemon("테스트포켓몬", 20, 10, null);
	    _pokemon.HP = startHp;


	    inGameContext = new InGameContext { IsInBattle = isInBattle, NotifyMessage = msg => Debug.Log(msg) };
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
	    item.Use(null,inGameContext);
    }

    public void AddBadge(int idx)
    {
	    Manager.Data.LdhPlayerData.GetBadge(idx);
    }
}
