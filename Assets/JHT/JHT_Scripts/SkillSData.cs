using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillSData
{
	//Init1 
	Dictionary<string, SkillS> skillSData;



	//public string name;
	//public string description;
	//public int damage;
	//public SkillType skillType;


	public void Init()
	{
		skillSData = new Dictionary<string, SkillS>()
		{
			["누르기"] = new SkillS("누르기", "몸 전체의 무게로 짓누른다. 때때로 혈액순환을 방해해 마비시킨다", 85,false, SkillType.Physical),
			["독침"] = new SkillS("독침", "유독한 침으로 찌른다. 때때로 상대를 중독시킨다", 15, false, SkillType.Physical),
			["핥기"] = new SkillS("핥기", "혀로 소름 끼치게 핥는다. 때때로 상대를 마비시킨다", 20, false, SkillType.Physical),
			["할퀴기"] = new SkillS("할퀴기", "단단하고, 뾰족하면서 날카로운 손톱이나 발톱으로 상대를 할퀸다.", 40, false, SkillType.Physical),
			["몸통박치기"] = new SkillS("몸통박치기", "몸전체를 이용해 들이받는다", 35, false, SkillType.Physical),
			["용해액"] = new SkillS("용해액", "부식성의 강한 산성액을 뿌린다. 때때로 상대의 방어를 떨어트린다", 40, false, SkillType.Physical),
			["김밥말이"] = new SkillS("김밥말이", "긴 몸이나 넝쿨 등으로 말아 묶어서 지속적인 피해를 준다", 15, false, SkillType.Physical),
			["더블니들"] = new SkillS("더블니들", "2개의 뾰족한 바늘로 연달아 찌른다. 때때로 상대를 중독시킨다", 25, false, SkillType.Physical),
			["바늘미사일"] = new SkillS("바늘미사일", "뾰족한 침을 마구 발사하여 2~5회 연속으로 상대를 찌른다", 14, false, SkillType.Physical),
			["휘감기"] = new SkillS("휘감기", "넝쿨이나 촉수 등으로 휘감아 미미한 피해를 준다. 때때로 상대의 스피드를 떨어트린다", 10, false, SkillType.Physical),
			["마구할퀴기"] = new SkillS("마구할퀴기", "뾰족하면서 날카로운 손톱이나 발톱으로 2~5회 연속으로 난도질한다", 18, false, SkillType.Physical),
			["힘껏치기"] = new SkillS("힘껏치기", "비교적 길고 넓적한 부위를 이용해 세게 내려친다", 80, false, SkillType.Physical),
			["흡혈"] = new SkillS("흡혈", "이빨로 깨물고 피를 빨아 생명력을 빼앗는다", 20, false, SkillType.Physical),
			["돌떨구기"] = new SkillS("돌떨구기", "작은 바위를 던져 공격한다", 50, false, SkillType.Physical),
			["자폭"] = new SkillS("자폭", "스스로 폭발하여 주변에 피해를 주고 자신은 전투불능이 된다", 200, false, SkillType.Physical),
			["구르기"] = new SkillS("구르기", "스스로 걷잡을 수 없이 계속 구른다. 미리 웅크렸다면 더욱 거세진다", 30, false, SkillType.Physical),
			["지진"] = new SkillS("지진", "강한 지진을 일으켜 주변 땅에 있는 것들에 피해를 준다", 100, false, SkillType.Physical),
			["대폭발"] = new SkillS("대폭발", "큰 폭발을 일으켜 주변에 피해를 주고 자신은 전투불능이 된다", 250, false, SkillType.Physical),
			["스피드스타"] = new SkillS("스피드스타", "빗나가지 않는 별 모양의 빛을 날린다", 60, false, SkillType.Physical),
			["분노"] = new SkillS("분노", "공격 받을수록 점점 더 격한 분노를 표출한다", 20 , false, SkillType.Physical),
			["베어가르기"] = new SkillS("베어가르기", "예리한 발톱이나 칼날로 벤다. 급소에 맞히기 쉽다", 70, false, SkillType.Physical),
			["바람일으키기"] = new SkillS("바람일으키기", "세찬 바람을 일으켜 적을 타격한다", 40 , false, SkillType.Physical),
			["날개치기"] = new SkillS("날개치기", "날개를 크고 넓게 펼쳐서 가격한다", 60 , false, SkillType.Physical),
			["쪼기"] = new SkillS("쪼기", "주둥이나 부리로 쪼아댄다", 35 , false, SkillType.Physical),
			["돌진"] = new SkillS("돌진", "자신도 다칠 수 있지만 앞뒤를 가리지 않고 돌진해 들이 받는다", 90, false, SkillType.Physical),
			["필살앞니"] = new SkillS("필살앞니", "날카로운 앞니로 콱 물어 본때를 보여 떄때로 상대를 풀이 죽게 한다", 80 , false, SkillType.Physical),


			["따라가때리기"] = new SkillS("따라가때리기", "교체하려는 낌새를 보이면, 교체하기 전에 강하게 후려친다", 40 , false, SkillType.Special),
			["환상빔"] = new SkillS("환상빔", "이상야릇한 광선을 발사한다. 떄때로 약한 정신 분열을 유발한다", 64, false, SkillType.Special),
			["물대포"] = new SkillS("물대포", "물을 힘차게 뿜어 공격한다", 40, false, SkillType.Special),
			["화염방사"] = new SkillS("화염방사", "강렬한 불줄기를 뿜는다 상대에게 때때로 화상을 입힌다", 95, false, SkillType.Special),
			["물기"] = new SkillS("물기", "날카로운 송곳니로 포악하게 깨문다. 때때로 상대를 풀이 죽게 만든다", 60, false, SkillType.Special),
			["덩굴채찍"] = new SkillS("덩굴채찍", "가는 덩굴로 채찍질한다", 35, false, SkillType.Special),
			["솔라빔"] = new SkillS("솔라빔", "잠시동안 햇빛을 모은 후, 태양광선을 발사한다", 120, false, SkillType.Special),
			["불꽃세례"] = new SkillS("불꽃세례", "작은 불꽃을 날린다. 상대에게 때때로 화상을 입힌다 ", 40, false, SkillType.Special),
			["전광석화"] = new SkillS("전광석화", "눈으로 쫓을 수 없을 정도의 엄청난 속도로 돌진한다", 40, false, SkillType.Special),
			["잎날가르기"] = new SkillS("잎날가르기", "예리한 잎들을 날려 적을 베어버린다. 급소에 맞히기 쉽다", 55, false, SkillType.Special),
			["사이코키네시스"] = new SkillS("사이코키네시스", "강한 염력을 보내 공격한다. 때때로 상대의 특수방어를 떨어트린다", 90, false, SkillType.Special),
			["염동력"] = new SkillS("염동력", "약한 염력을 보내 공격한다. 상대는 때때로 정신이 혼미해진다", 50, false, SkillType.Special),
			["꿈먹기"] = new SkillS("꿈먹기", "자고 있는 포켓몬의 꿈을 먹어 생명력을 빼앗는다", 100, false, SkillType.Special),


			["웅크리기"] = new SkillS("웅크리기", "공격에 대비해 몸을 웅크려 둥글게 한다",0,true, SkillType.Status),
			["잠자기"] = new SkillS("잠자기 ", "바로 숙면을 취해 아픈 곳 없이 말끔히 낫는다. 한동안 푹 자다가 스스로 깬다", 0 ,true, SkillType.Status),
			["망각술"] = new SkillS("망각술", "잠시 머리 속을 비워 잡념을 없애 특수방어가 부쩍 상승한다", 0 ,true, SkillType.Status),
			["초음파"] = new SkillS("초음파", "몸에서 발산하는 초음파로 상대를 혼란시킨다", 0 ,false, SkillType.Status),
			["단단해지기"] = new SkillS("단단해지기", "몸 전체에 힘을 줘 단단하게 한다", 0 ,true, SkillType.Status),
			["원한"] = new SkillS("원한", "상대의 마지막 기술에 앙심을 품어 기술을 선보일 기회를 줄여버린다", 0 ,false, SkillType.Status),
			["기충전"] = new SkillS("기충전", "깊게 숨을 들이쉬고 기합을 넣어 집중력을 높힌다", 0,true, SkillType.Status),
			["검은눈빛"] = new SkillS("검은눈빛", "빨려들듯한 검은 눈빛으로 응시하여 도망가지 못하게 한다", 0 , false, SkillType.Status),
			["싫은소리"] = new SkillS("싫은소리", "귀 따가운 소리를 내어 방어를 크게 떨어트린다", 0, false, SkillType.Status),
			["하이드로펌프"] = new SkillS("겁나는얼굴", "무서운 얼굴로 위협한다. 상대는 움츠러들어 현저히 굼떠진다", 0, false, SkillType.Status),
			["모래뿌리기"] = new SkillS("모래뿌리기", "상대의 얼굴에 모래를 확 뿌린다. 모래 때문에 잘 볼 수 없어 기술이 빗나가기 쉬워진다", 0, false, SkillType.Status),
			["저주"] = new SkillS("저주", "자신의 무언가를 깎아내려 포켓몬에게 변화를 일으킨다", 0 , false, SkillType.Status), //둘다 작용해야됨
			["이상한빛"] = new SkillS("이상한빛", "쪼이면 발작을 일으키는 괴상한 빛을 발산한다", 0 , false, SkillType.Status),
			["길동무"] = new SkillS("길동무", "자신을 쓰러트린 적 포켓몬을 길동무 삼아 같이 기절한다", 0 , false, SkillType.Status),
			["성장"] = new SkillS("성장", "몸을 단숨에 성장시켜 특수공격을 높힌다", 0 ,true, SkillType.Status),
			["고속이동"] = new SkillS("고속이동", "힘을 빼고 몸을 가볍게 해 매우 빨리 움직일 수 있게 된다", 0,true, SkillType.Status),
			["꿰뚫어보기"] = new SkillS("꿰뚫어보기", "상대를 면밀히 파악한다. 간파된 포켓몬에게는 이후의 기술이 잘 맞게 된다", 0, false, SkillType.Status),
			["수면가루"] = new SkillS("수면가루", "졸음을 유발하는 가루를 뿌려 재운다", 0 , false, SkillType.Status),
			["연막"] = new SkillS("연막", "뿌연 연기나 진한 먹물을 뿌려 상대의 시야를 흐려 명중률을 떨어트린다", 0, false, SkillType.Status),
			["겁나는얼굴"] = new SkillS("겁나는얼굴", "무서운 얼굴로 위협한다. 상대는 움츠려들어 현저히 굼떠진다", 0, false, SkillType.Status),
			["달콤한향기"] = new SkillS("달콤한향기", "달콤한 향기를 풍겨 포켓몬이 몰려들게 한다. 향기를 맡은 적은 정신이 팔려 반사신경이 저하된다", 0 , false, SkillType.Status),
			["최면술"] = new SkillS("최면술", "최면을 걸어 잠들게 한다", 0, false, SkillType.Status),
			["울음소리"] = new SkillS("울음소리", "애교 석인 울음 소리를 내어 적들이 살살 공격하게 만든다", 0 , false, SkillType.Status),
			["꼬리흔들기"] = new SkillS("꼬리흔들기", "귀엽게 꼬리를 흔들어 적이 경계를 게을리 하도록 만든다", 0 , false, SkillType.Status),
			["실뿜기"] = new SkillS("실뿜기", "입에서 실을 뿜어서 적에게 얽혀 속도를 떨어트린다", 0 , false, SkillType.Status),
			["날려버리기"] = new SkillS("날려버리기", "상대 포켓몬을 멀리 날려버린다", 0, false, SkillType.Status),
			["리플렉터"] = new SkillS("리플렉터", "불가사의한 장벽을 형성해 물리 공격으로 입는 피해를 줄인다", 0,true, SkillType.Status),
			["독가루"] = new SkillS("독가루", "유독한 가루를 뿌려 중독시킨다", 0, false, SkillType.Status),
			["광합성"] = new SkillS("광합성", "광합성을 하여 생산된 에너지로 활력을 되찾는다. 더 강한 햇빛을 받을수록 효율이 높아진다", 0,true, SkillType.Status),
			["빛의장막"] = new SkillS("빛의장막", "불가사의한 장막을 형성해 특수 공격으로 입는 피해를 줄인다", 0,true, SkillType.Status),
			["신비의부적"] = new SkillS("신비의부적", "신비한 힘이 상대로부터 오는 각종 방해로부터 아군 전체를 지켜준다", 0,true, SkillType.Status),
			//["37"] = new SkillS(" ", " ", , SkillType.),
			//["벌레먹음"] = new SkillS(" ", " ", , SkillType.),
			//["저리가루"] = new SkillS(" ", " ", , SkillType.),
			//["화염자동차"] = new SkillS(" ", " ", , SkillType.),
			//["분노의앞니"] = new SkillS("분노의앞니", " ", ? , SkillType.Physical),
			//["나이트헤드"] = new SkillS("나이트헤드", " ", ? , SkillType.Physical),
			//["매그니튜드"] = new SkillS("매그니튜드", " ", ? , SkillType.Physical),
		};
	}

}
