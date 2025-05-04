using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class SkillSData
{
	Dictionary<string, SkillS> skillSData;

	public void Init()
	{
		skillSData = new Dictionary<string, SkillS>()
		{
			// Physical
			["누르기"] = new BodySlam(),
			["독침"] = new PoisonSting(),
			["핥기"] = new Lick(),
			["할퀴기"] = new Scratch(),
			["몸통박치기"] = new Tackle(),
			["김밥말이"] = new Wrap(),
			["더블니들"] = new Twineedle(),
			["바늘미사일"] = new PinMissile(),
			["휘감기"] = new Constrict(),
			["마구할퀴기"] = new FurySwipes(),
			["힘껏치기"] = new Slam(),
			["흡혈"] = new LeechLife(),
			["돌떨구기"] = new RockThrow(),
			["자폭"] = new Selfdestruct(),
			["구르기"] = new Rollout(),
			["지진"] = new Earthquake(),
			["대폭발"] = new Explosion(),
			["분노"] = new Rage(),
			["베어가르기"] = new Slash(),
			["날개치기"] = new WingAttack(),
			["쪼기"] = new Peck(),
			["돌진"] = new TakeDown(),
			["필살앞니"] = new HyperFang(),
			["따라가때리기"] = new Pursuit(),
			["물기"] = new Bite(),
			["덩굴채찍"] = new VineWhip(),
			["전광석화"] = new QuickAttack(),
			["잎날가르기"] = new RazorLeaf(),
			["화염자동차"] = new FlameWheel(),
			["마구찌르기"] = new FuryAttack(),
			["회전부리"] = new DrillPeck(),
			["벌레먹음"] = new BugBite(),
			["매그니튜드"] = new Magnitude(),
			["분노의앞니"] = new SuperFang(),

			// Special
			["스피드스타"] = new Swift(),
			["환상빔"] = new Psybeam(),
			["물대포"] = new WaterGun(),
			["화염방사"] = new Flamethrower(),
			["솔라빔"] = new SolarBeam(),
			["불꽃세례"] = new Ember(),
			["사이코키네시스"] = new Psychic(),
			["염동력"] = new Confusion(),
			["꿈먹기"] = new DreamEater(),
			["하이드로펌프"] = new HydroPump(),
			["용해액"] = new Acid(),
			["바람일으키기"] = new Gust(),
			["나이트헤드"] = new NightShade(),


			// Status
			["웅크리기"] = new DefenseCurl(),
			["잠자기"] = new Rest(),
			["망각술"] = new Amnesia(),
			["초음파"] = new Supersonic(),
			["단단해지기"] = new Harden(),
			["원한"] = new Grudge(),
			["기충전"] = new FocusEnergy(),
			["검은눈빛"] = new MeanLook(),
			["싫은소리"] = new Screech(),
			["모래뿌리기"] = new SandAttack(),
			["저주"] = new Curse(),
			["이상한빛"] = new ConfuseRay(),
			["길동무"] = new DestinyBond(),
			["성장"] = new Growth(),
			["고속이동"] = new Agility(),
			["꿰뚫어보기"] = new Foresight(),
			["수면가루"] = new SleepPowder(),
			["연막"] = new SmokeScreen(),
			["겁나는얼굴"] = new ScaryFace(),
			["달콤한향기"] = new SweetScent(),
			["최면술"] = new Hypnosis(),
			["울음소리"] = new Growl(),
			["꼬리흔들기"] = new TailWhip(),
			["실뿜기"] = new StringShot(),
			["날려버리기"] = new Whirlwind(),
			["리플렉터"] = new Reflect(),
			["독가루"] = new PoisonPowder(),
			["광합성"] = new Synthesis(),
			["빛의장막"] = new LightScreen(),
			["신비의부적"] = new Safeguard(),
			["거미집"] = new SpiderWeb(),
			["째려보기"] = new Leer(),
			["따라하기"] = new MirrorMove(),
			["저리가루"] = new StunSpore(),

			// HM
			["플래시"] = new Flash(),
			// TM
			["진흙뿌리기"] = new MudSlap(),
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

	//생성자 사용 중이길래 변수명 알맞게 수정했습니다.
	// //
	// //비전머신, 기술머신 구현을 위해서 필요한 스킬 데이터를 추가합니다.(이도현)
	// //딕셔너리 키 값으로 저는 기술 번호를 넣었습니다. 제가 찾은 사이트에 있는 데이터를 넣었습니다
	// ["플래시"] = new SkillS
	// (
	// 	_name : "플래시",
	// 	_description : "눈이 부신 빛으로 상대의 명중률을 떨어뜨린다",
	// 	_damage : 0, //명중률 -1 랭크 효과인데 명중률 계산이나 보정을 따로 하지 않을 예정이면 빼기
	// 	_skillType : SkillType.Status,
	// 	_type : PokeType.Normal,
	// 	_pp: 20,
	// 	_accuracy: 1,
	// 	isHm: true
	// ),
	// ["진흙뿌리기"] = new SkillS
	// (
	// 	_name : "진흙뿌리기",
	// 	_description : "상대의 얼굴 등에 진흙을 내던져서 공격한다. 명중률을 떨어뜨린다.",
	// 	_damage : 20,
	// 	_skillType : SkillType.Physical,
	// 	_type : PokeType.Ground,
	// 	_pp: 10,
	// 	_accuracy: 1
	// )
}
	
	

