using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class DialogAction : CutSceneAction
{
	[SerializeField] Dialog dialog;
	

	public override IEnumerator PlayEvent()
	{
		yield break;// Manager.Dialog.StartDialogue(dialog);
	}
}
