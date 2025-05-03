using System;
using UnityEngine;
using UnityEngine.UI;

public class Util
{
	/// <summary>
	/// 초 단위 시간을 입력받아 HH:MM 형식의 문자열로 변환하여 반환한다.
	/// </summary>
	/// <param name="seconds">변환할 시간(초 단위)</param>
	/// <returns>HH : MM 형식 문자열</returns>
	public static string FormatTimeHM(float seconds)
	{
		TimeSpan ts = TimeSpan.FromSeconds(seconds);
		return $"{ts.Hours:D2} : {ts.Minutes:D2}";
	}

	/// <summary>
	/// 초 단위 시간을 HH : MM 형식으로 반환한다. 콜론 표시 여부를 설정할 수 있다.
	/// </summary>
	/// <param name="seconds">변환할 시간(초 단위)</param>
	/// <param name="showColon">콜론(:)을 표시할지 여부</param>
	/// <returns>HH : MM 또는 HH MM 형식의 문자열</returns>
	public static string FormatTimeHMWithBlink(float seconds, bool showColon)
	{
		TimeSpan ts = TimeSpan.FromSeconds(seconds);
		string separator = showColon ? ":" : " ";
		return $"{ts.Hours:D2} {separator} {ts.Minutes:D2}";
	}
	
	
	public static void SetVisible(Graphic ui, bool isVisible)
	{
		if(ui==null) Debug.Log("ui 가 null");
		if (ui.TryGetComponent<CanvasGroup>(out var group))
		{
			group.alpha = isVisible ? 1f : 0f;
			group.interactable = isVisible;
			group.blocksRaycasts = isVisible;
		}
		else
		{
			ui.gameObject.SetActive(isVisible);
		}
	}
	

	public static void SetVisible(CanvasGroup group, bool isVisible)
	{
		group.alpha = isVisible ? 1f : 0f;
		group.interactable = isVisible;
		group.blocksRaycasts = isVisible;
	}

}
