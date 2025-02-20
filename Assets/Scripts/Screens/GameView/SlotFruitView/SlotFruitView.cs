using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class SlotFruitView : BaseSlotGameView
{
    public static SlotFruitView instance;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SlotFruitView.instance = this;
        Vector2 A = new Vector2(-100, 123);
        Vector2 A1 = new Vector2(345, 0);
        Vector2 B = new Vector2(-123, 200);
        Vector2 B1 = new Vector2(123, 200);
        Vector2 intersectionPoint=Globals.Config.LineLineIntersection(A, A1, B, B1);
    }

    // Update is called once per frame
  
    public override void setStateBtnSpin()
    {
        base.setStateBtnSpin();
        if (gameState==GAME_STATE.SPINNING)
        {
            //
            if (spintype == SPIN_TYPE.AUTO)
            {
                animBtnSpin.startingAnimation = "stop";
            }
            else
            {
                animBtnSpin.color = Color.gray;
            }
           
        }
        else
        {
            animBtnSpin.color = Color.white;
            if (gameState == GAME_STATE.SHOWING_RESULT)
            {
                animBtnSpin.startingAnimation = "stop";
            }
            else if(gameState==GAME_STATE.PREPARE)
            {
                animBtnSpin.startingAnimation = "eng";
            }
        }
        animBtnSpin.Initialize(true);
    }
}
