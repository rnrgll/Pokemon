using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

	public class UI_HpBarController : MonoBehaviour
	{
		[SerializeField] private Slider hpSlider;
		public Slider HpSlider => hpSlider;
		[SerializeField] private TMP_Text curHpText;
		[SerializeField] private TMP_Text maxHpText;

		private Coroutine animCoroutine;

		public void SetHp(int hp, int maxHp)
		{
			hpSlider.maxValue = maxHp;
			hpSlider.value = hp;
			if (curHpText != null) curHpText.text = $"{hp}/";
			if (maxHpText != null) maxHpText.text = maxHp.ToString();
			
			UpdateSliderColor(hp, maxHp);

		}

		public void AnimationHpChange(int formHp, int toHp, int maxHp)
		{
			if (animCoroutine != null) StopCoroutine(animCoroutine);
			animCoroutine = StartCoroutine(HpChangeCoroutine(formHp, toHp, maxHp));
		}

		//hp slider 차오르는 효과
		private IEnumerator HpChangeCoroutine(int fromHp, int toHp, int maxHp)
		{
			float duration = 0.4f; // 전체 애니메이션 시간
			float elapsed = 0f;
			int currentHp = fromHp;

			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float t = Mathf.Clamp(elapsed / duration, 0f, 1f);
				currentHp = Mathf.RoundToInt(Mathf.Lerp(fromHp, toHp, t));
				
				hpSlider.value = currentHp;
				if (curHpText != null) curHpText.text = $"{currentHp}/";
				if (maxHpText != null) maxHpText.text = maxHp.ToString();
				
				UpdateSliderColor(currentHp, maxHp);
				yield return null;
			}
		}


		private void UpdateSliderColor(int hp, int maxHp)
		{
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