using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSKPCcontroller : MonoBehaviour
{
    Animator anim;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        { 
        }
    }
    void PCRaycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 2f))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
        }
        else
        {
            Debug.Log("감지 없음");
        }

    }

    //	플레이어 진행 방향에 레이어를 확인
    //	인터랙트, 오브젝트 레이어가 존재할 시 이동 불가능
    bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(transform.position, LayerMask.GetMask("ObjectLayer") | LayerMask.GetMask("InteractableLayer")) != null)
        {
            return false;
        }
        return true;
    }
     

}
