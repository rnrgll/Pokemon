using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PokemonSlot : MonoBehaviour
{
	//인스펙터에서 할당
	[SerializeField] private Image arrow;
	[SerializeField] private Image icon;
	[SerializeField] private TMP_Text pokemonName;
	[SerializeField] private TMP_Text level;
	[SerializeField] private TMP_Text curHp;
	[SerializeField] private TMP_Text maxHp;
	[SerializeField] private Slider hpSlider;
	
	public void Deselect()
	{
		Util.SetVisible(arrow, false);
	}

	public void Select()
	{
		Util.SetVisible(arrow, true);
	}

	private void OnDisable()
	{
		Deselect();
	}

	public void SetData(Pokémon pokemon)
	{
		//아이콘..
		pokemonName.text = pokemon.pokeName;
		level.text = $":L{pokemon.level}";
		curHp.text = $"{pokemon.hp}/";
		maxHp.text = pokemon.maxHp.ToString();
		

	}

	private void SetHpSlider(int hp, int maxHp)
	{
		hpSlider.maxValue = maxHp;
		hpSlider.value = hp;
		
		float ratio = hp / (float)maxHp;

		Color color;
		if (ratio > 0.5f)
			ColorUtility.TryParseHtmlString(Define.ColorCode["hp_green"], out color); // 초록
		else if (ratio > 0.2f)
			ColorUtility.TryParseHtmlString(Define.ColorCode["hp_yellow"], out color); // 노랑
		else
			ColorUtility.TryParseHtmlString(Define.ColorCode["hp_red"], out color); // 빨강

		hpSlider.fillRect.GetComponent<Image>().color = color;
	}

	
}
