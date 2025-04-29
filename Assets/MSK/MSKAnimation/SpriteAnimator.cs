using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator 
{  
	SpriteRenderer spriteRenderer;
	List<Sprite> frames;
	float frameRate;

	int currnetFrame;

	float animTimer;

	public SpriteAnimator(List<Sprite> frames, SpriteRenderer spriteRenderer, float frameRate = 0.2f)
	{
		this.frames = frames;
		this.spriteRenderer = spriteRenderer;
		this.frameRate = frameRate;
	}
	public List<Sprite> NpcFrames
	{
		get { return frames; }
	}


	public void Start()
	{
		currnetFrame = 0;
		animTimer = 0;
		spriteRenderer.sprite = frames[0];
	}

	public void HandleUpdate()
	{
		//	프레임을 확인, 보정
		animTimer += Time.deltaTime;
		if (animTimer > frameRate)
		{
			currnetFrame = (currnetFrame + 1) % frames.Count;
			spriteRenderer.sprite = frames[currnetFrame];
			animTimer -= frameRate;
		}
	}

}
