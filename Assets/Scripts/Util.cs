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
	
	
	//UI 위치 조정
	/// <summary>
	/// 대상 RectTransform을 **왼쪽 하단 기준(anchor/pivot)**으로 정렬하고,
	/// 주어진 X, Y 오프셋만큼 위치를 조정한다.
	/// </summary>
	/// <param name="targetRect">위치를 조정할 RectTransform</param>
	/// <param name="offsetX">왼쪽 기준의 X 오프셋</param>
	/// <param name="offsetY">하단 기준의 Y 오프셋</param>
	public static void SetPositionFromBottomLeft(RectTransform tartgetRect, float offsetX, float offsetY)
	{
		tartgetRect.anchorMin = new Vector2(0f, 0f);
		tartgetRect.anchorMax = new Vector2(0f, 0f);
		tartgetRect.pivot = new Vector2(0f, 0f);
		tartgetRect.anchoredPosition = new Vector2(offsetX, offsetY);
	}

	/// <summary>
	/// 대상 RectTransform을 **오른쪽 하단 기준(anchor/pivot)**으로 정렬하고,
	/// 주어진 X, Y 오프셋만큼 위치를 조정한다.
	/// </summary>
	/// <param name="targetRect">위치를 조정할 RectTransform</param>
	/// <param name="offsetX">오른쪽 기준의 X 오프셋 (양수로 입력 시 왼쪽으로 이동)</param>
	/// <param name="offsetY">하단 기준의 Y 오프셋</param>
	public static void SetPositionFromBottomRight(RectTransform targetRect, float offsetX, float offsetY)
	{
		targetRect.anchorMin = new Vector2(1f, 0f);
		targetRect.anchorMax = new Vector2(1f, 0f);
		targetRect.pivot = new Vector2(1f, 0f);
		targetRect.anchoredPosition = new Vector2(-offsetX, offsetY);
	}

	/// <summary>
	/// anchoredPosition의 Y값을 캔버스 높이에 비례한 비율만큼 이동시킨다.
	/// (예: factor = 0.34 → 캔버스 하단 기준으로 34% 위치에 배치)
	/// </summary>
	/// <param name="targetRect">대상 RectTransform</param>
	/// <param name="canvas">기준이 되는 캔버스</param>
	/// <param name="factor">0.0~1.0 사이의 상대 위치 비율</param>
	public static void SetRelativeVerticalOffset(RectTransform targetRect, Canvas canvas, float factor)
	{
		RectTransform canvasRect = canvas.GetComponent<RectTransform>();
		float canvasHeight = canvasRect.rect.height;
		
		Vector2 pos = targetRect.anchoredPosition;
		targetRect.anchoredPosition = new Vector2(pos.x, canvasHeight * factor);
	}

}
