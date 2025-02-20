using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro;
using Newtonsoft.Json.Linq;

public class HistorySicbo : BaseView
{
    // Start is called before the first frame update
    [SerializeField] Image spr_trang, spr_trang_top;
    [SerializeField] TextMeshProUGUI lb_number_small_top, lb_number_big_top, lb_number_small, lb_number_big;
    [SerializeField] protected List<Sprite> listSpriteFrameItemHistory;
    [SerializeField] GameObject tableItemHistory;
    [SerializeField] GameObject initHorizontalUp;
    [SerializeField] GameObject initVerticallUp;
    [SerializeField] ScrollRect scr_result;

    [SerializeField] Vector2 initPosTableItem;

    private int[][] historyArray;
    protected GameObject[][] listItemHistory;
    private int numberSmall = 0, totalBet = 0, numberBig = 0;
    protected int HISTORY_COT = 0, HISTORY_HANG = 0;
    private int HISTORY_OLD_VALUE = -1, currentColUp = 0;
    private List<float> listInitPosXUp = new List<float>();
    private List<float> listInitPosYUp = new List<float>();
    protected HiloView sicboGameView;


    // Update is called once per frame
    void Update()
    {

    }
    public void onStart()
    {
        numberSmall = 0;
        numberBig = 0;
        totalBet = 0;
        resetIndex();
        UIManager.instance.destroyAllChildren(scr_result.content);
        UIManager.instance.destroyAllChildren(tableItemHistory.transform);
        sicboGameView = (HiloView)UIManager.instance.gameView;
        transform.SetAsLastSibling();
    }

