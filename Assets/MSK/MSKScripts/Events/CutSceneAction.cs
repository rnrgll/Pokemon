using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]	
public class CutSceneAction
{
	[SerializeField] string name;


	public virtual IEnumerator PlayEvent()
	{
		yield break;
	}

	public string Name
	{
		get => name; 
		set => name = value;
	}
}
