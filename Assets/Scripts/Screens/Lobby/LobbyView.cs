using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using DG.Tweening;
using System;
using UnityEngine.UI.Extensions;
using System.Linq;
using Globals;
using System.Threading;

public class LobbyView : BaseView
{
    [SerializeField] private List<Sprite> lsSpMore = new();
    [SerializeField]
    private GameObject objDot, btnEx, gameItemParent, gameItemObject, btnFreechip, btnLobby, btnDailybonus, btnMail, btnShop,
    btnMail2, btnChat, btnChat_more, btnSafe, btnGiftCode, btnLeaderboard, btnSetting, btnSupport, btnBannerNews, btnBannerNews2,
    icNotiMail, icNotiMessageNodeMore, icNotiIconMessMore, icNotiFree, icNotiMessage, btnVipFarm, bannerTemp;
    [SerializeField] private RectTransform tfBot, rectMore, CenterNode;
    [SerializeField] private TextMeshProUGUI lb_name, lb_id, lb_ag, lb_safe, lbTimeOnline;
    [SerializeField] private Image btnMore;
    [SerializeField] private ScrollRect scrListGame;
    [SerializeField] private Avatar avatar;
    [SerializeField] private VipFarmProcessPercent vipFarmProcessPercent;
    [SerializeField] private HorizontalScrollSnap scrollSnapView;
    public bool isClicked;
    private List<ItemGame> listAllGame = new();
    private List<string> listShowPopupNoti = new List<string>();
    private Vector2 BasePosBot;
    private CancellationTokenSource cancelTokenLoadBanner;
    private Guid uid_action_center;
    private bool isRunStart, isRight = true, isHideBot;
    private float timeRun;

