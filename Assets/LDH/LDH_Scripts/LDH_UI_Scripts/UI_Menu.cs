using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu : UI_Linked
{
    private static int _curIdx = 0;
    private int _preIdx = 0;

    [SerializeField] private Transform UI_MenuBox;
    
   [SerializeField] private List<GameObject> menuArrows;
   
    private void Awake()
    {
        foreach (Transform child in UI_MenuBox)
        {
            GameObject arrow = child.GetChild(0). gameObject;
            menuArrows.Add(arrow);
            arrow.SetActive(false);
        }
    }

    void Start()
    {
        UpdateArrow();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveIdx(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
           MoveIdx(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            CloseSelf();
        }
    }

    void MoveIdx(int direction)
    {
        _preIdx = _curIdx;
        _curIdx += direction;
        if (_curIdx < 0)
            _curIdx = menuArrows.Count - 1;
        else if (_curIdx >= menuArrows.Count)
            _curIdx = 0;
        
        UpdateArrow();
    }
    
    void UpdateArrow()
    {
       menuArrows[_preIdx].SetActive(false);
       menuArrows[_curIdx].SetActive(true);
    }

    void CloseSelf()
    {
        UIManager.Instance.UndoLinkedUI();
    }
}
