using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static Define;

public class SJH_PokemonData
{
	private Dictionary<int, SJH_PokemonData> dataById;
	private Dictionary<string, SJH_PokemonData> dataByName;

	public Dictionary<string, string> pokemonKorName = new Dictionary<string, string>()
	{
		["Chikorita"] = "치코리타",
		["Bayleef"] = "베이리프",
		["Meganium"] = "메가니움",
		["Cyndaquil"] = "브케인",
		["Quilava"] = "마그케인",
		["Typhlosion"] = "블레이범",
		["Totodile"] = "리아코",
		["Croconaw"] = "엘리게이",
		["Feraligatr"] = "장크로다일",
		["Pidgey"] = "구구",
		["Pidgeotto"] = "피죤",
		["Pidgeot"] = "피죤투",
		["Spearow"] = "깨비참",
		["Fearow"] = "깨비드릴조",
		["Hoothoot"] = "부우부",
		["Noctowl"] = "야부엉",
		["Rattata"] = "꼬렛",
		["Raticate"] = "레트라",
		["Sentret"] = "꼬리선",
		["Furret"] = "다꼬리",
		["Caterpie"] = "캐터피",
		["Metapod"] = "단데기",
		["Butterfree"] = "버터플",
		["Weedle"] = "뿔충이",
		["Kakuna"] = "딱충이",
		["Beedrill"] = "독침붕",
		["Spinarak"] = "페이검",
		["Ariados"] = "아리아도스",
		["Geodude"] = "꼬마돌",
		["Graveler"] = "데구리",
		["Gastly"] = "고오스",
		["Haunter"] = "고우스트",
		["Bellsprout"] = "모다피",
		["Weepinbell"] = "우츠동",
	};

	public Dictionary<string, string> pokemonEngName = new Dictionary<string, string>()
	{
		["치코리타"] = "Chikorita",
		["베이리프"] = "Bayleef",
		["메가니움"] = "Meganium",
		["브케인"] = "Cyndaquil",
		["마그케인"] = "Quilava",
		["블레이범"] = "Typhlosion",
		["리아코"] = "Totodile",
		["엘리게이"] = "Croconaw",
		["장크로다일"] = "Feraligatr",
		["구구"] = "Pidgey",
		["피죤"] = "Pidgeotto",
		["피죤투"] = "Pidgeot",
		["깨비참"] = "Spearow",
		["깨비드릴조"] = "Fearow",
		["부우부"] = "Hoothoot",
		["야부엉"] = "Noctowl",
		["꼬렛"] = "Rattata",
		["레트라"] = "Raticate",
		["꼬리선"] = "Sentret",
		["다꼬리"] = "Furret",
		["캐터피"] = "Caterpie",
		["단데기"] = "Metapod",
		["버터플"] = "Butterfree",
		["뿔충이"] = "Weedle",
		["딱충이"] = "Kakuna",
		["독침붕"] = "Beedrill",
		["페이검"] = "Spinarak",
		["아리아도스"] = "Ariados",
		["꼬마돌"] = "Geodude",
		["데구리"] = "Graveler",
		["고오스"] = "Gastly",
		["고우스트"] = "Haunter",
		["모다피"] = "Bellsprout",
		["우츠동"] = "Weepinbell",
	};

	// 앞면
	private Dictionary<string, Sprite> frontSprites = new();
	// 뒷면
	private Dictionary<string, Sprite> backSprites = new();

	// 도감번호
	public int Id;
	// 이름
	public string Name;
	// 종족값
	public PokemonStat BaseStat;
	// 타입1
	public Define.PokeType PokeType1;
	// 타입2
	public Define.PokeType PokeType2;
	// 경험치 타입
	public Define.ExpType ExpType;
	// 진화 레벨
	public int EvolveLevel;
	// 진화 포켓몬 이름
	public string EvolveName;
	// 기본 스킬
	public List<string> DefaultSkill;
	// 배우는 스킬 <level, skillName>
	public Dictionary<int, string> SkillDic;
	// 기본 경험치
	public int BaseExp;
	
	// 따라다니는 포켓몬 오브젝트
	Dictionary<string, Follower> fieldPokemon = new();