    public void resetIndex()
    {
        setup2dArray();
        HISTORY_OLD_VALUE = -1;
        HISTORY_COT = 0;
        HISTORY_HANG = 0;
    }
    public void handleDataHistory(List<List<int>> dataHistory)
    {
        //Globals.Logging.Log("handleDataHistory:" + dataHistory.Count);
        //-------------- HANDLE DATA TAI XIU -----------//
        //dataHistory = new List<List<int>> { new List<int> { 2, 4, 5 } };
        //for (int i = 0; i < 154; i++)
        //{
        //    dataHistory.Add(new List<int> { Random.Range(1, 6), Random.Range(1, 6), Random.Range(1, 6) });
        //}
        Globals.Logging.Log("handleDataHistory:");
        if (initHorizontalUp != null)
        {
            foreach (Transform itemInit in initHorizontalUp.transform)
            {
                listInitPosXUp.Add(itemInit.transform.localPosition.x);
            }
            Destroy(initHorizontalUp);
            initHorizontalUp = null;
        }
        if (initVerticallUp != null)
        {
            foreach (Transform itemInit in initVerticallUp.transform)
            {
                listInitPosYUp.Add(itemInit.transform.localPosition.y);
            }
            Destroy(initVerticallUp);
            initVerticallUp = null;
        }
        int sumResult = 0;
        List<int> arrSumResultHistory = new List<int>();
        for (int i = 0; i < dataHistory.Count; i++)
        {
            List<int> dataHis = dataHistory[i];
            for (int j = 0; j < dataHis.Count; j++)
            {
                int itemDataHis = dataHis[j];
                sumResult += itemDataHis;
            }
            arrSumResultHistory.Add(sumResult);
            sumResult = 0;
        }

        //------------ UPDATE FILL RANGE ---------//
        for (int i = 0; i < arrSumResultHistory.Count; i++)
        {
            int itemData = arrSumResultHistory[i];
            //Globals.Logging.Log("itemData:" + itemData);
            if (itemData == 11) continue;
            if (itemData > 10) numberBig++;
            else numberSmall++;
            totalBet++;
        } 
        if (totalBet != 0)
        {
            float valueFillRangeNumberBig = (float)numberBig / totalBet * 100;
            float valueFillRangeNumberSmall = (float)numberSmall / totalBet * 100;

            spr_trang.fillAmount = valueFillRangeNumberSmall / 100;
            lb_number_big.text = Mathf.Ceil(valueFillRangeNumberBig).ToString() + "%";
            lb_number_small.text = Mathf.Floor(valueFillRangeNumberSmall).ToString() + "%";
        }
        else
        {
            spr_trang.fillAmount = 0;
            lb_number_big.text = "0%";
            lb_number_small.text = "0%";
        }
      

        //----------- CREAT ITEM HISTORY - TOP ----------------//
        for (int i = 0; i < arrSumResultHistory.Count; i++)
        {
            int itemData = arrSumResultHistory[i];
            if (itemData > 10) genIndexHistory(1);
            else genIndexHistory(0);
        }
        listItemHistory = new GameObject[HISTORY_COT + 1][];
        for (int i = 0; i < listItemHistory.Length; i++)
        {
            listItemHistory[i] = new GameObject[5];
        }
        resetIndex();
        for (int i = 0; i < arrSumResultHistory.Count; i++)
        {
            int itemData = arrSumResultHistory[i];
            if (itemData > 10) creatItemHistory(1, itemData);
            else creatItemHistory(0, itemData);
        }
        setPositionItem();
        //----------- CREAT ITEM HISTORY - BOTTOM ----------//
        for (int i = 0; i < arrSumResultHistory.Count; i++)
        {
            int itemData = arrSumResultHistory[i];
            Sprite spr;
            if (itemData == 11) spr = listSpriteFrameItemHistory[2];
            else if (itemData > 10) spr = listSpriteFrameItemHistory[1];
            else spr = listSpriteFrameItemHistory[0];
            Image itemHis = Globals.Config.createSprite(spr);
            itemHis.transform.SetParent(scr_result.content);
            itemHis.transform.localScale = Vector3.one;
            itemHis.transform.localRotation = Quaternion.identity;
        }
    }
    private void setPositionItem()
    {
        int numberBig = 0;
        int numberSmall = 0;
        int totalItem = 0;
        resetIndex();

        for (int i = (listItemHistory.Length >= 20 ? listItemHistory.Length - 20 : 0); i < listItemHistory.Length; i++)
        {
            for (int j = 0; j < listItemHistory[i].Length; j++)
            {
                GameObject itemHis = listItemHistory[i][j];
                if (itemHis == null) continue;
                genIndexHistory(int.Parse(itemHis.tag));
                if (int.Parse(itemHis.gameObject.tag) == 2)
                    totalItem--;
                else if (int.Parse(itemHis.tag) > 0)
                    numberBig++;
                else numberSmall++;
                totalItem++;
                Vector2 vector = convertArrayIndexToPosition(HISTORY_COT, HISTORY_HANG);
                itemHis.transform.localPosition = vector;
            }
        }
        float valueFillRangeNumberBig = 0;
        float valueFillRangeNumberSmall = 0;
        if (totalItem > 0)
        {
            valueFillRangeNumberBig = (float)numberBig / totalItem * 100;
            valueFillRangeNumberSmall = (float)numberSmall / totalItem * 100;
        }
        //----- detam ----/;/
        spr_trang_top.fillAmount = valueFillRangeNumberSmall / 100;
        lb_number_big_top.text = Mathf.Ceil(valueFillRangeNumberBig).ToString() + "%";
        lb_number_small_top.text = Mathf.FloorToInt(valueFillRangeNumberSmall).ToString() + "%";
    }
    private Vector2 convertArrayIndexToPosition(int col, int row)
    {
        if (col > listInitPosXUp.Count - 1)
        {
            col = col - listInitPosXUp.Count;
        }
        if (row > listInitPosYUp.Count - 1)
        {
            row = row - listInitPosYUp.Count;
        }
        Vector2 posDot = new Vector2(listInitPosXUp[col], listInitPosYUp[row]);
        return posDot;
    }
    protected virtual void creatItemHistory(int typeWin, int value)
    {
        //----------- CREAT ITEM HISTORY ------------//

        Sprite spr = listSpriteFrameItemHistory[value == 11 ? 2 : typeWin];
        Image itemHistory = Globals.Config.createSprite(spr);
        itemHistory.transform.SetParent(tableItemHistory.transform);
        itemHistory.transform.localRotation = Quaternion.identity;
        //itemHistory.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        itemHistory.SetNativeSize();
        itemHistory.transform.localScale = Vector3.one;
        itemHistory.transform.localPosition = Vector3.zero;
        TextMeshProUGUI nodeLabel = Globals.Config.createLabel(value.ToString(), 15);
        nodeLabel.alignment = TextAlignmentOptions.Center;
        if (value == 11)
        {
            nodeLabel.color = new Color(255, 255, 255);
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

        nodeLabel.transform.SetParent(itemHistory.transform);
        nodeLabel.transform.localPosition = new Vector2(0, 3);
        nodeLabel.transform.localScale = Vector3.one;
        nodeLabel.transform.localRotation = Quaternion.identity;
        genIndexHistory(typeWin);
        listItemHistory[HISTORY_COT][HISTORY_HANG] = itemHistory.gameObject;
    }
    protected void genIndexHistory(int typeWin)
    { // Hàm này quyết định là có nên tạo layout mới ko , sau đó addchild vào table
        // init item đầu tiên
        //Globals.Logging.Log("genIndexHistory Typewin=" + typeWin);
        if (HISTORY_OLD_VALUE == -1)
        {
            HISTORY_HANG = 0;
            HISTORY_COT = 0;
            HISTORY_OLD_VALUE = typeWin;
            historyArray[HISTORY_HANG][HISTORY_COT] = 1;
        }
        else
        {
            // Kết quả giống ván trước hoặc size cột nhỏ hơn 5
            if (typeWin == HISTORY_OLD_VALUE && HISTORY_HANG < 4)
            {
                HISTORY_HANG++;
                historyArray[HISTORY_HANG][HISTORY_COT] = 1;
                HISTORY_OLD_VALUE = typeWin;
            }
            else
            { // Kết quả không giống ván trước ( đổi cột) hoặc size cột lớn hơn 5
                // Mặc định quay lại thằng đầu tiên trống trên hàng 1
                for (int j = 0; j < historyArray[0].Length; j++)
                {
                    if (historyArray[0][j] == 0)
                    {
                        HISTORY_COT = j;
                        break;
                    }
                }
                HISTORY_HANG = 0;
                HISTORY_OLD_VALUE = typeWin;
                historyArray[HISTORY_HANG][HISTORY_COT] = 1;
            }
        }
    }
    private void setup2dArray()
    {
        historyArray = new int[5][];
        for (int i = 0; i < historyArray.Length; i++)
        {
            historyArray[i] = new int[200];
        }
        for (int j = 0; j < 5; j++)
        {
            for (int k = 0; k < 200; k++)
            {
                historyArray[j][k] = 0;
            }
        }
    }
}
