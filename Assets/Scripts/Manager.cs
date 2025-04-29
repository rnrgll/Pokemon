using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager
{
	public static UIManager UI { get { return UIManager.GetInstance(); } }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		UIManager.CreateInstance();
	}
}