	public void Init()
	{
		dataById = new Dictionary<int, SJH_PokemonData>()
		{
			[1] = new SJH_PokemonData
			{
				Id = 1,
				Name = "치코리타",
				BaseStat = new PokemonStat(45, 49, 65, 49, 65, 45),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 16,
				EvolveName = "베이리프",
				// 기본 기술
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "울음소리"
				},
				// 스킬 딕셔너리 <int 레벨, string 기술명 or Skill class>
				SkillDic = new Dictionary<int, string>()
				{
					[8] = "잎날가르기",
					[12] = "리플렉터",
					[15] = "독가루",
					[22] = "광합성",
					[29] = "누르기",
					[36] = "빛의장막",
					[43] = "신비의부적",
					[50] = "솔라빔"
				},
				BaseExp = 64,
			},
			[2] = new SJH_PokemonData
			{
				Id = 2,
				Name = "베이리프",
				BaseStat = new PokemonStat(60, 62, 80, 63, 80, 60),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 32,
				EvolveName = "메가니움",
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "울음소리", "잎날가르기", "리플렉터"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[8] = "잎날가르기",
					[12] = "리플렉터",
					[15] = "독가루",
					[23] = "광합성",
					[31] = "누르기",
					[39] = "빛의장막",
					[47] = "신비의부적",
					[55] = "솔라빔"
				},
				BaseExp = 141,
			},
			[3] = new SJH_PokemonData
			{
				Id = 3,
				Name = "메가니움",
				BaseStat = new PokemonStat(80, 82, 100, 83, 100, 80),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "울음소리", "잎날가르기", "리플렉터"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[8] = "잎날가르기",
					[12] = "리플렉터",
					[15] = "독가루",
					[22] = "광합성",
					[31] = "누르기",
					[41] = "빛의장막",
					[51] = "신비의부적",
					[61] = "솔라빔"
				},
				BaseExp = 208,
			},
			[4] = new SJH_PokemonData
			{
				Id = 4,
				Name = "브케인",
				BaseStat = new PokemonStat(39, 52, 43, 60, 50, 65),
				PokeType1 = Define.PokeType.Fire,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 14,
				EvolveName = "마그케인",
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "째려보기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "연막",
					[12] = "불꽃세례",
					[19] = "전광석화",
					[27] = "화염자동차",
					[36] = "스피드스타",
					[46] = "화염방사",
				},
				BaseExp = 65,
			},
			[5] = new SJH_PokemonData
			{
				Id = 5,
				Name = "마그케인",
				BaseStat = new PokemonStat(58, 64, 58, 80, 65, 80),
				PokeType1 = Define.PokeType.Fire,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 36,
				EvolveName = "블레이범",
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "째려보기", "연막"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "연막",
					[12] = "불꽃세례",
					[21] = "전광석화",
					[31] = "화염자동차",
					[42] = "스피드스타",
					[54] = "화염방사",
				},
				BaseExp = 142,
			},
			[6] = new SJH_PokemonData
			{
				Id = 6,
				Name = "블레이범",
				BaseStat = new PokemonStat(78, 84, 78, 109, 85, 100),
				PokeType1 = Define.PokeType.Fire,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "째려보기", "연막", "불꽃세례"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "연막",
					[12] = "불꽃세례",
					[21] = "전광석화",
					[31] = "화염자동차",
					[45] = "스피드스타",
					[60] = "화염방사",
				},
				BaseExp = 209,
			},
			[7] = new SJH_PokemonData
			{
				Id = 7,
				Name = "리아코",
				BaseStat = new PokemonStat(50, 65, 64, 44, 48, 43),
				PokeType1 = Define.PokeType.Water,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 18,
				EvolveName = "엘리게이",
				DefaultSkill = new List<string>()
				{
					"할퀴기", "째려보기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[7] = "분노",
					[13] = "물대포",
					[20] = "물기",
					[27] = "겁나는얼굴",
					[35] = "베어가르기",
					[43] = "싫은소리",
					[52] = "하이드로펌프",
				},
				BaseExp = 66,
			},
			[8] = new SJH_PokemonData
			{
				Id = 8,
				Name = "엘리게이",
				BaseStat = new PokemonStat(65, 80, 80, 59, 63, 58),
				PokeType1 = Define.PokeType.Water,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 30,
				EvolveName = "장크로다일",
				DefaultSkill = new List<string>()
				{
					"할퀴기", "째려보기", "분노"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "분노",
					[12] = "물대포",
					[21] = "물기",
					[28] = "겁나는얼굴",
					[37] = "베어가르기",
					[45] = "싫은소리",
					[55] = "하이드로펌프",
				},
				BaseExp = 143,
			},
			[9] = new SJH_PokemonData
			{
				Id = 9,
				Name = "장크로다일",
				BaseStat = new PokemonStat(85, 105, 100, 79, 83, 78),
				PokeType1 = Define.PokeType.Water,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"할퀴기", "째려보기", "분노", "물대포"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "분노",
					[12] = "물대포",
					[21] = "물기",
					[28] = "겁나는얼굴",
					[38] = "베어가르기",
					[47] = "싫은소리",
					[58] = "하이드로펌프",
				},
				BaseExp = 210,
			},
			[10] = new SJH_PokemonData
			{
				Id = 10,
				Name = "구구",
				BaseStat = new PokemonStat(40, 45, 40, 35, 35, 56),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 18,
				EvolveName = "피죤",
				DefaultSkill = new List<string>()
				{
					"몸통박치기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[5] = "모래뿌리기",
					[9] = "바람일으키기",
					[15] = "전광석화",
					[21] = "날려버리기",
					[29] = "날개치기",
					[37] = "고속이동",
					[47] = "따라하기",
				},
				BaseExp = 55,
			},
			[11] = new SJH_PokemonData
			{
				Id = 11,
				Name = "피죤",
				BaseStat = new PokemonStat(63, 60, 55, 50, 50, 71),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 36,
				EvolveName = "피죤투",
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "모래뿌리기", "바람일으키기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[5] = "모래뿌리기",
					[9] = "바람일으키기",
					[15] = "전광석화",
					[23] = "날려버리기",
					[33] = "날개치기",
					[43] = "고속이동",
					[55] = "따라하기",
				},
				BaseExp = 113,
			},
			[12] = new SJH_PokemonData
			{
				Id = 12,
				Name = "피죤투",
				BaseStat = new PokemonStat(83, 80, 75, 70, 70, 91),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "모래뿌리기", "바람일으키기", "전광석화"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[5] = "모래뿌리기",
					[9] = "바람일으키기",
					[15] = "전광석화",
					[23] = "날려버리기",
					[33] = "날개치기",
					[46] = "고속이동",
					[61] = "따라하기",
				},
				BaseExp = 172,
			},
			[13] = new SJH_PokemonData
			{
				Id = 13,
				Name = "깨비참",
				BaseStat = new PokemonStat(40, 60, 30, 31, 31, 70),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 20,
				EvolveName = "깨비드릴조",
				DefaultSkill = new List<string>()
				{
					"쪼기", "울음소리",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[7] = "째려보기",
					[13] = "마구찌르기",
					[25] = "따라가때리기",
					[31] = "따라하기",
					[37] = "회전부리",
					[43] = "고속이동",
				},
				BaseExp = 58,
			},
			[14] = new SJH_PokemonData
			{
				Id = 14,
				Name = "깨비드릴조",
				BaseStat = new PokemonStat(65, 90, 65, 61, 61, 100),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"쪼기", "울음소리", "째려보기", "마구찌르기"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[9] = "째려보기",
					[13] = "마구찌르기",
					[26] = "따라가때리기",
					[32] = "따라하기",
					[40] = "회전부리",
					[47] = "고속이동",
				},
				BaseExp = 162,
			},
			[15] = new SJH_PokemonData
			{
				Id = 15,
				Name = "부우부",
				BaseStat = new PokemonStat(60, 30, 30, 36, 56, 50),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 20,
				EvolveName = "야부엉",
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "울음소리",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "꿰뚫어보기",
					[11] = "쪼기",
					[16] = "최면술",
					[22] = "리플렉터",
					[28] = "돌진",
					[34] = "염동력",
					[48] = "꿈먹기",
				},
				BaseExp = 58,
			},
			[16] = new SJH_PokemonData
			{
				Id = 16,
				Name = "야부엉",
				BaseStat = new PokemonStat(100, 50, 50, 76, 96, 70),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "울음소리", "꿰뚫어보기", "쪼기"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "꿰뚫어보기",
					[11] = "쪼기",
					[16] = "최면술",
					[25] = "리플렉터",
					[33] = "돌진",
					[41] = "염동력",
					[57] = "꿈먹기",
				},
				BaseExp = 162,
			},
			[17] = new SJH_PokemonData
			{
				Id = 17,
				Name = "꼬렛",
				BaseStat = new PokemonStat(30, 56, 35, 25, 35, 72),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 20,
				EvolveName = "레트라",
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "꼬리흔들기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[7] = "전광석화",
					[13] = "필살앞니",
					[20] = "기충전",
					[27] = "따라가때리기",
					[34] = "분노의앞니",
				},
				BaseExp = 57,
			},
			[18] = new SJH_PokemonData
			{
				Id = 18,
				Name = "레트라",
				BaseStat = new PokemonStat(55, 81, 60, 50, 70, 97),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "꼬리흔들기", "전광석화"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[7] = "전광석화",
					[13] = "필살앞니",
					[20] = "기충전",
					[30] = "따라가때리기",
					[40] = "분노의앞니",
				},
				BaseExp = 116,
			},
			[19] = new SJH_PokemonData
			{
				Id = 19,
				Name = "꼬리선",
				BaseStat = new PokemonStat(35, 46, 34, 35, 45, 20),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 15,
				EvolveName = "다꼬리",
				DefaultSkill = new List<string>()
				{
					"몸통박치기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[5] = "웅크리기",
					[11] = "전광석화",
					[17] = "마구할퀴기",
					[25] = "힘껏치기",
					[33] = "잠자기",
					[41] = "망각술",
				},
				BaseExp = 57,
			},
			[20] = new SJH_PokemonData
			{
				Id = 20,
				Name = "다꼬리",
				BaseStat = new PokemonStat(85, 76, 64, 45, 55, 90),
				PokeType1 = Define.PokeType.Normal,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "웅크리기", "전광석화"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[5] = "웅크리기",
					[11] = "전광석화",
					[18] = "마구할퀴기",
					[28] = "힘껏치기",
					[38] = "잠자기",
					[48] = "망각술",
				},
				BaseExp = 116,
			},
			[24] = new SJH_PokemonData
			{
				Id = 24,
				Name = "캐터피",
				BaseStat = new PokemonStat(45, 30, 35, 20, 20, 45),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 7,
				EvolveName = "단데기",
				DefaultSkill = new List<string>()
				{
					"몸통박치기", "실뿜기"
				},
				SkillDic = new Dictionary<int, string>()
				{

				},
				BaseExp = 53,
			},
			[25] = new SJH_PokemonData
			{
				Id = 25,
				Name = "단데기",
				BaseStat = new PokemonStat(50, 20, 55, 25, 25, 30),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.None,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 10,
				EvolveName = "버터플",
				DefaultSkill = new List<string>()
				{
					"단단해지기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[7] = "단단해지기",
				},
				BaseExp = 72,
			},
			[26] = new SJH_PokemonData
			{
				Id = 26,
				Name = "버터플",
				BaseStat = new PokemonStat(60, 45, 50, 80, 80, 70),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Flying,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"염동력",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[10] = "염동력",
					[13] = "독가루",
					[14] = "저리가루",
					[15] = "수면가루",
					[18] = "초음파",
					[23] = "날려버리기",
					[28] = "바람일으키기",
					[34] = "환상빔",
					[40] = "신비의부적",
				},
				BaseExp = 160,
			},
			[27] = new SJH_PokemonData
			{
				Id = 27,
				Name = "뿔충이",
				BaseStat = new PokemonStat(40, 35, 30, 20, 20, 50),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 7,
				EvolveName = "딱충이",
				DefaultSkill = new List<string>()
				{
					"독침", "실뿜기"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[9] = "벌레먹음",
				},
				BaseExp = 52,
			},
			[28] = new SJH_PokemonData
			{
				Id = 28,
				Name = "딱충이",
				BaseStat = new PokemonStat(45, 25, 50, 25, 25, 35),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = 10,
				EvolveName = "독침붕",
				DefaultSkill = new List<string>()
				{
					"단단해지기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[7] = "단단해지기",
				},
				BaseExp = 71,
			},
			[29] = new SJH_PokemonData
			{
				Id = 29,
				Name = "독침붕",
				BaseStat = new PokemonStat(60, 80, 40, 45, 80, 75),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumFast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"마구찌르기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[10] = "마구찌르기",
					[15] = "기충전",
					[20] = "더블니들",
					[25] = "분노",
					[30] = "따라가때리기",
					[35] = "바늘미사일",
					[40] = "고속이동",
				},
				BaseExp = 159,
			},
			[32] = new SJH_PokemonData
			{
				Id = 32,
				Name = "페이검",
				BaseStat = new PokemonStat(40, 60, 40, 40, 40, 30),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.Fast,
				EvolveLevel = 22,
				EvolveName = "아리아도스",
				DefaultSkill = new List<string>()
				{
					"독침", "실뿜기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "겁나는얼굴",
					[11] = "휘감기",
					[27] = "나이트헤드",
					[23] = "흡혈",
					[30] = "마구할퀴기",
					[37] = "거미집",
					[45] = "싫은소리",
					[53] = "사이코키네시스",
				},
				BaseExp = 54,
			},
			[33] = new SJH_PokemonData
			{
				Id = 33,
				Name = "아리아도스",
				BaseStat = new PokemonStat(70, 90, 70, 60, 70, 40),
				PokeType1 = Define.PokeType.Bug,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.Fast,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"독침", "실뿜기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "겁나는얼굴",
					[11] = "휘감기",
					[17] = "나이트헤드",
					[25] = "흡혈",
					[34] = "마구할퀴기",
					[43] = "거미집",
					[53] = "싫은소리",
					[63] = "사이코키네시스",
				},
				BaseExp = 134,
			},
			[34] = new SJH_PokemonData
			{
				Id = 34,
				Name = "꼬마돌",
				BaseStat = new PokemonStat(40, 80, 100, 30, 30, 20),
				PokeType1 = Define.PokeType.Rock,
				PokeType2 = Define.PokeType.Ground,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 25,
				EvolveName = "데구리",
				DefaultSkill = new List<string>()
				{
					"몸통박치기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "웅크리기",
					[11] = "돌떨구기",
					[16] = "매그니튜드",
					[21] = "자폭",
					[26] = "단단해지기",
					[31] = "구르기",
					[41] = "지진",
					[48] = "대폭발",
				},
				BaseExp = 86,
			},
			[35] = new SJH_PokemonData
			{
				Id = 35,
				Name = "데구리",
				BaseStat = new PokemonStat(55, 95, 115, 45, 45, 35),
				PokeType1 = Define.PokeType.Rock,
				PokeType2 = Define.PokeType.Ground,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"몸통박치기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "웅크리기",
					[11] = "돌떨구기",
					[16] = "매그니튜드",
					[21] = "자폭",
					[27] = "단단해지기",
					[34] = "구르기",
					[41] = "지진",
					[48] = "대폭발",
				},
				BaseExp = 134,
			},
			[58] = new SJH_PokemonData
			{
				Id = 58,
				Name = "고오스",
				BaseStat = new PokemonStat(30, 35, 30, 100, 35, 80),
				PokeType1 = Define.PokeType.Ghost,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 25,
				EvolveName = "고우스트",
				DefaultSkill = new List<string>()
				{
					"최면술", "핥기",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[8] = "원한",
					[13] = "검은눈빛",
					[16] = "저주",
					[21] = "나이트헤드",
					[28] = "이상한빛",
					[33] = "꿈먹기",
					[36] = "길동무",
				},
				BaseExp = 95,
			},
			[59] = new SJH_PokemonData
			{
				Id = 59,
				Name = "고우스트",
				BaseStat = new PokemonStat(45, 50, 45, 115, 55, 95),
				PokeType1 = Define.PokeType.Ghost,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"최면술", "핥기", "원한"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[8] = "원한",
					[13] = "검은눈빛",
					[16] = "저주",
					[21] = "나이트헤드",
					[31] = "이상한빛",
					[39] = "꿈먹기",
					[48] = "길동무",
				},
				BaseExp = 126,
			},
			[64] = new SJH_PokemonData
			{
				Id = 64,
				Name = "모다피",
				BaseStat = new PokemonStat(50, 75, 35, 70, 30, 40),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = 21,
				EvolveName = "우츠동",
				DefaultSkill = new List<string>()
				{
					"덩굴채찍",
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "성장",
					[11] = "김밥말이",
					[15] = "수면가루",
					[17] = "독가루",
					[19] = "저리가루",
					[23] = "용해액",
					[30] = "달콤한향기",
					[37] = "잎날가르기",
					[45] = "힘껏치기",
				},
				BaseExp = 84,
			},
			[65] = new SJH_PokemonData
			{
				Id = 65,
				Name = "우츠동",
				BaseStat = new PokemonStat(65, 90, 50, 85, 45, 55),
				PokeType1 = Define.PokeType.Grass,
				PokeType2 = Define.PokeType.Poison,
				ExpType = Define.ExpType.MediumSlow,
				EvolveLevel = -1,
				EvolveName = null,
				DefaultSkill = new List<string>()
				{
					"덩굴채찍", "성장", "김밥말이"
				},
				SkillDic = new Dictionary<int, string>()
				{
					[6] = "성장",
					[11] = "김밥말이",
					[15] = "수면가루",
					[17] = "독가루",
					[19] = "저리가루",
					[24] = "용해액",
					[33] = "달콤한향기",
					[42] = "잎날가르기",
					[54] = "힘껏치기",
				},
				BaseExp = 151,
			},
		};

		dataByName = new Dictionary<string, SJH_PokemonData>();
		foreach (var data in dataById.Values)
		{
			dataByName[data.Name] = data;
		};
		
		// 앞뒷면 스프라이트 
		GameObject[] prefabs1 = Resources.LoadAll<GameObject>("PokemonFront_Sprites");
		foreach (GameObject prefab in prefabs1)
		{
			// 이름은 프리팹 이름 기준
			string name = pokemonKorName[prefab.name];

			// SpriteRenderer에서 Sprite를 가져옴
			Sprite sprite = prefab.GetComponent<SpriteRenderer>()?.sprite;
			if (sprite != null && !frontSprites.ContainsKey(name))
			{
				frontSprites.Add(name, sprite);
			}
		}
		// 앞뒷면 스프라이트 
		GameObject[] prefabs2 = Resources.LoadAll<GameObject>("PokemonBack_Sprites");
		foreach (GameObject prefab in prefabs2)
		{
			// 이름은 프리팹 이름 기준
			string name = pokemonKorName[prefab.name];

			// SpriteRenderer에서 Sprite를 가져옴
			Sprite sprite = prefab.GetComponent<SpriteRenderer>()?.sprite;
			if (sprite != null && !backSprites.ContainsKey(name))
			{
				backSprites.Add(name, sprite);
			}
		}

		Follower[] followers = Resources.LoadAll<Follower>("PokemonField_Prefabs");
		foreach (Follower prefab in followers)
		{
			string name = prefab.name;

			if (prefab != null && !fieldPokemon.ContainsKey(name))
			{
				fieldPokemon.Add(name, prefab);
			}
		}
	}

	public SJH_PokemonData GetPokemonData(int index)
	{
		var check = dataById[index];	
		if (check == null)
			return null;
		else
			return check;
	}

	public SJH_PokemonData GetPokemonData(string name)
	{
		var check = dataByName[name];
		if (check == null)
			return null;
		else
			return check;
	}

	public Sprite GetBattleFrontSprite(string pokeName)
	{
		var check = frontSprites[pokeName];
		if (check == null)
			return null;
		else
			return check;
	}

	public Sprite GetBattleBackSprite(string pokeName)
	{
		var check = backSprites[pokeName];
		if (check == null)
			return null;
		else
			return check;
	}

	public Follower GetFieldPokemon(string pokeName)
	{
		var check = fieldPokemon[pokeName];
		if (check == null)
			return null;
		else
			return check;
	}
}
