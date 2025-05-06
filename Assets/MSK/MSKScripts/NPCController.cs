using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
	[SerializeField] Dialog dialog;
	[SerializeField] PokeEvent pokeEvent;
	private NpcMover npcMover;
	private Vector2 npcPos;
	public Animator anim;

	private void Awake()
	{
		pokeEvent = GetComponent<PokeEvent>();
		anim = GetComponent<Animator>();
		npcMover = GetComponent<NpcMover>();
	}
	public void Interact(Vector2 position)
	{
		// Npc위치 현재 위치로 갱신

		npcPos = transform.position;
		npcMover.AnimChange(position);

		if (npcMover != null)
		{
			npcMover.isPaused = true;
			npcMover.isNPCTurnCheck = false;
			npcMover.StopMoving();
		}
		anim.SetBool("npcMoving", false);

		if (pokeEvent != null)
		{
			Debug.Log("이벤트 대화 실행");
			pokeEvent.OnPokeEvent(gameObject);
		}
		else if (!Manager.Dialog.isTyping)
		{

			Manager.Dialog.npcState = Define.NpcState.Talking;
			Manager.Dialog.StartDialogue(dialog);
		}
	}

}
