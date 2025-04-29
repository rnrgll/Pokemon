using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class JHTHealth : MonoBehaviour
{
    [SerializeField] int healthPoints = 50;

    bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }
    void Start()
    {
        healthPoints = GetComponent<JHTPokemonStat>().GetPokeStat(JHTStat.체력);
    }

    public void TakeDamage(int damage) //GameObject instigator, -> 해당 적이 죽었을경우
    {
        healthPoints = Mathf.Max(healthPoints - damage, 0);
        if (healthPoints <= 0)
        {
            Die();
            //GetRewardExp(instigator); ->경험치 획득 나중에 배틀씬과 합쳤을 때 구현
        }
    }

    public void GetRewardExp(GameObject instigator)
    {
        JHTExperience experience = instigator.GetComponent<JHTExperience>();
        if (experience == null) return;

        experience.GetExperience(GetComponent<JHTPokemonStat>().GetPokeStat(JHTStat.경험치_획득량));
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
    }

}
