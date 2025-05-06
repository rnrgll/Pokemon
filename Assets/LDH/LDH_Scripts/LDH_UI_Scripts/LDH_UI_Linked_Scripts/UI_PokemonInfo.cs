using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_PokemonInfo : UI_Linked
{
		public Pokémon pokemon;
		
		[SerializeField] private GameObject[] infoPanels;
		[SerializeField] private int curIdx;

		[Header("right panel UI")]
		[SerializeField] private TMP_Text number;
		[SerializeField] private Image pokemonImg;
		[SerializeField] private TMP_Text pokemonName;
		[SerializeField] private TMP_Text level;
		[SerializeField] private TMP_Text pageNum;

		[Header("panel 1 UI")] [SerializeField]
		private Slider hpSlider;

		[SerializeField] private TMP_Text statusText;
		[SerializeField] private TMP_Text type1Text;
		[SerializeField] private TMP_Text type2Text;
		[SerializeField] private TMP_Text curExpText;
		[SerializeField] private TMP_Text offsetExpText;
		[SerializeField] private Slider expSlider;


		[Header("panel 2 UI")] [SerializeField]
		private Transform skillListRoot;
		
		
		[Header("panel 3 UI")]
		[SerializeField] private TMP_Text playerId;
		[SerializeField] private TMP_Text ownerInfo;
		[SerializeField] private TMP_Text attack;
		[SerializeField] private TMP_Text defense;
		[SerializeField] private TMP_Text speAttack;
		[SerializeField] private TMP_Text speDefense;
		[SerializeField] private TMP_Text speed;

		private void Awake()
		{
			if (infoPanels == null || infoPanels.Length == 0)
			{
				infoPanels = new GameObject[3];
				for (int i = 0; i < 3; i++)
				{
					infoPanels[i] = transform.GetChild(i + 1).gameObject;
				}

			}

			curIdx = 0;
		}
		
		private void OnEnable()
		{
			if (pokemon == null) return;

			UpdateRightInfoData();
			UpdateInfoPanels();
		}

		public override void HandleInput(Define.UIInputType inputType)
		{
			switch (inputType)
			{
				case Define.UIInputType.Left:
					curIdx = Mathf.Clamp(curIdx - 1, 0, infoPanels.Length - 1);
					UpdateInfoPanels();
					break;
				case Define.UIInputType.Right:
					curIdx = Mathf.Clamp(curIdx + 1, 0, infoPanels.Length - 1);
					UpdateInfoPanels();
					break;
				case Define.UIInputType.Select:
					OnSelect();
					break;
				case Define.UIInputType.Cancel:
					OnCancel();
					break;
			}

		}
		
		private void UpdateInfoPanels()
		{
			pageNum.text = $"< {curIdx + 1} / {infoPanels.Length} >";
			for (int i = 0; i < infoPanels.Length; i++)
			{
				infoPanels[i].SetActive(i == curIdx);
			}

			switch (curIdx)
			{
				case 0:
					int hp = pokemon.hp;
					int maxHp = pokemon.maxHp;
					int exp = pokemon.curExp;
					int nextExp = pokemon.nextExp;

					hpSlider.maxValue = maxHp;
					hpSlider.value = hp;

					expSlider.maxValue = exp + nextExp;
					expSlider.value = exp;

					curExpText.text = exp.ToString();
					offsetExpText.text = nextExp.ToString();

					//statusText.text = pokemon.condition.ToString();
					//type1Text.text = pokemon.pokeType1.ToString();
					//type2Text.text = pokemon.pokeType2.ToString();
					statusText.text = Define.GetKoreanState[pokemon.condition]; // 한글로 변환
					type1Text.text = Define.GetKoreanPokeType[pokemon.pokeType1];
					type2Text.text = Define.GetKoreanPokeType[pokemon.pokeType2];
					if(pokemon.pokeType2==Define.PokeType.None)
						type2Text.gameObject.SetActive(false);
					
					break;
				case 1:
					for (int i = 0; i < 4; i++)
					{
						if (i < pokemon.skills.Count)
						{
							//ui 반영
							SkillData skillData = pokemon.skillDatas[i];
							string skillName = skillData.Name;
							int skillCurPP = skillData.CurPP;
							int skillMaxPP = skillData.MaxPP;
							Transform skillSlot = skillListRoot.GetChild(i);
							skillSlot.GetChild(0).GetComponent<TMP_Text>().text = skillName;
							SkillS skillSData = Manager.Data.SkillSData.GetSkillDataByName(skillName);
							//todo : current pp 값 가져올 수 있으면 수정하기 일단 max/max로 함
							
							TMP_Text ppText = skillSlot.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
							if (skillSData != null)
								ppText.text =
									$"{skillCurPP} / {skillMaxPP}";
							else
							{
								ppText.text = "스킬데이터없음";
							}
						}
						else
						{
							skillListRoot.GetChild(i).gameObject.SetActive(false);
						}
					}
					break;
				case 2:
					playerId.text = $"IDNo. {Manager.Data.PlayerData.PlayerID}";
					ownerInfo.text = $"소유자/{Manager.Data.PlayerData.PlayerName}";
					PokemonStat pokemonStat = pokemon.pokemonStat;
					attack.text = pokemonStat.attack.ToString();
					speAttack.text = pokemonStat.speAttack.ToString();
					defense.text = pokemonStat.defense.ToString();
					speDefense.text = pokemonStat.speDefense.ToString();
					speed.text = pokemonStat.speed.ToString();
					break;
			}
		}

		private void UpdateRightInfoData()
		{
			//todo: 포켓몬 이미지빼고 정보 반영. 이미지 반영 추가해야함
			number.text =  $"No. {pokemon.id}";
			level.text = $":L{pokemon.level}";
			pokemonName.text = pokemon.pokeName;
			
		}


		public void SetPokemonInfo(Pokémon pokémon)
		{
			pokemon = pokémon; //선택한 포켓몬(플레이 보유한)
			
		}


	}
