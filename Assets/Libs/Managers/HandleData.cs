
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class HandleData
{
    public static void handleLoginResponse(string strData)
    {
        Globals.Logging.Log("-=- =handleLoginResponse:  " + strData);
        //LoginResponsePacket packet = JsonConvert.DeserializeObject<LoginResponsePacket>(strData);
        LoginResponsePacket packet = JsonUtility.FromJson<LoginResponsePacket>(strData);

        if (packet.status == Globals.CMD.OK)
        {
            
            if (Globals.Config.typeLogin == Globals.LOGIN_TYPE.NORMAL)
            {
                Globals.Config.setDataUser();
            }
            if (Globals.Config.typeLogin == Globals.LOGIN_TYPE.PLAYNOW)
            {
                PlayerPrefs.SetString("USER_PLAYNOW", UIManager.instance.loginView.accPlayNow);
                PlayerPrefs.SetString("PASS_PLAYNOW", UIManager.instance.loginView.passPlayNow);
                PlayerPrefs.Save();
            }

            PlayerPrefs.SetInt("type_login", (int)Globals.Config.typeLogin);

            string data = Globals.Config.Base64Decode(packet.credentials);
           Debug.Log("-=- =dang nhap thanh cong:  " + data);
            //data = data.Replace("\"VIP\":2", "\"VIP\":0");
            //Globals.Logging.LogWarning("-=- =dang nhap thanh cong2:  " + data);
            JObject obj = JObject.Parse(data);
            string strUser = (string)obj["data"];
            Globals.Logging.LogWarning(strUser);
           
            //Globals.User.userMain = JsonUtility.FromJson<Globals.User>(strUser);


            //{ "evt":"0","data":"{\"Userid\":5311,\"Username\":\"playforfun\",\"Tinyurl\":\"\",\"AG\":321194,\"LQ\":0,\"VIP\":1,\"MVip\":0,\"markLevel\":0,\"PD\":0,\"OD\":16,\"A\":26,\"NM\":0,\"ListDP\":\"500;500;500_0;100;100_0;100;100_0;\",\"NewAccFBInDevice\":0,\"chipbank\":0,\"gameid\":0,\"NumFriendMail\":0,\"gameNo\":0,\"Diamond\":0,\"vippoint\":1,\"vippointMax\":10,\"FacebookName\":\"\",\"displayName\":\"playforfun\",\"LQ0\":0.0,\"CO\":0.0,\"CO0\":0.0,\"LQSMS\":0.0,\"LQIAP\":0.0,\"LQOther\":0.0,\"BLQ1\":0.0,\"BLQ3\":0.0,\"BLQ5\":0.0,\"BLQ7\":0.0,\"AVG7\":0.0,\"Group\":0.0,\"CreateTime\":1620630212017,\"keyObjectInGame\":0,\"idChatContents\":[1,3,13,17,20,24,26,40,43,47],\"UsernameLQ\":\"\"}","time":1636367771387,"auth":""}
            JObject objUser = JObject.Parse(strUser);

            Globals.User.userMain = new Globals.User();
            Globals.User.userMain.Userid = (int)objUser["Userid"];
            Globals.User.userMain.Username = (string)objUser["Username"];
            Globals.User.userMain.Tinyurl = (string)objUser["Tinyurl"];
            Globals.User.userMain.AG = (long)objUser["AG"];
            Globals.User.userMain.LQ = (long)objUser["LQ"];
            Globals.User.userMain.VIP = (int)objUser["VIP"];
            Globals.User.userMain.MVip = (int)objUser["MVip"];
            Globals.User.userMain.markLevel = (int)objUser["markLevel"];
            Globals.User.userMain.PD = (int)objUser["PD"];
            Globals.User.userMain.OD = (int)objUser["OD"];
            Globals.User.userMain.Avatar = (int)objUser["A"];
            Globals.User.userMain.NM = (int)objUser["NM"] % 100;
            Globals.User.userMain.nmAg = (long)objUser["NM"] / 100;
            Globals.User.userMain.ListDP = (string)objUser["ListDP"];
            Globals.User.userMain.NewAccFBInDevice = (int)objUser["NewAccFBInDevice"];
            Globals.User.userMain.agSafe = (long)objUser["chipbank"];
            Globals.User.userMain.NumFriendMail = (int)objUser["NumFriendMail"];
            Globals.User.userMain.gameNo = (int)objUser["gameNo"];
            Globals.User.userMain.Diamond = (int)objUser["Diamond"];
            Globals.User.userMain.vippoint = (int)objUser["vippoint"];
            Globals.User.userMain.vippointMax = (int)objUser["vippointMax"];
            Globals.User.userMain.FacebookName = (string)objUser["FacebookName"];
            Globals.User.userMain.displayName = (string)objUser["displayName"];
            Globals.User.userMain.LQ0 = (float)objUser["LQ0"];
            Globals.User.userMain.CO = (float)objUser["CO"];
            Globals.User.userMain.CO0 = (float)objUser["CO0"];
            Globals.User.userMain.LQSMS = (float)objUser["LQSMS"];
            Globals.User.userMain.LQIAP = (float)objUser["LQIAP"];
            Globals.User.userMain.LQOther = (float)objUser["LQOther"];
            Globals.User.userMain.BLQ1 = (float)objUser["BLQ1"];
            Globals.User.userMain.BLQ3 = (float)objUser["BLQ3"];
            Globals.User.userMain.BLQ5 = (float)objUser["BLQ5"];
            Globals.User.userMain.BLQ7 = (float)objUser["BLQ7"];
            Globals.User.userMain.AVG7 = (float)objUser["AVG7"];
            Globals.User.userMain.Group = (float)objUser["Group"];
            Globals.User.userMain.CreateTime = (long)objUser["CreateTime"];
            Globals.User.userMain.keyObjectInGame = (int)objUser["keyObjectInGame"];
            Globals.User.userMain.UsernameLQ = (string)objUser["UsernameLQ"];
            Globals.User.userMain.isShowMailAg = true;
            Globals.User.userMain.uidInvite = (int)objUser["uidInvite"];
            Globals.User.userMain.canInputInvite = (bool)objUser["canInputInvite"];
            Globals.User.userMain.timeInputInvite = (int)objUser["timeInputInvite"];
            Globals.User.userMain.timeInputInviteRemain = DateTimeOffset.Now.ToUnixTimeMilliseconds() + Globals.User.userMain.timeInputInvite * 1000;
            Globals.User.userMain.lastGameID = (int)objUser["gameid"];

            PlayerPrefs.SetInt("isFirstOpen", 1);
            PlayerPrefs.Save();

            SocketIOManager.getInstance().isSendFirst = true;
            Globals.Config.isLoginSuccess = true;
            JObject objLogin = new JObject();
            objLogin["evt"] = "0";
            objLogin["data"] = obj.ToString(Formatting.None);

            Debug.Log("data login   " + objLogin.ToString());

            SocketIOManager.getInstance().DATAEVT0 = objLogin;
            if (Globals.Config.u_SIO != "")
            {
                SocketIOManager.getInstance().startSIO();
            }
          
            SocketIOManager.getInstance().emitLogin();
            SocketIOManager.getInstance().emitSIOWithValue(objLogin, "LoginPacket", false);

            Globals.Config.curGameId = (int)objUser["gameid"];
            LoadConfig.instance.getConfigInfo();
            LoadConfig.instance.getInfoUser(strUser);
            if (Globals.Config.typeLogin == Globals.LOGIN_TYPE.NORMAL)
            {
                Globals.Config.saveLoginAccount();

            }


            UIManager.instance.showLobbyScreen(true);
            //Globals.Logging.Log("emit update info o day nua");
            SocketIOManager.getInstance().emitUpdateInfo();
            Dictionary<string, object> tags = new Dictionary<string, object>();

            tags.Add("deviceid", Globals.Config.deviceId);
            tags.Add("publisher", Globals.Config.publisher);
            tags.Add("username", Globals.User.userMain.Username);
            tags.Add("displayName", Globals.User.userMain.displayName);
            tags.Add("uservip", Globals.User.userMain.VIP.ToString());
            tags.Add("userid", Globals.User.userMain.Userid.ToString());
            tags.Add("usergold", Globals.User.userMain.AG.ToString());
            tags.Add("gameport", Globals.Config.OPERATOR.ToString());
            tags.Add("pkgname", Globals.Config.package_name);
            tags.Add("version", Globals.Config.versionGame);
            tags.Add("userlq", Globals.User.userMain.LQ.ToString());
            tags.Add("co", Globals.User.userMain.CO.ToString());
            tags.Add("co0", Globals.User.userMain.CO0.ToString());
            tags.Add("lqsms", Globals.User.userMain.LQSMS.ToString());
            tags.Add("lqiap", Globals.User.userMain.LQIAP.ToString());
            tags.Add("lqother", Globals.User.userMain.LQOther.ToString());
            tags.Add("blq1", Globals.User.userMain.BLQ1.ToString());
            tags.Add("blq3", Globals.User.userMain.BLQ3.ToString());
            tags.Add("blq5", Globals.User.userMain.BLQ5.ToString());
            tags.Add("blq7", Globals.User.userMain.BLQ7.ToString());
            tags.Add("avg7", Globals.User.userMain.AVG7.ToString());
            tags.Add("group", Globals.User.userMain.Group.ToString());
            OneSignalSDK.OneSignal.Default.SendTags(tags);

            new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(100);
                SocketSend.sendRef();
                Thread.Sleep(100);
                SocketSend.sendSelectG2(Globals.Config.curGameId);
                Thread.Sleep(100);
                SocketSend.getInfoSafe();
                Thread.Sleep(100);
                SocketSend.sendPromotion();
                Thread.Sleep(100);
                SocketSend.getMessList();
                Thread.Sleep(100);
                SocketSend.getMail(10);
                Thread.Sleep(100);
                SocketSend.getMail(12);
                Thread.Sleep(100);
                SocketSend.getFarmInfo();
            })).Start();
        }
        else
        {
            Globals.Logging.Log(packet.message);

            UIManager.instance.showMessageBox(packet.message);
            //{ "screenname":null,"pid":0,"status":"DENIED","code":-3,"message":"Username and Password do not match!","credentials":"","classId":11}

            var objData = new JObject();
            objData["codeError"] = packet.code;
            objData["MsgError"] = packet.message;
            SocketIOManager.getInstance().emitSIOWithValue(objData, "LoginPacket", false);
        }
    }


    public static void handleServiceTransportPacket(string strData)
    {
        ServiceTransportPacket packet = JsonUtility.FromJson<ServiceTransportPacket>(strData);
        packet.str_servicedata = (string)JObject.Parse(strData)["servicedata"];
        string data = Globals.Config.Base64Decode(packet.str_servicedata);
        HandleService.processData(JObject.Parse(data));
    }

    public static void handleGameTransportPacket(string strData)
    {
        //Globals.Logging.Log("handleGameTransportPacket   " + strData);
        GameTransportPacket packet = JsonUtility.FromJson<GameTransportPacket>(strData);
        packet.str_gamedata = (string)JObject.Parse(strData)["gamedata"];
        string data = Globals.Config.Base64Decode(packet.str_gamedata);
        HandleGame.processData(JObject.Parse(data));
    }
    public static void handleForcedLogoutPacket(string strData)
    {
        JObject data = JObject.Parse(strData);
        string message = (string)data["message"];

    }
    public static void handleJoinResponsePacket(string strData)
    {
        //{ "tableid":14,"seat":0,"status":"OK","code":0,"classId":31}
        JObject data = JObject.Parse(strData);
        //string message = (string)data["message"];

        if ((string)data["status"] == "OK")
        {

            //    NetworkManager.getInstance().listEvtGame.length = 0;
            //    cc.NGWlog('========================== packet OnShowGame');

            //    let dataJson = { };
            //    dataJson.tableid = packet.tableid;
            //    dataJson.curGameID = Globals.Config.curGameId;
            //    require('SMLSocketIO').getInstance().emitSIOWithValue(dataJson, "JoinPacket", false);
            //    require("UIManager").instance.onShowGame();
            Globals.Config.tableId = (int)data["tableid"];

            JObject dataJson = new JObject();
            dataJson["tableid"] = Globals.Config.tableId;
            dataJson["curGameID"] = Globals.Config.curGameId;
            SocketIOManager.getInstance().emitSIOWithValue(dataJson, "JoinPacket", false);
            UIManager.instance.showGame();
        }
        else
        {
            //    cc.NGWlog('hide load ben vao` ban` ko thanh cong====')
            //        require('UIManager').instance.onHideLoad();
            //    var _str = Globals.Config.getTextConfig(
            //        "show_join_error"
            //    );
            string _str = "";
            switch ((int)data["code"])
            {
                case -4:
                    _str = "";
                    break;
                case -5:
                    _str = Globals.Config.getTextConfig(
                        "err_table_another_table"
                    );
                    break;
                case -6:
                    _str = Globals.Config.getTextConfig("err_table_full");
                    break;
                case -7:
                    // _str = Globals.Config.getTextConfig("err_table_vip");
                    break;
                case -8:
                    _str = Globals.Config.getTextConfig("txt_not_enough_money_gl");
                    break;
            }

            //    let dataJson = { };
            //    dataJson.codeError = packet.code;
            //    dataJson.msgError = _str;
            //    require('SMLSocketIO').getInstance().emitSIOWithValue(dataJson, "JoinPacket", false);

            JObject dataJson = new JObject();
            dataJson["codeError"] = data["code"];
            dataJson["msgError"] = _str;
            SocketIOManager.getInstance().emitSIOWithValue(dataJson, "JoinPacket", false);
            if (_str != "")
                UIManager.instance.showMessageBox(_str);
        }
    }

    public static void handleLeaveResponsePacket(string strData)
    {
        Globals.Logging.Log("handleLeaveResponsePacket  " + strData);
        Globals.Logging.Log("-=-= " + Globals.Config.curGameId);
        JObject packet = JObject.Parse(strData);
        //string message = (string)data["message"];

        if ((string)packet["status"] == "OK")
        {
            if (UIManager.instance.gameView != null)
            {
                if (Globals.Config.curGameId != (int)Globals.GAMEID.SLOTNOEL
                    && Globals.Config.curGameId != (int)Globals.GAMEID.SLOTTARZAN
                    && Globals.Config.curGameId != (int)Globals.GAMEID.SLOT_9900)
                {
                    HandleGame.handleLeave();
                    return;
                }

                if (UIManager.instance.gameView.dataLeave != null)
                {
                    SocketIOManager.getInstance().emitSIOWithValue(UIManager.instance.gameView.dataLeave, "LeavePacket", false);
                }
                UIManager.instance.gameView.dataLeave = null;
                UIManager.instance.gameView.destroyThis();
                UIManager.instance.gameView = null;

            }

            //if (TableView.instance
            //    && Globals.Config.curGameId != (int)Globals.GAMEID.SLOTNOEL
            //    && Globals.Config.curGameId != (int)Globals.GAMEID.SLOTTARZAN)
            if (Globals.Config.isShowTableWithGameId(Globals.Config.curGameId) && Globals.User.userMain.VIP >= 1)
            {
                UIManager.instance.openTableView();
            }
            else
            {
                UIManager.instance.showLobbyScreen(false);
            }
        }
        else
        {
            JObject dataJson = new JObject();
            dataJson["codeError"] = packet["code"];
            dataJson["msgError"] = packet["status"];
            SocketIOManager.getInstance().emitSIOWithValue(dataJson, "LeavePacket", false);
        }

    }
}
