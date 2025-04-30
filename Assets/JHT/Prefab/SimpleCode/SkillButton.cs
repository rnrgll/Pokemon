using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
	public PokeSkill pokeSkill;

	public PokeController controller;

	public Image image;

    void Start()
    {
		image.sprite = pokeSkill.icon;
    }

    public void OnClicked()
	{
		controller.ActiveSkill(pokeSkill);
	}

}
