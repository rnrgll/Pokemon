using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillSData
{
	Dictionary<string, SkillS> skillSData;

	public void Init()
	{
		skillSData = new Dictionary<string, SkillS>()
		{
			["누르기"] = new Press(),
			["독침"] = new stinger(),
			["핥기"] = new Lick(),
			["할퀴기"] = new Scratch(),
			["몸통박치기"] = new BodySlam(),
			["용해액"] = new Solution(),
			["김밥말이"] = new RolledKimbap(),
			["더블니들"] = new DoubleNeedle(),
			["바늘미사일"] = new NeedleMissile(),
			["휘감기"] = new Winding(),
			["마구할퀴기"] = new Scratching(),
			["힘껏치기"] = new StrikeAllMight(),
			["흡혈"] = new BloodSucking(),
			["돌떨구기"] = new ThrowingStones(),
			["자폭"] = new SelfDestruct(),
			["구르기"] = new Roll(),
			["지진"] = new Earthquake(),
			["대폭발"] = new BigExplosion(),
			["스피드스타"] = new SpeedStar(),
			["분노"] = new Anger(),
			["베어가르기"] = new CutApart(),
			["바람일으키기"] = new MakeBreeze(),
			["날개치기"] = new WingBeat(),
			["쪼기"] = new Peck(),
			["돌진"] = new Dash(),
			["필살앞니"] = new CriticalFrontTeeth(),


			["따라가때리기"] = new FollowHit(),
			["환상빔"] = new PhantomBeam(),
			["물대포"] = new WaterCannon(),
			["화염방사"] = new Flamethrower(),
			["물기"] = new Bite(),
			["덩굴채찍"] = new VineWhip(),
			["솔라빔"] = new SolarBeam(),
			["불꽃세례"] = new BaptismFire(),
			["전광석화"] = new LightningFlash(),
			["잎날가르기"] = new LeafCutting(),
			["사이코키네시스"] = new Psychokinesis(),
			["염동력"] = new Telekinesis(),
			["꿈먹기"] = new EatingDream(),


			["웅크리기"] = new Crouch(),
			["잠자기"] = new Sleep(),
			["망각술"] = new Oblivion(),
			["초음파"] = new Ultrasound(),
			["단단해지기"] = new BecomeHard(),
			["원한"] = new Resentment(),
			["기충전"] = new Recharge(),
			["검은눈빛"] = new BlackEyes(),
			["싫은소리"] = new DisgustingSound(),
			["하이드로펌프"] = new HydroPump(),
			["모래뿌리기"] = new SprinkleSand(),
			["저주"] = new Curse(),
			["이상한빛"] = new Strangelight(),
			["길동무"] = new ComradeGil(),
			["성장"] = new Growth(),
			["고속이동"] = new HighSpeedMovement(),
			["꿰뚫어보기"] = new SeeThrough(),
			["수면가루"] = new SleepingPowder(),
			["연막"] = new SmokeScreen(),
			["겁나는얼굴"] = new ScaryFace(),
			["달콤한향기"] = new SweetScent(),
			["최면술"] = new Mesmerism(),
			["울음소리"] = new CrySound(),
			["꼬리흔들기"] = new TailWagging(),
			["실뿜기"] = new SpitOut(),
			["날려버리기"] = new BlowAway(),
			["리플렉터"] = new Reflector(),
			["독가루"] = new PoisonPowder(),
			["광합성"] = new Photosynthesis(),
			["빛의장막"] = new CurtainOfLight(),
			["신비의부적"] = new MysteriousAmulet(),
			//["37"] = new SkillS(" ", " ", , SkillType.),
			//["벌레먹음"] = new SkillS(" ", " ", , SkillType.),
			//["저리가루"] = new SkillS(" ", " ", , SkillType.),
			//["화염자동차"] = new SkillS(" ", " ", , SkillType.),
			//["분노의앞니"] = new SkillS("분노의앞니", " ", ? , SkillType.Physical),
			//["나이트헤드"] = new SkillS("나이트헤드", " ", ? , SkillType.Physical),
			//["매그니튜드"] = new SkillS("매그니튜드", " ", ? , SkillType.Physical),
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
			// ["잎날가르기"] = new SkillS
			// (
			// 	_name : "잎날가르기",
			// 	_description : "적에게 10의 데미지를 입힙니다",
			// 	_damage : 10,
			// 	_skillType : SkillType.Physical,
			// 	_type: PokeType.Grass,
			// 	_pp:25,
			// 	_accuracy: 0.95f
			// ),
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
	
	

