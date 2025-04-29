using System.Collections.Generic;
using UnityEngine;

public class NPCAnimations : MonoBehaviour
{
	[SerializeField] List<Sprite> NpcUp;
	[SerializeField] List<Sprite> NpcDown;
	[SerializeField] List<Sprite> NpcRight;
	[SerializeField] List<Sprite> NpcLeft;
	[SerializeField] List<Sprite> NpcRound;

	// Parameters
	public float MoveX { get; set; }
	public float MoveY { get; set; }
	public bool IsNpcMoving { get; set; }

	// States
	SpriteAnimator npcUpAnim;
	SpriteAnimator npcDownAnim;
	SpriteAnimator npcRightAnim;
	SpriteAnimator npcLeftAnim;
	SpriteAnimator NpcRoundAnim;

	SpriteAnimator currentAnim;
	bool isNpcPrevMove;
	//References
	SpriteRenderer spriteRenderer;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		npcUpAnim = new SpriteAnimator(NpcUp, spriteRenderer);
		npcDownAnim = new SpriteAnimator(NpcDown, spriteRenderer);
		npcRightAnim = new SpriteAnimator(NpcRight, spriteRenderer);
		npcLeftAnim = new SpriteAnimator(NpcLeft, spriteRenderer);
		NpcRoundAnim = new SpriteAnimator(NpcRound, spriteRenderer);

		// 아래방향 기본값 설정
		currentAnim = npcDownAnim;
	}

	private void Update()
	{
		var prevNpcAnim = currentAnim;

		if (MoveX == 1)
			currentAnim = npcRightAnim;
		else if (MoveX == -1)
			currentAnim = npcLeftAnim;
		else if (MoveY == 1)
			currentAnim = npcUpAnim;
		else if (MoveY == -1)
			currentAnim = npcDownAnim;

		if(currentAnim != prevNpcAnim || isNpcPrevMove) 
			currentAnim.Start();

		if (IsNpcMoving)
			currentAnim.HandleUpdate();
		else
			spriteRenderer.sprite = currentAnim.NpcFrames[0];

		isNpcPrevMove = IsNpcMoving;
	}
}
