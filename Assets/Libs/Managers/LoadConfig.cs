using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
public class CertificateWhore : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
public class LoadConfig : MonoBehaviour
{
    public static LoadConfig instance;
    string url_start = "https://conf.topbangkokclub.com/info";
    string config_info = "";

    public bool isLoadedConfig = false;
    void Awake()
    {
        instance = this;
        if (Application.platform == RuntimePlatform.Android)
        {
            config_info = @"{""gamenotification"":false,""allowPushOffline"":true,""is_reg"":false,""isShowLog"":false,""is_login_guest"":true,""is_login_fb"":true,""time_request"":5,""avatar_change"":2,""avatar_count"":10,""avatar_build"":""https://cdn.topbangkokclub.com/api/public/dl/VbfRjo1c/avatar/%avaNO%.png"",""avatar_fb"":""https://graph.facebook.com/v9.0/%fbID%/picture?width=200&height=200&redirect=true&access_token=%token%"",""name_fb"":""https://graph.facebook.com/%userID%/?fields=name&access_token=%token%"",""text"":[{""lang"":""EN"",""url"":""https://conf.topbangkokclub.com/textEnglish""},{""lang"":""THAI"",""url"":""https://conf.topbangkokclub.com/textThai""}],""url_help"":"""",""bundleID"":""71D97F59-4763-5A1E-8862-B29980CF2D4C"",""version"":""1.20"",""operatorID"":7000,""os"":""android_unity"",""publisher"":""dummy_co_1_10"",""disID"":1007,""fbprivateappid"":"""",""fanpageID"":"""",""groupID"":"""",""hotline"":"""",""listGame"":[{""id"":8015,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8100,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8013,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8010,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8802,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9008,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":9007,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8818,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9950,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9900,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2}],""u_chat_fb"":"""",""infoChip"":"""",""infoDT"":"""",""infoBNF"":""https://conf.topbangkokclub.com/infoBNF"",""url_rule_js_new"":"""",""delayNoti"":[{""time"":5,""title"":""Pusoy"",""text"":""⚡️ Chip Free ⚡️"",""ag"":100000},{""time"":600,""title"":""Pusoy"",""text"":""💰Chip Free 💰"",""ag"":0},{""time"":86400,""title"":""Pusoy"",""text"":""⏰ Chip Free ⏰"",""ag"":0}],""data0"":false,""infoUser"":"""",""umode"":0,""uop1"":""Quit"",""umsg"":""This version don't allow to play game"",""utar"":"""",""newest_versionUrl"":""""}";

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            config_info = @"{""gamenotification"":false,""allowPushOffline"":true,""is_reg"":false,""isShowLog"":false,""is_login_guest"":true,""is_login_fb"":true,""time_request"":5,""avatar_change"":2,""avatar_count"":10,""avatar_build"":""https://cdn.topbangkokclub.com/api/public/dl/VbfRjo1c/avatar/%avaNO%.png"",""avatar_fb"":""https://graph.facebook.com/v9.0/%fbID%/picture?width=200&height=200&redirect=true&access_token=%token%"",""name_fb"":""https://graph.facebook.com/%userID%/?fields=name&access_token=%token%"",""text"":[{""lang"":""EN"",""url"":""https://conf.topbangkokclub.com/textEnglish""},{""lang"":""THAI"",""url"":""https://conf.topbangkokclub.com/textThai""}],""url_help"":"""",""bundleID"":""71D97F59-4763-5A1E-8862-B29980CF2D4C"",""version"":""1.20"",""operatorID"":7000,""os"":""android_unity"",""publisher"":""dummy_co_1_10"",""disID"":1007,""fbprivateappid"":"""",""fanpageID"":"""",""groupID"":"""",""hotline"":"""",""listGame"":[{""id"":8015,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8100,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8013,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8010,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8802,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9008,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":9007,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8818,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9950,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9900,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2}],""u_chat_fb"":"""",""infoChip"":"""",""infoDT"":"""",""infoBNF"":"""",""url_rule_js_new"":"""",""delayNoti"":[{""time"":5,""title"":""Pusoy"",""text"":""⚡️ Chip Free ⚡️"",""ag"":100000},{""time"":600,""title"":""Pusoy"",""text"":""💰Chip Free 💰"",""ag"":0},{""time"":86400,""title"":""Pusoy"",""text"":""⏰ Chip Free ⏰"",""ag"":0}],""data0"":false,""infoUser"":"""",""umode"":0,""uop1"":""Quit"",""umsg"":""This version don't allow to play game"",""utar"":"""",""newest_versionUrl"":""""}";

        }
        else
        {
            config_info = @"{""gamenotification"":false,""allowPushOffline"":true,""is_reg"":false,""isShowLog"":false,""is_login_guest"":true,""is_login_fb"":true,""time_request"":5,""avatar_change"":2,""avatar_count"":10,""avatar_build"":""https://cdn.topbangkokclub.com/api/public/dl/VbfRjo1c/avatar/%avaNO%.png"",""avatar_fb"":""https://graph.facebook.com/v9.0/%fbID%/picture?width=200&height=200&redirect=true&access_token=%token%"",""name_fb"":""https://graph.facebook.com/%userID%/?fields=name&access_token=%token%"",""text"":[{""lang"":""EN"",""url"":""https://conf.topbangkokclub.com/textEnglish""},{""lang"":""THAI"",""url"":""https://conf.topbangkokclub.com/textThai""}],""url_help"":""https://paragon.policy.topbangkokclub.com"",""bundleID"":""71D97F59-4763-5A1E-8862-B29980CF2D4C"",""version"":""1.20"",""operatorID"":7000,""os"":""android_unity"",""publisher"":""dummy_co_1_10"",""disID"":1007,""fbprivateappid"":"""",""fanpageID"":"""",""groupID"":"""",""hotline"":"""",""listGame"":[{""id"":8015,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8100,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8013,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8010,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8802,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9008,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":9007,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8818,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9950,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9900,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2}],""u_chat_fb"":"""",""infoChip"":"""",""infoDT"":"""",""infoBNF"":"""",""url_rule_js_new"":"""",""delayNoti"":[{""time"":5,""title"":""Pusoy"",""text"":""⚡️ Chip Free ⚡️"",""ag"":100000},{""time"":600,""title"":""Pusoy"",""text"":""💰Chip Free 💰"",""ag"":0},{""time"":86400,""title"":""Pusoy"",""text"":""⏰ Chip Free ⏰"",""ag"":0}],""data0"":false,""infoUser"":"""",""umode"":0,""uop1"":""Quit"",""umsg"":""This version don't allow to play game"",""utar"":"""",""newest_versionUrl"":""""}";
        }

        var configOff = PlayerPrefs.GetString("config_save", "");
        init();
        Debug.Log("Isload Config Off:" + configOff.Equals(""));
        handleConfigInfo(configOff.Equals("") ? config_info : configOff);
        isLoadedConfig = false;
        getConfigInfo();
    }

    void init()
    {
        Globals.Config.deviceId = SystemInfo.deviceUniqueIdentifier;
        //Globals.Config.versionGame = Application.version;

    }

    //IEnumerator GetRequest(string uri, WWWForm wwwForm, System.Action<string> callback)
    //{
    //    //Thread trd = new Thread(new ThreadStart(()=> {
    //    Globals.Logging.Log("-=-=uri " + uri);
    //    using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, wwwForm))
    //        {
    //        // Request and wait for the desired page.
    //        yield return webRequest.SendWebRequest();

    //        Globals.Logging.Log("Received: " + webRequest.downloadHandler.text);
    //        //Globals.Logging.Log("Received code: " + webRequest.responseCode);

    //        if (!webRequest.isNetworkError)
    //        {
    //            callback.Invoke(webRequest.downloadHandler.text);
    //        }
    //        else {
    //            Globals.Logging.LogError(webRequest.error);
    //        }
    //        }
    //    //}));

    //    //trd.Start();
    //}

    async void ProgressHandle(string url, string json, Action<string> callback, Action callbackError = null)
    {
        UIManager.instance.showWatting();
        UnityWebRequest www = new UnityWebRequest(url, "POST");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        //uwr.SetRequestHeader("Content-Type", "application/json");
        www.certificateHandler = new CertificateWhore();
        // begin request:
        www.SetRequestHeader("Access-Control-Allow-Origin", "*");
        www.SetRequestHeader("Content-type", "application/json; charset=UTF-8");
        if (Application.isMobilePlatform)
            www.SetRequestHeader("X-Requested-With", "XMLHttpRequest");
        var asyncOp = www.SendWebRequest();

        //// await until it's done: 
        while (!asyncOp.isDone)
        {
            await Task.Yield();
            //await Task.Delay(200);//30 hertz
        }
        UIManager.instance.hideWatting();
        // read results:
        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.DataProcessingError)
        {
            Globals.Logging.Log("Error While Sending: " + www.error);
            if (callbackError != null)
            {
                callbackError.Invoke();
            }
        }
        else
        {
            //Globals.Logging.Log("Received: " + www.downloadHandler.text);
            callback.Invoke(www.downloadHandler.text);

        }

        //StartCoroutine(GetRequest(url, json, callback));
    }

    //IEnumerator GetRequest(string url, string json, Action<string> callback, Action callbackError = null)
    //{

    //    //Globals.Logging.Log("===> datapost ===>> : " + json);
    //    UIManager.instance.showWatting();
    //    var uwr = new UnityWebRequest(url, "POST");
    //    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
    //    uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    //    uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //    //uwr.SetRequestHeader("Content-Type", "application/json");
    //    uwr.certificateHandler = new CertificateWhore();
    //    //    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
    //    //(sender, certificate, chain, sslPolicyErrors) => true;

    //    uwr.SetRequestHeader("Access-Control-Allow-Origin", "*");
    //    uwr.SetRequestHeader("Content-type", "application/json; charset=UTF-8");
    //    if (Application.isMobilePlatform)
    //        uwr.SetRequestHeader("X-Requested-With", "XMLHttpRequest");

    //    //Send the request then wait here until it returns
    //    yield return uwr.SendWebRequest();

    //    UIManager.instance.hideWatting();
    //    if (uwr.result == UnityWebRequest.Result.ConnectionError)
    //    {
    //        Globals.Logging.Log("Error While Sending: " + uwr.error);
    //        if (callbackError != null)
    //        {
    //            callbackError.Invoke();
    //        }
    //    }
    //    else
    //    {
    //        Globals.Logging.Log("Received2: " + uwr.downloadHandler.text);
    //        callback.Invoke(uwr.downloadHandler.text);
    //    }
    //}

    JObject createBodyJsonNormal()
    {

        var osName = "android_unity";
        if (Application.platform == RuntimePlatform.Android)
            osName = "android_unity";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            osName = "ios_unity";

        //form.AddField("os", osName);
        //form.AddField("mcc", "[0,0]");

        JObject wWForm = new JObject();
        wWForm["bundleID"] = Globals.Config.package_name;
        wWForm["version"] = Globals.Config.versionGame + "";
        wWForm["operatorID"] = Globals.Config.OPERATOR + "";
        wWForm["publisher"] = Globals.Config.publisher + "";
        wWForm["os"] = osName;
        wWForm["mcc"] = "[0,0]";
        if (Globals.User.userMain != null)
        {
            wWForm["vip"] = Globals.User.userMain.VIP;
        }

        return wWForm;
    }

    JObject createBodyJson()
    {
        var wWForm = createBodyJsonNormal();
        if (Globals.User.userMain != null)
        {
            wWForm["id"] = Globals.User.userMain.Userid + "";
            wWForm["ag"] = Globals.User.userMain.AG + "";
            wWForm["lq"] = Globals.User.userMain.LQ + "";
            wWForm["vip"] = Globals.User.userMain.VIP;
            wWForm["group"] = (int)Globals.User.userMain.Group + "";
        }
        return wWForm;
    }


    public void getConfigInfo()
    {
        //loadInfo();
        var wWForm = createBodyJsonNormal();
        Debug.Log("-=-=getConfigInfo   " + wWForm.ToString());
        //StartCoroutine(GetRequest(url_start, wWForm.ToString(), handleConfigInfo));
        ProgressHandle(url_start, wWForm.ToString(), handleConfigInfo);
    }


    public void getInfoUser(string _data0)
    {
        var wWForm = createBodyJson();
        if (Globals.Config.data0)
            wWForm["data0"] = _data0;

        Debug.Log("-=-=getInfoUser   " + wWForm.ToString());
        //StartCoroutine(GetRequest(Globals.Config.infoUser, wWForm.ToString(), handleUserInfo));
        ProgressHandle(Globals.Config.infoUser, wWForm.ToString(), handleUserInfo);
    }

    public void getInfoShop(Action<string> callback, Action callbackError = null)
    {
        var wWForm = createBodyJson();
        //Globals.Logging.Log("JSON GetShop:" + wWForm);

        //Debug.Log("-=-=Globals.Config.infoChip url==" + Globals.Config.infoChip);
        if (Globals.Config.infoChip == "")
        {
            if (callbackError != null)
            {
                callbackError.Invoke();
            }
        }
        else
        {
            ProgressHandle(Globals.Config.infoChip, wWForm.ToString(), callback, callbackError);
        }

    }

    public void getInfoEX(Action<string> callback)
    {
        var wWForm = createBodyJson();
        //StartCoroutine(GetRequest(Globals.Config.infoDT, wWForm.ToString(), callback));
        ProgressHandle(Globals.Config.infoDT, wWForm.ToString(), callback);
    }

    public void getInfoBenefit(Action<string> callback)
    {
        var wWForm = createBodyJson();
        //StartCoroutine(GetRequest(Globals.Config.infoBNF, wWForm.ToString(), callback));
        ProgressHandle(Globals.Config.infoBNF, wWForm.ToString(), callback);
    }
    public void getTextConfig(string _url, string _language, bool isInit)
    {
        var wWForm = createBodyJsonNormal();
        //StartCoroutine(GetRequest(_url, wWForm.ToString(), (string strData) =>
        //{
        //    //Globals.Logging.Log("___ language  " + _language);
        //    //Globals.Logging.Log(_url + ": " + strData);
        //    JObject jConfig = null;
        //    try
        //    {
        //        jConfig = JObject.Parse(strData);
        //    }
        //    catch (Exception e)
        //    {
        //        Globals.Logging.LogException(e);
        //    }

        //    if (jConfig == null) return;
        //    var key = "config_text_" + _language.ToUpper();
        //    PlayerPrefs.SetString(key, strData);
        //    if (isInit)
        //        Globals.Config.loadTextConfig();
        //}));

        ProgressHandle(_url, wWForm.ToString(), (string strData) =>
        {
            //Globals.Logging.Log("___ language  " + _language);
            //Globals.Logging.Log(_url + ": " + strData);
            JObject jConfig = null;
            try
            {
                jConfig = JObject.Parse(strData);
            }
            catch (Exception e)
            {
                Globals.Logging.LogException(e);
            }

            if (jConfig == null) return;
            var key = "config_text_" + _language.ToUpper();
            PlayerPrefs.SetString(key, strData);
            if (isInit)
                Globals.Config.loadTextConfig();
        });
    }

    void handleConfigInfo(string strData)
    {
        //strData = @"{""gamenotification"":false,""allowPushOffline"":true,""is_reg"":false,""isShowLog"":false,""is_login_guest"":true,""is_login_fb"":true,""time_request"":5,""avatar_change"":2,""avatar_count"":10,""avatar_build"":""https:\/\/cdn.topbangkokclub.com\/api\/public\/dl\/VbfRjo1c\/avatar\/%avaNO%.png"",""avatar_fb"":""https:\/\/graph.facebook.com\/v9.0\/%fbID%\/picture?width=200&height=200&redirect=true&access_token=%token%"",""name_fb"":""https:\/\/graph.facebook.com\/%userID%\/?fields=name&access_token=%token%"",""text"":[{""lang"":""EN"",""url"":""https:\/\/conf.topbangkokclub.com\/textEnglish""},{""lang"":""THAI"",""url"":""https:\/\/conf.topbangkokclub.com\/textThai""}],""bundleID"":""dummy.co.slots"",""version"":""1.10"",""operatorID"":7000,""os"":""android_unity"",""publisher"":""dummy_co_1_10"",""disID"":1079,""fbprivateappid"":"""",""fanpageID"":""100079966884463"",""groupID"":""678006740187009"",""hotline"":""Fb.com\/100079966884463"",""listGame"":[{""id"":8015,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8100,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8013,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8010,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":8802,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9008,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":10000,""v_tb"":2},{""id"":9007,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":8818,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9950,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2},{""id"":9900,""ip"":""35.240.208.204"",""ip_dm"":""app1.topbangkokclub.com"",""agSvipMin"":25000,""v_tb"":2}],""u_chat_fb"":""https:\/\/m.me\/100079966884463"",""infoChip"":""https:\/\/conf.topbangkokclub.com\/infoChip"",""infoDT"":""https:\/\/conf.topbangkokclub.com\/infoDT"",""infoBNF"":""https:\/\/conf.topbangkokclub.com\/infoBNF"",""url_rule_js_new"":""https:\/\/conf.topbangkokclub.com\/rule\/index.html?gameid=%gameid%&language=%language%&list=true"",""url_help"":""https:\/\/paragon.policy.topbangkokclub.com"",""url_rule_refGuide"":""https:\/\/conf.topbangkokclub.com\/refGuide\/"",""delayNoti"":[{""time"":5,""title"":""Pusoy"",""text"":""\u26a1\ufe0f Chip Free \u26a1\ufe0f"",""ag"":100000},{""time"":600,""title"":""Pusoy"",""text"":""\ud83d\udcb0Chip Free \ud83d\udcb0"",""ag"":0},{""time"":86400,""title"":""Pusoy"",""text"":""\u23f0 Chip Free \u23f0"",""ag"":0}],""data0"":false,""u_SIO"":""https:\/\/sio.topbangkokclub.com\/dummy.co.slots"",""listTop"":[{""id"":8015,""url_img_js"":""https:\/\/conf.topbangkokclub.com\/image\/top\/8813_js_1.png""},{""id"":8100,""url_img_js"":""https:\/\/conf.topbangkokclub.com\/image\/top\/8813_js_1.png""},{""id"":8013,""url_img_js"":""https:\/\/conf.topbangkokclub.com\/image\/top\/8813_js_1.png""},{""id"":8010,""url_img_js"":""https:\/\/conf.topbangkokclub.com\/image\/top\/8813_js_1.png""},{""id"":8802,""url_img_js"":""https:\/\/conf.topbangkokclub.com\/image\/top\/8813_js_1.png""}],""infoUser"":""https:\/\/conf.topbangkokclub.com\/infoUser"",""umode"":0,""uop1"":""Quit"",""umsg"":""This version don't allow to play game"",""utar"":"""",""newest_versionUrl"":""https:\/\/play.google.com\/store\/apps\/details?id=dummy.co.slots""}";//dev
        PlayerPrefs.SetString("config_save", strData);
        isLoadedConfig = true;
        Debug.Log("-=-=handleConfigInfo: " + strData);
        JObject jConfig = null;
        try
        {
            jConfig = JObject.Parse(strData);
        }
        catch (Exception e)
        {
            Globals.Logging.LogException(e);
        }

