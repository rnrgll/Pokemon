using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDH_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            UIManager.Instance.ShowLinkedUI<UI_Menu>("UI_Menu");
        }
    }
}
