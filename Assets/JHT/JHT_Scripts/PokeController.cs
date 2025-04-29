using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
	None,
	Poison,
	Freeze,
	Burn,
	Sleep,
	Paralysis
}

public class PokeController : PokeClass
{
	PokeHealth target;
	public State state;

	public List<PokeSkill> skill;
	Animator animator;

	private void Start()
	{
		skill = new List<PokeSkill>();
		animator = GetComponent<Animator>();
	}


	public void ActiveSkill(PokeSkill skill)
	{
		animator.Play(skill.skillName);
		Debug.Log($"{skill.damage * this.power}만큼 데이지를 줍니다");
	}


	public void GetXp(float amount)
	{
		exp += amount;
		
	}
}
