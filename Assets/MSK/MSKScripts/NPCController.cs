using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;
    public void Interact() 
    {   // 상호작용 시 플레이어를 바라보는 로직을 추가
        Debug.Log("NPC Interact success");
        DialogManager.Instance.ShowText(dialog);
    }

}
