using DG.Tweening;
using System;
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
	[SerializeField] private Sprite emptyArrow;
	[SerializeField] private Sprite originalArrow;
	
	
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
	
}
