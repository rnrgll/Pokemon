using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class BattleIntro : MonoBehaviour
{
	[SerializeField] private Image introImage;

	[SerializeField] private List<Sprite> trainerSprites; // 트레이너
	[SerializeField] private List<Sprite> wildSprites; // 야생

	[SerializeField] private float frameTime = 0.1f;

	public string battleScene;

	public event Action OnIntroComplete;

	private IEnumerator PlayIntro(List<Sprite> intro)
	{
		introImage.enabled = true;
		foreach (var spr in intro)
		{
			introImage.sprite = spr;
			yield return new WaitForSeconds(frameTime);
		}

		OnIntroComplete?.Invoke();
	}

	public void PlayTrainerIntro()
	{
		StopAllCoroutines();
		StartCoroutine(PlayIntro(trainerSprites));
	}

	public void PlayWildIntro()
	{
		StopAllCoroutines();
		StartCoroutine(PlayIntro(wildSprites));
	}
}
