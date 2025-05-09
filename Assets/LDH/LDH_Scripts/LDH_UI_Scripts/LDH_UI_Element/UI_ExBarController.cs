using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExpBarController : MonoBehaviour
{
	[SerializeField] private Slider expSlider;

	public IEnumerator AnimateExpBar(int from, int to, int max)
	{
		expSlider.maxValue = max;

		float duration = 0.5f;
		float elapsed = 0f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / duration);
			expSlider.value = Mathf.Lerp(from, to, t);
			yield return null;
		}

		expSlider.value = to;
	}

	public void SetExp(int cur, int max)
	{
		expSlider.maxValue = max;
		expSlider.value = cur;
		
		Debug.Log($"경험치 슬라이더 값 : {cur} / {max}");
	}
}
