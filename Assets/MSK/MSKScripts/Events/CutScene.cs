using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutScene : MonoBehaviour
{
	public bool onTrigger;
	[SerializeReference]
	[SerializeField] List<CutSceneAction> actions;

	public IEnumerator PlayEvent()
	{
		//	이벤트 중 움직임 제한
		Manager.Game.Player.State = Define.PlayerState.Dialog;

		foreach (var action in actions) {

			yield return action.PlayEvent();
		}
		//	제한 해방
		Manager.Game.Player.State = Define.PlayerState.Field;
	}
	public void Addaction(CutSceneAction action) {
		
		action.Name = action.GetType().ToString();
		actions.Add(action);
	}
}
