using System;
using DG.Tweening;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.U2D;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.UI;
using Spine.Unity;
using OneSignalSDK;
using System.Collections.Generic;
using UnityEngine.Networking;
using static Globals.Config;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using System.Threading.Tasks;
using System.Collections;
using Unity.VisualScripting;
#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

using System.Threading;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    Sprite sf_toast = null;
    [SerializeField]
    GameObject nodeLoad;

    [SerializeField]
    public Transform parentPopups, parentGame, parentPopupsInMain, parentBanner;
    public TMP_FontAsset fontDefault = null;

    //public AnimatorController animatorButton;

    public LoginView loginView;
    public LobbyView lobbyView;

    [SerializeField]
    TextMeshProUGUI testFont;

    float timeShowLoad = 0;

    public SpriteAtlas avatarAtlas, cardAtlas;
    [SerializeField]
    Sprite avtDefault;
    [SerializeField]
    Canvas canvasGame;
    [HideInInspector]
    public GameView gameView;
    [SerializeField]
    AlertMessage alertMessage;

    private DialogView currentDialogNoti = null;

    //private DialogView messageBox;
    //private DialogView dialogViewPopup;

    public List<DialogView> dialogPool = new List<DialogView>();
    public List<DialogView> listDialogOne = new List<DialogView>();


    public Sprite spAvatarMe;
    void Awake()
    {
        Application.targetFrameRate = 60;
        instance = this;
        curGameId = PlayerPrefs.GetInt("curGameId", 0);
        curServerIp = PlayerPrefs.GetString("curServerIp", "");
        loadTextConfig();
        getConfigSetting();

        TimeOpenApp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        Input.multiTouchEnabled = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Application.runInBackground = true;

        //DOTween.Sequence().AppendCallback(() =>
        //{
        //    checkSalert();
        //}).AppendInterval()

    }
    public void forceDisconnect()
    {
        //SoundManager.instance.soundClick();
        //SocketSend.sendLogOut();
        //Config.typeLogin = LOGIN_TYPE.NONE;
        //PlayerPrefs.SetInt("type_login", (int)LOGIN_TYPE.NONE);
        //PlayerPrefs.Save();
        //UIManager.instance.showLoginScreen(false);
        //SocketIOManager.getInstance().emitSIOCCCNew("ClickLogOut");
        showLoginScreen(false);
    }

    void Start()
    {
        lobbyView.hide(false);
        OneSignal.Default.Initialize("def6e8d4-8348-439e-8294-760cb99e4864");
        OneSignal.Default.PromptForPushNotificationsWithUserResponse();
        //OneSignal.Default.LogLevel = LogLevel.Verbose;


#if UNITY_ANDROID
        //if (Permission.HasUserAuthorizedPermission(Permission.))
        //{
        //    // The user authorized use of the microphone.
        //}
        //else
        //{
        //    bool useCallbacks = false;
        //    if (!useCallbacks)
        //    {
        //        // We do not have permission to use the microphone.
        //        // Ask for permission or proceed without the functionality enabled.
        //        Permission.RequestUserPermission(Permission.Microphone);
        //    }
        //    else
        //    {
        //        var callbacks = new PermissionCallbacks();
        //        callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
        //        callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
        //        callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
        //        Permission.RequestUserPermission(Permission.Microphone, callbacks);
        //    }
        //}
#elif UNITY_IOS
#endif

        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
    delegate (AndroidNotificationIntentData data)
    {
        var msg = "Notification received id: " + data.Id + "\n";
        msg += "\n Notification received: ";
        msg += "\n .Title: " + data.Notification.Title;
        msg += "\n .Body: " + data.Notification.Text;
        msg += "\n .Channel: " + data.Channel;
        msg += "\n .Group: " + data.Notification.Group;
        Globals.Logging.Log(msg);
    };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
    }

    public string environment = "production";

    async void init()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            // An error occurred during initialization.
        }
    }


    public void pushLocalNotiOff()
    {
        return;
        if (!allowPushOffline || !isLoginSuccess) return;
        var timeLeft = Globals.Promotion.time;
#if UNITY_ANDROID
        var c = new AndroidNotificationChannel()
        {
            Id = DateTime.Now.Millisecond.ToString(),
            Name = "Default Channel",
            Importance = Importance.High,
            Description = getTextConfig("txt_freechip")
        };
        AndroidNotificationCenter.RegisterNotificationChannel(c);
        var notification = new AndroidNotification();
        notification.Title = getTextConfig("txt_freechip");
        notification.Text = getTextConfig("txt_noti_free_chip");
        notification.FireTime = DateTime.Now.AddSeconds(timeLeft);
        notification.Group = "free_chip";
        AndroidNotificationCenter.SendNotification(notification, c.Id);
#elif UNITY_IOS
var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, 0, timeLeft),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can optionally specify a custom identifier which can later be 
            // used to cancel the notification, if you don't set one, a unique 
            // string will be generated automatically.
            Identifier = "_notification_01",
            Title = "Title",
            Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
            Subtitle = "This is a subtitle, something, something important...",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }

    public void pushLocalNotiEveryDay()
    {
        if (!allowPushOffline || !isLoginSuccess) return;
        //var listNoti = delayNoti;
        //    let date = new Date();
        //    let hour = date.getHours();
        //    let min = date.getMinutes();
        //    let time = hour * 3600 + min * 60;
        //    for (let i = 0; i < listNoti.length; i++)
        //    {
        //        let timeLeft = time;
        //        let content = listNoti[i].text;
        //        let title = listNoti[i].title;
        //        let data = listNoti[i].data;
        //        var noti = {
        //                title: title,
        //                time: timeLeft,
        //                content: Global.encode(content),
        //                category: "",
        //                identifier: "",
        //                data: data,
        //                isLoop: true
        //            };
        //    require("Util").pushNotiOffline(JSON.stringify(noti));
        //}
    }
    private void checkSalert()
    {
        if (list_Alert.Count > 0)
        {
            JObject data = list_Alert[0];
            list_Alert.RemoveAt(0);
            showAlertMessage(data);
        }
    }
    public Sprite getRandomAvatar()
    {

        return avtDefault;
    }
    public void changeOrientation(ScreenOrientation screenOrient)
    {
        canvasGame.GetComponent<CanvasScaler>().matchWidthOrHeight = screenOrient == ScreenOrientation.Portrait ? 0 : 1.0f;
        canvasGame.GetComponent<CanvasScaler>().referenceResolution = screenOrient == ScreenOrientation.Portrait ? new Vector2(720, 1280) : new Vector2(1280, 720);
        Screen.orientation = screenOrient;
        DOTween.Sequence().AppendInterval(0.5f).AppendCallback(() =>
        {
            SafeArena.instance.changeOrient();
            AlertShort.instance.updateChangeOrient();
        });
    }


    public void changeOrientTest()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            changeOrientation(ScreenOrientation.LandscapeLeft);
        }
        else
        {
            changeOrientation(ScreenOrientation.Portrait);
        }

        DOTween.Sequence().SetDelay(5.0f).AppendCallback(() =>
        {
            changeOrientation(ScreenOrientation.Portrait);

        });
    }
    void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("-=-=-=-OnApplicationFocus " + hasFocus);
        if (Globals.Config.isClickBuyItem && hasFocus)
        {
            SocketSend.sendUAG();
            SocketSend.getFarmInfo();
            Globals.Config.isClickBuyItem = false;
        }
    }
    public void OnApplicationQuit()
    {

        Globals.Logging.LogWarning("-=-=OnApplicationQuit ");
        SocketIOManager.getInstance().stopIO();
    }
    //long timeOnPause = 0;
    public void OnApplicationPause(bool pause)
    {
        Debug.Log("-=-=-=-OnApplicationPause " + pause);
        if (pause)
        {
            pushLocalNotiOff();
            if (gameView != null)
                showLoginScreen(true);
        }
        else if (Globals.Config.isClickBuyItem)
        {
            SocketSend.sendUAG();
            SocketSend.getFarmInfo();
            Globals.Config.isClickBuyItem = false;
        }
    }
    public bool isLoginShow()
    {
        return loginView.getIsShow();
    }

    public void showAlertMessage(JObject data)
    {
        if (loginView.getIsShow()) return;
        alertMessage.addAlertMessage(data);
    }
    void Update()
    {
        if (timeShowLoad > 0)
        {
            timeShowLoad -= Time.deltaTime;
            if (timeShowLoad <= 0)
            {
                hideWatting();
            }
        }
    }

    public void showWatting(float timeOut = 10)
    {
        if (nodeLoad.activeSelf) return;
        timeShowLoad = timeOut;
        nodeLoad.SetActive(true);
    }

    public void hideWatting()
    {
        timeShowLoad = 0;
        nodeLoad.SetActive(false);
    }
    public void updateMailAndMessageNoti()
    {
        lobbyView.updateMailandMessageNoti();
    }
    public void setNotiMessage(bool state)
    {
        lobbyView.setNotiMessage(state);
    }
    public void updateChipUser()
    {
        lobbyView.updateAg();
        if (ProfileView.instance != null && ProfileView.instance.gameObject.activeSelf)
        {
            ProfileView.instance.updateAg();
        }
        if (ShopView.instance != null && ShopView.instance.gameObject.activeSelf)
        {
            ShopView.instance.updateAg();
        }
        if (gameView != null && gameView.gameObject.activeSelf)
        {
            gameView.thisPlayer.updateMoney();
        }
        if (TableView.instance != null && TableView.instance.gameObject.activeSelf)
        {
            TableView.instance.updateAg();
        }
    }
    public void showLoginScreen(bool isReconnect = false)
    {

        if (loginView.getIsShow()) return;
        Globals.Logging.Log("UImanager showLoginScreen");
        //if (!isReconnect)
        //{
        //    WebSocketManager.getInstance().stop();
        //}
        if (seqPing != null)
        {
            seqPing.Kill();
        }
        seqPing = null;


        lobbyView.hide(false);

        if (TableView.instance != null)
        {
            Destroy(TableView.instance.gameObject);
        }
        TableView.instance = null;

        Globals.Logging.Log("gameView   " + (gameView != null));
        if (gameView != null)
        {
            Destroy(gameView);
        }
        gameView = null;
        Globals.Config.dataVipFarm = null;
        destroyAllChildren(parentGame);
        dialogPool.Clear();
        listDialogOne.Clear();

        destroyAllPopup();

        WebSocketManager.getInstance().stop();
        SocketIOManager.getInstance().stopIO();

        loginView.show();
        if (isReconnect)
        {
            loginView.reconnect();
        }
    }

    public void showGame()
    {
        if (gameView != null)
        {
            Destroy(gameView.gameObject);
        }
        gameView = null;
        switch (curGameId)
        {
            case (int)Globals.GAMEID.DUMMY:
                {
                    Globals.Logging.Log("Di vao day RUMMY");
                    gameView = Instantiate(loadPrefabGame("DummyView"), parentGame).GetComponent<DummyView>();
                    gameView.transform.eulerAngles = new Vector3(0, 0, -90);
                    Globals.Logging.Log("showGame RUMMY 2   " + (gameView != null));
                    break;
                }

            case (int)Globals.GAMEID.SLOTNOEL:
                {
                    Globals.Logging.Log("showGame SLOTNOEL");
                    gameView = Instantiate(loadPrefabGame("SlotNoelView"), parentGame).GetComponent<SlotNoelView>();
                    break;
                }
            case (int)Globals.GAMEID.SLOTTARZAN:
                {
                    Globals.Logging.Log("showGame SLOTTARZAN");
                    gameView = Instantiate(loadPrefabGame("SlotTarzanView"), parentGame).GetComponent<SlotTarzanView>();
                    break;
                }
            case (int)Globals.GAMEID.SLOT_9900:
                {
                    Globals.Logging.Log("showGame SLOT_9900");
                    gameView = Instantiate(loadPrefabGame("SlotJuicyGardenView"), parentGame).GetComponent<SlotJuicyGardenView>();
                    break;
                }
            //case (int)Globals.GAMEID.SLOT_SIXIANG:
            //    {
            //        Globals.Logging.Log("showGame SLOT_SIXIANG");
            //        gameView = Instantiate(loadPrefabGame("SiXiangView"), parentGame).GetComponent<SiXiangView>();
            //        break;
            //    }
            case (int)Globals.GAMEID.BORKDENG:
                {
                    Globals.Logging.Log("showGame BORKDENG");
                    gameView = Instantiate(loadPrefabGame("BorkKdengView"), parentGame).GetComponent<BorkKDengView>();
                    break;
                }
            case (int)Globals.GAMEID.KEANG:
                {
                    Globals.Logging.Log("showGame KEANG");
                    gameView = Instantiate(loadPrefabGame("KeangView"), parentGame).GetComponent<KeangView>();
                    gameView.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                }
            case (int)Globals.GAMEID.GAOGEA:
                {
                    Globals.Logging.Log("showGame GAOGEA");
                    gameView = Instantiate(loadPrefabGame("GaoGeaView"), parentGame).GetComponent<GaoGeaView>();
                    //gameView.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                }
            case (int)Globals.GAMEID.SICBO:
                {
                    Globals.Logging.Log("showGame SICBO");
                    gameView = Instantiate(loadPrefabGame("HiloView2"), parentGame).GetComponent<HiloView>();
                    gameView.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                }
            default:
                {
                    Globals.Logging.Log("-=-= chua co game nao ma vao. Lm thi tu them vao di;;;;");
                    break;
                }
        }
        if (gameView != null)
        {
            Globals.CURRENT_VIEW.setCurView(curGameId.ToString());
            if (TableView.instance)
                TableView.instance.hide(false);
            //if (!isShowTableWithGameId(curGameId))
            //{
            if (lobbyView.getIsShow())
                lobbyView.hide(false);
            //}
            gameView.transform.localScale = Vector3.one;

            destroyAllPopup();
        }
    }

    public void clearPopupLobby()
    {
        destroyAllChildren(parentPopupsInMain);
    }


    public void destroyAllChildren(Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    public void destroyAllPopup()
    {
        destroyAllChildren(parentPopups);
        destroyAllChildren(parentBanner);
    }
    public void DOTextTmp(TextMeshProUGUI tmp, string text, float time = 0.5f)
    {
        GameObject lbTemp = new GameObject("lbTemp");
        Text lbText = lbTemp.AddComponent<Text>();
        lbText.DOText(text, time).OnUpdate(() =>
        {
            tmp.text = lbText.text;
        }).OnComplete(() =>
        {
            Destroy(lbTemp);
        });
    }
    public void updateAvatar()
    {
        if (ProfileView.instance != null)
        {
            ProfileView.instance.onChangeAvatar();
            showToast(getTextConfig("success_change_ava"));
        }
        lobbyView.updateAvatar();
    }

    public void updateVip()
    {
        lobbyView.updateAg();
        lobbyView.updateAgSafe();
        lobbyView.refreshUIFromConfig(true);
        if (gameView != null)
        {
            gameView.updateVip();
        }
    }
    public void updateInfo()
    {
        lobbyView.updateInfo();
    }
    public void updateCanInviteFriend()
    {
        lobbyView.updateCanInviteFriend();
    }
    public void updateAG()
    {
        lobbyView.updateAg();
        if (SendGiftView.instance && SendGiftView.instance.gameObject.activeSelf)
        {
            SendGiftView.instance.updateChip();
        }

        if (ProfileView.instance != null && ProfileView.instance.gameObject.activeSelf)
        {
            ProfileView.instance.updateAg();
        }
        if (ShopView.instance != null && ShopView.instance.gameObject.activeSelf)
        {
            ShopView.instance.updateAg();
        }
        if (gameView != null && gameView.gameObject.activeSelf)
        {
            gameView.thisPlayer.updateMoney();
        }
        if (TableView.instance != null && TableView.instance.gameObject.activeSelf)
        {
            TableView.instance.updateAg();
        }
    }
    public void updateAGSafe()
    {
        lobbyView.updateAgSafe();
    }

    DG.Tweening.Sequence seqPing;
    public void showLobbyScreen(bool isFromLogin = false)
    {
        Globals.Logging.Log("showLobbyScreen  ");
        loginView.hide(false);
        destroyAllChildren(parentPopups);
        lobbyView.show();
        lobbyView.updateInfo();
        SocketSend.getFarmInfo();
        if (isFromLogin)
        {
            if (seqPing != null)
            {
                seqPing.Kill();
            }
            seqPing = DOTween.Sequence();
            seqPing.AppendInterval(5.0f).AppendCallback(() =>
            {
                SocketSend.sendPing();
            }).SetLoops(-1);
        }
        if (gameView != null)
            Destroy(gameView.gameObject);
    }

    public void showAlert(bool isShow)
    {
        lobbyView.showAlert(isShow);
    }

    public void refreshUIFromConfig()
    {
        lobbyView.refreshUIFromConfig();
    }

    public void updateBotWithScrollShop(Vector2 value)
    {
        lobbyView.updateBotWithScrollShop(value);
    }

    public void setTimeOnline()
    {
        lobbyView.setTimeGetMoney();
    }

    public GameObject loadPrefabPopup(string name)
    {
        return loadPrefab("Popups/" + name);
    }

    public GameObject loadPrefabGame(string name)
    {
        return loadPrefab("GameView/" + name);
    }

    public GameObject loadPrefab(string path)
    {
        return Resources.Load(path) as GameObject;
    }
    public SkeletonDataAsset loadSkeletonData(string path)
    {
        return Resources.Load<SkeletonDataAsset>(path);
    }
    public IEnumerator loadSkeletonDataAsync(string path, Action<SkeletonDataAsset> cb)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<SkeletonDataAsset>(path);
        yield return resourceRequest;
        cb(resourceRequest.asset as SkeletonDataAsset);
        //loadAsyncTask.Start();
    }

    void createMessageBox(GameObject prefab, string msg, Action callback1 = null, bool isHaveClose = false)
    {
        //new Thread(new ThreadStart(() =>
        //{
        DialogView dialog;
        if (dialogPool.Count == 0)
        {
            //messageBox = Instantiate(loadPrefabPopup("Dialog"), parentPopups).GetComponent<DialogView>();
            Debug.Log("-=-=listDialogOne  " + listDialogOne.Count);
            if (listDialogOne.FirstOrDefault(x => x.getMessage().Equals(msg)) == null)
            {
                dialog = Instantiate(prefab, parentPopups).GetComponent<DialogView>();
            }
            else return;
        }
        else
        {
            Globals.Logging.Log("co dialogPool  r " + dialogPool.Count);
            //dialog = dialogPool[0];
            //dialogPool.RemoveAt(0);
            //dialog.transform.parent = parentPopups;
            dialog = listDialogOne.FirstOrDefault(x => x.getMessage().Equals(msg));
            if (dialog == null)
            {
                dialog = dialogPool[0];
                dialogPool.RemoveAt(0);
                dialog.transform.SetParent(parentPopups);
            }
        }

        listDialogOne.Add(dialog);
        dialog.gameObject.SetActive(true);
        dialog.transform.localScale = Vector3.one;
        dialog.transform.SetAsLastSibling();
        dialog.setMessage(msg);
        dialog.setIsShowButton1(true, getTextConfig("ok"), callback1);
        dialog.setIsShowButton2(false, "", null);
        dialog.setIsShowClose(isHaveClose, null);

        if (gameView != null)
        {
            dialog.transform.eulerAngles = gameView.transform.eulerAngles;
            if (dialog.transform.eulerAngles.z != 0)
            {
                dialog.setLanscape();
            }
        }
    }

    public void showMessageBox(string msg, Action callback1 = null, bool isHaveClose = false)
    {
        if (msg == "") return;
#if DEVGAME
        AssetBundleManager.instance.loadPrefab(Globals.AssetBundleName.POPUPS, "Dialog", (prefab) =>
        {
            createMessageBox(prefab, msg, callback1, isHaveClose);
        });
#else
        createMessageBox(loadPrefabPopup("Dialog"), msg, callback1, isHaveClose);
#endif
    }

    DialogView createDialog(GameObject prefab, string msg, string nameBtn1 = "", Action callback1 = null, string nameBtn2 = "", Action callback2 = null, bool isShowClose = false, Action callback3 = null)
    {
        DialogView dialog;
        if (dialogPool.Count == 0)
        {
            //dialog = Instantiate(loadPrefabPopup("Dialog"), parentPopups).GetComponent<DialogView>();
            Debug.Log("-=-=listDialogOne  " + listDialogOne.Count);
            dialog = listDialogOne.FirstOrDefault(x => x.getMessage().Equals(msg));
            if (dialog == null)
            {
                dialog = Instantiate(prefab, parentPopups).GetComponent<DialogView>();
                listDialogOne.Add(dialog);
            }
        }
        else
        {
            dialog = listDialogOne.FirstOrDefault(x => x.getMessage().Equals(msg));
            if (dialog == null)
            {
                dialog = dialogPool[0];
                dialogPool.RemoveAt(0);
                dialog.transform.SetParent(parentPopups);
                listDialogOne.Add(dialog);
            }
        }
        dialog.gameObject.SetActive(true);
        dialog.transform.localScale = Vector3.one;
        dialog.transform.SetAsLastSibling();
        dialog.setMessage(msg);
        dialog.setIsShowButton1(nameBtn1 != "", nameBtn1, callback1);
        dialog.setIsShowButton2(nameBtn2 != "", nameBtn2, callback2);
        dialog.setIsShowClose(isShowClose, callback3);
        if (gameView != null)
        {
            dialog.transform.eulerAngles = gameView.transform.eulerAngles;
            if (dialog.transform.eulerAngles.z != 0)
            {
                dialog.setLanscape();
            }
        }
        return dialog;
    }
    public void showDialog(string msg, string nameBtn1 = "", Action callback1 = null, string nameBtn2 = "", Action callback2 = null, bool isShowClose = false, Action callback3 = null, Action<DialogView> callbaclReturn = null)
    {
#if DEVGAME
        AssetBundleManager.instance.loadPrefab(Globals.AssetBundleName.POPUPS, "Dialog", (prefab) =>
        {
            var dialog = createDialog(loadPrefabPopup("Dialog"), msg, nameBtn1 = "", callback1, nameBtn2, callback2, isShowClose, callback3);
            if (callbaclReturn != null)
            {
                callbaclReturn.Invoke(dialog);
            }
        });
#else
        var dialog = createDialog(loadPrefabPopup("Dialog"), msg, nameBtn1, callback1, nameBtn2, callback2, isShowClose, callback3);
        if (callbaclReturn != null)
        {
            callbaclReturn.Invoke(dialog);
        }
#endif
    }

    public void showWebView(string url, string title = "")
    {
        //if (url == "") return;
        //var webview = Instantiate(loadPrefabPopup("WebView"), transform).GetComponent<WebViewControl>();

        //webview.loadUrl(url, title);
        //webview.transform.SetAsLastSibling();
        Application.OpenURL(url);
    }
    public void showToast(string msg, Transform tfParent)
    {
        showToast(msg, 2, tfParent);
    }
    public void showToast(string msg, float timeShow = 2, Transform tfParent = null)
    {
        Globals.Logging.Log("Show Toast:" + msg);
        var compToast = createSprite(sf_toast);
        compToast.transform.SetParent(tfParent != null ? tfParent : transform);
        compToast.transform.SetAsLastSibling();
        compToast.type = Image.Type.Sliced;
        compToast.rectTransform.sizeDelta = new Vector2(400, 80);
        compToast.rectTransform.localScale = Vector3.one;
        compToast.transform.localPosition = new Vector2(0, -400);


        var lbCom = createLabel(msg, 30);
        lbCom.rectTransform.SetParent(compToast.rectTransform);
        lbCom.rectTransform.localScale = Vector3.one;
        lbCom.color = Color.white;
        lbCom.alignment = TextAlignmentOptions.Center;
        lbCom.enableWordWrapping = false;

        if (lbCom.preferredWidth > compToast.rectTransform.sizeDelta.x)
        {
            compToast.rectTransform.sizeDelta = new Vector2(lbCom.preferredWidth + 80, compToast.rectTransform.sizeDelta.y);
        }

        lbCom.rectTransform.sizeDelta = new Vector2(390, 50);
        lbCom.transform.localPosition = new Vector2(0, 0);

        if (gameView != null)
        {
            compToast.transform.eulerAngles = gameView.transform.eulerAngles;
            if (gameView.transform.eulerAngles.z == 0)
            {
                compToast.rectTransform.anchoredPosition = new Vector3(0, -Screen.width * .35f);
            }
            else
                compToast.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }


        compToast.rectTransform.localScale = Vector3.zero;
        DOTween.Sequence().Append(compToast.rectTransform.DOScale(1, .5f).SetEase(Ease.OutBack)).Append(compToast.rectTransform.DOScale(0, .5f).SetEase(Ease.InBack).SetDelay(timeShow)).AppendCallback(() =>
        {
            Destroy(compToast.gameObject);
        }).SetAutoKill(true);
    }

    public void openShop(bool isLobby = false)
    {
        var shopView = Instantiate(loadPrefabPopup("PopupShop"), parentPopups).GetComponent<ShopView>();
        shopView.init(isLobby);
        shopView.transform.localScale = Vector3.one;
        if (shopView.getIsPopupOnTab() && isLobby)
        {
            shopView.transform.SetParent(parentPopupsInMain);
        }
    }
    public void openVipFarm()
    {
        GameObject subView;

        if (TableView.instance != null && TableView.instance.gameObject.activeSelf && TableView.instance.isHorizontal)
        {
            subView = Instantiate(loadPrefabPopup("VipfarmsViewHorizontal"), parentPopups);
            subView.transform.localScale = Vector3.one;
            //subView.transform.localEulerAngles = new Vector3(0, 0, -90);
        }
        else
        {
            subView = Instantiate(loadPrefabPopup("VipFarmsView"), parentPopups);
            subView.transform.localScale = Vector3.one;
        }

    }
    public void openConfirmVipFarm()
    {
        showDialog(getTextConfig("txt_rewards_vip_farm"), getTextConfig("ok"), () =>
        {
            openVipFarm();
        }, getTextConfig("label_cancel"));
    }
    public void openChatWorld()
    {
        var chatWorldView = Instantiate(loadPrefabPopup("PopupChatWorld"), parentPopups).GetComponent<ChatWorldView>();
        chatWorldView.transform.localScale = Vector3.one;
        if (chatWorldView.getIsPopupOnTab())
        {
            chatWorldView.transform.SetParent(parentPopupsInMain);
        }

    }
    public void clickTabChatWorld()
    {
        lobbyView.onShowChatWorld(false);
    }
    public void clickTabFreeChip()
    {
        lobbyView.onClickFreechip();
    }
    public void openFriendInfo()
    {
        var friendInfoView = Instantiate(loadPrefabPopup("PopupFriendInfo"), parentPopups).GetComponent<FriendInfoView>();
        friendInfoView.transform.localScale = Vector3.one;
    }

    public void openEx()
    {
        var exchangeView = Instantiate(loadPrefabPopup("PopupExchange"), parentPopups).GetComponent<ExchangeView>();
        exchangeView.transform.localScale = Vector3.one;
    }

    bool isOnProfile = true;
    public void openProfile()
    {
        if (!isOnProfile) return;
        isOnProfile = false;
        DOTween.Sequence().AppendInterval(1).AppendCallback(() =>
        {
            isOnProfile = true;
        });
        var profileView = Instantiate(loadPrefabPopup("PopupProfile"), parentPopups).GetComponent<ProfileView>();
        profileView.transform.localScale = Vector3.one;
    }
    public void openGiftCode()
    {
        var giftCodeView = Instantiate(loadPrefabPopup("PopupGiftCode"), parentPopups).GetComponent<GiftCodeView>();
        giftCodeView.transform.localScale = Vector3.one;
    }
    public void openSetting()
    {
        //curGameId = (int)Globals.GAMEID.KEANG;
        //UIManager.instance.showGame();
        var settingView = Instantiate(loadPrefabPopup("PopupSetting"), parentPopups).GetComponent<SettingView>();
        settingView.transform.localScale = Vector3.one;
    }
    public void openLeaderBoard(int gameID = -1)
    {
        var leaderBoardView = Instantiate(loadPrefabPopup("PopupLeaderBoard"), parentPopups).GetComponent<LeaderBoardView>();
        leaderBoardView.transform.localScale = Vector3.one;
        leaderBoardView.openTabGameWithID(gameID);
    }
    public void openSendGift(string idPlayerInit = "")
    {
        var sendGiftView = Instantiate(loadPrefabPopup("PopupSendGift"), parentPopups).GetComponent<SendGiftView>();
        if (idPlayerInit != "")
        {
            sendGiftView.edbID.text = idPlayerInit;
        }
        sendGiftView.transform.localScale = Vector3.one;
    }
    public void openCreateTableView()
    {
        var createTableView = Instantiate(loadPrefabPopup("PopupCreateTable"), parentPopups).GetComponent<CreateTableView>();
        createTableView.transform.localScale = Vector3.one;
    }
    public void openChangePass()
    {
        var changePassView = Instantiate(loadPrefabPopup("PopupChangeName"), parentPopups).GetComponent<ChangeNameView>();
    }
    public void openMailDetail(JObject data)
    {
        var mailDetailView = Instantiate(loadPrefabPopup("PopupMailDetail"), parentPopups).GetComponent<MailDetailView>();
        mailDetailView.transform.localScale = Vector3.one;
        MailDetailView.instance.setInfo(data);
    }

    public void openInputPass(int tableID)
    {
        var inputPassView = Instantiate(loadPrefabPopup("PopupInputPass"), parentPopups).GetComponent<InputPassView>();
        inputPassView.setTableID(tableID);
        inputPassView.transform.localScale = Vector3.one;
    }


    public void openFeedback()
    {
        var inputPassView = Instantiate(loadPrefabPopup("PopupFeedBack"), parentPopups).GetComponent<PopupFeedBack>();
        inputPassView.transform.localScale = Vector3.one;
    }

    bool isOnSafe = true;
    public void openSafeView()
    {
        if (!isOnSafe) return;
        isOnSafe = false;
        DOTween.Sequence().AppendInterval(1).AppendCallback(() =>
        {
            isOnSafe = true;
        });
        lobbyView.setTabSafe();
        var safeView = Instantiate(loadPrefabPopup("PopupSafe"), parentPopups).GetComponent<SafeView>();
        safeView.transform.localScale = Vector3.one;
        if (safeView.getIsPopupOnTab())
        {
            safeView.transform.SetParent(parentPopupsInMain);
        }
    }
    public void openDailyBonus()
    {
        var dailyBonusView = Instantiate(loadPrefabPopup("PopupCountDownBonus"), parentPopups).GetComponent<DailyBonusView>();
        dailyBonusView.transform.localScale = Vector3.one;
        if (dailyBonusView.getIsPopupOnTab())
        {
            dailyBonusView.transform.SetParent(parentPopupsInMain);
        }
        dailyBonusView.setInfo();

    }
    public void openTableView()
    {
        if (TableView.instance == null)
        {
            if (curGameId == (int)Globals.GAMEID.KEANG || curGameId == (int)Globals.GAMEID.DUMMY || curGameId == (int)Globals.GAMEID.SICBO)
            {
                Instantiate(loadPrefab("Table/TableViewHorizontal"), transform).GetComponent<TableView>();
            }
            else
            {
                Instantiate(loadPrefab("Table/TableView"), transform).GetComponent<TableView>();
            }
        }
        else
        {
            TableView.instance.transform.SetParent(transform);
            TableView.instance.show();
        }
        TableView.instance.transform.SetSiblingIndex(2);
        TableView.instance.transform.localScale = Vector3.one;
        lobbyView.hide(false);
    }
    //public void lobbyHide()
    //{
    //    lobbyView.hide(false);
    //}
    public void openMailView()
    {
        var safeView = Instantiate(loadPrefabPopup("PopupMail"), parentPopups).GetComponent<MailView>();
        safeView.transform.localScale = Vector3.one;
        if (safeView.getIsPopupOnTab())
        {
            safeView.transform.SetParent(parentPopupsInMain);
        }

    }
    public void openFreeChipView()
    {
        var subview = Instantiate(loadPrefabPopup("PopupFreeChip"), parentPopups).GetComponent<FreeChipView>();
        subview.transform.localScale = Vector3.one;
        if (subview.getIsPopupOnTab() && (TableView.instance == null || (TableView.instance != null && !TableView.instance.getIsShow())))
        {
            subview.transform.SetParent(parentPopupsInMain);
            subview.setShowBack(false);
        }
        else
        {
            subview.setShowBack(true);
        }
    }

    public void showPopupWhenNotEnoughChip()
    {
        var isInGame = false;
        if (gameView != null) isInGame = true;
        //var typeBTN = isInGame ? DIALOG_TYPE.ONE_BTN : DIALOG_TYPE.TWO_BTN;
        var textShow = getTextConfig("txt_not_enough_money_gl");
        var textBtn1 = getTextConfig("txt_free_chip");
        var textBtn2 = getTextConfig("shop");
        var textBtn3 = getTextConfig("label_cancel");
        if (isInGame)
        {
            textShow = textShow.Split(",")[0];
            textBtn1 = textBtn3;
            textBtn2 = textBtn3;
        }
        if (Globals.User.userMain.nmAg > 0 || Globals.Promotion.countMailAg > 0 || Globals.Promotion.adminMoney > 0)
        {
            showDialog(textShow, textBtn1, () =>
            {
                if (!isInGame)
                    openFreeChipView();
            }, textBtn2, () =>
            {
                openShop();
            }, true);
        }
        else
        {
            textShow = getTextConfig("txt_not_enough_money_gl");
            showDialog(textShow, textBtn2, () =>
            {
                if (!isInGame)
                    openShop();
            }, textBtn3);
        }
    }

    public void showPopupWhenLostChip(bool isBackFromGame = false, bool isChooseGame = false)
    {
        var money = Globals.User.userMain.AG;
        if (money <= 0)
        {
            var isInGame = false;
            if (gameView != null && !isBackFromGame) isInGame = true;
            //var typeBTN = isInGame ? DIALOG_TYPE.ONE_BTN : DIALOG_TYPE.TWO_BTN;
            var textShow = getTextConfig("has_mail_show_gold");
            var textBtn1 = getTextConfig("txt_free_chip");
            var textBtn2 = getTextConfig("shop");
            var textBtn3 = getTextConfig("label_cancel");
            if (isInGame)
            {
                textShow = textShow.Split(",")[0];
                textBtn1 = textBtn3;
                textBtn2 = textBtn3;
            }
            if (isChooseGame) textShow = getTextConfig("txt_not_enough_money_gl");
            if (Globals.User.userMain.nmAg > 0 || Globals.Promotion.countMailAg > 0 ||
                 Globals.Promotion.adminMoney > 0
            )
            {
                showDialog(textShow, textBtn1, () =>
                {
                    if (!isInGame)
                        openFreeChipView();
                }, textBtn2, () =>
                {
                    openShop();
                }, true);
            }
            else
            {
                textShow = getTextConfig("txt_not_enough_money_gl");
                showDialog(textShow, textBtn2, () =>
                {
                    if (!isInGame)
                        openShop();
                }, textBtn3);
            }
        }
    }

    public void showPopupListBanner()
    {
        var subview = Instantiate(loadPrefabPopup("ListBannerView"), parentPopups).GetComponent<ListBannerView>();
        subview.transform.localScale = Vector3.one;
    }

    public void showListBannerOnLobby()
    {
        lobbyView.showBanner();
    }

    public void updateBannerNews()
    {
        lobbyView.updateBannerNews();
    }

    public JArray arrayDataBannerIO;
    int indexCurrentDataBannerIO = 0;
    [SerializeField] GameObject banerTemp;
    [SerializeField] GameObject banerHorizontalTemp;

    List<BannerView> arrBannerNotShowGame = new List<BannerView>();
    void removeAllBanerShowGame()
    {
        //let length = arrBannerNotShowGame.length;
        //cc.NGWlog("push vao banner ko show game length " + length);
        //for (let i = 0; i < length; i++)
        //{
        //    let item = arrBannerNotShowGame[i];
        //    if (item.node != null) item.node.destroy();
        //}
        //arrBannerNotShowGame.length = 0;
    }

    void removeBanerShowGame(BannerView banner)
    {
        //let indexOf = arrBannerNotShowGame.indexOf(banner);
        //if (indexOf != -1) arrBannerNotShowGame.splice(indexOf, 1);
    }
    JObject currentDataBannerIO = null;
    async void showBannerIO()
    {

        Debug.Log("showBannerIOindexCurrentDataBannerIO===:" + indexCurrentDataBannerIO);
        Debug.Log(" arrayDataBannerIO.Count:" + arrayDataBannerIO.Count);
        if (indexCurrentDataBannerIO < 0 || indexCurrentDataBannerIO >= arrayDataBannerIO.Count || gameView != null)
        {
            return;
        }

        var dataBanner = (JObject)arrayDataBannerIO[indexCurrentDataBannerIO];

        if (currentDataBannerIO != null && (string)currentDataBannerIO["urlImg"] == (string)dataBanner["urlImg"])
        {
            indexCurrentDataBannerIO++;
            return;
        }
        if (!dataBanner.ContainsKey("urlImg") || (string)dataBanner["urlImg"] == "")
        {
            indexCurrentDataBannerIO++;
            nextBanner();
            return;
        }
        currentDataBannerIO = dataBanner;
        var urlImg = (string)dataBanner["urlImg"];
        Debug.Log("indexCurrentDataBannerIO++1=:" + indexCurrentDataBannerIO);
        indexCurrentDataBannerIO++;
        Debug.Log("indexCurrentDataBannerIO++2=:" + indexCurrentDataBannerIO);
        Texture2D texture = await GetRemoteTexture(urlImg);
        currentDataBannerIO = null;
        if (texture == null)
        {
            nextBanner();
            return;
        }

        if (loginView.getIsShow()) return;

        var nodeBanner = Instantiate(TableView.instance != null && TableView.instance.gameObject.activeSelf && TableView.instance.isHorizontal && ShopView.instance == null ? banerHorizontalTemp : banerTemp, parentBanner).GetComponent<BannerView>();

        //cc.NGWlog("push vao banner ko show game");
        if (!dataBanner.ContainsKey("isShowGameView") || !(bool)dataBanner["isShowGameView"])
        {
            arrBannerNotShowGame.Add(nodeBanner);
        }
        nodeBanner.setInfo(dataBanner, true, () =>
        {
            Destroy(nodeBanner.gameObject);
        });
    }
    public void nextBanner(bool isNotShow = false)
    {
        //Debug.Log("nextBanner");
        //indexCurrentDataBannerIO++;
        if (isNotShow) return;
        showBannerIO();
    }
    public void handleBannerIO(JArray arrayData)
    {

        if (gameView != null && ShopView.instance == null) return;
        arrayDataBannerIO = arrayData;

        if (arrayDataBannerIO.Count > 0)
        {
            indexCurrentDataBannerIO = 0;
            showBannerIO();
        }
    }

    JArray arrayDataBanner;
    int indexCurrentDataBanner = 0;
    public void handleBanner(JArray arrayData)
    {
        arrayDataBanner = arrayData;
        if (arrayDataBanner.Count > 0)
        {
            indexCurrentDataBanner = 0;
            showBanner();
        }
    }
    public void showBanner()
    {
        //        var _this = this;
        //        if (require("GameManager").getInstance().gameView != null) return;
        //        cc.NGWlog("-------> showBanner");
        //if (
        //    indexCurrentDataBanner < 0 ||
        //    indexCurrentDataBanner >= arrayDataBanner.length
        //)
        //    return;

        //        cc.NGWlog("------->2 showBanner");
        //        let dataNoti = arrayDataBanner[indexCurrentDataBanner];
        //        if (
        //            !dataNoti.hasOwnProperty("type") ||
        //            !dataNoti.hasOwnProperty("arrButton")
        //        )
        //            return;
        //        if (dataNoti.type != 20) return;
        //        let idPack = "";
        //        let allowClose = false;
        //        let urlBkg = "";
        //        let posButtonClose = [];
        //        let arrButton = [];

        //        if (dataNoti.hasOwnProperty("_id")) idPack = dataNoti._id;
        //        if (dataNoti.hasOwnProperty("allowClose")) allowClose = dataNoti.allowClose;
        //        if (dataNoti.hasOwnProperty("url")) urlBkg = dataNoti.url;
        //        if (dataNoti.hasOwnProperty("posButtonClose"))
        //            posButtonClose = dataNoti.posButtonClose;

        //        let siz = cc.view.getVisibleSize();
        //        let close = false;


        //        //End Close

        //        if (dataNoti.hasOwnProperty("arrButton")) arrButton = dataNoti.arrButton;
        //        let urlCor = "https://cors-anywhere.herokuapp.com/";
        //        if (cc.sys.isNative)
        //        {
        //            urlCor = "";
        //        }
        //        if (urlBkg.indexOf(".png") === -1)
        //        {
        //            cc.loader.load(urlCor + urlBkg, (err, tex) => {

        //            // if (err) {
        //            //   cc.NGWlog("loadTextureFromUrl FB error:" + err);
        //            //   return;
        //            // }
        //            // let node = new cc.Node();
        //            // let spr = node.addComponent(cc.Sprite);
        //            // spr.spriteFrame = new cc.SpriteFrame(tex);
        //            // instantiate_parent.addChild(node);
        //            cc.NGWlog("onShowbaner=========================================== ");
        //            if (err)
        //            {
        //                cc.NGWlog(err);
        //                return;
        //            }

        //            let nodeBanner = new cc.Node("Banner");
        //            //   cc.director.getScene().addChild(nodeBanner, cc.macro.MAX_ZINDEX - 10);
        //            // nodeBanner.parent = node;
        //            node.addChild(nodeBanner);
        //            nodeBanner.zIndex = 1;
        //            // _instantiate_parent.addChild(nodeBanner, cc.macro.MAX_ZINDEX - 10);

        //            //  nodeBanner.position = cc.v2(siz.width / 2, siz.height / 2);
        //            let spMask = nodeBanner.addComponent(cc.Sprite);
        //            nodeBanner.addComponent(cc.Button);
        //            spMask.type = cc.Sprite.Type.SLICED;
        //            spMask.sizeMode = cc.Sprite.SizeMode.CUSTOM;
        //            spMask.spriteFrame = bkg_black;
        //            nodeBanner.setContentSize(siz.width, siz.height);

        //            // let btnMask = nodeBanner.addComponent(cc.Button);
        //            // btnMask.target = nodeBanner;
        //            // btnMask.transition = cc.Button.Transition.NONE;


        //            let nodeBkg = new cc.Node("Bkg");
        //            nodeBanner.addChild(nodeBkg);

        //            let imgBkg = nodeBkg.addComponent(cc.Sprite);

        //            //Button Close
        //            let nodeClose = new cc.Node("Close");
        //            nodeBkg.addChild(nodeClose);
        //            nodeClose.position = cc.v2(siz.width * 0.45, siz.height * 0.45);

        //            let imgClose = nodeClose.addComponent(cc.Sprite);
        //            imgClose.spriteFrame = btn_close;

        //            let btnClose = nodeClose.addComponent(cc.Button);
        //            //  btnClose.target = nodeClose;


        //            nodeClose.on(cc.Node.EventType.TOUCH_END, function(event) {
        //        nodeBanner.destroy();
        //        UIManager.instance.indexCurrentDataBanner++;
        //        UIManager.instance.showBanner();
        //        close = true;
        //    });
        //    imgBkg.spriteFrame = new cc.SpriteFrame(tex);
        //                nodeClose.position = cc.v2(
        //                    nodeBkg.width* 0.5 - nodeClose.width* 0.25,
        //                    nodeBkg.height* 0.5 - nodeClose.height* 0.25
        //                );



        //                for (let i = 0; i<arrButton.length; i++) {
        //                    let data = arrButton[i];

        //    let type = "";
        //                    if (data.hasOwnProperty("type")) {
        //                        type = data.type;
        //                    }

        //// if (type === 'sms') continue;
        //let pos = [];
        //if (data.hasOwnProperty("pos"))
        //{
        //    pos = data.pos;
        //}

        //let size = [];
        //if (data.hasOwnProperty("size"))
        //{
        //    size = data.size;
        //}

        //let urlBtn = "";
        //if (data.hasOwnProperty("btn"))
        //{
        //    urlBtn = data.btn;
        //}

        //let nodeBtn = new cc.Node("Btn");
        //nodeBkg.addChild(nodeBtn);

        //let imgBtn = nodeBtn.addComponent(cc.Sprite);



        //imgBtn.type = cc.Sprite.Type.SLICED;
        //imgBtn.sizeMode = cc.Sprite.SizeMode.CUSTOM;
        //imgBtn.spriteFrame = btn_normal;
        //nodeBtn.setContentSize(200, 70);
        //if (urlBtn !== "")
        //{
        //    cc.NGWlog("-----> URL BTN: ", urlBtn);
        //    cc.loader.load(urlCor + urlBtn, (err, tex) => {

        //        if (err != null || close) return;
        //        imgBtn.spriteFrame = new cc.SpriteFrame(tex);
        //        cc.NGWlog("load dc button ve============================= ")
        //                        });
        //}
        //cc.NGWlog("------> size  ", size);
        //if (size.length > 1)
        //{
        //    // nodeBtn.setContentSize(size[0], size[1]);
        //}
        //if (pos.length > 1)
        //{
        //    nodeBtn.position = cc.v2(
        //        nodeBkg.width * pos[0],
        //        nodeBkg.height * pos[1]
        //    );
        //}

        //let btnTemp = nodeBtn.addComponent(cc.Button);
        //// btnTemp.target = nodeBtn;
        //btnTemp.transition = cc.Button.Transition.COLOR;

        //let urllink = "";
        //if (data.hasOwnProperty("urllink"))
        //{
        //    urllink = data.urllink;
        //}

        //let urlPopupSms = "";
        //if (data.hasOwnProperty("urlPopupSms"))
        //{
        //    urlPopupSms = data.urlPopupSms;
        //}

        //let styleButton = -1;
        //if (data.hasOwnProperty("styleButton"))
        //{
        //    styleButton = data.styleButton;
        //}

        //let isFull = false;
        //if (data.hasOwnProperty("isFull"))
        //{
        //    isFull = data.isFull;
        //}

        //let isClose = false;
        //if (data.hasOwnProperty("isClose"))
        //{
        //    isClose = data.isClose;
        //}

        //let itemID = "";
        //if (data.hasOwnProperty("itemID"))
        //{
        //    itemID = data.itemID;
        //}

        //let ccost = 0;
        //if (data.hasOwnProperty("ccost"))
        //{
        //    ccost = data.ccost;
        //}
        //let value = 0;
        //if (data.hasOwnProperty("value"))
        //{
        //    value = data.value;
        //}

        //let amount;
        //if (data.hasOwnProperty("amount"))
        //{
        //    amount = data.amount;
        //}

        //let bonus = "";
        //if (data.hasOwnProperty("bonus"))
        //{
        //    bonus = data.bonus;
        //}
        //let suggestBet = 0;
        //if (data.hasOwnProperty("suggestBet"))
        //{
        //    suggestBet = data.suggestBet;
        //}


        //nodeBtn.on(cc.Node.EventType.TOUCH_END, function(event) {
        //    cc.NGWlog("--------> Type Click Button:  ", type.toUpperCase());
        //    switch (type.toUpperCase())
        //    {
        //        case "SMS":
        //        case "CARD":
        //        case "OK":
        //        case "":
        //            {
        //                //Open Shop
        //                UIManager.instance.onShowShop();
        //                break;
        //            }
        //        case "OPENLINK":
        //            {
        //                // urllink/
        //                urllink = urllink.replace("%userid%", require('GameManager').getInstance().user.id);
        //                urllink = urllink.replace("%uid%", require('GameManager').getInstance().user.id);
        //                cc.NGWlog("sio: OPENLINK urllink: " + urllink);
        //                // if (urllink.indexOf("%userid%") != -1 || urllink.indexOf("%uid%") != -1){
        //                cc.sys.openURL(urllink);
        //                // }


        //                break;
        //            }
        //        case "SHOWWEBVIEW":
        //            {
        //                cc.NGWlog("sio: SHOWWEBVIEW urllink: " + urllink);
        //                _OpenWebviewNapTien(urllink)
        //                                break;
        //            }
        //        case "SHARE":
        //            {
        //                break;
        //            }
        //        case "BONGDA":
        //            {
        //                break;
        //            }
        //        case "UPDATE":
        //            {
        //                break;
        //            }
        //        case "AGGIFT":
        //            {
        //                break;
        //            }
        //        case "AGDAILY":
        //            {
        //                break;
        //            }
        //        case "CAPSA":
        //        case "SUGGESTBET":
        //            {
        //                break;
        //            }
        //        case "IAP":
        //            {
        //                Util.onBuyIap("itemID");
        //                break;
        //            }
        //        case "VIDEO":
        //            {
        //                break;
        //            }
        //    }

        //    //dong banner lai
        //    nodeBanner.destroy();
        //    UIManager.instance.indexCurrentDataBanner++;
        //    UIManager.instance.showBanner();
        //    close = true;
        //});

        //_node.runAction(cc.repeat(cc.sequence(cc.callFunc(() => {
        //    if (require("GameManager").getInstance().gameView != null)
        //    {
        //        cc.NGWlog("Vao day la tat banner luon");
        //        nodeBanner.destroy();
        //        _node.stopAllActions();
        //    }
        //}), cc.delayTime(0.2)), 150));

        //let showTextButton = false;
        //if (data.hasOwnProperty("showTextButton"))
        //{
        //    showTextButton = data.showTextButton;
        //}
        //if (!showTextButton)
        //{
        //    continue;
        //}
        //                }

        //            });
        //        }
        //        cc.loader.load(urlCor + urlBkg, (err, tex) => {

        //        });
    }
    public void onShowListBaner()
    {
        //cc.NGWlog('onShowListBaner length', arrBanerOnList.length)
        //    if (arrBanerOnList.length <= 0) return;
        //let item = cc.instantiate(Global.ListBaner).getComponent("ListBaner");
        //item.node._tag = TAG.BANNER;
        //node.addChild(item.node);
        //item.init(arrBanerOnList);
    }
    //preLoadBaner(data) {
    //return;
    //let length = data.length;
    //let urlCor = "https://cors-anywhere.herokuapp.com/";
    //if (cc.sys.isNative)
    //{
    //    urlCor = "";
    //}
    //for (let i = 0; i < length; i++)
    //{
    //    cc.NGWlog("url tai la=== " + data[i].urlImg);
    //    if (data[i].urlImg != null && data[i].urlImg != "")
    //    {
    //        cc.loader.load(urlCor + data[i].urlImg, (err, res) => { })
    //        }

    //}
    //}

    public KeyboardController showKeyboardCustom(Transform tfParrent)
    {
        var inputKeyBoard = Instantiate(loadPrefabPopup("KeyboardController"), tfParrent).GetComponent<KeyboardController>();
        inputKeyBoard.transform.localScale = Vector3.one;
        return inputKeyBoard;
    }
    public void checkAlertMail(bool isEvt22 = true)
    {
        lobbyView.checkAlertMail(isEvt22);
    }
    public void userHasNewMailAdmin()
    {
        if (FreeChipView.instance != null && FreeChipView.instance.gameObject.activeSelf)
        {
            SocketSend.getMail(12);
        }
        //
        Globals.User.userMain.nmAg++;
        updateMailAndMessageNoti();
        if (FreeChipView.instance != null && FreeChipView.instance.gameObject.activeSelf)
        {
            showToast(getTextConfig("has_mail_show_gold").Split(",")[0]);
            return;
        }
        bool isIngame = (gameView != null && gameView.gameObject.activeSelf);
        string textShow = getTextConfig("has_mail_show_gold");
        if (isIngame)
        {
            textShow = textShow.Split(",")[0];
            showToast(textShow);
        }
        else
        {
            showDialog(textShow, getTextConfig("ok"), () =>
            {
                //if(ProfileView.instance != null)
                //{
                //    ProfileView.instance.hide();
                //}
                //if()

                UIManager.instance.clearPopupLobby();
                UIManager.instance.destroyAllPopup();
                clickTabFreeChip();
            }, getTextConfig("label_cancel"));
        }

    }

    public void FixedUpdate()
    {
        foreach (var wwload in listDataLoad.ToArray())//new List<DataLoadImage>(listDataLoad))
        {
            if (wwload != null && !wwload.isDone && wwload.www.isDone)
            {
                wwload.isDone = true;
                if (wwload.sprite != null && !wwload.sprite.IsDestroyed())
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(wwload.www);
                    wwload.sprite.sprite = Sprite.Create(texture,
                    new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }

                if (wwload.callback != null)
                {
                    wwload.callback.Invoke(DownloadHandlerTexture.GetContent(wwload.www));
                }
                if (wwload.callback2 != null)
                {
                    wwload.callback2.Invoke();
                }
            }
        }

        for (var i = 0; i < listDataLoad.Count; i++)
        {
            if (listDataLoad[i].isDone)
            {
                listDataLoad.RemoveAt(i);
                i--;
            }
        }
    }

    List<DataLoadImage> listDataLoad = new List<DataLoadImage>();
    public void addJobLoadImage(DataLoadImage dataLoadImage)
    {
        listDataLoad.Add(dataLoadImage);
    }

    public void sendLog(string str, bool isDel)
    {
        return;
        //StartCoroutine(sendLog(str, isDel));
    }
}
