using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealth : MonoBehaviour
{
    private int maxHp;
    public int curHp;
    public int expPoints = 10;

    TestExp exp;
    void Start()
    {
        exp = GetComponent<TestExp>();
        curHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        curHp -= (int)damage;
        if (curHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        exp.GetExp(expPoints);
    }
}
