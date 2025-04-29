using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTPokemonStat : MonoBehaviour
{
    [Range(1,20)]
	public int startLevel = 1;
	public float exp;
   

    public Sprite icon;
    public JHTPokeType type;
    [SerializeField] JHTPokeController controller = null;
    [SerializeField] JHTSkill[] skill = null;

    public int GetPokeStat(JHTStat stat)
    {
        return controller.GetStat(stat,type,startLevel);
    }

    //public int GetLevel()
    //{
    //    int currentExp = GetComponent<Experience>().GetExp();
    //}
}
