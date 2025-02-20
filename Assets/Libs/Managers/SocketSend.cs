using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SocketSend
{
    //static bool isCheckFast = false;
    public static void sendLogin(string username, string pass, bool isReg)
    {
        //if (isCheckFast)
        //{
        //    UIManager.instance.showToast()
        //    return;
        //}
        Globals.Config.user_name = username;
        Globals.Config.user_pass = pass;

        WebSocketManager.getInstance().Connect(() =>
        {
            JObject user = new JObject();
            user["Userid"] = 1;
            user["From"] = "mbacay";
            user["gameid"] = Globals.Config.curGameId;
            user["deviceId"] = Globals.Config.deviceId;
            user["Signid"] = "qazwsxedcrfv123$%^789";
            user["loginCount"] = 0;
            user["versionCode"] = Globals.Config.versionGame;
            user["language"] = Globals.Config.language;
            user["Username"] = username;
            user["Usertype"] = 1;
            if (isReg)
            {
                user["Reg"] = 1;
            }
            if (Globals.Config.typeLogin == Globals.LOGIN_TYPE.FACEBOOK)
            {
                user["Username"] = 1;
                user["Usertype"] = 2;
            }
            else if (Globals.Config.typeLogin == Globals.LOGIN_TYPE.FACEBOOK_INSTANT)
            {
                user["Username"] = 1;
                user["Usertype"] = 3;
            }

            string strUser = user.ToString(Newtonsoft.Json.Formatting.None);
            //Globals.Logging.Log("-=-=strUser1  " + strUser);
            //Globals.Logging.Log("-=-=pass  " + pass);
            byte[] credentials = { };
            LoginRequestPacket loginRequest = new LoginRequestPacket(strUser, pass, Globals.Config.OPERATOR, credentials);
            WebSocketManager.getInstance().SendData(JsonUtility.ToJson(loginRequest));

            SocketIOManager.getInstance().emitSIOWithValue(user, "LoginPacket", true);
        });
    }
    public static void onPlayNow()
    {
        //#if UNITY_EDITOR
        //        PlayerPrefs.DeleteKey("USER_PLAYNOW");
        //        PlayerPrefs.DeleteKey("PASS_PLAYNOW");
        //#endif
        UIManager.instance.loginView.accPlayNow = PlayerPrefs.GetString("USER_PLAYNOW", "");
        UIManager.instance.loginView.passPlayNow = PlayerPrefs.GetString("PASS_PLAYNOW", "");

        //Globals.Logging.Log("-=-=accPlayNow " + UIManager.instance.loginView.accPlayNow);
        var isReg = false;
        if (UIManager.instance.loginView.accPlayNow == "")
        {
            PlayerPrefs.SetInt("isReg", 0);
            isReg = true;
            var timeSta = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var textRandom = "";
            var possible =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (var i = 0; i < 10; i++)
                textRandom += possible[UnityEngine.Random.Range(0, possible.Length)];
            UIManager.instance.loginView.accPlayNow = "Te." + timeSta + "_" + Globals.Config.deviceId;
            if (UIManager.instance.loginView.accPlayNow.Length > 40) UIManager.instance.loginView.accPlayNow = UIManager.instance.loginView.accPlayNow.Substring(0, 40);
            UIManager.instance.loginView.passPlayNow = "Te" + timeSta + textRandom;
            //PlayerPrefs.SetString("USER_PLAYNOW", accPlayNow);
            //PlayerPrefs.SetString("PASS_PLAYNOW", passPlayNow);
            //PlayerPrefs.Save();

        }

        Globals.Logging.Log("-=-=accPlayNow2 " + UIManager.instance.loginView.accPlayNow);
        sendLogin(UIManager.instance.loginView.accPlayNow, UIManager.instance.loginView.passPlayNow, isReg);
    }

    public static void sendPing()
    {
        JObject data = new JObject();
        data["evt"] = "ping";
        //WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
        PingPacket serviceTransport = new PingPacket();
        WebSocketManager.getInstance().SendData(JsonUtility.ToJson(serviceTransport));
    }
    public static void sendIdReferFriend(int idFriend)
    {

        JObject data = new JObject();
        data["evt"] = "addInviteFriendID";
        data["inviteId"] = idFriend;
        Globals.Logging.Log("sendIdReferFriend:" + data.ToString(Newtonsoft.Json.Formatting.None).Replace("\n", ""));
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None).Replace("\n", ""));
    }
    public static void sendLogOut()
    {
        JObject data = new JObject();
        data["evt"] = "logout";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
        WebSocketManager.getInstance().stop();
    }
    public static void getChatWorld()
    {
        JObject data = new JObject();
        data["evt"] = "getChatWorld";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendChatWorld(string msg, int type)
    {
        string user = Globals.User.userMain.displayName;
        JObject data = new JObject();

        data["evt"] = 16;
        data["T"] = type;
        data["N"] = user;
        data["D"] = msg;
        if (type == 2)
        {
            data["GameId"] = 8044;
        }
        Globals.Logging.Log("Send Chat:" + data.ToString(Newtonsoft.Json.Formatting.None));
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getMessList()
    {
        JObject data = new JObject();
        data["evt"] = "messagelist";

        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getMessageDetail(int id)
    {
        JObject data = new JObject();
        data["evt"] = "messagedetail";
        data["Id"] = id;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendMessage(int idFr, string msg, string name)
    {
        try
        {
            string FbId = Globals.User.FacebookID;
            JObject data = new JObject();
            data["evt"] = "message";
            data["Id"] = idFr;
            data["Msg"] = msg;
            data["AG"] = 0;
            data["I"] = 0;
            data["N"] = name;
            if (FbId != null)
            {
                data["Fid"] = Globals.User.FacebookID;
            }
            Globals.Logging.Log("sendMessage:" + data.ToString(Newtonsoft.Json.Formatting.None));
            WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
        }
        catch (Exception e)
        {
            Globals.Logging.LogException(e);
        }
    }
    public static void searchFriend(string idFr)
    {
        JObject data = new JObject();
        data["evt"] = "followfind";
        data["id"] = idFr;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void deleteMessage(int idFr)
    {
        JObject data = new JObject();
        data["evt"] = "messagedeleteall";
        data["Id"] = idFr;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendDTHistory()
    {
        JObject data = new JObject();
        data["evt"] = "cashOutHistory";
        data["userid"] = Globals.User.userMain.Userid;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendRejectCashout(int status, int id)
    {
        JObject data = new JObject();
        data["evt"] = "rejectCashout";
        data["status"] = status;
        data["id"] = id;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendCashOut(int value, string wingId, string typeName)
    { //"Gcash" or "Mobile"
        JObject data = new JObject();
        data["evt"] = "getgift";
        data["Id"] = Globals.User.userMain.Userid;
        data["CashValue"] = value;
        data["GcashId"] = wingId;
        data["TypeName"] = typeName;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void changeAvatar(int idAvatar)
    {
        JObject data = new JObject();
        data["evt"] = "changea";
        data["A"] = idAvatar;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendGift(int idFriend, int chip)
    {
        JObject data = new JObject();
        data["idevt"] = 800;
        data["toname"] = "";
        data["toid"] = idFriend;
        data["chip"] = chip;

        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendCreateTable(int bet)
    {
        JObject data = new JObject();
        data["idevt"] = 1;
        data["M"] = bet;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendCreateTableWithPass(int bet, string name, string Pass)
    {
        JObject data = new JObject();
        data["idevt"] = 1;
        data["M"] = bet;
        data["N"] = name;
        data["P"] = Pass;

        //var data = {
        //    idevt: 1,
        //    M: bet,
        //    N: name,
        //    P: Pass
        //}
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getHistorySafe()
    {
        JObject data = new JObject();
        data["idevt"] = 500;

        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendRef()
    {
        JObject data = new JObject();
        data["evt"] = "ref";
        data["data"] = Globals.Config.publisher;
        data["email"] = "";
        data["version"] = Globals.Config.versionGame;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getStatus()
    {
        JObject data = new JObject();
        data["idevt"] = 200;

        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void changeStatus(string status)
    {
        JObject data = new JObject();
        data["idevt"] = 201;
        data["status"] = status;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void changePassword(string oldPass, string newPass)
    {
        JObject data = new JObject();
        data["evt"] = "changepass";
        data["OP"] = oldPass;
        data["NP"] = newPass;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendSelectGame(int gameId)
    {
        var severIp = "";

        for (var i = 0; i < Globals.Config.listGame.Count; i++)
        {
            JObject itemGa = (JObject)Globals.Config.listGame[i];
            if (gameId == (int)itemGa["id"])
            {
                severIp = (string)itemGa["ip_dm"];
                break;
            }
        }

        Globals.Logging.Log("send select game " + gameId + " server ip " + severIp);

        if (severIp != Globals.Config.curServerIp)
        {
            //this.isConnectSimilar = false;
            //require('GameManager').getInstance().isLogOut = false;
            //this.isInGame = true;
            Globals.Config.isChooseGame = true;
            //this.onLogout();
            //this.connect_sv(severIp, "443");
        }
        else
        {
            //this.isConnectSimilar = true;
            sendSelectG2(gameId);

            //cc.NGWlog("gameid_network", gameId);
            if (Globals.Config.listGamePlaynow.Contains(gameId) || Globals.User.userMain.VIP < 1)
            {
                //require('SMLSocketIO').getInstance().emitSIOCCCNew(cc.js.formatStr("ActionPlayNow_%d", gameId));
                Globals.Logging.Log("Send Play Now:" + gameId);
                sendPlayNow(gameId);
                SocketIOManager.getInstance().emitSIOCCCNew(Globals.Config.formatStr("ActionPlayNow_%d", gameId));
            }
            else
            {
                //if (require("GameManager").getInstance().isPlayNowBanner)
                //{
                //    this.sendPlayNow(gameId);
                //    require("GameManager").getInstance().isPlayNowBanner = false;
                //}
            }
        }

        PlayerPrefs.SetInt("curGameId", gameId);
        PlayerPrefs.Save();

        Globals.Config.curServerIp = severIp;
        Globals.Config.curGameId = gameId;
    }

    public static void sendSelectG2(int gameId)
    {
        Globals.Logging.Log("sesendSelectG2:" + gameId);
        JObject data = new JObject();
        data["evt"] = "selectG2";
        data["gameid"] = gameId;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));

    }

    public static void sendUpdateJackpot(int gameID)
    {
        JObject data = new JObject();
        data["evt"] = "updatejackpot";
        data["gameid"] = gameID;
        Globals.Logging.Log("sendUpdateJackpot  " + data.ToString(Newtonsoft.Json.Formatting.None));
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendPlayNow(int gameID)
    {
        JObject data = new JObject();
        data["evt"] = "searchT";
        data["gameid"] = gameID;
        Globals.Logging.Log("_+_=  " + data.ToString(Newtonsoft.Json.Formatting.None));
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendRoomTable()
    {
        JObject data = new JObject();
        data["evt"] = "roomTable";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendRoomVip()
    {
        JObject data = new JObject();
        data["evt"] = "roomVip";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }


    public static void sendCheckPass(int tableID)
    {
        JObject data = new JObject();
        data["evt"] = "checkPass";
        data["tableId"] = tableID;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendChangeTable(int mark, int tableid)
    {
        JObject data = new JObject();
        data["idevt"] = 2;
        data["M"] = mark;
        data["idtable"] = tableid;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getArrGold(List<int> arr)
    {
        JObject data = new JObject();
        data["evt"] = 15;
        data["T"] = 2;
        data["Arr"] = JArray.FromObject(arr);
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendFeedback(string content)
    {
        JObject data = new JObject();
        data["evt"] = 15;
        data["T"] = 7;
        data["NN"] = "Admin";
        data["D"] = content;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendJoinTable(int tableid)
    {
        var packet = new JoinRequestPacket();
        packet.tableid = tableid;
        packet.seat = -1;
        WebSocketManager.getInstance().SendData(JsonUtility.ToJson(packet));
        var jsonData = new JObject();
        jsonData["tableid"] = tableid;
        SocketIOManager.getInstance().emitSIOWithValue(jsonData, "JoinPacket", true);
    }

    public static void sendJoinTableWithPass(int tableid, string pass)
    {
        var packet = new JoinRequestPacket();
        packet.tableid = tableid;
        packet.seat = -1;

        JObject para = new JObject();
        para["key"] = "Pass";
        para["type"] = 0;

        JArray pp = new JArray();
        var bytess = Globals.Config.getByte(pass);
        pp.Add(0);
        pp.Add(bytess.Length);
        foreach (var c in bytess)
        {
            pp.Add(c);
        }
        para["value"] = pp;

        Globals.Logging.Log(pp.ToString(Newtonsoft.Json.Formatting.None));
        JObject dataSend = JObject.Parse(JsonUtility.ToJson(packet));
        dataSend["params"] = new JArray(para);

        Debug.Log("sendJoinTableWithPass:  " + dataSend.ToString(Newtonsoft.Json.Formatting.None));
        WebSocketManager.getInstance().SendData(dataSend.ToString(Newtonsoft.Json.Formatting.None));

        var jsonData = new JObject();
        jsonData["tableid"] = tableid;
        jsonData["pass"] = true;
        SocketIOManager.getInstance().emitSIOWithValue(jsonData, "JoinPacket", true);
    }

    public static void sendUpVip()
    {
        JObject data = new JObject();
        data["evt"] = "uvip";
        data["vip"] = 1;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendToSafe(long chips)
    {
        JObject data = new JObject();
        data["idevt"] = 301;
        data["chip"] = chips;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendTip()
    {
        JObject data = new JObject();
        data["evt"] = "tip";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
        Globals.Logging.Log("sendTip");
    }
    public static void sendWithDraw(long chips)
    {
        JObject data = new JObject();
        data["idevt"] = 302;
        data["chip"] = chips;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getInfoSafe()
    {
        JObject data = new JObject();
        data["idevt"] = 300;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendGiftCode(string giftCodeId)
    {
        JObject data = new JObject();
        data["evt"] = "GiftCode";
        data["C"] = giftCodeId;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getMail(int type)
    {
        JObject data = new JObject();
        data["evt"] = 15;
        data["T"] = type;
        data["P"] = 0;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendReadMail(int idMess)
    {
        JObject data = new JObject();
        data["evt"] = 15;
        data["T"] = 1;
        data["ID"] = idMess;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
        Globals.Logging.Log("Send read mail:" + idMess);
    }
    public static void deleteMailAdmin(List<int> arrId)
    {
        //string arrStr = JArray.(arrId.ToString(Newtonsoft.Json.Formatting.None));
        JObject data = new JObject();
        data["evt"] = 15;
        data["T"] = 3;
        data["Arr"] = JArray.FromObject(arrId);
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendPromotion()
    {
        JObject data = new JObject();
        data["evt"] = "promotion_info";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendPromotinGold(int receiveType, int chip)
    {
        JObject data = new JObject();
        data["evt"] = "promotion";
        data["T"] = receiveType;
        data["G"] = chip;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendGetDataSpin()
    {
        JObject data = new JObject();
        data["evt"] = "promotion_online_2";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getTopGamer(int idGame, int type)
    {
        JObject data = new JObject();
        data["evt"] = "topgamer_new";
        data["Gameid"] = idGame;
        data["Typeid"] = type;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void searchInfoPlayer(int idFr)
    {
        JObject data = new JObject();
        data["evt"] = "followfind";
        data["id"] = idFr;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendExitGame()
    {
        JObject data = new JObject();
        data["evt"] = "autoExit";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }


    public static void sendChatEmo(string nameSe, string nameRe, string type)
    {
        JObject data = new JObject();
        data["evt"] = "chattable";
        data["Name"] = nameSe;
        data["NName"] = nameRe;
        data["Data"] = "";
        data["Time"] = DateTime.Now;
        data["T"] = "*f" + type;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendChat(string username, string text)
    {
        JObject data = new JObject();
        data["evt"] = "chattable";
        data["Name"] = username;
        data["NName"] = "";
        data["Data"] = text;
        data["Time"] = DateTime.Now;
        data["T"] = "";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendUpdateItemVip(int id)
    {
        //var data = {
        //    evt: "updateObjectGame",
        //    keyObject: key
        //}
        JObject data = new JObject();
        data["evt"] = "updateObjectGame";
        data["keyObject"] = id * 10;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }


    //===================Dummy===========================//
    public static void sendDummyDraw()
    {
        JObject data = new JObject();
        data["evt"] = "draw";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendDummyShow()
    {
        JObject data = new JObject();
        data["evt"] = "show";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendDummyMeld(JArray array)
    {
        //    var data = {
        //           evtName ="meld",
        //            arrCard: array,
        //        }
        //this.sendDataGame(JSON.stringify(data));

        JObject data = new JObject();
        data["evt"] = "meld";
        data["arrCard"] = array;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendDummyLayoff(JObject data)
    {
        data["evt"] = "layoff";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendDummyEat(JArray _arrHand, JArray _arrEat)
    {
        JObject data = new JObject();
        data["evt"] = "eat";
        data["arrHand"] = _arrHand;
        data["arrEat"] = _arrEat;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendDummyDiscard(int idCard)
    {
        JObject data = new JObject();
        data["evt"] = "disCard";
        data["id"] = idCard;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendSpinSlot(int singleLineBet, bool isFreeSpin, int index = -1)
    {
        JObject data = new JObject();
        //{ "evt":"spin","singleLineBet":50,"isFreeSpin":false}
        data["evt"] = "spin";
        data["singleLineBet"] = singleLineBet;
        data["isFreeSpin"] = isFreeSpin;
        if (index != -1)
        {
            data["index"] = index;
        }
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendForceFreeSpin(int singleLineBet, bool isFreeSpin, int index = -1)
    {
        Globals.Logging.Log("Send force free");
        JObject data = new JObject();
        //{ "evt":"spin","singleLineBet":50,"isFreeSpin":false}
        data["evt"] = "spin";
        data["singleLineBet"] = singleLineBet;
        data["isFreeSpin"] = isFreeSpin;
        data["setFS"] = true;
        if (index != -1)
        {
            data["index"] = index;
        }
        Globals.Logging.Log(data.ToString(Newtonsoft.Json.Formatting.None));
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendDummyKnock(JObject data)
    {
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    //===================End Dummy===========================//
    // ================= START SICBO ========================//

    public static void sendBetSicbo(List<int> n, List<int> m, List<int> t)
    {
        if (m.Count <= 0) return;
        JObject data = new JObject();
        data["evt"] = "bet";
        data["N"] = JArray.FromObject(n);
        data["M"] = JArray.FromObject(m);
        data["T"] = JArray.FromObject(t);
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendBetSicbo(List<string> n, List<int> m, List<int> t)
    {
        if (m.Count <= 0) return;
        JObject data = new JObject();
        data["evt"] = "bet";
        data["N"] = JArray.FromObject(n);
        data["M"] = JArray.FromObject(m);
        data["T"] = JArray.FromObject(t);
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    // ================= END SICBO ========================//
    //===================Start BORKKDeng======================//
    public static void sendRaise(long chipbet)
    {
        JObject data = new JObject();
        data["evt"] = "bm";
        data["M"] = chipbet;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendActionTurn(int typeAc)
    {
        JObject data = new JObject();
        data["evt"] = "ac";
        data["A"] = typeAc;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    //===================End BORKKDeng======================//

    //===================KEANG==============================//
    public static void sendKaengBc()
    {
        JObject data = new JObject();
        data["evt"] = "bc";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendKaengDc(JArray array)
    {

        JObject data = new JObject();
        data["evt"] = "dc";
        data["arr"] = array;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendKaengChop(JArray array)
    {
        JObject data = new JObject();
        data["evt"] = "chop";
        data["arr"] = array;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendKaeng()
    {
        JObject data = new JObject();
        data["evt"] = "kaeng";
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    //===================END KEANG==========================//

    //===================GAOGEA============================//
    public static void sendMakeBetShow(string _data, int amount = 0)
    {
        string evtName = "";
        switch (_data)
        {
            case "call":
                evtName = "call";
                break;
            case "check":
                evtName = "check";
                break;
            case "fold":
                evtName = "fold";
                break;
            case "raise":
                evtName = "raise";
                break;
            case "allin":
                evtName = "allin";
                break;
            case "show":
                evtName = "show";
                break;
            default:
                Globals.Logging.Log("Ban da gui sai Data");
                break;
        }

        JObject data = new JObject();
        data["evt"] = evtName;
        data["ag"] = amount;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void sendChangeCard(bool _data)
    {
        JObject data = new JObject();
        data["evt"] = "cs";
        data["isChange"] = _data;
        WebSocketManager.getInstance().sendDataGame(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    //===================END GAOGEA============================//

    public static void sendChangeName(string strname)
    {
        JObject data = new JObject();
        data["evt"] = "RUF";
        data["U"] = strname;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendRegister(string nameReg, string pass, string _oldpass)
    {
        JObject data = new JObject();
        data["idevt"] = "202";
        data["name"] = nameReg;
        data["pass"] = pass;
        data["oldpass"] = _oldpass;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendUAG()
    {
        JObject data = new JObject();
        data["evt"] = "uag";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void sendInviteTable(string id_fr, int chip)
    {
        JObject data = new JObject();
        data["evt"] = "ivp";
        data["T"] = 1;
        data["OID"] = id_fr;
        data["AG"] = chip;
        Debug.Log("SendINvite:" + data.ToString(Newtonsoft.Json.Formatting.None));
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }

    public static void getInviteTableList(int chip)
    {
        JObject data = new JObject();
        data["evt"] = "ivp";
        data["T"] = 0;
        data["AG"] = chip;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
        Globals.Logging.Log("getInviteTableList");
    }

    public static void sendIAPResult(string receipt)
    {
        JObject onj = JObject.Parse(receipt);
        JObject onj2 = JObject.Parse((string)onj["Payload"]);
        //Debug.Log(onj2["json"].ToString(Newtonsoft.Json.Formatting.None));
        //JObject ojjj = new JObject();
        //ojjj["orderId"] = "GPA.3354-7860-3377-42417";
        //ojjj["packageName"] = "dummy.co.slots";
        //ojjj["productId"] = "dummy.co.slots.50";
        //ojjj["purchaseTime"] = 1673425536713;
        //ojjj["purchaseState"] = 0;
        //ojjj["purchaseToken"] = "oimicidocglndjfkoojpimon.AO-J1Ow54d1wcZOHgVtvE6y77qvuGfT7Zjsrhbx6_HJ4T1ODkU7OcoBnMyeUY7ddpVw_Z1A6wswCOqjyiIP3l3egWPAiju6r8w";
        //ojjj["quantity"] = 1;
        //ojjj["acknowledged"] = false;

        //string _signdata = ojjj.ToString(Newtonsoft.Json.Formatting.None);//onj2["json"].ToString(Newtonsoft.Json.Formatting.None);
        //string _signature = "cNxlmtDI18fP6YR5IrfHxUKikKFKlepaYlLIjIJJ1SXbt3xHVoO79DZeC4cvHOOB7K8USziNMcSpNxCj33ezxK57zxKKbDBxWgUcEZ8exfQNtqsylba0mwjBuhOsXx4Q1V+3KFwNEA7FyIc9KdPRFXjpTGtkHFkHYfGsq5Z7fGyCn6WiM7lNBAGawA78rxBbeDBCvV+bNG6ktbsk/N4gCKmAjVAmbKbWpI1EpS3NbWuPyt4Z/eNOFQfNG5pVtKu4EK56VZfRSdSudR54mJLdiup299gqzdjhWpL5vVtenHb8cK/wVreSbxZGif0Ldm12Se0PG3C93GxCBwushvSy4Q==";//(string)onj2["signature"];
        ////JObject onj = JObject.Parse(receipt);
        ////JObject onj2 = JObject.Parse((string)onj["Payload"]);
        ////Debug.Log(onj2["json"].ToString(Newtonsoft.Json.Formatting.None));
        //JObject ojjj = new JObject();
        //ojjj["orderId"] = "GPA.3354-7860-3377-42417";
        //ojjj["packageName"] = "dummy.co.slots";
        //ojjj["productId"] = "dummy.co.slots.50";
        //ojjj["purchaseTime"] = 1673425536713;
        //ojjj["purchaseState"] = 0;
        //ojjj["purchaseToken"] = "oimicidocglndjfkoojpimon.AO-J1Ow54d1wcZOHgVtvE6y77qvuGfT7Zjsrhbx6_HJ4T1ODkU7OcoBnMyeUY7ddpVw_Z1A6wswCOqjyiIP3l3egWPAiju6r8w";
        //ojjj["quantity"] = 1;
        //ojjj["acknowledged"] = false;

        string _signdata = (string)onj2["json"];//.ToString(Newtonsoft.Json.Formatting.None);
        string _signature = (string)onj2["signature"];

        JObject data = new JObject();
        data["evt"] = "iap";
        data["signedData"] = _signdata;
        data["signature"] = _signature;
        Debug.Log(data);
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
        var key_iap = Globals.User.userMain.Userid + "_iap_count";
        var countIAP = PlayerPrefs.GetInt(key_iap, 0);
        var key_signdata = Globals.User.userMain.Userid + "_signdata_" + countIAP;
        var key_signature = Globals.User.userMain.Userid + "_signature_" + countIAP;
        PlayerPrefs.SetString(key_signdata, _signdata);
        PlayerPrefs.SetString(key_signature, _signature);
        countIAP++;
        PlayerPrefs.SetInt(key_iap, countIAP);
    }
    public static void validateIAPReceipt(string _receipt)
    {
        JObject data = new JObject();
        data["evt"] = "iap_ios";
        data["receipt_encoded64"] = _receipt;
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));

        var key_iap = Globals.User.userMain.Userid + "_iap_count";
        var countIAP = PlayerPrefs.GetInt(key_iap, 0);

        var key_receipt = Globals.User.userMain.Userid + "_receipt_" + countIAP;
        PlayerPrefs.SetString(key_receipt, _receipt);
        countIAP++;
        PlayerPrefs.SetInt(key_iap, countIAP);
    }
    public static void sendRequestAccountDeletion()
    {
        JObject data = new JObject();
        data["evt"] = "deleteAccount";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
    public static void getFarmInfo()
    {
        JObject data = new JObject();
        data["evt"] = "farmInfo";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
      
    }

    public static void getFarmReward()
    {
        JObject data = new JObject();
        data["evt"] = "farmReward";
        WebSocketManager.getInstance().sendService(data.ToString(Newtonsoft.Json.Formatting.None));
    }
}