using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcMovVector2 { 
	[SerializeField] List<Vector2> movePattens;

	public List<Vector2> MovePattens
	{
		get { return MovePattens; }
	}
}

