using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuDescription", menuName = "ScriptableObject/UI/Menu Description", order = 0)]
public class MenuDescriptasbleData : ScriptableObject
{
   public string menuName;
   [TextArea(2, 2)] public string description;
}
