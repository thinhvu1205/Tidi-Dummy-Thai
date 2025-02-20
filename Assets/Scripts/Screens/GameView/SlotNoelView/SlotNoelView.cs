using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Spine.Unity;
public class SlotNoelView : BaseSlotGameView
{
    public static SlotNoelView instance;
    public override void setStateBtnSpin()
    {
        base.setStateBtnSpin();
        Globals.Logging.Log("setStateBtnSpin:" + gameState);
        if (gameState == GAME_STATE.SPINNING)
        {
            if (spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL)
            {
                animBtnSpin.startingAnimation = "freespin my";
                //animBtnSpin.color = Color.gray;
            }
            else if (spintype == SPIN_TYPE.AUTO)
            {
                animBtnSpin.startingAnimation = "stop my";
            }
            else
            {
                animBtnSpin.startingAnimation = "spin my";
                //animBtnSpin.color = Color.gray;
            }
        }
        else
        {
            //animBtnSpin.color = Color.white;
            if (gameState == GAME_STATE.SHOWING_RESULT)
            {
                if (spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL)
                {
                    animBtnSpin.startingAnimation = "freespin my";
                }
                else
                {
                    if (spintype == SPIN_TYPE.AUTO)
                    {
                        animBtnSpin.startingAnimation = "stop my";
                    }
                    else
                    {
                        animBtnSpin.startingAnimation = "spin my";
                    }
                }
            }
            else if (gameState == GAME_STATE.PREPARE|| gameState == GAME_STATE.JOIN_GAME)
            {
                animBtnSpin.startingAnimation = "spin my";
                if (listBetRoom.Count == 0)
                {
                    //animBtnSpin.color = Color.gray;
                }
                else if (agPlayer < totalListBetRoom[currentMarkBet]) //het cmn tien roi.an di.
                {
                    //animBtnSpin.color = Color.gray;
                }
                if (spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL)
                {
                    animBtnSpin.startingAnimation = "freespin my";
                    //animBtnSpin.color = Color.white;
                }
            }
        }
        animBtnSpin.Initialize(true);
    }
    public override void showAnimChipBay()
    {
        Transform transFrom = lbChipWins.transform;
        Transform transTo = lbCurrentChips.transform.parent.Find("icChip").transform;
        coinFly(transFrom, transTo);
    }
}
