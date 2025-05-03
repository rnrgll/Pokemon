using UnityEngine;

public class NpcMover : MonoBehaviour
{
	//	NPC이동 루틴
	[SerializeField] NpcMovVector2 npcMov;

	//	이동 거리
	[SerializeField] float npcSpeed = 2;
	//	이동 시간
	[SerializeField] float moveDirection;
	//	이동의 여부
	bool npcMoving;
	//	이동이 가능한지 여부
	bool isWalkAble;
	//	NPC방향, 위치
	public Vector2 currentDirection;
	private Vector2 npcPos;
	Animator anim;
	private void Awake()
	{   // 초기 방향 아래
		currentDirection = Vector2.down;
		npcPos = this.transform.position;
		anim = GetComponent<Animator>();
	}
	private void FixedUpdate()
	{
		NpcMoveStart();
	}
	public void NpcMoveStart()
	{
		//대화를 진행중인 Npc가 아니라면
		if (Manager.Dialog.isTyping != true)
		{
			npcMoving = true;
			Debug.Log(this.name);
			IsWalkAble(currentDirection);
			if (isWalkAble)
			{
				//	StartCoroutine(NpcMoving(npcMov));
			}
		}
		else
		{
			npcMoving = false;
		}
	}

	//public IEnumerator NpcMoving(NpcMovVector2 vector)
	//{
	//		리스트 벡터2에 있는 값대로 움직이기
	//		리스트 카운트만큼 반복하기
	//		리스트[0]에 있는 벡터대로 움직이기
	//		리스트 카운트 ++
	//		일정 시간 대기
	//}

	// npc 전방 확인
	private bool IsWalkAble(Vector2 currentDirection)
	{
		//  npcPos에서 NpcDir로 레이케스트 발사
		Debug.DrawRay(npcPos + currentDirection * 2f, currentDirection, Color.red);
		//	Npc위치 + 방향에서 발사, 방향으로 1f만큼 발사
		RaycastHit2D hit = Physics2D.Raycast(npcPos + currentDirection * 1.1f, currentDirection, 1f);
		if (hit.collider != null)
			Debug.Log($"{hit.transform.name}에 명중");
		else
			Debug.Log($"명중 없음");

		//	hit.Tag 검사후 이동 가능 여부 판단
		if (hit.transform.gameObject.transform.tag == "Wall" || hit.transform.gameObject.transform.tag == "NPC" || hit.transform.gameObject.transform.tag == "Player")
			isWalkAble = false;
		else
			isWalkAble = true;

		return isWalkAble;
	}
	//	Raycast 방향
	public void DirChange()
	{	
	//	if ("Npc가 위를 보면") currentDirection = Vector2.up;
	//	else if ("Npc가 아래를 보면") currentDirection = Vector2.down;
	//	else if ("Npc가 왼쪽을 보면") currentDirection = Vector2.left;
	//	else if ("Npc가 오른쪽을 보면") currentDirection = Vector2.right;
	}


}
