using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillSData
{
	//Init1
	Dictionary<string, SkillS> skillSData;

	//Init2
	List<SkillS> skillList;
	Dictionary<string, List<SkillS>> skillDatas;

	public string name;
	public string description;
	public int damage;
	public SkillType skillType;

	public void Init()
	{
		skillSData = new Dictionary<string, SkillS>()
		{
			//생성자 사용 중이길래 변수명 알맞게 수정했습니다.
			["잎날가르기"] = new SkillS
			(
				_name : "잎날가르기",
				_description : "적에게 10의 데미지를 입힙니다",
				_damage : 10,
				_skillType : SkillType.Physical,
				_type: PokeType.Grass,
				_pp:25,
				_accuracy: 0.95f
			),
			//
			//비전머신, 기술머신 구현을 위해서 필요한 스킬 데이터를 추가합니다.(이도현)
			//딕셔너리 키 값으로 저는 기술 번호를 넣었습니다. 제가 찾은 사이트에 있는 데이터를 넣었습니다
			["플래시"] = new SkillS
			(
				_name : "플래시",
				_description : "눈이 부신 빛으로 상대의 명중률을 떨어뜨린다",
				_damage : 0, //명중률 -1 랭크 효과인데 명중률 계산이나 보정을 따로 하지 않을 예정이면 빼기
				_skillType : SkillType.Status,
				_type : PokeType.Normal,
				_pp: 20,
				_accuracy: 1,
				isHm: true
			),
			["진흙뿌리기"] = new SkillS
			(
				_name : "진흙뿌리기",
				_description : "상대의 얼굴 등에 진흙을 내던져서 공격한다. 명중률을 떨어뜨린다.",
				_damage : 20,
				_skillType : SkillType.Physical,
				_type : PokeType.Ground,
				_pp: 10,
				_accuracy: 1
			)
		};
	}

	public void Init2()
	{
		skillDatas = new Dictionary<string, List<SkillS>>()
		{
			["치코리타"] = new List<SkillS>()
			{
				new SkillS
				(
					_name : "잎날가르기",
					_description : "적에게 10의 데미지를 입힙니다",
					_damage : 10,
					_skillType : SkillType.Physical,
					_type: PokeType.Grass,
					_pp:25,
					_accuracy: 0.95f
				),
				new SkillS
				(
					_name : "리플렉터",
					_description : "적의 방어력을 10 감소시킵니다",
					_damage : 10,
					_skillType : SkillType.Status,
					_type: PokeType.Psychic, 
					_pp:20,
					_accuracy: 1f
				)
			}
		};
	}
	
	
	public SkillS GetSkillDataByName(string name)
	{
		if (skillSData.TryGetValue(name, out SkillS skill))
		{
			return skill;
		}

		return null;
	}
}