    protected override void Start()
    {
        isRunStart = true;
        base.Start();
        BasePosBot = tfBot.localPosition;
        refreshUIFromConfig(true);
        //for (var i = 0; i < 10; i++)
        //{
        //    int money = (int)(25600 * Math.Pow(10, i));
        //    Debug.Log("FormatMoney=" + Config.FormatMoney(money));
        //    Debug.Log("FormatMoney isK=" + Config.FormatMoney(money, true));
        //    Debug.Log("FormatMoney2 =" + Config.FormatMoney2(money));
        //    Debug.Log("FormatMoney2 isK =" + Config.FormatMoney2(money, true));
        //    Debug.Log("FormatMoney2 isK false isBiggerhthan100 =" + Config.FormatMoney2(money, false, true));
        //    Debug.Log("FormatMoney2 isK true isBiggerhthan100 =" + Config.FormatMoney2(money, true, true));
        //    Debug.Log("FormatMoney3  =" + Config.FormatMoney3(money));
        //    Debug.Log("FormatMoney3  value min=100K=" + Config.FormatMoney3(money, 100000));
        //}

        //var dataFake = "{\"Payload\":\"{\\\"json\\\":\\\"{\\\\\\\"orderId\\\\\\\":\\\\\\\"GPA.3391-1022-3685-19315\\\\\\\",\\\\\\\"packageName\\\\\\\":\\\\\\\"dummy.co.slots\\\\\\\",\\\\\\\"productId\\\\\\\":\\\\\\\"dummy.co.slots.1\\\\\\\",\\\\\\\"purchaseTime\\\\\\\":1673342764896,\\\\\\\"purchaseState\\\\\\\":0,\\\\\\\"purchaseToken\\\\\\\":\\\\\\\"lmpdfognekgkppfageophnpe.AO-J1OxPQ26mj6lhS2m2XUSoRD1XGXVUEG4ob1iVYMzAnVGBtdGx_RbuTTKVmMIIZlA5n-JB5JjvyY-X2G6BtTqDxFBL6teUFg\\\\\\\",\\\\\\\"quantity\\\\\\\":1,\\\\\\\"acknowledged\\\\\\\":false}\\\",\\\"signature\\\":\\\"FqG77KttDCKiDaShcHvPnf4JH1eNRr6MhYSe1aSVD+FRB5hX79DBZkP9h9TWtwrwKp+BSASN+QE8qc651hP9eopgdaOh94YddjpM0YdGGsFgknZOXyKeL9ByGSTpQU2wjAzxuW7EF8LLsk9f86J9IuIWVU3+wDmbTywgGSr/n7BLqeIoO2oB/AzQJC+MUauyzIsu7CwBn3HvoQdfvhnUy7LXm6pFtcSW5nvJHdjwz/lst4v1D7KH3ak1sZ3Rjhwb3pJCbcXdqs5oPQSGb1CFI0GBNm2Ku17ieCll69+BM5BIpoVGjggzbRmjTvXakpCJOhXoMvnaV4XzdAR8Qt363A==\\\",\\\"skuDetails\\\":[\\\"{\\\\\\\"productId\\\\\\\":\\\\\\\"dummy.co.slots.1\\\\\\\",\\\\\\\"type\\\\\\\":\\\\\\\"inapp\\\\\\\",\\\\\\\"title\\\\\\\":\\\\\\\"Item 1$ (dummy.co.slots (unreviewed))\\\\\\\",\\\\\\\"name\\\\\\\":\\\\\\\"Item 1$\\\\\\\",\\\\\\\"description\\\\\\\":\\\\\\\"Item 1$\\\\\\\",\\\\\\\"price\\\\\\\":\\\\\\\"\\\\u20ab24,000\\\\\\\",\\\\\\\"price_amount_micros\\\\\\\":24000000000,\\\\\\\"price_currency_code\\\\\\\":\\\\\\\"VND\\\\\\\",\\\\\\\"skuDetailsToken\\\\\\\":\\\\\\\"AEuhp4L1TgwWUhSsDjtS_y-MATGGRgYrZWyv2bmqx2DaF94tz5pvgkR37U2fN5KJ-w8M\\\\\\\"}\\\"]}\",\"Store\":\"GooglePlay\",\"TransactionID\":\"lmpdfognekgkppfageophnpe.AO-J1OxPQ26mj6lhS2m2XUSoRD1XGXVUEG4ob1iVYMzAnVGBtdGx_RbuTTKVmMIIZlA5n-JB5JjvyY-X2G6BtTqDxFBL6teUFg\"}";

        //JObject onj = JObject.Parse(dataFake);
        //JObject onj2 = JObject.Parse((string)onj["Payload"]);
        //string onj3 = (string)onj2["json"];
        //string onj4 = (string)onj2["signature"];
        //Debug.Log(onj3);
        //Debug.Log(onj4);
        //SocketSend.sendIAPResult(dataFake);
        //SocketSend.sendIAPResult(onj3.ToString(), "lmpdfognekgkppfageophnpe.AO-J1OxPQ26mj6lhS2m2XUSoRD1XGXVUEG4ob1iVYMzAnVGBtdGx_RbuTTKVmMIIZlA5n-JB5JjvyY-X2G6BtTqDxFBL6teUFg");
    }
    protected override void OnEnable()
    {
        CURRENT_VIEW.setCurView(CURRENT_VIEW.GAMELIST_VIEW);
        tfBot.gameObject.SetActive(true);
        Config.isBackFromGame = false;
        SoundManager.instance.playMusic();

        if (AlertMessage.instance != null && AlertMessage.instance.gameObject.activeSelf)
        {
            showAlert(true);
        }
        if (Config.ket)
            updateAgSafe();
        if (isRunStart)
        {
            onClickLobby();
        }

        //User.userMain.FacebookID = UIManager.instance.loginView.fbid;
        //Debug.Log("User.userMain.FacebookID===" + User.userMain.FacebookID);

        if (Config.isChangeTable)
        {
            Config.isChangeTable = false;

            Debug.Log("==-=-=- doi ban lobby  " + Config.curGameId);
            if (Config.listGamePlaynow.Contains(Config.curGameId))
            {
                Debug.Log("==-=-=- doi ban lobby  sendPlayNow");

                SocketSend.sendPlayNow(Config.curGameId);
            }
            else
            {
                //Logging.Log("tableMark  " + Config.tableMark + " table id  " + Config.tableId);

                Debug.Log("==-=-=- doi ban lobby  sendChangeTable");
                SocketSend.sendChangeTable(Config.tableMark, Config.tableId);
            }
        }
    }
    private void OnDisable()
    {
        tfBot.gameObject.SetActive(false);
        removeAllPopupNoti();
    }

    public void updateInfo()
    {
        //updateBannerNews();
        updateName();
        updateAg();
        updateAgSafe();
        updateAvatar();
        updateIdUser();
        //checkAlertMail();

        updateCanInviteFriend();
        //reloadListGame();
    }

    public void updateCanInviteFriend()
    {
        objDot.SetActive(User.userMain.canInputInvite);
    }

