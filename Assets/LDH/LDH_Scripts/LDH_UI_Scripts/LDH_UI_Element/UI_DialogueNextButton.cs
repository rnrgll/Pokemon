using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_DialogueNextButton : MonoBehaviour
{
	private Image _image;
	private SpriteRenderer _spriteRenderer;
	private Coroutine blinkCoroutine;
	private WaitForSeconds blinkTime;

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

	private void OnEnable()
	{
		StartBlink();
	}

	private void OnDisable()
	{
		_image.DOKill(); // DOTween 애니메이션 정리
		_image.color = new Color(1,1,1,1); // 완전 불투명 복구
	}

	private void StartBlink()
	{
		// 알파값을 0 → 1 → 0으로 반복 (무한 반복)
		_image.DOFade(0f, 0.3f) //투명도 0으로 (0.3초동안)
			.SetLoops(-1, LoopType.Yoyo) // -1 : 무한 반복, 요요 : 1(원래값) <-> 0(목표값) 왕복
			.SetEase(Ease.InOutSine); //변화속도
	}

}