        if (jConfig == null) return;
        //Globals.Logging.Log("-=-=-=-=-=-=-=-=-= 1");
        //Globals.Logging.Log(jConfig);

        if (jConfig.ContainsKey("gamenotification"))
            Globals.Config.gamenotification = (bool)jConfig["gamenotification"];
        if (jConfig.ContainsKey("allowPushOffline"))
            Globals.Config.allowPushOffline = (bool)jConfig["allowPushOffline"];
        if (jConfig.ContainsKey("is_reg"))
            Globals.Config.is_reg = (bool)jConfig["is_reg"];
        if (jConfig.ContainsKey("isShowLog"))
            Globals.Config.isShowLog = (bool)jConfig["isShowLog"];
        if (jConfig.ContainsKey("is_login_guest"))
            Globals.Config.is_login_guest = (bool)jConfig["is_login_guest"];
        if (jConfig.ContainsKey("is_login_fb_open"))
            Globals.Config.is_login_fb_open = (bool)jConfig["is_login_fb_open"];
        if (jConfig.ContainsKey("is_login_fb"))
            Globals.Config.is_login_fb = (bool)jConfig["is_login_fb"];
        if (jConfig.ContainsKey("time_request"))
            Globals.Config.time_request = (int)jConfig["time_request"];
        if (jConfig.ContainsKey("avatar_change"))
            Globals.Config.avatar_change = (int)jConfig["avatar_change"];
        if (jConfig.ContainsKey("avatar_count"))
            Globals.Config.avatar_count = (int)jConfig["avatar_count"];
        if (jConfig.ContainsKey("avatar_build"))
            Globals.Config.avatar_build = (string)jConfig["avatar_build"];

