using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokeType
{
    fire,
    water,
    elec,
    wind,
    grass
}

public class PokemonStat : ScriptableObject
{
    public Sprite icon;
    public int ID;
    public PokeType type;
    public string name;
    public GameObject pokePrefab;
    public string description;
    public bool isMine = false;
    public float exp;
    private int hp = 10;
    public int Hp {  get { return hp; } }
    //[SerializeField] Skill skill;

}
