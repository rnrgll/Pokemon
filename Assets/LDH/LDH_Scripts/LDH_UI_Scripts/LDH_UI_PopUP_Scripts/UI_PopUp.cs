using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ClosePopupUI();
        }
    }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Manager.UI.SetCanvas(gameObject,true);
    }


public virtual void ClosePopupUI()
    {
        Manager.UI.ClosePopupUI(this);
    }
}