        if (jConfig.ContainsKey("u_SIO"))
        {
            Globals.Config.u_SIO = (string)jConfig["u_SIO"];
            Globals.Logging.LogWarning("-=-=-u_SIO-=-=-=-=-=-=-=-=-  " + Globals.Config.u_SIO);
            SocketIOManager.getInstance().intiSml();
            if (SocketIOManager.getInstance().DATAEVT0 != null)
            {
                SocketIOManager.getInstance().startSIO();
            }

        }
        else
        {
            Globals.Config.u_SIO = "";
        }

        if (jConfig.ContainsKey("avatar_fb"))
            Globals.Config.avatar_fb = (string)jConfig["avatar_fb"];
        if (jConfig.ContainsKey("name_fb"))
            Globals.Config.name_fb = (string)jConfig["name_fb"];
        if (jConfig.ContainsKey("text"))
        {
            Globals.Config.listTextConfig = jConfig["text"] as JArray;//arr
            for (var i = 0; i < Globals.Config.listTextConfig.Count; i++)
            {
                JObject itemLanguage = (JObject)Globals.Config.listTextConfig[i];
                getTextConfig((string)itemLanguage["url"], (string)itemLanguage["lang"], i >= Globals.Config.listTextConfig.Count - 1);
            }
        }
        if (jConfig.ContainsKey("disID"))
            Globals.Config.disID = (int)jConfig["disID"];

