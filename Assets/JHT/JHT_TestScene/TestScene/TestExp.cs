using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExp : MonoBehaviour
{
    public int curExp;
    private int startExp = 0;

    private void Start()
    {
        curExp = startExp;
    }

    public void GetExp(int amount)
    {
        curExp += amount;
        if (curExp > GameManager.Instance.LevelUpPoint1)
        {
            TestPokeClass poke = GetComponent<TestPokeClass>();
            if (poke.isMyPoke == true)
            {
                poke.level++;
                poke.damage *= 1.5f;
                
            }
            //ÁøÈ­
        }
    }

}
