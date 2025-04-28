using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image image;
    Button button;
    private PokemonStat pokeStat;
    public PokemonStat PokeStat
    {
        get { return pokeStat; }
        set
        {
            pokeStat = value;
            if (pokeStat != null)
            {
                image.sprite = PokeStat.icon;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }
        }
    }
}