        Globals.Logging.Log("-=-=disID   " + Globals.Config.disID);
        if (jConfig.ContainsKey("fbprivateappid"))
            Globals.Config.fbprivateappid = (string)jConfig["fbprivateappid"];
        if (jConfig.ContainsKey("fanpageID"))
            Globals.Config.fanpageID = (string)jConfig["fanpageID"];
        if (jConfig.ContainsKey("groupID"))
            Globals.Config.groupID = (string)jConfig["groupID"];
        if (jConfig.ContainsKey("hotline"))
            Globals.Config.hotline = (string)jConfig["hotline"];

        if (jConfig.ContainsKey("listGame"))
            Globals.Config.listGame = jConfig["listGame"] as JArray;//array

        if (Globals.Config.listGame.Count > 0)
        {
            JObject datGa = (JObject)Globals.Config.listGame[0];
            if (Globals.Config.curGameId == 0)
            {
                Globals.Config.curGameId = (int)datGa["id"];
            }
            Globals.Config.curServerIp = (string)datGa["ip_dm"];
            PlayerPrefs.SetInt("curGameId", Globals.Config.curGameId);
            PlayerPrefs.SetString("curServerIp", Globals.Config.curServerIp);
        }
        if (jConfig.ContainsKey("listTop"))
        {

            Globals.Config.listRankGame = jConfig["listTop"] as JArray;//array
        }
        else
        {
            Globals.Config.listRankGame.Clear();
        }

