using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Facebook.Unity;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine.EventSystems;
using System;

public class LoginView : BaseView
{
    [SerializeField]
    Transform trInput;

    [SerializeField]
    TMP_InputField ipfAcc;
    [SerializeField]
    TMP_InputField ipfPass;

    [SerializeField]
    TMP_InputField ipfIP;

    [SerializeField]
    LoginWithIDView loginIDView;

    [SerializeField]
    GameObject isCheckTest;
    [SerializeField]
    GameObject btnLogin_fb;
    public string accPlayNow = "";
    public string passPlayNow = "";
    bool isOpenFirst = true;

    bool isLoginingFB = false;
    //public string fbid = "";
    //protected override void Awake()
    //{
    //    base.Awake();

    //    //Globals.Config.getDataUser();
    //}
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        var isFirstOpen = PlayerPrefs.GetInt("isFirstOpen", 0);
        Globals.Logging.Log("isFirstOpen " + isFirstOpen);
        if (isFirstOpen == 0)
        {
            PlayerPrefs.SetInt("isFirstOpen", 1);
            onClickPlayNow();
            PlayerPrefs.Save();
        }
        else
        {
            Globals.Logging.Log("isOpenFirst " + isOpenFirst);
            if (isOpenFirst)
            {
                isOpenFirst = false;

                reconnect();
            }
        }
    }

    public void reconnect()
    {
        UIManager.instance.showWatting();
        switch (Globals.Config.typeLogin)
        {
            case Globals.LOGIN_TYPE.NORMAL:
                {
                    SocketSend.sendLogin(Globals.Config.user_name, Globals.Config.user_pass, false);
                    break;
                }
            case Globals.LOGIN_TYPE.FACEBOOK:
                {
                    onClickLoginFB();
                    //SocketSend.sendLogin("", aToken.TokenString, false);
                    break;
                }
            case Globals.LOGIN_TYPE.PLAYNOW:
                {
                    SocketSend.onPlayNow();
                    break;
                }
            default:
                {
                    Globals.Logging.Log("dclm Xem lai di nhe !!!");
                    break;
                }
        }
        btnLogin_fb.SetActive(Globals.Config.is_login_fb_open);

    }

    protected override void OnEnable()
    {
        //Globals.Config.arrBannerLobby.Clear();
        Globals.Config.arrOnlistTrue.Clear();
        Globals.Config.listPreloadTexture.Clear();
        Globals.Config.invitePlayGame = true;
        Globals.Config.isLoginSuccess = false;
        SoundManager.instance.pauseMusic();
        Globals.Config.getDataUser();

        if (Globals.Config.typeLogin == Globals.LOGIN_TYPE.NORMAL)
        {
            //ipfAcc.text = Globals.Config.username_normal;
            //ipfPass.text = Globals.Config.password_normal;
            if (Globals.Config.username_normal != "")
                ipfAcc.text = Globals.Config.username_normal;
            //if (Globals.Config.password_normal != "")
            ipfPass.text = Globals.Config.password_normal;
        }
        //Globals.Config.typeLogin = Globals.LOGIN_TYPE.NONE;
        //PlayerPrefs.SetInt("type_login",(int)Globals.LOGIN_TYPE.NONE);
        //PlayerPrefs.Save();
        if (UIManager.instance != null)
            UIManager.instance.destroyAllPopup();
        Globals.CURRENT_VIEW.setCurView(Globals.CURRENT_VIEW.LOGIN_VIEW);
        checkTest();
        //if(AlertMessage.instance && AlertMessage.instance.gameObject.ac)
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            if (isLoginingFB)
            {
                onClickLoginFB();
            }
        }
        else
        {
            Globals.Logging.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;

        }
    }

    //List<int> listTest = new List<int>() { 1, 22, 13, 4, 51, 26, 7,48, 39 };

    public void onClickShowLoginID()
    {
        //var arrCode = ((JObject)meldData["meldObject"])["arrMeld"].ToObject<List<int>>();

        //Globals.Logging.Log("-=-= 0");
        //listTest.ForEach(tm => Globals.Logging.Log(tm + " "));


        //listTest.Sort((a, b) =>
        //{
        //    int N1 = 0, S1 = 0;
        //    int N2 = 0, S2 = 0;
        //   Globals.Config.decodeCard(a, ref N1, ref S1);
        //    Globals.Config.decodeCard(b, ref N2, ref S2);
        //    return S1.CompareTo(S2);
        //});

        //listTest.Sort((a, b) =>
        //{
        //    int N1 = 0, S1 = 0;
        //    int N2 = 0, S2 = 0;
        //    Globals.Config.decodeCard(a, ref N1, ref S1);
        //    Globals.Config.decodeCard(b, ref N2, ref S2);
        //    return N1.CompareTo(N2);
        //});

        //Globals.Logging.Log("-=-= 1");
        //listTest.ForEach(tm => Globals.Logging.Log(tm + " "));

        //return;
        loginIDView.show();
        if (Globals.Config.username_normal != "")
            ipfAcc.text = Globals.Config.username_normal;
        //if (Globals.Config.password_normal != "")
        ipfPass.text = Globals.Config.password_normal;
    }
    public void refreshUIFromConfig()
    {
        btnLogin_fb.SetActive(Globals.Config.is_login_fb_open);
    }

    public void onClickLoginFB()
    {
        SoundManager.instance.soundClick();
        isLoginingFB = true;
        if (!FB.IsInitialized)
        {
            Debug.Log("-=- !FB.IsInitialized");
            return;
        }
        Globals.Config.typeLogin = Globals.LOGIN_TYPE.FACEBOOK;
        if (FB.IsLoggedIn)
        {
            try
            {
                if (checkTokenExpirated())
                {
                    var aToken = AccessToken.CurrentAccessToken;


                    Globals.User.AccessToken = aToken.TokenString;
                    Globals.User.FacebookID = aToken.UserId;

                    // Print current access token's User ID
                    Globals.Logging.Log("token:  " + aToken.TokenString + " UserId: " + Globals.User.FacebookID);
                    SocketSend.sendLogin("", aToken.TokenString, false);
                }
                else
                {
                    var perms = new List<string>() { "public_profile" };
                    FB.LogInWithReadPermissions(perms, AuthCallback);
                }
            }
            catch (Exception e)
            {
                Debug.Log("errorFB=" + e);
            }

        }
        else
        {
            var perms = new List<string>() { "public_profile" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        isLoginingFB = false;
    }

    bool checkTokenExpirated()
    {
        var timeNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var timeEnd = new DateTimeOffset(AccessToken.CurrentAccessToken.ExpirationTime).ToUnixTimeMilliseconds();
        return timeEnd >= timeNow;
    }
    public void onClickLoginToken()
    {
        isLoginingFB = true;
        if (!FB.IsInitialized)
        {
            Debug.Log("-=- !FB.IsInitialized");
            return;
        }
        Globals.Config.typeLogin = Globals.LOGIN_TYPE.FACEBOOK;
        Globals.User.AccessToken = "GGQVlZANVVFYzVPYUdrcEJES01SSjF2MHgyQUNrNmtvTUp5eHFycTd2dy1tR1VyeHQ4SEZAjaGN2OHB5dkprOFdicU8zbDZAITXVoTmNQVmxFZAlduR1dKZAnZAXMG05SDVtMFhsUGQ0ZAFR4MU5ZAQUFFaVl3SWJvZAU9zOFpNcWdCbTdFNnQwQTZAJU3pCRngwNXNqaHpuS2tPUS1B";

        // Print current access token's User ID
        //Globals.Logging.Log("token:  " + aToken.TokenString + " UserId: " + aToken.UserId);
        SocketSend.sendLogin("", Globals.User.AccessToken, false);
    }
    private void AuthCallback(ILoginResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Globals.Logging.Log("Error: " + result.Error);
        }
        else if (result.Cancelled)
        {
            Globals.Logging.Log("Cancelled: Access Token could not be retrieved");
        }
        else
        {
            // Successfully logged user in
            // A popup notification will appear that says "Logged in as <User Name>"
            Globals.Logging.Log("Success: " + result.AccessToken.UserId);
            //fbid = result.AccessToken.UserId;
            Globals.User.FacebookID = result.AccessToken.UserId;
            Globals.Logging.Log("FacebookID: " + Globals.User.FacebookID);
        }
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
            Globals.User.AccessToken = aToken.TokenString;

            // Print current access token's User ID
            Globals.Logging.Log("token:  " + aToken.TokenString + " UserId: " + aToken.UserId);

            SocketSend.sendLogin("", aToken.TokenString, false);
        }
        else
        {
            Globals.Logging.Log("User cancelled login");
        }
    }

    public void onClickPlayNow()
    {
        Globals.Config.typeLogin = Globals.LOGIN_TYPE.PLAYNOW;
        SoundManager.instance.soundClick();

        UIManager.instance.showWatting();

        //PlayerPrefs.DeleteKey("USER_PLAYNOW");
        //PlayerPrefs.DeleteKey("PASS_PLAYNOW");
        SocketSend.onPlayNow();
    }

    public void onClickLogin()
    {
        SoundManager.instance.soundClick();
        var strAcc = ipfAcc.text;
        var strPass = ipfPass.text;


        if (strAcc.Equals("") || strPass.Equals(""))
        {
            return;
        }
        Globals.Config.user_name = strAcc;
        Globals.Config.user_pass = strPass;

        UIManager.instance.showWatting();
        Globals.Config.typeLogin = Globals.LOGIN_TYPE.NORMAL;
        SocketSend.sendLogin(strAcc, strPass, false);
        loginIDView.hide(false);
    }

    void checkTest()
    {
        Globals.Config.isSvTest = PlayerPrefs.GetInt("is_sv_test", 0) == 1;
        isCheckTest.SetActive(Globals.Config.isSvTest);
    }

    public void onClickTest()
    {
        isCheckTest.SetActive(!isCheckTest.activeSelf);
        Globals.Config.isSvTest = isCheckTest.activeSelf;
        PlayerPrefs.SetInt("is_sv_test", Globals.Config.isSvTest ? 1 : 0);
    }

    public void onClickSet()
    {
        //http://192.168.1.132:8080
        if (ipfIP.text.Equals("")) return;
        Globals.Config.url_log = "http://192.168.1." + ipfIP.text + ":3000";
    }
}
