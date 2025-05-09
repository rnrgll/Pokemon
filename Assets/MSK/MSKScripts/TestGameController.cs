using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum GameState { FreeMove, Dialog }
public class TestGameController : MonoBehaviour
{
    [SerializeField] Player playerController;

    GameState state;
    private void Start() { 

        DialogManager.Instance.OnShowDialog += () => {
            state = GameState.Dialog;
        };
        DialogManager.Instance.CloseDialog += () => {
            if (state == GameState.Dialog)
            {
                state = GameState.FreeMove;
            }
        };
    }
    private void Update()
    {
        if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        } else if(state == GameState.FreeMove)
        {
        }   
    }
}
