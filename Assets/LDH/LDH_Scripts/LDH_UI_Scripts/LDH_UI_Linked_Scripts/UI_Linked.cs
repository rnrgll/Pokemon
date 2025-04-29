using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Linked : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Manager.UI.SetCanvas(gameObject,false);
    }
    
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    
}
