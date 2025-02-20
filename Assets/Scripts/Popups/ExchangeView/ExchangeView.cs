using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExchangeView : BaseView
{
    public static ExchangeView instance;
    [SerializeField]
    ScrollRect scrContentRedeem, scrContentAgency, scrContentHistory;

    [SerializeField]
    RectTransform trnTabLef;
    [SerializeField]
    RectTransform trnTabTop;

    [SerializeField]
    GameObject tabLeft;

    [SerializeField]
    GameObject tabTop;

    [SerializeField]
    GameObject itemEx;
    [SerializeField]
    GameObject itemAgency;
    [SerializeField]
    GameObject itemHistory;

    JArray dataCO;

    [SerializeField]
    BaseView popupInput;

    [SerializeField]
    TMP_InputField ifPhone, ifPhoneRetype;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        SocketIOManager.getInstance().emitSIOCCCNew(Globals.Config.formatStr("ClickShowExchange_%s", Globals.CURRENT_VIEW.getCurrentSceneName()));
        Globals.CURRENT_VIEW.setCurView(Globals.CURRENT_VIEW.DT_VIEW);
        LoadConfig.instance.getInfoEX(updateInfo);

        //string[] listTabLef = { "Redeem Reward", "Agency", "History" };
        //for (var i = 0; i < listTabLef.Length; i++) {
        //    var btnT = Instantiate(tabLeft);
        //    btnT.transform.parent = trnTabLef;
        //    btnT.transform.localScale = Vector3.one;
        //    btnT.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = listTabLef[i];
        //    btnT.GetComponent<Button>().onClick.AddListener(() => {
        //        onClickTabLeft(btnT.gameObject);
        //    });
        //}

        //if (trnTabLef.childCount > 0) {
        //    onClickTabLeft(trnTabLef.GetChild(0).gameObject);
        //}
    }


    void updateInfo(string strData)
    {
        Globals.Logging.Log("updateInfo EX   " + strData);
        //[{ "title":"Truemoney","type":"phil","child":[{ "title":"truemoney","TypeName":"truemoney","title_img":"https://cdn.topbangkokclub.com/api/public/dl/VbfRjo1c/co/Truemoney.png","textBox":[{ "key_placeHolder":"txt_enter_text_gc"},{ "key_placeHolder":"txt_conf_text_gc"}]}],"items":[{ "ag":1000000,"m":50},{ "ag":2000000,"m":100},{ "ag":4000000,"m":200},{ "ag":10000000,"m":500},{ "ag":20000000,"m":1000},{ "ag":40000000,"m":2000},{ "ag":100000000,"m":5000},{ "ag":200000000,"m":10000}]}]
        dataCO = JArray.Parse(strData);

        genTabLeft();
    }

    void genTabLeft()
    {
        if (dataCO.Count <= 0)
        {
            UIManager.instance.destroyAllChildren(trnTabLef);
            return;
        }

        var checkListCO = false;
        JObject objData = null;
        Globals.Logging.Log("dataCO.Count   " + dataCO.Count);
        for (var i = 0; i < dataCO.Count; i++)
        {
            JObject objDataItem = (JObject)dataCO[i];
            if (i == 0)
            {
                objData = objDataItem;
            }
            GameObject btnT = null;
            if (i < trnTabLef.childCount)
            {
                btnT = trnTabLef.GetChild(i).gameObject;
            }
            else
            {
                btnT = Instantiate(tabLeft, trnTabLef);
            }
            btnT.transform.SetParent(trnTabLef);
            btnT.transform.localScale = Vector3.one;
            btnT.transform.GetComponentInChildren<TextMeshProUGUI>().text = ((string)objDataItem["title"]).ToUpper();
            btnT.GetComponent<Button>().onClick.AddListener(() =>
            {
                onClickTabLeft(btnT.gameObject, objDataItem);
            });

            if (!((string)objDataItem["type"]).Equals("agency"))
            {
                checkListCO = true;
            }
        }

        if (checkListCO)
        {
            var btnT = Instantiate(tabLeft);
            btnT.transform.SetParent(trnTabLef);
            btnT.transform.localScale = Vector3.one;
            btnT.transform.GetComponentInChildren<TextMeshProUGUI>().text = Globals.Config.getTextConfig("history").ToUpper();
            btnT.GetComponent<Button>().onClick.AddListener(() =>
            {
                onClickTabLeft(btnT.gameObject, null);
            });
        }

        if (trnTabLef.childCount > 0)
        {
            onClickTabLeft(trnTabLef.GetChild(0).gameObject, objData);
        }
    }

    void onClickTabLeft(GameObject obj, JObject objDataItem)
    {
        SoundManager.instance.soundClick();
        for (var i = 0; i < trnTabLef.childCount; i++)
        {
            GameObject tb = trnTabLef.GetChild(i).gameObject;
            if (tb == obj)
            {
                tb.transform.GetChild(0).gameObject.SetActive(true);
                if (objDataItem == null && i >= trnTabLef.childCount - 1)
                {
                    scrContentRedeem.transform.parent.gameObject.SetActive(false);
                    scrContentAgency.transform.parent.gameObject.SetActive(false);
                    scrContentHistory.transform.parent.gameObject.SetActive(true);
                    SocketSend.sendDTHistory();
                    continue;
                }

                typeNet = (string)objDataItem["type"];
                scrContentRedeem.transform.parent.gameObject.SetActive(!typeNet.Equals("agency"));
                scrContentAgency.transform.parent.gameObject.SetActive(!scrContentRedeem.gameObject.activeInHierarchy);
                scrContentHistory.transform.parent.gameObject.SetActive(false);
                reloadListItem(objDataItem);
            }
            else
            {
                tb.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    void reloadListItem(JObject objDataItem)
    {
        if (objDataItem != null)
        {
            //[{ "title":"Truemoney","type":"phil","child":[{ "title":"truemoney","TypeName":"truemoney","title_img":"https://storage.googleapis.com/cdn.topbangkokclub.com/shop/Truemoney.png?v=1","textBox":[{ "key_placeHolder":"txt_enter_text_gc"},{ "key_placeHolder":"txt_conf_text_gc"}]}],"items":[{ "ag":1000000,"m":50},{ "ag":2000000,"m":100},{ "ag":4000000,"m":200},{ "ag":10000000,"m":500},{ "ag":20000000,"m":1000},{ "ag":40000000,"m":2000},{ "ag":100000000,"m":5000},{ "ag":200000000,"m":10000}]},{ "type":"agency","title":"agency","items":[{ "id":"1862315","name":"Agency Jason","tel":"09396196724","msg_fb":"http://bit.ly/jason-agency"}]}]
            JArray items;
            Transform parent;
            Globals.Logging.Log("type  " + objDataItem["type"]);

            if (((string)objDataItem["type"]).Equals("agency"))
            {
                items = (JArray)objDataItem["items"];
                parent = scrContentAgency.content;
            }
            else
            {
                items = (JArray)objDataItem["items"];
                parent = scrContentRedeem.content;
            }

            for (var i = 0; i < items.Count; i++)
            {
                JObject dt = (JObject)items[i];
                GameObject item;
                if (i < parent.childCount)
                {
                    item = parent.GetChild(i).gameObject;
                }
                else
                {
                    item = Instantiate(((string)objDataItem["type"]).Equals("agency") ? itemAgency : itemEx);
                }
                if (((string)objDataItem["type"]).Equals("agency"))
                {
                    item.GetComponent<ItemAgency>().setInfo(dt);
                }
                else
                {
                    item.GetComponent<ItemEx>().setInfo(dt, () =>
                    {
                        onChooseCashOut((int)dt["ag"], (int)dt["m"]);
                    });
                }

                item.gameObject.SetActive(true);
                item.transform.SetParent(parent);
                item.transform.localScale = Vector3.one;
            }

            for (var i = items.Count; i < parent.childCount; i++)
            {
                parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void reloadListItemHistory(JArray listItem)
    {
        for (var i = 0; i < listItem.Count; i++)
        {
            GameObject objItem;
            if (i < scrContentHistory.content.childCount)
            {
                objItem = scrContentHistory.content.GetChild(i).gameObject;
            }
            else
            {
                objItem = Instantiate(itemHistory);
            }

            objItem.transform.SetParent(scrContentHistory.content);
            objItem.transform.localScale = Vector3.one;
            var chip = 0;
            for (var j = 0; j < dataCO.Count; j++)
            {
               
                if (!((string)dataCO[j]["type"]).Equals("agency"))
                {
                    JArray itt = (JArray)dataCO[j]["items"];
                  
                    for (var k = 0; k < itt.Count; k++)
                    {
                      
                        if ((int)listItem[i]["CashValue"] == (int)itt[k]["m"])
                        {
                            chip = (int)itt[k]["ag"];
                            break;
                        }
                    }
                }
            }
            
            objItem.GetComponent<ItemHistoryEx>().setInfo((JObject)listItem[i], chip);
        }
    }

    int valueCO;
    string typeNet;
    void onChooseCashOut(int ag, int value)
    {
        SoundManager.instance.soundClick();
        if (Globals.User.userMain.AG < ag)
        {
            UIManager.instance.showMessageBox(Globals.Config.getTextConfig("txt_koduchip"));
        }
        else
        {
            //openInput();
            popupInput.show();
        }

        valueCO = value;
    }

    public void onConfirmCashOut()
    {
        SoundManager.instance.soundClick();
        //require('SMLSocketIO').getInstance().emitSIOCCC(cc.js.formatStr("onConfirmCashOut_%s", require('GameManager').getInstance().getCurrentSceneName()));
        var value = valueCO;
        var typeName = typeNet;

        var phoneNumber = ifPhone.text;
        var phoneNumberRetype = ifPhoneRetype.text;

        if (phoneNumber.Equals("") || phoneNumberRetype.Equals(""))
        {
            UIManager.instance.showMessageBox(Globals.Config.formatStr(Globals.Config.getTextConfig("txt_notEmty"), typeNet.Equals("Mobile") ? Globals.Config.getTextConfig("txt_phone_numnber") : Globals.Config.getTextConfig("txt_truemoney_id"), ""));
        }
        else if (!phoneNumber.Equals(phoneNumberRetype))
        {
            UIManager.instance.showMessageBox(Globals.Config.formatStr(Globals.Config.getTextConfig("txt_notSame"), typeNet.Equals("Mobile") ? Globals.Config.getTextConfig("txt_phone_numnber") : Globals.Config.getTextConfig("txt_truemoney_id")));
        }
        else
        {
            ifPhone.text = "";
            ifPhoneRetype.text = "";
            SocketSend.sendCashOut(value, phoneNumber, typeName);
            UIManager.instance.showWatting();
        }

    }

    public void cashOutReturn(JObject data)
    {
        Globals.Logging.Log("-=-=-=-=cashOutReturn  " + data.ToString());
        if ((bool)data["status"])
        {
            ifPhone.text = "";
            ifPhoneRetype.text = "";
            SocketSend.sendUAG();
            //          require("UIManager").instance.onShowConfirmDialog(data.data);

            //          // Log Tracking
            //          try
            //          {
            //              var log = {
            //                  select_content: EVENT_NAME.CASH_OUT,
            //			data:
            //              {
            //              A: this.valueCO

            //                  }
            //          }
            //              // require("Util").sendLogEventFirebase(JSON.stringify(log));
            //      } catch (error)
            //      {
            //          cc.log("log event err ", error);
            //}
        }
        //else
        //{
        UIManager.instance.showMessageBox((string)data["data"]);
        //}
    }
}
