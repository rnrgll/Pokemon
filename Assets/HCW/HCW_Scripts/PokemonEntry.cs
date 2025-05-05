using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonEntry : MonoBehaviour
{
	// public Image iconImage;        // TODO 추후에 추가 예정?
	public TMP_Text nameText;
	public TMP_Text levelText;
	public Slider hpBar;
	public TMP_Text hpText;
	public Button selectButton;

	public void Setup(Pokémon p, System.Action<Pokémon> onSelect)
	{
		// iconImage.sprite = p.Sprite;  
		nameText.text = p.name;
		levelText.text = $"Lv {p.level}";
		hpBar.maxValue = p.maxHp;
		hpBar.value = p.hp;
		hpText.text = $"{p.hp}/{p.maxHp}";

		selectButton.onClick.RemoveAllListeners();
		selectButton.onClick.AddListener(() => onSelect(p));
	}
}