        if (jConfig.ContainsKey("u_chat_fb"))
            Globals.Config.u_chat_fb = (string)jConfig["u_chat_fb"];
        if (jConfig.ContainsKey("infoChip"))
        {
            Globals.Config.infoChip = (string)jConfig["infoChip"];
        }
        else
        {
            Globals.Config.infoChip = "";
        }
        if (jConfig.ContainsKey("infoDT"))
            Globals.Config.infoDT = (string)jConfig["infoDT"];
        if (jConfig.ContainsKey("infoBNF"))
        {
            Globals.Config.infoBNF = (string)jConfig["infoBNF"];
            getInfoBenefit((res) =>
            {
                if (res == "") return;
                var objData = JObject.Parse(res);
                if (objData.ContainsKey("jackpot"))
                {
                    Globals.Config.listRuleJackPot.Clear();

                    var data = (JArray)objData["jackpot"];

                    for (var i = 0; i < data.Count; i++)
                    {
                        JObject item = new JObject();
                        item["gameid"] = data[i]["gameid"];
                        JArray arrMark = new JArray();
                        JArray arrChip = new JArray();
                        JArray mark = (JArray)data[i]["mark"];
                        JArray chip = (JArray)data[i]["chip"];

                        for (var id = 0; id < mark.Count; id++)
                        {
                            arrMark.Add(mark[id]);
                            arrChip.Add(chip[id]);
                        }
                        item["listMark"] = arrMark;
                        item["listChip"] = arrChip;
                        Globals.Config.listRuleJackPot.Add(item);
                        Globals.Config.listVipBonusJackPot.Add(data[i]["bonus_vip"]);
                    }
                }

                if (objData.ContainsKey("agContactAd"))
                    Globals.Config.agContactAd = (int)objData["agContactAd"];
                if (objData.ContainsKey("agRename"))
                    Globals.Config.agRename = (int)objData["agRename"];

            });
        }
        if (jConfig.ContainsKey("url_rule_js_new"))
            Globals.Config.url_rule = (string)jConfig["url_rule_js_new"];
        if (jConfig.ContainsKey("url_help"))
            Globals.Config.url_help = (string)jConfig["url_help"];
        if (jConfig.ContainsKey("url_rule_refGuide"))
            Globals.Config.url_rule_refGuide = (string)jConfig["url_rule_refGuide"];
        if (jConfig.ContainsKey("delayNoti"))
            Globals.Config.delayNoti = jConfig["delayNoti"] as JArray;//array
        Globals.Config.data0 = jConfig.ContainsKey("") ? (bool)jConfig["data0"] : false;
        if (jConfig.ContainsKey("infoUser"))
            Globals.Config.infoUser = (string)jConfig["infoUser"];

