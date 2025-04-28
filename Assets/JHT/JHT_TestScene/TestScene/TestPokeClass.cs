using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPokeClass : MonoBehaviour
{
    public bool isMyPoke;
    public float damage;
    public int level;

    [SerializeField] Skill[] skill;
    TestHealth health;
    private void Start()
    {
        health = GetComponent<TestHealth>();
    }
    public void Attack() //Skill skill
    {
        damage *= 5f; //skill.skillDamage;
        health.TakeDamage(damage);
    }

}
