using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BannerView : BaseView
{
    [SerializeField]
    GameObject btnClose;

    [SerializeField]
    RawImage imageBanner;

    System.Action callbaclClick = null;
    bool isNotShowNext = false;
    public JObject data;
    //[SerializeField]
    public bool isBannerType9 = false;
    protected override void OnEnable()
    {
        base.OnEnable();
        if (background != null)
        {
            if (TableView.instance != null && TableView.instance.isHorizontal && ShopView.instance == null)
            {
                background.transform.localEulerAngles = new Vector3(0, 0, 270);
            }
            else
            {
                background.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    public async void setInfo(JObject _data, bool isMark = true, System.Action _callbaclClick = null)
    {
        data = _data;
        if (UIManager.instance.gameView != null && !isBannerType9)
        {
            Debug.Log(" Co game view----> Destroy Banner");
            Destroy(gameObject);
            return;
        }

        callbaclClick = _callbaclClick;
        var mask = transform.Find("mask");
        if (mask != null)
            mask.gameObject.SetActive(isMark);

        JArray arrButton = (JArray)data["arrButton"];

        string urlImg = (string)data["urlImg"];

        bool isClose = (bool)data["isClose"];
        if (btnClose != null)
            btnClose.SetActive(isClose);
        //imageBanner.sprite = await Globals.Config.GetRemoteSprite(urlImg, true);
        if (data["texture"] != null)
        {
            Debug.Log("Load banner from preload");
            imageBanner.texture = Globals.Config.listPreloadTexture[(int)data["texture"]];

        }
        else
        {
            imageBanner.texture = await Globals.Config.GetRemoteTexture(urlImg, true);
        }

        if (this == null || gameObject == null || imageBanner.IsDestroyed()) return;
        if (UIManager.instance.gameView != null && !isBannerType9)
        {
            Destroy(gameObject);
            return;
        }
        imageBanner.SetNativeSize();
        var scale = 1.0f;
        if (imageBanner.rectTransform.rect.width >= 1280)
        {
            scale = 1280f / imageBanner.rectTransform.rect.width - 0.1f;
        }
        imageBanner.transform.localScale = new Vector3(scale, scale, scale);
        for (var i = 0; i < arrButton.Count; i++)
        {
            var dtBtn = arrButton[i];
            List<float> posss = ((JArray)dtBtn["pos"]).ToObject<List<float>>();

            Sprite spr = await Globals.Config.GetRemoteSprite((string)dtBtn["urlBtn"], true);


            if (this == null) return;
            if (UIManager.instance.gameView != null && !isBannerType9)
            {
                Destroy(gameObject);
                return;
            }
            if (spr != null)
            {
                //var sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                var btnView = Globals.Config.createNodeButton(spr);
                btnView.GetComponent<Image>().SetNativeSize();
                btnView.transform.SetParent(imageBanner.transform, false);
                btnView.transform.localScale = Vector3.one;
                if (imageBanner.rectTransform.rect.width == float.NaN)
                {
                    imageBanner.rectTransform.sizeDelta = new Vector2(270, 479);
                }
                Vector2 posBtn = new Vector3(imageBanner.rectTransform.rect.width * (posss[0] - 0.5f), imageBanner.rectTransform.rect.height * (posss[1] - 0.5f));
                btnView.transform.localPosition = posBtn;
                btnView.transform.localEulerAngles = new Vector3(0, 0, 0);
                btnView.onClick.RemoveAllListeners();
                btnView.onClick.AddListener(() =>
                {

                    SocketIOManager.getInstance().logEventSuggestBanner(2, data);
                    //}
                    string type = (string)dtBtn["type"];
                    Debug.Log("Type Banner Click-" + type);
                    switch (type)
                    {
                        case "openlink":
                            {
                                //Debug.Log("Chi chau open link=====" + (string)dtBtn["urlLink"]);
                                Globals.Config.isClickBuyItem = true;
                                Application.OpenURL((string)dtBtn["urlLink"]);
                                Debug.Log(" clmm click from banner");
                                break;
                            }
                        case "showwebview":
                            {
                                Globals.Config.isClickBuyItem = true;
                                string url = (string)dtBtn["urlLink"];
                                url = url.Replace("%userid%", Globals.User.userMain.Userid.ToString());
                                url = url.Replace("%uid%", Globals.User.userMain.Userid.ToString());
                                url = url.Replace("%dm%", "0");
                                UIManager.instance.showWebView(url);
                                break;
                            }
                        case "ok":
                            { //chi de thong bao

                                //this.Baner.onClose();

                                onClickClose(true);
                                break;
                            }
                        case "pm":
                            {
                                UIManager.instance.lobbyView.onClickShop();
                                break;
                            }
                        case "playnow":
                            {
                                int gameID = (int)dtBtn["gameID"];

                                UIManager.instance.lobbyView.onClickGameFromBanner(gameID);
                                isNotShowNext = true;
                                onClickClose(true);
                                break;
                            }
                        case "force":
                            { //buoc ra khoi cuoc doi bo
                                Application.Quit();
                                break;
                            }
                        case "update":
                            //              cc.sys.openURL(dataBtn.urlLink);
                            //isTypeUpdate = true;

                            Application.OpenURL((string)dtBtn["urlLink"]);
                            break;
                        case "cashout": // open CO
                                        //              cc.NGWlog('chay vao  btn  cash out', require("ConfigManager").getInstance().is_dt);
                            if (Globals.Config.is_dt)
                            {
                                //cc.NGWlog('chay vao  btn  cash out 22222');
                                UIManager.instance.openEx();
                            }
                            break;
                        case "topgame": // top game
                                        //require("UIManager").instance.onShowTopGame(dataBtn.gameID);
                                        //UIManager.instance.lobbyView.hideMore();
                            UIManager.instance.openLeaderBoard((int)dtBtn["gameID"]);
                            break;
                        case "gofarms":
                            UIManager.instance.openVipFarm();
                            break;
                    }

                    if (callbaclClick != null)
                    {
                        callbaclClick.Invoke();
                    }
                });
            }
        }
    }

    public new void onClickClose(bool isDestroy = true)
    {
        base.onClickClose(isDestroy);
        UIManager.instance.nextBanner(isNotShowNext);
        SocketIOManager.getInstance().logEventSuggestBanner(1, data);
    }
}
