using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PokemonSlot : MonoBehaviour
{
	public Pokémon Pokemon { get; private set; }
	
	//인스펙터에서 할당
	[SerializeField] private Image arrow;
	[SerializeField] private Image icon;
	[SerializeField] private TMP_Text pokemonName;
	[SerializeField] private TMP_Text level;
	[SerializeField] private TMP_Text condition;
	[SerializeField] private TMP_Text curHp;
	[SerializeField] private TMP_Text maxHp;
	[SerializeField] private UI_HpBarController hpSlider;
	[SerializeField] private Sprite emptyArrow;
	[SerializeField] private Sprite originalArrow;
	[SerializeField] private TMP_Text canLearn;
	[SerializeField] private Transform hpUIRoot;
	[SerializeField] private Image pokemonImg;
	
	
	
	//애니메이션 용
	private CanvasGroup canvasGroup;
	private RectTransform rectTransform;

	private Vector2 originPos;

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		rectTransform = GetComponent<RectTransform>();
	}


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
		ChangeArrow(true);
	}

	public void SetData(Pokémon pokemon, Item_SkillMachine skillMachine = null)
	{
		this.Pokemon = pokemon;
		
		//todo: 애니메이션 가져오는거로 수정하기
		//pokemonImg.sprite = Manager.Data.SJH_PokemonData.GetBattleFrontSprite(pokemon.pokeName);
		
		pokemonName.text = pokemon.pokeName;
		level.text = $":L{pokemon.level}";
		if (pokemon.condition == Define.StatusCondition.Normal)
		{
			condition.gameObject.SetActive(false);
		}
		else
		{
			//condition.text = pokemon.condition.ToString();
			condition.text = Define.GetKoreanState[pokemon.condition];
			condition.gameObject.SetActive(true);
		}

		if (Manager.Game.SlotType == UI_PokemonParty.PartySlotType.Skill && skillMachine != null)
		{
			hpUIRoot.gameObject.SetActive(false);
			canLearn.text = skillMachine.CanLearn(pokemon) ? "배울 수 있다" : "배울 수 없다";
			canLearn.gameObject.SetActive(true);
		}
		else
		{
			canLearn.gameObject.SetActive(false);
			hpUIRoot.gameObject.SetActive(true);
			hpSlider.SetHp(pokemon.hp, pokemon.maxHp);
		}
		
		

	}
	
	
	public void ChangeArrow(bool toFullArrow)
	{
		arrow.sprite = toFullArrow ? originalArrow : emptyArrow;
	}
	
	
		
	//애니메이션
	
	public IEnumerator PlayExitLeftAnimation()
	{
		originPos = rectTransform.anchoredPosition;
		float duration = 0.25f;
		Vector3 targetPos = rectTransform.anchoredPosition + new Vector2(50f, 0); // 오른쪽으로 살짝 이동

		Sequence seq = DOTween.Sequence();
		seq.Join(rectTransform.DOAnchorPos(targetPos, duration))
			.Join(canvasGroup.DOFade(0f, duration));
        
		yield return seq.WaitForCompletion();
	}

	public IEnumerator PlayEnterLeftAnimation()
	{
		float duration = 0.25f;
		Vector2 startPos = originPos + new Vector2(-50f, 0); // 왼쪽 밖에서 시작
		rectTransform.anchoredPosition = startPos;
		canvasGroup.alpha = 0f;

		Sequence seq = DOTween.Sequence();
		seq.Join(rectTransform.DOAnchorPos(startPos + new Vector2(50f, 0), duration))
			.Join(canvasGroup.DOFade(1f, duration));
        
		yield return seq.WaitForCompletion();
	}

	public void StartSliderAnimation(int fromHp, int toHp, int maxHp)
	{
		hpSlider.AnimationHpChange(fromHp,toHp, maxHp);
	}

}
