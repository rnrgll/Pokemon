using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JHTSlot : MonoBehaviour
{
    [SerializeField] Image image;
    Button button;
    private JHTPokemonStat pokeStat;
    public JHTPokemonStat PokeStat
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
