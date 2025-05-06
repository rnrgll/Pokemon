using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class SkillS
{
	//public Sprite icon;
	public string name;
	public string description;
	public float damage;
	public SkillType skillType;
	public bool isMyStat;	// isMyStat?

	public PokeType type; //포켓몬 타입이랑 기술 타입이랑 동일한 타입 사용해서 추가함(이도현)
	public int curPP;	// pp에서 curPP와 maxPP로 분류 (손재훈)
	public int maxPP;
	public float accuracy; //위력
	public bool isHM; //비전머신 기술인지의 여부
	public GameObject Physicsparticle;
	public GameObject specialParticle;

	public SkillS(string _name, string _description, float _damage, SkillType _skillType,bool _isMyStat, PokeType _type, int _pp, float _accuracy, bool _isHm=false)
	{
		this.name = _name;
		this.description = _description;
		this.damage = _damage;
		this.skillType = _skillType;
		this.type = _type;
		this.isMyStat = _isMyStat;
		this.maxPP = _pp;
		this.curPP = maxPP;
		this.accuracy = _accuracy; //명중률 구현 안할꺼면 빼기
		this.isHM = _isHm;
	}

	public abstract void UseSkill(Pokémon attacker, Pokémon defender, SkillS skill);
}
