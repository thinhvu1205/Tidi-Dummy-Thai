using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HistorySicbo2 : HistorySicbo
{
    //protected HiloView2 sicboGameView;
    // Start is called before the first frame update
    protected override void creatItemHistory(int typeWin, int value)
    {

        //----------- CREAT ITEM HISTORY ------------//
        TextMeshProUGUI nodeLabel = Globals.Config.createLabel(value.ToString(), 15);
        Image itemHistory = Globals.Config.createSprite(listSpriteFrameItemHistory[typeWin], nodeLabel.transform);
        itemHistory.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
        itemHistory.transform.localScale = Vector2.one;
        //label.lineHeight = 20;
        nodeLabel.alignment = TextAlignmentOptions.Center;
        if (value == 11)
        {
            nodeLabel.color = new Color(1, 1, 1);
            itemHistory.gameObject.tag = "2";
        }
        else if (value > 10)
        {
            nodeLabel.color = new Color(255, 255, 255);
            itemHistory.gameObject.tag = "1";

        }
        else
        {
            nodeLabel.color = new Color(0, 0, 0);
            itemHistory.gameObject.tag = "0";
        }

        nodeLabel.transform.localPosition = new Vector2(15, -15);
        //itemHistory.addChild(nodeLabel);
        nodeLabel.transform.SetParent(itemHistory.transform);
        genIndexHistory(typeWin);
        //Globals.Logging.Log("listItemHistory:" + listItemHistory.Length);
        //Globals.Logging.Log("HISTORY_COT:" + HISTORY_COT);
        //Globals.Logging.Log("listItemHistory[HISTORY_COT]=" + listItemHistory[HISTORY_COT].Length);
        //Globals.Logging.Log("HISTORY_HANG:" + HISTORY_HANG);
        listItemHistory[HISTORY_COT][HISTORY_HANG] = itemHistory.gameObject;
    }
}
