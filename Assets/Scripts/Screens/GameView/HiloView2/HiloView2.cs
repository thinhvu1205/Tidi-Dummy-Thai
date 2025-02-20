using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine.EventSystems;

public class HiloView2 : HiloView
{
    protected override void Start()
    {
        base.Start();
        //UIManager.instance.gameView = GetComponent<HiloView2>();
        Globals.Logging.Log("chay vao ben HiloView2");

        //listDataFake.Add("{\"evt\":\"stable\",\"data\":\"{\\\"N\\\":\\\"รับโชคและชนะด้วย ไฮโลไทย!\\\",\\\"M\\\":20,\\\"ArrP\\\":[{\\\"id\\\":346337,\\\"N\\\":\\\"แล้ว\\u0027ว\\u0027 แต่\\u0027มึง\\u0027งง\\\",\\\"Url\\\":\\\"fb.157448426746089\\\",\\\"AG\\\":2638,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":8,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"แล้ว\\u0027ว\\u0027 แต่\\u0027มึง\\u0027งง\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":346175,\\\"N\\\":\\\"ทรงพล ศิริสอน\\\",\\\"Url\\\":\\\"fb.352291593601038\\\",\\\"AG\\\":12,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":4,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"ทรงพล ศิริสอน\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":344893,\\\"N\\\":\\\"มณีรัตน์ พลสุวรรณ์\\\",\\\"Url\\\":\\\"fb.1815075475348779\\\",\\\"AG\\\":448,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":3,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":6,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"มณีรัตน์ พลสุวรรณ์\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":348715,\\\"N\\\":\\\"จตุพร เพชรกุล\\\",\\\"Url\\\":\\\"fb.1456070531473972\\\",\\\"AG\\\":628,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":8,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"จตุพร เพชรกุล\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":308000,\\\"N\\\":\\\"Bank Bank\\\",\\\"Url\\\":\\\"fb.660164731882673\\\",\\\"AG\\\":1188,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":6,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"Bank Bank\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":287490,\\\"N\\\":\\\"Kiadtisak Pranitphonkang\\\",\\\"Url\\\":\\\"fb.106242338694942\\\",\\\"AG\\\":160,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":4,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":3,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"Kiadtisak Pranitphonkang\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":306430,\\\"N\\\":\\\"คา คิน นน\\\",\\\"Url\\\":\\\"fb.411687094097146\\\",\\\"AG\\\":820,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":3,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":8,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"คา คิน นน\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":342099,\\\"N\\\":\\\"วีระยุทธ แซ่ลิ้ม\\\",\\\"Url\\\":\\\"fb.359548799524763\\\",\\\"AG\\\":720,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":2,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"Av\\\":1,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"วีระยุทธ แซ่ลิ้ม\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":8240,\\\"N\\\":\\\"hienndm\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":12703,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":3,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":12,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"hienndm\\\",\\\"keyObjectInGame\\\":0}],\\\"Id\\\":9507,\\\"T\\\":0,\\\"V\\\":0,\\\"AG\\\":20,\\\"S\\\":0,\\\"History\\\":[[3,3,4],[1,6,6],[1,6,6],[2,4,4],[1,1,6],[4,6,6],[2,4,5],[2,5,6],[3,3,6],[3,4,5],[4,4,5],[2,3,4],[4,4,6],[2,3,5],[1,2,4],[2,3,5],[3,5,6],[1,2,2],[2,4,5],[3,4,6],[3,5,6],[1,1,6]]}\"}");
        //listDataFake.Add("{\"evt\":\"stable\",\"data\":\"{\\\"N\\\":\\\"มาเอาชนะผู้เล่นมืออาชีพ ไฮโลไทย ทุกคนกันเถอะ!\\\",\\\"M\\\":100,\\\"ArrP\\\":[{\\\"id\\\":314026,\\\"N\\\":\\\"te.1648976060761_283858af-53f4-41c9-a27e\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":754,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":1,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"TBK.314026\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":265174,\\\"N\\\":\\\"te.1647076970800_4f677180-ac8a-4bba-b3eb\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":10224,\\\"A\\\":true,\\\"LQ\\\":100,\\\"VIP\\\":2,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":11,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"TBK.265174\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":284130,\\\"N\\\":\\\"fb.360567199412366\\\",\\\"Url\\\":\\\"fb.360567199412366\\\",\\\"AG\\\":306938,\\\"A\\\":true,\\\"LQ\\\":20,\\\"VIP\\\":2,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":0,\\\"FId\\\":360567199412366,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"บีม บีบบังคับ\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":308169,\\\"N\\\":\\\"maew7463\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":6938,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":4,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"maew7463\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":8240,\\\"N\\\":\\\"hienndm\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":136870,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":3,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":4,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"hienndm\\\",\\\"keyObjectInGame\\\":0}],\\\"Id\\\":3588,\\\"T\\\":0,\\\"V\\\":0,\\\"AG\\\":1000,\\\"S\\\":0,\\\"History\\\":[[2,3,6],[1,1,3],[1,2,3],[1,2,4],[3,4,4],[3,4,5],[2,4,6],[1,1,5],[2,4,5],[2,3,5],[1,2,5],[3,4,4],[4,5,5],[2,3,6],[3,4,5],[2,2,3],[1,1,1],[2,4,5],[1,4,4]]}\"}");
        //listDataFake.Add("{\"evt\":\"stable\",\"data\":\"{\\\"N\\\":\\\"รับโชคและชนะด้วย ไฮโลไทย!\\\",\\\"M\\\":100,\\\"ArrP\\\":[{\\\"id\\\":112745,\\\"N\\\":\\\"te.1644941503483_1363a6cc-8158-41ff-bd6f\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":1852,\\\"A\\\":true,\\\"LQ\\\":50,\\\"VIP\\\":2,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":3,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"TBK.112745\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":8240,\\\"N\\\":\\\"hienndm\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":137304,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":3,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":4,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"hienndm\\\",\\\"keyObjectInGame\\\":0}],\\\"Id\\\":2525,\\\"T\\\":6000,\\\"V\\\":0,\\\"AG\\\":1000,\\\"S\\\":1,\\\"History\\\":[[1,3,4],[1,5,6],[1,2,4],[4,6,6],[5,6,6],[1,1,2],[1,1,5],[2,4,6],[4,5,6],[3,4,6],[1,5,6],[4,5,6],[2,3,4],[1,2,5],[2,3,6],[2,3,6],[1,2,6],[2,2,6],[3,6,6],[2,2,2],[2,4,5],[1,1,5],[3,3,4],[1,4,4],[1,2,2],[1,4,5],[1,3,4],[3,4,6],[2,4,6],[3,3,5],[1,4,6],[2,4,6],[1,2,3],[2,4,6],[1,2,2],[2,3,6]]}\"}");
        //listDataFake.Add("{\"evt\":\"stable\",\"data\":\"{\\\"N\\\":\\\"มาเอาชนะผู้เล่นมืออาชีพ ไฮโลไทย ทุกคนกันเถอะ!\\\",\\\"M\\\":100,\\\"ArrP\\\":[{\\\"id\\\":301785,\\\"N\\\":\\\"fb.173628515006174\\\",\\\"Url\\\":\\\"fb.173628515006174\\\",\\\"AG\\\":196,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":0,\\\"FId\\\":173628515006174,\\\"D\\\":0,\\\"Arr\\\":[{\\\"M\\\":100,\\\"N\\\":\\\"Lo\\\",\\\"T\\\":1,\\\"W\\\":0}],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"Sutipong Tuensati\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":312658,\\\"N\\\":\\\"ios.b11072560@gmail.com\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":4621,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":1,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":0,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"TBK.312658\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":8240,\\\"N\\\":\\\"hienndm\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":136916,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":3,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":4,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"hienndm\\\",\\\"keyObjectInGame\\\":0}],\\\"Id\\\":7174,\\\"T\\\":-1000,\\\"V\\\":0,\\\"AG\\\":1000,\\\"S\\\":1,\\\"History\\\":[[1,3,6],[3,5,6],[2,2,3],[1,3,5],[2,2,4],[1,3,5],[3,5,5],[1,5,6],[1,3,4],[1,4,6],[4,4,6],[1,1,1],[4,5,6],[1,3,4],[3,4,5],[2,2,5],[3,4,6],[3,4,4],[2,3,4],[1,1,6],[3,3,4],[1,5,5]]}\"}");
        //listDataFake.Add("{\"evt\":\"finish\",\"data\":\"{\\\"listDice\\\":[1,2,2],\\\"listNumberWin\\\":[{\\\"N\\\":\\\"1 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"2 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"Lo\\\",\\\"typewin\\\":1},{\\\"N\\\":\\\"1-2\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"1-2-3\\\",\\\"typewin\\\":7},{\\\"N\\\":\\\"LO 1\\\",\\\"typewin\\\":2}],\\\"listUser\\\":[{\\\"pid\\\":301785,\\\"ag\\\":294,\\\"vip\\\":1,\\\"listNumWin\\\":[{\\\"M\\\":100,\\\"N\\\":\\\"Lo\\\",\\\"T\\\":1,\\\"W\\\":100}]},{\\\"pid\\\":312658,\\\"ag\\\":4621,\\\"vip\\\":1,\\\"listNumWin\\\":[]},{\\\"pid\\\":8240,\\\"ag\\\":136916,\\\"vip\\\":3,\\\"listNumWin\\\":[]}],\\\"History\\\":[[1,3,6],[3,5,6],[2,2,3],[1,3,5],[2,2,4],[1,3,5],[3,5,5],[1,5,6],[1,3,4],[1,4,6],[4,4,6],[1,1,1],[4,5,6],[1,3,4],[3,4,5],[2,2,5],[3,4,6],[3,4,4],[2,3,4],[1,1,6],[3,3,4],[1,5,5],[1,2,2]]}\"}");
        //listDataFake.Add("{\"errorCode\":0,\"evt\":\"ltable\",\"Name\":\"ios.b11072560@gmail.com\"}");
        //listDataFake.Add("{\"evt\":\"start\",\"data\":\"4000\"}");
        //listDataFake.Add("{\"evt\":\"lc\",\"data\":\"15000\"}");
        //listDataFake.Add("{\"evt\":\"bet\",\"data\":\"{\\\"N\\\":\\\"hienndm\\\",\\\"Num\\\":[\\\"3-6\\\",\\\"2-4\\\",\\\"3-5\\\",\\\"4-1\\\",\\\"1-2-3\\\",\\\"4-5-6\\\"],\\\"M\\\":[100,200,100,100,100,100],\\\"T\\\":[5,5,5,5,7,7]}\"}");

        //listDataFake.Add("{\"evt\":\"finish\",\"data\":\"{\\\"listDice\\\":[1,5,6],\\\"listNumberWin\\\":[{\\\"N\\\":\\\"1 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"5 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"6 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"Hi\\\",\\\"typewin\\\":1},{\\\"N\\\":\\\"1-5\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"1-6\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"5-6\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"4-5-6\\\",\\\"typewin\\\":7},{\\\"N\\\":\\\"HI 6\\\",\\\"typewin\\\":2}],\\\"listUser\\\":[{\\\"pid\\\":8240,\\\"ag\\\":138112,\\\"vip\\\":3,\\\"listNumWin\\\":[{\\\"M\\\":100,\\\"N\\\":\\\"4-5-6\\\",\\\"T\\\":7,\\\"W\\\":100}]}],\\\"History\\\":[[1,5,6]]}\"}");
        //listDataFake.Add("{\"evt\":\"start\",\"data\":\"4000\"}");
        //listDataFake.Add("{\"evt\":\"lc\",\"data\":\"15000\"}");
        //listDataFake.Add("{\"evt\":\"bet\",\"data\":\"{\\\"N\\\":\\\"hienndm\\\",\\\"Num\\\":[\\\"3-6\\\",\\\"2-4\\\",\\\"3-5\\\",\\\"4-1\\\",\\\"1-2-3\\\",\\\"4-5-6\\\"],\\\"M\\\":[100,200,100,100,100,100],\\\"T\\\":[5,5,5,5,7,7]}\"}");
        //listDataFake.Add("{\"evt\":\"finish\",\"data\":\"{\\\"listDice\\\":[1,5,6],\\\"listNumberWin\\\":[{\\\"N\\\":\\\"1 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"5 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"6 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"Hi\\\",\\\"typewin\\\":1},{\\\"N\\\":\\\"1-5\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"1-6\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"5-6\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"4-5-6\\\",\\\"typewin\\\":7},{\\\"N\\\":\\\"HI 6\\\",\\\"typewin\\\":2}],\\\"listUser\\\":[{\\\"pid\\\":8240,\\\"ag\\\":138112,\\\"vip\\\":3,\\\"listNumWin\\\":[{\\\"M\\\":100,\\\"N\\\":\\\"4-5-6\\\",\\\"T\\\":7,\\\"W\\\":100}]}],\\\"History\\\":[[1,5,6]]}\"}");
        //listDataFake.Add("{\"evt\":\"HETROI\",\"data\":\"15000\"}");
    }

