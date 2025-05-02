using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines;

	public Dialog() { }

	public Dialog(List<string> lines)
	{
		this.lines = lines;
	}
	public List<string> Lines => lines;
}
