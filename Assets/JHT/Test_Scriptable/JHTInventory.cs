using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class JHTInventory : MonoBehaviour
{
    public List<JHTPokemonStat> pokeStats;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private JHTSlot[] slots;

    public UnityEvent<JHTPokemonStat> OnChoicePoke;

    JHTFightScene fightScene;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<JHTSlot>();
    }

    void Awake()
    {
        fightScene = FindObjectOfType<JHTFightScene>();
        FreshSlot();
    }

    //private void OnEnable()
    //{
    //    OnChoicePoke.AddListener(Choice);
    //}
    //
    //private void OnDisable()
    //{
    //    OnChoicePoke.RemoveListener(Choice);
    //}
    public void FreshSlot()
    {
        for (int i = 0; i < pokeStats.Count && i < slots.Length; i++)
        {
            slots[i].PokeStat = pokeStats[i];
        }
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].PokeStat = null;
        }
    }

    public void AddItem(JHTPokemonStat poke)
    {
        if (pokeStats.Count < slots.Length)
        {
            pokeStats.Add(poke);
            FreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
    }


    //public void Choice(PokemonStat stat) 
    //{
    //    if (stat !=null)
    //    {
    //        OnChoicePoke?.Invoke(stat);
    //        fightScene.myPoke = stat.pokePrefab;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("포켓몬이 존재하지 않습니다!");
    //    }
    //}

}
