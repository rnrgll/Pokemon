using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class MovingAction : CutSceneAction
{
	[SerializeField] List<Vector2> movPatterns;
	[SerializeField] NpcMover NpcMover;

	public override IEnumerator PlayEvent()
	{
		foreach (var movVec in movPatterns)
		{

			yield return NpcMover.NPCMove();
		}
	}
}