    public async void showBanner()
    {
        Debug.Log("showBanner: ListBanner=" + Config.arrBannerLobby.Count);

        GameObject[] ChildObjects;
        scrollSnapView.RemoveAllChildren(out ChildObjects);
        for (var i = 0; i < ChildObjects.Length; i++)
        {
            Destroy(ChildObjects[i]);
        }
        var rectTransform = scrListGame.GetComponent<RectTransform>();
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0);
        bool isShow = Config.arrBannerLobby.Count > 0;
        scrollSnapView.gameObject.SetActive(isShow);

        isRight = true;

        var isOne = true;
        if (cancelTokenLoadBanner == null)
        {
            cancelTokenLoadBanner = new CancellationTokenSource();
        }
        for (var i = 0; i < Config.arrBannerLobby.Count; i++)
        {
            var dataBanner = (JObject)Config.arrBannerLobby[i];
            dataBanner["isClose"] = false;
            var urlImg = (string)dataBanner["urlImg"];
            var index = i;
            Texture2D texture = await Config.GetRemoteTexture(urlImg, cancelTokenLoadBanner);
            if (texture == null) return;
            if (isOne)
            {
                isOne = false;
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -250);
            }
            var nodeBanner = Instantiate(bannerTemp).GetComponent<BannerView>();
            nodeBanner.gameObject.SetActive(true);
            scrollSnapView.AddChild(nodeBanner.gameObject);
            nodeBanner.setInfo(dataBanner, false);
        }
    }

    protected override void Update()
    {
        if (scrollSnapView.ChildObjects.Length > 0)
        {
            timeRun += Time.deltaTime;
            if (timeRun >= 5)
            {
                timeRun = 0;
                var page = scrollSnapView.CurrentPage;
                if (isRight)
                    page++;
                else
                    page--;
                if (page >= Config.arrBannerLobby.Count - 1)
                {
                    page = Config.arrBannerLobby.Count - 1;
                    isRight = false;
                }
                if (page <= 0)
                {
                    page = 0;
                    isRight = true;
                }
                //Debug.Log("-=-page  " + page);
                scrollSnapView.ChangePage(page);
            }
        }
    }

    public void showAlert(bool isShow)
    {
        DOTween.Kill(uid_action_center);
        uid_action_center = Guid.NewGuid();
        DOTween.To(offsetNormal => CenterNode.offsetMax = new Vector2(0, offsetNormal), CenterNode.offsetMax.y, isShow ? -180 : -120, 0.3f).id = uid_action_center;
    }

    public void checkAlertMail(bool isEvt22 = true)
    {
        //Logging.Log("User.userMain.nmAg:" + User.userMain.nmAg);
        if ((User.userMain.nmAg > 0 || Promotion.countMailAg > 0) && !listShowPopupNoti.Contains("FREE_CHIP"))
        {
            listShowPopupNoti.Add("FREE_CHIP");
        }
        if (User.userMain.mailUnRead > 0 && !listShowPopupNoti.Contains("MAIL_ADMIN"))
        {
            listShowPopupNoti.Add("MAIL_ADMIN");
        }

        if (User.userMain.messageUnRead > 0 && !listShowPopupNoti.Contains("CHAT_PRIVATE"))
        {
            listShowPopupNoti.Add("CHAT_PRIVATE");
        }
        //if ((User.userMain.nmAg > 0 || Promotion.countMailAg > 0) && (FreeChipView.instance == null && ExchangeView.instance == null))// && User.userMain.isShowMailAg)
        //{
        //    //User.userMain.isShowMailAg = false;
        //    Action cb = null;
        //    if (User.userMain.mailUnRead > 0 && isEvt22)
        //    {
        //        cb = () =>
        //        {
        //            UIManager.instance.showDialog(Config.getTextConfig("has_mail_show_system"), Config.getTextConfig("txt_ok"), () =>
        //            {
        //                onClickMail();
        //            }, Config.getTextConfig("label_cancel"));
        //        };
        //    }

        //UIManager.instance.showDialog(Config.getTextConfig("has_mail_show_gold"), Config.getTextConfig("txt_free_chip"), () =>
        //{
        //    onClickFreechip();
        //}, Config.getTextConfig("label_cancel"), cb);
        checkShowPopupNoti();
        //}
    }
    public void removePopupNoti(string typePopup) //vao view do roi thi thoi khi back quay lai ko show popup nua.
    {
        if (listShowPopupNoti.Contains(typePopup))
        {
            listShowPopupNoti.Remove(typePopup);
        }
    }
    public void removeAllPopupNoti()
    {
        listShowPopupNoti.Clear();
    }
    public void checkShowPopupNoti()
    {
        if (listShowPopupNoti.Count > 0)
        {
            string typePopup = listShowPopupNoti[0];
            listShowPopupNoti.RemoveAt(0);
            switch (typePopup)
            {
                case "MAIL_ADMIN":
                    {

                        UIManager.instance.showDialog(Config.getTextConfig("has_mail_show_system"), Config.getTextConfig("txt_ok"), () =>
                        {
                            onClickMail();
                        }, Config.getTextConfig("label_cancel"), () =>
                        {
                            checkShowPopupNoti();
                        });
                        break;
                    }
                case "FREE_CHIP":
                    {
                        UIManager.instance.showDialog(Config.getTextConfig("has_mail_show_gold"), Config.getTextConfig("txt_free_chip"), () =>
                        {
                            onClickFreechip();
                        }, Config.getTextConfig("label_cancel"), () =>
                        {
                            checkShowPopupNoti();
                        });
                        break;
                    }
                case "CHAT_PRIVATE":
                    {
                        UIManager.instance.showDialog(Config.getTextConfig("has_mail"), Config.getTextConfig("txt_ok"), () =>
                        {
                            UIManager.instance.destroyAllPopup();
                            UIManager.instance.lobbyView.onShowChatWorld(true);
                        }, Config.getTextConfig("label_cancel"), () =>
                        {
                            checkShowPopupNoti();
                        });
                        break;
                    }
            }
        }
    }
    public void updateName()
    {
        lb_name.text = User.userMain.displayName;
        Config.effectTextRunInMask(lb_name, true);
    }

    public void updateAg()
    {
        lb_ag.text = Config.FormatNumber(User.userMain.AG);
        if (User.userMain != null)
        {
            btnVipFarm.SetActive(User.userMain.VIP >= 2);
            refreshUIFromConfig();
        }
    }

    public void updateAgSafe()
    {
        lb_safe.text = Config.FormatNumber(User.userMain.agSafe);
    }
    public void updateIdUser()
    {
        lb_id.text = "ID:" + User.userMain.Userid;
    }
    public void updateAvatar()
    {
        //string fbId = "";
        //if (Config.typeLogin == LOGIN_TYPE.FACEBOOK)
        //{
        //    fbId = User.FacebookID;
        //}
        avatar.loadAvatar(User.userMain.Avatar, User.userMain.Username, User.FacebookID);
        avatar.setVip(User.userMain.VIP);
    }

    public void updateBannerNews()
    {
        bool isShow = Config.arrOnlistTrue.Count >= 1;
        Logging.LogWarning("updateBannerNews  " + isShow);
        Logging.LogWarning("arrOnlistTrue  " + Config.arrOnlistTrue.ToString());

        if (isShow) //co baner
        {
            if (btnVipFarm.activeSelf) //nut vip farm co mo
            {
                btnBannerNews.SetActive(false);
                btnBannerNews2.SetActive(true);
            }
            else
            {
                btnBannerNews.SetActive(true);
                btnBannerNews2.SetActive(false);
            }
        }
        refreshUIFromConfig();
    }

    public void onClickBannerNews()
    {
        UIManager.instance.showPopupListBanner();
    }

    void reloadListGame()
    {
        UIManager.instance.destroyAllChildren(scrListGame.content);
        listAllGame.Clear();
        List<JObject> listLarge = new List<JObject>();
        List<Sprite> listImageLarge = new List<Sprite>();
        List<SkeletonDataAsset> listSkeLarge = new List<SkeletonDataAsset>();
        List<Material> listMatLarge = new List<Material>();
        List<JObject> listGame = new List<JObject>();
        List<Sprite> listImgDatas = new List<Sprite>();
        List<SkeletonDataAsset> listSkeDatas = new List<SkeletonDataAsset>();
        List<Material> listMatDatas = new List<Material>();

        for (var i = 0; i < Config.listGame.Count; i++)
        {
            //Logging.Log("game Config:" + ((JObject)Config.listGame[i]).ToString());
            JObject dtGame = (JObject)Config.listGame[i];
            var skeGame = Resources.Load<SkeletonDataAsset>("AnimIconGame/" + (int)dtGame["id"] + "/skeleton_SkeletonData");
            Sprite imgGame = null;
            if (skeGame == null)
            {
                imgGame = Resources.Load<Sprite>("IconGame/" + (int)dtGame["id"]);
            }

            if (skeGame == null && imgGame == null) continue;

            if ((int)dtGame["id"] == (int)GAMEID.DUMMY || (int)dtGame["id"] == (int)GAMEID.KEANG)
            {
                listLarge.Add(dtGame);
                listImageLarge.Add(imgGame);
                listSkeLarge.Add(skeGame);
                listMatLarge.Add(Resources.Load<Material>("AnimIconGame/" + (int)dtGame["id"] + "/skeleton_Material"));
            }
            else
            {
                listGame.Add(dtGame);
                listImgDatas.Add(imgGame);
                listSkeDatas.Add(skeGame);
                listMatDatas.Add(Resources.Load<Material>("AnimIconGame/" + (int)dtGame["id"] + "/skeleton_Material"));
            }
        }

        var count = listGame.Count / 3;
        var count2 = listGame.Count % 3;
        var indexRun = 0;
        for (var i = 0; i < count + count2; i++)
        {
            if (i == 0)
            {
                if (listLarge.Count > 0)
                {
                    var pa = createItemGameColumn(listLarge, listSkeLarge, listImageLarge, listMatLarge, true);
                    pa.SetParent(scrListGame.content);
                    pa.sizeDelta = new Vector2(720, 500);
                    pa.localScale = Vector3.one;
                }
            }
            List<JObject> listSmall = new List<JObject>();
            List<Sprite> listImageDatasSmall = new List<Sprite>();
            List<SkeletonDataAsset> listSkeDatasSmall = new List<SkeletonDataAsset>();
            List<Material> listMatDatasSmall = new List<Material>();
            for (var j = 0; j < 3 && indexRun < listGame.Count; j++)
            {
                JObject dtGame = listGame[indexRun];
                listSmall.Add(dtGame);
                listImageDatasSmall.Add(listImgDatas[indexRun]);
                listSkeDatasSmall.Add(listSkeDatas[indexRun]);
                listMatDatasSmall.Add(listMatDatas[indexRun]);
                indexRun++;
            }

            if (listSmall.Count > 0)
            {
                var pa2 = createItemGameColumn(listSmall, listSkeDatasSmall, listImageDatasSmall, listMatDatasSmall);
                pa2.SetParent(scrListGame.content);
                pa2.position = Vector3.zero;
                pa2.localScale = Vector3.one;
            }
        }
    }

    RectTransform createItemGameColumn(List<JObject> list, List<SkeletonDataAsset> listskDatas, List<Sprite> listImgDatas, List<Material> listMatDatas, bool isLarge = false)
    {
        var paren = Instantiate(gameItemParent);
        //var sizePa = paren.GetComponent<RectTransform>().sizeDelta;
        //sizePa.x = Screen.width;
        //paren.GetComponent<RectTransform>().sizeDelta = sizePa;
        paren.GetComponent<RectTransform>().sizeDelta = new Vector2(626, isLarge ? 500 : 276);
        var indexRun = 0;
        for (var i = 0; i < list.Count; i++)
        {
            JObject dtGame = list[indexRun];
            var objItem = Instantiate(gameItemObject, paren.transform).GetComponent<ItemGame>();
            objItem.name = (int)dtGame["id"] + "";
            objItem.transform.localScale = Vector3.one;
            objItem.transform.position = Vector3.zero;
            objItem.GetComponent<RectTransform>().sizeDelta = new Vector2(isLarge ? 313 : 198, isLarge ? 500 : 276);
            objItem.gameObject.SetActive(true);

            objItem.setInfo((int)dtGame["id"], listskDatas[i], listImgDatas[i], listMatDatas[i], () =>
            {
                onClickGame(objItem);
            });
            listAllGame.Add(objItem);
            indexRun++;

        }
        if (isLarge && list.Count < 2)
        {
            paren.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;
        }
        else if (!isLarge)
        {
            paren.GetComponent<HorizontalLayoutGroup>().spacing = 13;
            if (list.Count < 3)
                paren.GetComponent<HorizontalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;

        }
        return paren.GetComponent<RectTransform>();
    }


    public void onClickGameFromBanner(int gameID)
    {
        for (var i = 0; i < listAllGame.Count; i++)
        {
            if (listAllGame[i].gameID == gameID)
            {
                onClickGame(listAllGame[i]);
                break;
            }
        }
    }


    void onClickGame(ItemGame itemGame)
    {
        Debug.Log("isClick=" + isClicked);
        Debug.Log("lastGameID=" + User.userMain.lastGameID);
        Debug.Log("itemGame.gameID=" + itemGame.gameID);
        if (isClicked || (User.userMain.lastGameID != 0 && User.userMain.lastGameID != itemGame.gameID)) return;
        isClicked = true;
        DOTween.Sequence().AppendInterval(.5f).AppendCallback(() =>
        {
            isClicked = false;
        });

        if (User.userMain.AG <= 0)
        {
            UIManager.instance.showPopupWhenLostChip(false, true);
            return;
        }
        Config.curGameId = itemGame.gameID;
        if (Config.isShowTableWithGameId(Config.curGameId) && User.userMain.VIP >= 1)
        {
            UIManager.instance.openTableView();
        }

        SocketSend.sendSelectGame(itemGame.gameID);
    }


    public void onClickEX()
    {
        UIManager.instance.openEx();
    }

    public void onClickProfile()
    {
        UIManager.instance.openProfile();
    }

    public void onClickGiftcode()
    {
        hideMore();
        onClickLobby();
        UIManager.instance.openGiftCode();
    }
    public void onClickSetting()
    {
        hideMore();
        UIManager.instance.openSetting();
    }

    public void onClickLobby()
    {
        checkShowPopupNoti();
        CURRENT_VIEW.setCurView(CURRENT_VIEW.GAMELIST_VIEW);
        showTab(false, false, true, false, false, false);
    }
    public void onClickLeaderBoard()
    {
        hideMore();
        UIManager.instance.openLeaderBoard();
    }
    /**bool isChat, bool isFreechip, bool isLobby, bool isDailyBonus, bool isMail*/
    void showTab(bool isChat, bool isFreechip, bool isLobby, bool isShop, bool isMail, bool isSafe)
    {
        if (btnChat.activeSelf)
        {
            OnOffButton(btnChat, isChat);
        }
        OnOffButton(btnFreechip, isFreechip);
        OnOffButton(btnLobby, isLobby);
        OnOffButton(btnShop, isShop);
        OnOffButton(btnMail, isMail);
        if (btnSafe.activeSelf)
        {
            OnOffButton(btnSafe, isSafe);
        }

        onShowHideBot(true);
        UIManager.instance.clearPopupLobby();

        //setDefaultPosBtnMore();
        hideMore();
    }

    void OnOffButton(GameObject btn, bool isOn)
    {
        btn.transform.Find("on").gameObject.SetActive(isOn);
        btn.transform.Find("off").gameObject.SetActive(!isOn);
    }

    bool isCheckOnButton(GameObject btn)
    {
        return btn.transform.Find("on").gameObject.activeSelf;
    }

    public void onClickShop()
    {
        if (isCheckOnButton(btnShop)) return;
        showTab(false, false, false, true, false, false);
        UIManager.instance.openShop(true);
    }

    public void onShowChatWorld(bool isTab)
    {
        if (isCheckOnButton(btnChat) && isTab) return;
        if (!isTab)
        {
            hideMore();
        }

        showTab(true, false, false, false, false, false);
        UIManager.instance.openChatWorld();
    }
    public void onClickFreechip()
    {
        if (isCheckOnButton(btnFreechip)) return;
        showTab(false, true, false, false, false, false);
        UIManager.instance.openFreeChipView();
    }
    public void onClickDailyBonus()
    {
        //if (isCheckOnButton(btnDailybonus)) return;
        //showTab(false, false, false, true, false, false);
        //UIManager.instance.openDailyBonus();
        UIManager.instance.showToast("COMMING SOON!");
    }
    public void onClickMail()
    {
        if (isCheckOnButton(btnMail)) return;
        showTab(false, false, false, false, true, false);
        UIManager.instance.openMailView();
    }

    public void setTabSafe()
    {
        if (isCheckOnButton(btnSafe)) return;
        showTab(false, false, false, false, false, true);
    }

    public void onClickSafe()
    {
        UIManager.instance.openSafeView();
    }

    public void setTimeGetMoney()
    {
        if (Promotion.time <= 0)
        {
            lbTimeOnline.text = Config.getTextConfig("click_to_spin");
            SocketSend.sendPromotion();
        }
        else
        {
            lbTimeOnline.text = Config.convertTimeToString(Promotion.time);
            Promotion.time--;
            DOTween.Sequence().AppendInterval(1).AppendCallback(() =>
            {
                setTimeGetMoney();
            });
        }
    }
    public void updateMailandMessageNoti()
    {
        icNotiMail.SetActive(User.userMain.mailUnRead > 0);
        icNotiMessage.SetActive(User.userMain.messageUnRead > 0);
        icNotiMessageNodeMore.SetActive(User.userMain.messageUnRead > 0 && Config.ket);
        icNotiIconMessMore.SetActive(User.userMain.messageUnRead > 0);
        icNotiFree.SetActive(User.userMain.nmAg > 0 || Promotion.countMailAg > 0);
    }
    public void hideMore()
    {
        rectMore.gameObject.SetActive(false);
        setDefaultPosBtnMore();
    }
    public void setNotiMessage(bool state)
    {
        icNotiMessageNodeMore.gameObject.SetActive(state && Config.ket);
        icNotiIconMessMore.gameObject.SetActive(state);
        icNotiMessage.gameObject.SetActive(state);
    }
    void setDefaultPosBtnMore()
    {
        btnMore.sprite = lsSpMore[0];
        if (Config.ket && Config.ismaqt)
        {
            btnMore.transform.eulerAngles = new Vector3(btnMore.transform.eulerAngles.x, btnMore.transform.eulerAngles.y, -90);
            btnMore.transform.localPosition = new Vector3(BasePosBot.x + tfBot.rect.width / 2 - btnMore.rectTransform.rect.height / 2, BasePosBot.y + tfBot.rect.height + btnMore.rectTransform.rect.width / 2);
        }
        else
        {
            btnMore.transform.eulerAngles = new Vector3(btnMore.transform.eulerAngles.x, btnMore.transform.eulerAngles.x, 0);
            btnMore.transform.localPosition = new Vector3(BasePosBot.x + tfBot.rect.width / 2 - btnMore.rectTransform.rect.width / 2, BasePosBot.y + tfBot.rect.height + rectMore.rect.height / 2);
        }
    }

    public void refreshUIFromConfig(bool isStart = false)
    {
        //Config.is_dt = true;
        //Config.ketT = true;
        //Config.ket = true;
        //Config.ismaqt = true;
        //Config.is_bl_salert = true;
        //Config.is_bl_fb = true;
        //Config.is_xs = true;
        //Config.show_new_alert = true;

        //Config.ket = false;
        //Config.ismaqt = false;
        rectMore.gameObject.SetActive(true);
        btnEx.SetActive(Config.is_dt);
        UIManager.instance.loginView.refreshUIFromConfig();
        var issket = Config.ket;
        if (User.userMain != null && User.userMain.VIP == 0)
        {
            issket = false;
        }
        btnChat_more.SetActive(issket);
        btnChat.SetActive(!issket);
        //Debug.Log("Config.listRankGame.Count==" + Config.listRankGame.Count);
        btnLeaderboard.SetActive(Config.listRankGame.Count > 0);
        //iconSafe.SetActive(Config.ket);
        btnSafe.SetActive(issket);
        lb_safe.transform.parent.gameObject.SetActive(issket);
        if (btnVipFarm.activeSelf)
        {
            if (!btnBannerNews2.activeSelf)
            {
                btnMail.SetActive(true);
                btnMail2.SetActive(false);
            }
            else
            {
                btnMail.SetActive(false);
                btnMail2.SetActive(true);
                btnBannerNews.SetActive(false);
            }
            if (btnBannerNews.gameObject.activeSelf)
            {
                btnBannerNews.gameObject.SetActive(false);
                btnMail.SetActive(false);
                btnMail2.SetActive(true);
                btnBannerNews2.gameObject.SetActive(true);
            }

        }
        else
        {
            btnMail.SetActive(true);
            btnMail2.SetActive(false);
        }

        if (issket)
            updateAgSafe();

        btnGiftCode.SetActive(Config.ismaqt);
        float delta = tfBot.rect.width / 6;

        var bkgMore = rectMore.GetChild(0);
        if (issket && Config.ismaqt && btnMail2.gameObject.activeSelf)
        {
            btnSetting.transform.localPosition = new Vector3(2.5f * delta, 0);
            btnChat_more.transform.localPosition = new Vector3(1.5f * delta, 0);
            btnLeaderboard.transform.localPosition = new Vector3(0.5f * delta, 0);
            btnGiftCode.transform.localPosition = new Vector3(-0.5f * delta, 0);
            btnSupport.transform.localPosition = new Vector3(-1.5f * delta, 0);
            btnMail2.transform.localPosition = new Vector3(-2.5f * delta, 0);

            rectMore.sizeDelta = new Vector2(tfBot.rect.width, rectMore.rect.height);
            bkgMore.GetComponent<RectTransform>().sizeDelta = new Vector2(tfBot.rect.width + 50, rectMore.rect.height);
        }
        else
        {
            var count = 6;
            if (!btnMail2.gameObject.activeSelf)
            {
                count = 5;
                delta = tfBot.rect.width / 5;
            }
            if (!btnChat_more.activeSelf)
            {
                count--;
            }
            //if (!btnMail2.activeSelf)
            //{
            //    count--;
            //}
            if (!btnGiftCode.activeSelf)
            {
                count--;
            }
            if (!btnLeaderboard.activeSelf)
            {
                count--;
            }
            Debug.Log("Count==" + count);
            bkgMore.GetComponent<RectTransform>().sizeDelta = new Vector2(count * delta, rectMore.rect.height);
            rectMore.sizeDelta = new Vector2(count * delta, rectMore.rect.height);

            float index = count / 2 - (count % 2 == 0 ? 0.5f : 0);
            btnSetting.transform.localPosition = new Vector3(index * delta, 0);
            index--;
            if (btnChat_more.activeSelf)
            {
                btnChat_more.transform.localPosition = new Vector3(index * delta, 0);
                index--;
            }
            if (btnLeaderboard.activeSelf)
            {
                btnLeaderboard.transform.localPosition = new Vector3(index * delta, 0);
                index--;
            }
            if (btnGiftCode.activeSelf)
            {
                btnGiftCode.transform.localPosition = new Vector3(index * delta, 0);
                index--;
            }
            btnSupport.transform.localPosition = new Vector3(index * delta, 0);
        }
        if (User.userMain != null)
        {
            btnVipFarm.SetActive(User.userMain.VIP >= 2);
        }
        hideMore();
        if (!isStart)
            reloadListGame();
        //setDefaultPosBtnMore();
    }

    public void onClickMore()
    {
        SoundManager.instance.soundClick();
        rectMore.gameObject.SetActive(!rectMore.gameObject.activeSelf);
        rectMore.transform.DOKill();
        btnMore.transform.DOKill();
        if (rectMore.gameObject.activeInHierarchy)
        {
            btnMore.sprite = lsSpMore[1];
            if (Config.ket && Config.ismaqt)
            {
                rectMore.localPosition = new Vector3(0, BasePosBot.y - rectMore.rect.height);
                var posy = BasePosBot.y + tfBot.rect.height + rectMore.rect.height;
                rectMore.DOLocalMoveY(posy, 0.1f);
                btnMore.transform.DOLocalMoveY(posy + btnMore.rectTransform.rect.width / 2 - 10, 0.15f);
            }
            else
            {
                rectMore.localPosition = new Vector3(BasePosBot.x + tfBot.rect.width / 2 + rectMore.rect.width / 2, BasePosBot.y + tfBot.rect.height + rectMore.rect.height);
                var posx = BasePosBot.x + tfBot.rect.width / 2 - rectMore.rect.width / 2;
                rectMore.DOLocalMoveX(posx, 0.1f);

                btnMore.transform.DOLocalMoveX(posx - rectMore.rect.width / 2 - btnMore.rectTransform.rect.width / 2, 0.15f);
            }
        }
        else
        {
            setDefaultPosBtnMore();
        }
    }

    public void onClickSupport()
    {
        hideMore();
        if (!Config.fanpageID.Equals("") && Config.is_bl_fb)
        {
            SoundManager.instance.soundClick();
            Application.OpenURL(Config.u_chat_fb);
        }
        else
        {
            UIManager.instance.openFeedback();
        }
    }
    public void updateBotWithScrollShop(Vector2 value)
    {
        if (value.y <= 0.25f && !isHideBot)
        {
            isHideBot = true;
            onShowHideBot(false);
        }
        else if (value.y >= 0.75f && isHideBot)
        {
            isHideBot = false;
            onShowHideBot(true);
        }
    }


    void onShowHideBot(bool isShow)
    {
        tfBot.DOKill();
        btnMore.gameObject.SetActive(isShow);
        if (isShow)
        {
            tfBot.DOLocalMoveY(BasePosBot.y, 0.2f);
        }
        else
        {
            tfBot.DOLocalMoveY(BasePosBot.y - tfBot.rect.height, 0.2f);
        }
    }
    public void OnClickVipFarm()
    {
        SoundManager.instance.soundClick();
        UIManager.instance.openVipFarm();
    }

    public void UpdateVipFarm()
    {
        var _farmPercent = (float)Config.dataVipFarm["farmPercent"];
        if (_farmPercent >= 100) _farmPercent = 100;
        vipFarmProcessPercent.setInfo(_farmPercent);
    }
}
