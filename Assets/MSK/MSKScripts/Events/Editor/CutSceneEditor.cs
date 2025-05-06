using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CutScene))]
public class CutSceneEditor : Editor
{
	public override void OnInspectorGUI()
	{

		var cutscene = target as CutScene;


		if (GUILayout.Button("Add DialogAct"))
		{
			cutscene.Addaction(new DialogAction());
		}
		else if (GUILayout.Button("Add MovAct"))
		{
			cutscene.Addaction(new MovingAction());
		}

		base.OnInspectorGUI();
	}
}
