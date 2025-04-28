using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTTestHealth : MonoBehaviour
{
    private int maxHp = 10;
    public int curHp;
    public int expPoints = 10;

    public bool isDead;

    JHTTestExp exp;

    void Start()
    {
        exp = GetComponent<JHTTestExp>();
        curHp = maxHp;
        
    }

    public void TakeDamage(float damage)
    {
        curHp -= (int)damage;

        if (curHp <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        exp.GetExp(expPoints);
    }
}
