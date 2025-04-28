using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonStat : MonoBehaviour
{
    [Range(1,20)]
    public float exp;
    public int startLevel = 1;

    public Sprite icon;
    public PokeType type;
    [SerializeField] PokeController controller = null;
    [SerializeField] Skill[] skill = null;

    public int GetPokeStat(Stat stat)
    {
        return controller.GetStat(stat,type,startLevel);
    }

    //public int GetLevel()
    //{
    //    int currentExp = GetComponent<Experience>().GetExp();
    //}
}
