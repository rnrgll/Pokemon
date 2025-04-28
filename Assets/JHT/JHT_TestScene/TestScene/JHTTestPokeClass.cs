using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTTestPokeClass : MonoBehaviour
{
    public bool isMyPoke;
    public float damage;
    public int level;

    [SerializeField] JHTSkill[] skill;
    JHTTestHealth health;
    JHTTestExp exp;
    private void Awake()
    {
        if (exp == null && isMyPoke == true)
        {
            gameObject.AddComponent<JHTTestExp>();
        }
    }
    private void Start()
    {
        health = GetComponent<JHTTestHealth>();
    }
    public void Attack() //Skill skill
    {
        damage *= 5f; //skill.skillDamage;
        health.TakeDamage(damage);
    }

}