        if (jConfig.ContainsKey("newest_versionUrl"))
            Globals.Config.newest_versionUrl = (string)jConfig["newest_versionUrl"];

        var umode = jConfig.ContainsKey("umode") ? (int)jConfig["umode"] : 0;
        var uop1 = jConfig.ContainsKey("uop1") ? (string)jConfig["uop1"] : "";
        var uop2 = jConfig.ContainsKey("uop2") ? (string)jConfig["uop2"] : "";
        var umsg = jConfig.ContainsKey("umsg") ? (string)jConfig["umsg"] : "";
        var utar = jConfig.ContainsKey("utar") ? (string)jConfig["utar"] : "";
        //Globals.Logging.Log("dmmm    " + umode);
        updateConfigUmode(umode, uop1, uop2, utar, umsg);
        UIManager.instance.refreshUIFromConfig();


        PlayerPrefs.Save();
    }

    void handleUserInfo(string strData)
    {
        //-=-= handleUserInfo { "bundleID":"7E26B7BB-77C6-5938-AF2B-401DFB79724A","version":"1.00","operatorID":7000,"os":"android_unity","publisher":"dummy_co_1_10","disID":1006,"ketPhe":5,"is_dt":true,"ketT":true,"ket":true,"ismaqt":true,"is_bl_salert":true,"is_bl_fb":true,"is_xs":false}
        Globals.Logging.Log("-=-=handleUserInfo " + strData);
        JObject jConfig = null;
        try
        {
            jConfig = JObject.Parse(strData);
        }
        catch (Exception e)
        {
            Globals.Logging.LogException(e);
        }

        if (jConfig == null) return;
        Globals.Logging.Log("-------------------->Config Game<------------------>\n" + jConfig);

        if (jConfig.ContainsKey("disID"))
            Globals.Config.disID = (int)jConfig["disID"];

        if (jConfig.ContainsKey("ketPhe"))
            Globals.Config.ketPhe = (int)jConfig["ketPhe"];
        if (jConfig.ContainsKey("is_dt"))
            Globals.Config.is_dt = (bool)jConfig["is_dt"];
        else
            Globals.Config.is_dt = false;
        if (jConfig.ContainsKey("ketT"))
            Globals.Config.ketT = (bool)jConfig["ketT"];
        if (jConfig.ContainsKey("ket"))
            Globals.Config.ket = (bool)jConfig["ket"];
        else
            Globals.Config.ket = false;
        if (jConfig.ContainsKey("ismaqt"))
            Globals.Config.ismaqt = (bool)jConfig["ismaqt"];
        else
            Globals.Config.ismaqt = false;
        if (jConfig.ContainsKey("is_bl_salert"))
            Globals.Config.is_bl_salert = (bool)jConfig["is_bl_salert"];
        else
            Globals.Config.is_bl_salert = false;
        if (jConfig.ContainsKey("is_bl_fb"))
            Globals.Config.is_bl_fb = (bool)jConfig["is_bl_fb"];
        else
            Globals.Config.is_bl_fb = false;
        if (jConfig.ContainsKey("is_xs"))
            Globals.Config.is_xs = (bool)jConfig["is_xs"];
        if (jConfig.ContainsKey("show_new_alert"))
            Globals.Config.show_new_alert = (bool)jConfig["show_new_alert"];
        else
            Globals.Config.show_new_alert = false;

        UIManager.instance.refreshUIFromConfig();
    }

    void updateConfigUmode(int umode, string uop1, string uop2, string utar, string umsg)
    {
        //// let umode = 0; /*FIXED CHANGE WHEN RELEASE*/
        //umode = 0;//dev de test
        Debug.Log("umode===" + umode);
        switch (umode)
        {
            case 0: // mode == 0, vao thang ko can hoi
                    //cc.NGWlog('umode0: show login');
                break;
            case 1: // mode == 1, hoi update, 2 lua chon
                UIManager.instance.showDialog(umsg, uop1, () =>
                {
                    Application.OpenURL(utar);
                    Application.Quit();
                }, uop2);
                break;
            case 2: // mode == 2, hoi update, khong lua chon
                UIManager.instance.showDialog(umsg, uop1, () =>
                {
                    Application.OpenURL(utar);
                    Application.Quit();
                });
                break;
            case 3: // mode == 3, thong bao, 1 lua chon OK va vao game
                UIManager.instance.showMessageBox(umsg);
                break;
            case 4:// mode == 4, thong bao, 1 lua chon OK va finish
                UIManager.instance.showMessageBox(umsg, () =>
                {
                    Application.Quit();
                });
                break;
        }
    }
}
