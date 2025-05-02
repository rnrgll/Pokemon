using UnityEngine;

public class UI_BasicPopUp : UI_PopUp
{
	private Dialog dialog;


	public void SetDialog(string message)
	{
		string[] lines = message.Split('\n');
		foreach (string line in lines)
		{
			dialog.Lines.Add(line);
		}
	}
}
