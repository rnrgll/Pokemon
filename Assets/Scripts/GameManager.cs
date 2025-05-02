using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private Player player;
	public Player Player => player;

	public void SetPlayer(Player player)
	{
		this.player = player;
	}

	public void ReleasePlayer()
	{
		player = null;
	}
}