    public void onClickDemo()
    {
        UIManager.instance.showMessageBox("ahihi  ");
    }

    public override void onClickFake(Button btnFake)
    {
        //return;
        Globals.Config.curGameId = (int)Globals.GAMEID.SICBO;
        string strData = listDataFake[0];
        Globals.Logging.Log("Fake strData:" + strData);

        string strDataNext = listDataFake[1];
        JObject dataFake = JObject.Parse(strData);
        JObject dataFakeNext = JObject.Parse(strDataNext);
        btnFake.GetComponentInChildren<TextMeshProUGUI>().text = getString(dataFakeNext, "evt");
        HandleGame.processData(dataFake);
        listDataFake.RemoveAt(0);
    }
    public override void handleBet(string objData)
    {
        //require('SoundManager1').instance.dynamicallyPlayMusic(ResDefine.sound_bet);

        Globals.Logging.Log("-=-=handleBet   " + objData);

        JObject data = JObject.Parse(objData);
        Player player = getPlayer(getString(data, "N"), true);
        JArray NumArr = getJArray(data, "Num");
        //List<string> Num = NumArr.ToObject<List<string>>();
        List<int> T = getListInt(data, "T");
        List<int> M = getListInt(data, "M");
  //      {
  //          "evt": "bet",
  //"data": "{\"N\":\"เจ เจ เจริญภักดิ์\",\"Num\":[\"Hi\",\"Hi\",\"11HiLo\",\"HI 6\",\"LO 1\",\"3 nut\"],\"M\":[300,20,20,20,20,20],\"T\":[1,1,7,5,5,6]}"
//}
        for (int i = 0; i < NumArr.Count; i++)
        {
            var str = (string)NumArr[i];
            int numberBet = convertBetToInteger(str, T[i]);
            player.ag -= M[i];
            player.setAg();
            if (player == thisPlayer)
            {
                stateGame = Globals.STATE_GAME.PLAYING;
                totalBet += M[i];
                boxBet.onBet(numberBet, M[i]);
                boxBet.creatDataBet();
                buttonBet.btn_Rebet.interactable = false;
                DOTween.Sequence()
                    .AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        buttonBet.activeDouble();
                    });
                if (buttonBet != null)
                    buttonBet.setStateButtonOnBet();
            }
            effectDatCuocChip(player, M[i], numberBet);
        }

        if (player == thisPlayer && player.ag < curChipBet)
        {
            curChipBet = player.ag;
        }
    }
    protected override int convertBetToInteger(string N, int T)
    {
        int index = 0;
        switch (N)
        {
            case "1 nut":
                index = 1;
                break;
            case "2 nut":
                index = 2;
                break;
            case "3 nut":
                index = 3;
                break;
            case "4 nut":
                index = 4;
                break;
            case "5 nut":
                index = 5;
                break;
            case "6 nut":
                index = 6;
                break;
            case "1-5":
                index = 7;
                break;
            case "1-6":
                index = 8;
                break;
            case "2-5":
                index = 9;
                break;
            case "2-6":
                index = 10;
                break;
            case "1-3":
                index = 11;
                break;
            case "4-6":
                index = 12;
                break;
            case "3-6":
                index = 13;
                break;
            case "2-4":
                index = 14;
                break;
            case "3-5":
                index = 15;
                break;
            case "4-1":
                index = 16;
                break;
            case "1-2":
                index = 17;
                break;
            case "2-3":
                index = 18;
                break;
            case "4-5":
                index = 19;
                break;
            case "5-6":
                index = 20;
                break;
            case "3-4":
                index = 21;
                break;
            case "1-2-3":
                index = 22;
                break;
            case "4-5-6":
                index = 23;
                break;
            case "5 LO":
                index = 24;
                break;
            case "6 LO":
                index = 25;
                break;
            case "LO 1":
                index = 26;
                break;
            case "LO 3":
                index = 27;
                break;
            case "HI 6":
                index = 28;
                break;
            case "HI 4":
                index = 29;
                break;
            case "Hi":
                index = 30;
                break;
            case "Lo":
                index = 31;
                break;
            case "11HiLo":
                index = 32;
                break;
        }
        return index;
    }
    protected override void updatePositionPlayerView()
    {
        listPlayerSicbo = new List<Player>();
        players.Sort((a, b) =>
        {
            return b.ag.CompareTo(a.ag);
        });

        for (int i = 0; i < players.Count; i++)
        {
            if (thisPlayer == players[i])
            {
                players.RemoveAt(i);
            }
        }
        List<Player> newListPl = new List<Player> { thisPlayer };
        players = newListPl.Concat(players).ToList();
        for (int i = 0; i < players.Count; i++)
        {
            int index = getDynamicIndex(getIndexOf(players[i]));
            players[i]._indexDynamic = index;
            if (index < listPosView.Count)
            {
                players[i].playerView.transform.localPosition = listPosView[index];// new Vector2(listPosView[index].x / 1280 * Screen.width, listPosView[index].y / 720 * Screen.height);
            }
          
            //------- SET SCALE PLAYERVIEW ------//
            if (players[i] == thisPlayer)
                players[i].playerView.transform.localScale = new Vector2(0.8f, 0.8f);
            else
                players[i].playerView.transform.localScale = new Vector2(0.7f, 0.7f);
            //----------- END ----------//
            if (index >=6)
            {
                listPlayerSicbo.Add(players[i]);
                players[i].playerView.gameObject.SetActive(false);
                players[i].playerView.transform.localPosition = avatar_chung.transform.localPosition;
                
            }
            else
            {
                players[i].playerView.gameObject.SetActive(true);
            }


            players[i].updateItemVip(players[i].vip);
        }
    }
    protected override JObject converIntegerToBet(int data)
    {
        Globals.Logging.Log("converIntegerToBet HIlo2");
        List<string> N = new List<string>();
        List<int> T = new List<int>();
        switch (data)
        {
            case 1:
                N.Add("1 nut");
                T.Add(6);
                break;
            case 2:
                N.Add("2 nut");
                T.Add(6);
                break;
            case 3:
                N.Add("3 nut");
                T.Add(6);
                break;
            case 4:
                N.Add("4 nut");
                T.Add(6);
                break;
            case 5:
                N.Add("5 nut");
                T.Add(6);
                break;
            case 6:
                N.Add("6 nut");
                T.Add(6);
                break;
            case 7:
                N.Add("1-5");
                T.Add(5);
                break;
            case 8:
                N.Add("1-6");
                T.Add(5);
                break;
            case 9:
                N.Add("2-5");
                T.Add(5);
                break;
            case 10:
                N.Add("2-6");
                T.Add(5);
                break;
            case 11:
                N.Add("1-3");
                T.Add(5);
                break;
            case 12:
                N.Add("4-6");
                T.Add(5);
                break;
            case 13:
                N.Add("3-6");
                T.Add(5);
                break;
            case 14:
                N.Add("2-4");
                T.Add(5);
                break;
            case 15:
                N.Add("3-5");
                T.Add(5);
                break;
            case 16:
                N.Add("4-1");
                T.Add(5);
                break;
            case 17:
                N.Add("1-2");
                T.Add(5);
                break;
            case 18:
                N.Add("2-3");
                T.Add(5);
                break;
            case 19:
                N.Add("4-5");
                T.Add(5);
                break;
            case 20:
                N.Add("5-6");
                T.Add(5);
                break;
            case 21:
                N.Add("3-4");
                T.Add(5);
                break;
            case 22:
                N.Add("1-2-3");
                T.Add(7);
                break;
            case 23:
                N.Add("4-5-6");
                T.Add(7);
                break;
            case 24:
                N.Add("5 LO");
                T.Add(5);
                break;
            case 25:
                N.Add("6 LO");
                T.Add(5);
                break;
            case 26:
                N.Add("LO 1");
                T.Add(2);
                break;
            case 27:
                N.Add("LO 3");
                T.Add(3);
                break;
            case 28:
                N.Add("HI 6");
                T.Add(2);
                break;
            case 29:
                N.Add("HI 4");
                T.Add(3);
                break;
            case 30:
                N.Add("Hi");
                T.Add(1);
                break;
            case 31:
                N.Add("Lo");
                T.Add(1);
                break;
            case 32:
                N.Add("11HiLo");
                T.Add(7);
                break;
        }
        JObject objData = new JObject();
        objData["numberBet"] = JArray.FromObject(N);
        objData["typeBet"] = JArray.FromObject(T);
        return objData;
    }
    // Update is called once per frame

}
