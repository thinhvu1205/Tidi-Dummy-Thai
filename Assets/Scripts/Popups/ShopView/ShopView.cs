using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopView : BaseView
{
    public static ShopView instance = null;
    [SerializeField]
    ScrollRect scrTabs;
    [SerializeField]
    ScrollRect scrContent;

    [SerializeField]
    Sprite spTab;


    [SerializeField]
    GameObject btnTab;

    [SerializeField]
    GameObject itemShop;

    [SerializeField]
    TextMeshProUGUI txt_best;

    JObject itemBest = null;

    [SerializeField]
    BaseView inputView;

    [SerializeField]
    List<TMP_InputField> listEdb;
    [SerializeField]
    Button btnConfirmInput;

    private IAPManager iapManager = null;
    [SerializeField]
    GameObject btnClose;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    private string previousView = "";
    //new void Start()
    //{
    //    base.Start();
    //  
    //    LoadConfig.instance.getInfoShop(updateInfo);
    //}

    bool isTab = false;

    string dataDefault = "[{\"type\":\"iap\",\"title\":\"iap\",\"title_img\":\"https://storage.googleapis.com/cdn.davaogames.com/img/shop/IAPAND.png\",\"items\":[{\"url\":\"dummy.co.slots.1\",\"txtPromo\":\"1USD=392,727Chips\",\"txtChip\":\"388,800Chips\",\"txtBuy\":\"0.99USD\",\"txtBonus\":\"0%\",\"cost\":1},{\"url\":\"dummy.co.slots.2\",\"txtPromo\":\"1USD=390,754Chips\",\"txtChip\":\"777,600Chips\",\"txtBuy\":\"1.99USD\",\"txtBonus\":\"0%\",\"cost\":2},{\"url\":\"dummy.co.slots.5\",\"txtPromo\":\"1USD=389,579Chips\",\"txtChip\":\"1,944,000Chips\",\"txtBuy\":\"4.99USD\",\"txtBonus\":\"0%\",\"cost\":5},{\"url\":\"dummy.co.slots.10\",\"txtPromo\":\"1USD=486,486Chips\",\"txtChip\":\"4,860,000Chips\",\"txtBuy\":\"9.99USD\",\"txtBonus\":\"25%\",\"cost\":10},{\"url\":\"dummy.co.slots.20\",\"txtPromo\":\"1USD=486,243Chips\",\"txtChip\":\"9,720,000Chips\",\"txtBuy\":\"19.99USD\",\"txtBonus\":\"25%\",\"cost\":20},{\"url\":\"dummy.co.slots.50\",\"txtPromo\":\"1USD=486,097Chips\",\"txtChip\":\"24,300,000Chips\",\"txtBuy\":\"49.99USD\",\"txtBonus\":\"25%\",\"cost\":50}]}]";

    public void init(bool _isTab)
    {
        previousView = Globals.CURRENT_VIEW.getCurrentSceneName();
        if (UIManager.instance.gameView == null)
        {
            Globals.CURRENT_VIEW.setCurView(Globals.CURRENT_VIEW.PAYMENT);
            SocketIOManager.getInstance().emitSIOCCCNew(Globals.Config.formatStr("ClickShop_%s", Globals.CURRENT_VIEW.getCurrentSceneName()));
        }
        instance = this;
        LoadConfig.instance.getInfoShop(updateInfo, () =>
        {
            updateInfo(dataDefault);
        });
        isTab = _isTab;
        btnClose.SetActive(!isTab);

        scrContent.onValueChanged.AddListener(onDragScroll);
        //var rectTransform = scrContent.gameObject.GetComponent<RectTransform>();
        //rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, isTab ? 125 : 0);
        //scrContent
    }


    //// Update is called once per frame
    //void Update()
    //{

    //}

    void updateInfo(string strData)
    {
        Globals.Logging.Log("updateInfo shop   " + strData);
        if (strData == "")
        {
            updateInfo(dataDefault);
            return;
        }
        JArray arrayData = JArray.Parse(strData);
        if (arrayData.Count <= 0)
        {
            updateInfo(dataDefault);
            return;
        }

        UIManager.instance.destroyAllChildren(scrTabs.content.transform);
        UIManager.instance.destroyAllChildren(scrContent.content.transform);


        var rec = scrContent.GetComponent<RectTransform>();
        if (arrayData.Count == 1)
        {
            rec.offsetMax = new Vector2(rec.offsetMax.x, -238);
            scrTabs.gameObject.SetActive(false);
            reloadListContent((JObject)arrayData[0]);
            getBest((JObject)arrayData[0]);
            iapManager = new IAPManager((JObject)arrayData[0]);
            return;
        }

        rec.offsetMax = new Vector2(rec.offsetMax.x, -340);
        scrTabs.gameObject.SetActive(true);
        JObject item0 = null;
        var indSelect = 0;
        for (var i = 0; i < arrayData.Count; i++)
        {
            JObject obItem = (JObject)arrayData[i];

            if (i == 0) item0 = obItem;
            Globals.Logging.Log(obItem);
            string title = (string)obItem["title"];
            string title_img = (string)obItem["title_img"];

            if (title.Equals("truemoney"))
            {
                indSelect = i;
                item0 = obItem;
            }
            else if (title.Equals("iap") && iapManager == null)
            {
                Debug.Log("-=-= new IAPManager  ");
                iapManager = new IAPManager(obItem);
            }
            GameObject btn = Instantiate(btnTab);

            var txt = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            txt.text = "";

            var spLogo = btn.transform.GetChild(1).GetComponent<Image>();
            spLogo.gameObject.SetActive(false);
            if (title_img.Equals(""))
            {
                txt.text = title.ToUpper();
            }
            else
            {
                Globals.Config.loadImgFromUrlAsync(spLogo, title_img, () =>
                {
                    if (spLogo != null && spLogo.sprite != null)
                    {
                        spLogo.gameObject.SetActive(true);
                        spLogo.SetNativeSize();
                    }
                    else
                    {
                        txt.text = title.ToUpper();
                    }
                });
            }
            btn.transform.SetParent(scrTabs.content);
            btn.transform.localScale = Vector3.one;
            btn.transform.position = new Vector3(btn.transform.position.x, 0);
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                onClickTab(btn.gameObject, obItem);
            });
        }

        if (scrTabs.content.childCount > indSelect)
        {
            Globals.Logging.Log("item   " + item0.ToString());
            onClickTab(scrTabs.content.GetChild(indSelect).gameObject, item0);
        }
    }

    public void updateAg()
    {

    }
    void onClickTab(GameObject evv, JObject dataItem)
    {
        SoundManager.instance.soundClick();
        for (var i = 0; i < scrTabs.content.childCount; i++)
        {
            var col = scrTabs.content.GetChild(i).GetComponent<Image>().color;
            if (evv == scrTabs.content.GetChild(i).gameObject)
            {
                col.a = 255;
            }
            else
            {
                col.a = 0;
            }

            scrTabs.content.GetChild(i).GetComponent<Image>().color = col;
        }

        reloadListContent(dataItem);
        getBest(dataItem);
      
    }

    void reloadListContent(JObject dataItem)
    {
        JArray listItem = (JArray)dataItem["items"];
        //Globals.Logging.Log("-=-=-Globals.User.userMain.VIP " + Globals.User.userMain.VIP);

        itemBest = null;
        bool isCheckBestDeal = !dataItem.ContainsKey("bestDeal");

        for (var i = 0; i < listItem.Count; i++)
        {
            JObject dtItem = (JObject)listItem[i];
            dtItem["type"] = dataItem["type"];
            if (isCheckBestDeal)
            {
                //0 - 2 mệnh giá bé nhất,
                //3 - 4 mệnh giá t2
                //5 - 7 mệnh giá t3
                //8 - 10 mệnh giá t4
                if (itemBest == null)
                {
                    if (Globals.User.userMain.VIP <= 2 && i == 0)
                    {
                        //Globals.Logging.Log("-=-=-Globals.User.userMain.VIP0 " + Globals.User.userMain.VIP);
                        itemBest = dtItem;
                        txt_best.text = (string)dtItem["txtBuy"];
                    }
                    else if (Globals.User.userMain.VIP <= 4 && i == 1)
                    {
                        //Globals.Logging.Log("-=-=-Globals.User.userMain.VIP1 " + Globals.User.userMain.VIP);
                        itemBest = dtItem;
                        txt_best.text = (string)dtItem["txtBuy"];
                    }
                    else if (Globals.User.userMain.VIP <= 7 && i == 2)
                    {
                        //Globals.Logging.Log("-=-=-Globals.User.userMain.VIP2 " + Globals.User.userMain.VIP);
                        itemBest = dtItem;
                        txt_best.text = (string)dtItem["txtBuy"];
                    }
                    else if (i == 4)
                    {
                        //Globals.Logging.Log("-=-=-Globals.User.userMain.VIP3 " + Globals.User.userMain.VIP);
                        itemBest = dtItem;
                        txt_best.text = (string)dtItem["txtBuy"];
                    }
                    //}
                }
            }

            GameObject itemS = null;
            if (i < scrContent.content.childCount)
            {
                itemS = scrContent.content.GetChild(i).gameObject;
            }
            else
            {
                itemS = Instantiate(itemShop);
            }
            itemS.SetActive(true);
            itemS.transform.SetParent(scrContent.content);
            itemS.transform.localScale = Vector3.one;
            itemS.GetComponent<ItemShop>().setInfo(dtItem, i, () =>
            {
                onBuy(dtItem);
            });
        }

        for (var i = listItem.Count; i < scrContent.content.childCount; i++)
        {
            scrContent.content.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void onClickBest()
    {
        onBuy(itemBest);
    }
    void getBest(JObject data)
    {
        if (data.ContainsKey("bestDeal"))
        {
            JObject itemData = null;
            JObject itemData2 = null;
            JArray items = (JArray)data["items"];
            if ((string)data["type"] == "iap")
            {
                itemData2 = (JObject)items[0];
            }
            else
            {
                JArray bestDeal = (JArray)data["bestDeal"];

                string bestDealForMe = (string)bestDeal[Globals.User.userMain.VIP];
                itemData2 = (JObject)items.FirstOrDefault(it => (string)it["txtBuy"] == bestDealForMe);
            }

            if (itemData2 == null)
            {
                itemData = (JObject)items[0];
            }
            else
            {
                itemData = (JObject)itemData2;
            }

            Debug.Log("-=-= data best   " + itemData.ToString());
            var txtBuy = (string)itemData["txtBuy"];

            if (txtBuy.Contains("USD"))
            {
                txt_best.text = Globals.Config.convertStringToNumber(txtBuy).ToString().Replace(",", ".") + "$";
            }
            else
            {
                txt_best.text = txtBuy;//Globals.Config.FormatNumber(this.convertStringToNumber(txtBuy));
            }
            //itemData["partner"] = partner;
            itemBest = itemData;
        }
    }
    public void onBuy(JObject dataItem)
    {
        Globals.Logging.Log("onBuy  " + Globals.Config.formatStr("ClickPrice_%s", Globals.CURRENT_VIEW.getCurrentSceneName()));
        SocketIOManager.getInstance().emitSIOCCCNew(Globals.Config.formatStr("ClickPrice_%s", Globals.CURRENT_VIEW.getCurrentSceneName()));
        Globals.Logging.Log("ShopView: Data Item= " + dataItem);
        var url = (string)dataItem["url"];
        url.Replace("%uid%", Globals.User.userMain.Userid.ToString());

        switch ((string)dataItem["type"])
        {
            case Globals.CMD.W_DEFAULT:
                {
                    //open webview
                    //require("Util").onCallWebView(data.url);
                    UIManager.instance.showWebView(url);
                    break;
                }
            case Globals.CMD.W_REPLACE:
                {
                    //show input, replace in textbox, open webview
                    onShowInput(dataItem);
                    break;
                }
            case Globals.CMD.U_DEFAULT:
                {
                    //cc.sys.openURL(data.url);
                    Application.OpenURL(url);
                    break;
                }
            case Globals.CMD.U_REPLACE:
                {
                    ////show input, replace in textbox open url

                    onShowInput(dataItem);
                    break;
                }
            case Globals.CMD.IAP:
                {
                    //require("Util").onBuyIap(data.url);
                    Debug.Log("-=-= buy iapp 0");
                    if (iapManager != null)
                    {
                        Debug.Log("-=-= buy iapp 1");
                        iapManager.buyIAP(url);
                    }
                    break;
                }
        }
    }

    void onShowInput(JObject dataItem)
    {

        inputView.show();
        var url = (string)dataItem["url"];
        var lsTextBox = (JArray)dataItem["textBox"];
        for (var i = 0; i < listEdb.Count; i++)
        {
            if (i < lsTextBox.Count)
            {
                listEdb[i].gameObject.SetActive(true);
                listEdb[i].placeholder.GetComponent<TextMeshProUGUI>().text = (string)lsTextBox[i]["placeHolder"];
            }
            else
            {
                listEdb[i].gameObject.SetActive(false);

            }
        }

        btnConfirmInput.onClick.RemoveAllListeners();
        btnConfirmInput.onClick.AddListener(() =>
        {
            var canSend = true;
            for (var i = 0; i < lsTextBox.Count; i++)
            {
                var str = listEdb[i].text;
                var key = (string)lsTextBox["key"];
                url = url.Replace(key, str);
                if (str == "") canSend = false;
            }
            if (canSend)
            {
                UIManager.instance.showWebView(url);
                Globals.Logging.Log("URL====" + url);
                inputView.hide(false);
            }
        });
    }
    //public void onDragScroll() { }
    public void onDragScroll(Vector2 value)
    {
        if (!isTab) return;
        if (scrContent.content.childCount <= 0) return;
        UIManager.instance.updateBotWithScrollShop(value);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        if (UIManager.instance.gameView == null)
        {
            Globals.CURRENT_VIEW.setCurView(Globals.CURRENT_VIEW.GAMELIST_VIEW);
        }

    }
}
