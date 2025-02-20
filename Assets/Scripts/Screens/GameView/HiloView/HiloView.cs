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

public class HiloView : GameView
{
    [SerializeField]
    public GameObject dealer;
    [SerializeField]
    protected GameObject nodeResult;
    [SerializeField]
    protected GameObject nodeHistory;
    [SerializeField]
    protected GameObject nodeAniWinLose;
    [SerializeField]
    protected GameObject avatar_chung;

    [SerializeField]
    protected GameObject gate_bet_prefab;

    [SerializeField]
    protected GameObject box_bet_prefab;

    [SerializeField]
    protected GameObject prefab_button_bet;
    [SerializeField]
    protected GameObject history_prefab;

    [SerializeField]
    protected GameObject chip_bet_prefab;



    [SerializeField]
    public SkeletonGraphic aniClock;

    [SerializeField]
    public SkeletonGraphic aniStart;


    [SerializeField]
    public SkeletonGraphic aniXocDia;


    [SerializeField]
    public List<Image> listXucXac;

    [SerializeField]
    public List<Sprite> listSpriteFrameXucXac;

    [SerializeField]
    public List<Sprite> listSpriteFrameXucXacHis;


    [SerializeField]
    public List<Image> listXucXacHistory;


    [SerializeField]
    GameObject prefab_popup_player;

    [SerializeField]
    protected GameObject prefab_rule;

    [SerializeField]
    protected Transform layerPopup;
    [SerializeField]
    protected Transform layerChip;

    public static HiloView instance;
    [HideInInspector] public int matchCount = 0, chipDealLastMatch = 0;
    [HideInInspector] public long curChipBet = 0, totalBet = 0, currentTempBet = 0, limitTotalBet = 0;
    protected bool isBet = false;
    [HideInInspector] public List<int> listValue = new List<int>();
    protected List<int> listDataChip = new List<int>();
    protected List<ChipBetSicbo> listChipInTable = new List<ChipBetSicbo>();
    public List<GameObject> chipBetPool = new List<GameObject>();
    public List<Player> listPlayerSicbo = new List<Player>();
    [HideInInspector] public ButtonBetSicbo buttonBet = null;
    [HideInInspector] public GateBetSicbo gateBet;
    [HideInInspector] public BoxbetSicbo boxBet;
    protected HistorySicbo popupHistory;
    protected NodePlayerSicbo listPlayer = null;
    protected RuleSicbo popupRule;
    protected JObject dataFinish;
    protected List<string> listDataFake = new List<string>();

    protected override void Start()
    {
        base.Start();
        listDataFake.Clear();
        //listDataFake.Add("{\"evt\":\"stable\",\"data\":\"{\\\"N\\\":\\\"รับโชคและชนะด้วย ไฮโลไทย!\\\",\\\"M\\\":5000,\\\"ArrP\\\":[{\\\"id\\\":69320,\\\"N\\\":\\\"fb.1075968683255893\\\",\\\"Url\\\":\\\"fb.1075968683255893\\\",\\\"AG\\\":285000,\\\"A\\\":true,\\\"LQ\\\":50,\\\"VIP\\\":2,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":0,\\\"FId\\\":1075968683255893,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"อะอะ โอ๊ตโอ๊ต\\\",\\\"keyObjectInGame\\\":0},{\\\"id\\\":8240,\\\"N\\\":\\\"hienndm\\\",\\\"Url\\\":\\\"\\\",\\\"AG\\\":136916,\\\"A\\\":true,\\\"LQ\\\":0,\\\"VIP\\\":3,\\\"isStart\\\":true,\\\"IK\\\":0,\\\"sIP\\\":\\\"10.148.0.4\\\",\\\"Av\\\":4,\\\"FId\\\":0,\\\"D\\\":0,\\\"Arr\\\":[],\\\"MB\\\":{\\\"M\\\":0,\\\"T\\\":0,\\\"W\\\":0},\\\"level\\\":0,\\\"displayName\\\":\\\"hienndm\\\",\\\"keyObjectInGame\\\":0}],\\\"Id\\\":4828,\\\"T\\\":0,\\\"V\\\":0,\\\"AG\\\":50000,\\\"S\\\":0,\\\"History\\\":[[2,3,4]]}\"}");
        //listDataFake.Add("{\"evt\":\"start\",\"data\":\"4000\"}");
        //listDataFake.Add("{\"evt\":\"lc\",\"data\":\"15000\"}");
        //listDataFake.Add("{\"evt\":\"bet\",\"data\":\"{\\\"N\\\":\\\"hienndm\\\",\\\"Num\\\":[[1],[2],[1,1],[2,2],[4,4],[5,5],[6,6],[30]],\\\"M\\\":[5000,5000,10000,20000,5000,10000,5000,50000],\\\"T\\\":[1,1,10,10,10,10,10,30]}\"}");
        //listDataFake.Add("{\"evt\":\"finish\",\"data\":\"{\\\"listDice\\\":[1,4,6],\\\"listNumberWin\\\":[{\\\"N\\\":\\\"1 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"4 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"6 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"11HiLo\\\",\\\"typewin\\\":7},{\\\"N\\\":\\\"1-6\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"4-6\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"4-1\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"4-5-6\\\",\\\"typewin\\\":7}],\\\"listUser\\\":[{\\\"pid\\\":69320,\\\"ag\\\":309100,\\\"vip\\\":2,\\\"listNumWin\\\":[{\\\"M\\\":25000,\\\"N\\\":\\\"4-6\\\",\\\"T\\\":5,\\\"W\\\":25000},{\\\"M\\\":20000,\\\"N\\\":\\\"4-5-6\\\",\\\"T\\\":7,\\\"W\\\":20000}]},{\\\"pid\\\":8240,\\\"ag\\\":136916,\\\"vip\\\":3,\\\"listNumWin\\\":[]}],\\\"History\\\":[[2,3,4],[1,4,6]]}\"}");
        //listDataFake.Add("{\"evt\":\"HETROI\",\"data\":\"15000\"}");
    }

    public virtual void onClickFake(Button btnFake)
    {
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
    protected override void Awake()
    {
        HiloView.instance = this;
        base.Awake();
        agTable = 0;
        matchCount = 0;
        curChipBet = 0;
        totalBet = 0;
        currentTempBet = 0;
        limitTotalBet = 0;
        chipDealLastMatch = 0;
        //----- OBJECT -----//
        buttonBet = null;
        gateBet = null;
        boxBet = null;
        popupHistory = null;
        listPlayer = null;
        popupRule = null;
        isBet = false;
    }
    // Update is called once per frame
    private void OnDisable()
    {
        if (buttonBet != null)
        {
            Destroy(buttonBet.gameObject);
        }
        if (popupHistory != null)
        {
            Destroy(popupHistory.gameObject);
        }
        if (popupRule != null)
        {
            Destroy(popupRule.gameObject);
        }
        chipBetPool.ForEach(chip =>
        {
            Destroy(chip);
        });
    }
    public void sendBetSicbo(string data)
    {
        if (!isBet) return;
        playSound(Globals.SOUND_GAME.BET);

        if (limitTotalBet + curChipBet > agTable * 100)
        {
            string msg = Globals.Config.getTextConfig("maximum_bet");
            UIManager.instance.showToast(msg);
            return;
        }
        limitTotalBet += curChipBet;
        currentTempBet += curChipBet;
        int index = int.Parse(data);
        boxBet.onClickBet(index, curChipBet);

        if (buttonBet != null)
            buttonBet.setStateButtonChip();

    }
    public override void handleSTable(string objData)
    {
        base.handleSTable(objData);
        Globals.Logging.Log("handleStable:" + objData);
        stateGame = Globals.STATE_GAME.WAITING;
        JObject data = JObject.Parse(objData);

        thisPlayer.playerView.setPosThanhBarThisPlayer();

        agTable = getInt(data, "M");
        listValue = new List<int> { agTable, agTable * 2, agTable * 5, agTable * 10, agTable * 20 };
        if (gateBet == null)
        {
            gateBet = gate_bet_prefab.GetComponent<GateBetSicbo>();
        }
        if (boxBet == null)
        {
            boxBet = box_bet_prefab.GetComponent<BoxbetSicbo>();
        }
        //--------------- CREAT CHIP BETTED --------------//
        JArray ArrP = getJArray(data, "ArrP");
        for (int i = 0; i < ArrP.Count; i++)
        {
            JObject dataPlayer = (JObject)ArrP[i];
            Player player = getPlayerWithID(getInt(dataPlayer, "id"));
            JArray Arr = getJArray(dataPlayer, "Arr");
            for (int j = 0; j < Arr.Count; j++)
            {
                JObject dataChip = (JObject)Arr[j];
                int numberBet = convertBetToInteger(getString(dataChip, "N"), getInt(dataChip, "T"));
                effectDatCuocChip(player, getInt(dataChip, "M"), numberBet);
            }
        }
        //--------------- END CREAT CHIP BETTED --------------//

        //--------------- STABLE WHEN STARTED -----------//
        if (getInt(data, "T") > 1)
        {

            TweenCallback effectShowButtonBet = () =>
            {
                if (buttonBet == null)
                {
                    buttonBet = Instantiate(prefab_button_bet, transform).GetComponent<ButtonBetSicbo>();
                    buttonBet.transform.SetSiblingIndex(layerPopup.transform.GetSiblingIndex() - 1);
                }
                buttonBet.gameObject.SetActive(true);
            };

            TweenCallback effectShowClock = () =>
            {
                showClock(true, Mathf.FloorToInt(getInt(data, "T") / 1000) - 1);
                isBet = true;
                aniXocDia.gameObject.SetActive(true);
            };
            DOTween.Sequence()
                .AppendCallback(effectShowButtonBet)
                .AppendInterval(1.0f)
                .AppendCallback(effectShowClock);
        }
        else
        {
            if (buttonBet == null)
            {
                buttonBet = Instantiate(prefab_button_bet, transform).GetComponent<ButtonBetSicbo>();
                buttonBet.transform.SetSiblingIndex(layerPopup.transform.GetSiblingIndex() - 1);
            }
            buttonBet.gameObject.SetActive(false);

            UIManager.instance.showToast(Globals.Config.getTextConfig("txt_view_table"), transform);
        }
        //------------- END ------------//
        if (popupHistory == null)
        {
            popupHistory = Instantiate(history_prefab, layerPopup).GetComponent<HistorySicbo>();
        }
        popupHistory.onStart();
        popupHistory.gameObject.SetActive(false);
        JArray his = (JArray)data["History"];
        List<List<int>> History = his.ToObject<List<List<int>>>();
        popupHistory.handleDataHistory(History);
        //------------- Instance Rule ----------------//
        if (popupRule == null)
        {
            popupRule = Instantiate(prefab_rule, layerPopup).GetComponent<RuleSicbo>();
        }        //-----------------------------------//
        //------------- CREAT HISTORY ------------//
        popupRule.gameObject.SetActive(false);
        nodeHistory.gameObject.SetActive(false);
        if (History.Count > 0)
        {
            List<int> dataHis = History.Last();
            History.RemoveAt(History.Count - 1);
            nodeHistory.gameObject.SetActive(true);
            int sc = 0;
            dataHis.ForEach(dice => { sc += dice; });
            nodeHistory.transform.Find("lb_number").GetComponent<TextMeshProUGUI>().text = sc + "";
            int index = 0;
            for (int i = 0; i < listXucXacHistory.Count; i++)
            {
                Image itemHistory = listXucXacHistory[i];
                if (dataHis.Count < i) return;

                Transform itHistTf = itemHistory.transform;
                DOTween.Sequence().AppendInterval(0.05f * i).Append(itHistTf.DOLocalMove(new Vector2(itHistTf.localPosition.x - 40, -40), 0.2f).SetEase(Ease.InCubic))
                    .AppendCallback(() =>
                    {
                        itHistTf.localPosition = new Vector2(itHistTf.localPosition.x, 60);
                        itemHistory.sprite = listSpriteFrameXucXacHis[dataHis[index]];
                        index++;
                    })
                    .Append(itHistTf.DOLocalMove(new Vector2(itHistTf.localPosition.x, 0), 0.1f).SetEase(Ease.InCubic));
            }
        }
        //------------- END CREAT ---------------//
        avatar_chung.transform.Find("lb_number_per").GetComponent<TextMeshProUGUI>().text = "+" + listPlayerSicbo.Count;
    }
    public override void handleCTable(string objData)
    {
        base.handleCTable(objData);
        stateGame = Globals.STATE_GAME.WAITING;
        JObject data = JObject.Parse(objData);

        thisPlayer.playerView.setPosThanhBarThisPlayer();
        agTable = getInt(data, "M");
        listValue.AddRange(new List<int> { agTable, agTable * 2, agTable * 5, agTable * 10, agTable * 20 });
        if (gateBet == null)
        {
            gateBet = gate_bet_prefab.GetComponent<GateBetSicbo>();
        }
        if (boxBet == null)
        {
            boxBet = box_bet_prefab.GetComponent<BoxbetSicbo>();
        }
        //------------- Instance Rule ----------------//
        if (popupRule == null)
        {
            popupRule = Instantiate(prefab_rule, layerPopup).GetComponent<RuleSicbo>();
        }
        popupRule.gameObject.SetActive(false);
        //-----------------------------------//
        // History
        if (popupHistory == null)
        {
            popupHistory = Instantiate(history_prefab, layerPopup).GetComponent<HistorySicbo>();
        }
        popupHistory.onStart();
        popupHistory.gameObject.SetActive(false);
        nodeHistory.gameObject.SetActive(false);
        avatar_chung.transform.Find("lb_number_per").GetComponent<TextMeshProUGUI>().text = "+" + (listPlayerSicbo.Count).ToString();
    }
    public override void handleJTable(string objData)
    {
        base.handleJTable(objData);
        avatar_chung.transform.Find("lb_number_per").GetComponent<TextMeshProUGUI>().text = "+" + (listPlayerSicbo.Count).ToString();
    }
    public override void handleLTable(JObject objData)
    {
        base.handleLTable(objData);
        playSound(Globals.SOUND_GAME.REMOVE);
        avatar_chung.transform.Find("lb_number_per").GetComponent<TextMeshProUGUI>().text = "+" + (listPlayerSicbo.Count).ToString();
    }
    public void handleStart()
    {
        //hideAllDialog();
        TweenCallback effectStart = () =>
        {
            aniStart.gameObject.SetActive(true);
            aniStart.Initialize(true);
            aniStart.AnimationState.SetAnimation(0, "starttime_thai", false);
            playSound(Globals.SOUND_HILO.START_GAME);
        };
        TweenCallback effectXocDia = () =>
        {
            aniStart.gameObject.SetActive(false);
            aniXocDia.gameObject.SetActive(true);
            playSound(Globals.SOUND_HILO.DICE_SHAKE);
            aniXocDia.Initialize(true);
            aniXocDia.AnimationState.SetAnimation(0, "lac", false);
        };
        TweenCallback effectBetTime = () =>
        {
            aniStart.gameObject.SetActive(true);
            aniStart.Initialize(true);
            aniStart.AnimationState.SetAnimation(0, "betime_thai", false);
            DOTween.Sequence().AppendInterval(1.0f).AppendCallback(() =>
            {
                aniStart.gameObject.SetActive(false);
            });
        };
        DOTween.Sequence()
            .AppendCallback(effectStart)
            .AppendInterval(1.5f)
            .AppendCallback(effectXocDia)
            .AppendInterval(2.5f)
            .AppendCallback(effectBetTime);
    }
    public void handleStartBet(string objData)
    {
        Globals.Logging.Log("handleStartBet");
        int data = int.Parse(objData);
        TweenCallback effectShowButtonBet = () =>
        {
            //------ SHOW BUTTON BET ------//
            if (buttonBet == null)
            {
                buttonBet = Instantiate(prefab_button_bet, transform).GetComponent<ButtonBetSicbo>();
                buttonBet.transform.SetSiblingIndex(layerPopup.transform.GetSiblingIndex() - 1);
            }

            buttonBet.gameObject.SetActive(true);
            Globals.Logging.Log("handleStartBet show button bet len");
            //----- END SHOW BUTTON -----//
        };
        TweenCallback effectShowClock = () =>
        {
            //---------- SHOW CLOCK ---------//
            showClock(true, Mathf.FloorToInt(data / 1000) - 1);
            isBet = true;
            //---------- END SHOW ----------//
        };
        DOTween.Sequence()
            .AppendCallback(effectShowButtonBet)
            .AppendInterval(1.0f)
            .AppendCallback(effectShowClock);
    }
    private string convertArrayToStr(List<int> list)
    {
        string str = "";

        for (int i = 0; i < list.Count; i++)
        {
            if (i != list.Count - 1)
            {
                str += list[i] + ",";
            }
            else
            {
                str += list[i].ToString();
            }
        }
        return str;
    }
    public virtual void handleBet(string objData)
    {
        playSound(Globals.SOUND_GAME.BET);
        JObject data = JObject.Parse(objData);
        Player player = getPlayer(getString(data, "N"), true);
        JArray NumArr = getJArray(data, "Num");
        List<List<int>> Num = NumArr.ToObject<List<List<int>>>();
        List<int> T = getListInt(data, "T");
        List<int> M = getListInt(data, "M");
        for (int i = 0; i < Num.Count; i++)
        {
            int numberBet = convertBetToInteger(convertArrayToStr(Num[i]), T[i]);
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
    protected void effectDatCuocChip(Player player, int valueBet, int numberBet)
    {
        Vector2 pos = player.playerView.transform.localPosition;
        //---------  Gen vi trí và tổng cược mỗi ô bet ----------//
        Vector2 posGate = gateBet.getPositionGateBet(numberBet);
        int totalBetPerGate = gateBet.setValueGateBet(numberBet, valueBet);

        TweenCallback effectCreatChipTable = () =>
        {
            gateBet.setValueChipForGate(numberBet, valueBet);
        };
        TweenCallback effectBet = () =>
        {
            ChipBetSicbo ChipBet = createChip(numberBet, valueBet, pos);
            ChipBet.pid = player.id;
            ChipBet.chipMoveTo(posGate, false, effectCreatChipTable);
            listChipInTable.Add(ChipBet);
            getPlayerView(player).listChipBetPl.Add(ChipBet);
        };

        DOTween.Sequence()
            .AppendCallback(effectBet);
    }
    private PlayerViewSicbo getPlayerView(Player player)
    {
        if (player != null)
        {
            return (PlayerViewSicbo)player.playerView;
        }
        return null;
    }
    public ChipBetSicbo createChip(int numberBet, int valueChip, Vector2 posInit)
    {
        if (chipBetPool.Count == 0)
        {
            chipBetPool.Add(Instantiate(chip_bet_prefab, layerChip));
        }
        ChipBetSicbo chipBet = chipBetPool[0].GetComponent<ChipBetSicbo>();
        chipBet.chipDeal = 0;
        chipBetPool.RemoveAt(0);
        chipBet.transform.SetSiblingIndex(transform.childCount - 2);
        chipBet.transform.localPosition = posInit;
        chipBet.setChip(valueChip, numberBet);
        chipBet.gameObject.SetActive(true);

        return chipBet;
    }
    private void showClock(bool isShow, int timeClock)
    {
        //aniClock.ga.stopAllActions();
        aniClock.gameObject.SetActive(false);
        if (isShow)
        {
            aniClock.gameObject.SetActive(true);
            int del = 1;
            TweenCallback time = () =>
            {
                //timeClock -= Globals.Config.;
                //require("GameManager").getInstance().time_out_game = 0;

                aniClock.transform.Find("lb_clock").GetComponent<TextMeshProUGUI>().text = timeClock.ToString();
                if (timeClock > 0)
                {
                    timeClock--;
                    if (timeClock > 5)
                    {
                        playSound(Globals.SOUND_GAME.TICKTOK);
                    }
                    else
                    {
                        playSound(Globals.SOUND_GAME.CLOCK_HURRY);
                    }
                    aniClock.Initialize(true);
                    aniClock.AnimationState.SetAnimation(0, "animation", false);
                }
                else
                {
                    //aniClock.node.stopAction();
                    aniClock.gameObject.SetActive(false);
                    buttonBet.gameObject.SetActive(false);
                }
            };
            DOTween.Sequence()
                .AppendCallback(time)
                .AppendInterval(del)
                .SetLoops(timeClock + 1);
        }
        else
        {
            timeClock = -1;
        }
    }
    protected virtual int convertBetToInteger(string N, int T)
    {
        int index = 0;
        switch (T)
        {
            case 1:
                switch (N)
                {
                    case "1":
                        index = 1;
                        break;
                    case "2":
                        index = 2;
                        break;
                }

                break;
            case 2:
                switch (N)
                {
                    case "1":
                        index = 3;
                        break;
                    case "2":
                        index = 4;
                        break;
                    case "3":
                        index = 5;
                        break;
                    case "4":
                        index = 6;
                        break;
                    case "5":
                        index = 7;
                        break;
                    case "6":
                        index = 8;
                        break;
                }

                break;
            case 6:
                switch (N)
                {
                    case "10":
                        index = 9;
                        break;
                    case "11":
                        index = 10;
                        break;
                }
                break;

            case 7:
                switch (N)
                {
                    case "9":
                        index = 11;
                        break;
                    case "12":
                        index = 12;
                        break;
                }
                break;

            case 8:
                switch (N)
                {
                    case "8":
                        index = 13;
                        break;
                    case "13":
                        index = 14;
                        break;
                }
                break;

            case 10:
                switch (N)
                {
                    case "1,1":
                        index = 15;
                        break;
                    case "2,2":
                        index = 16;
                        break;
                    case "3,3":
                        index = 17;
                        break;
                    case "4,4":
                        index = 18;
                        break;
                    case "5,5":
                        index = 19;
                        break;
                    case "6,6":
                        index = 20;
                        break;
                }
                break;

            case 12:
                switch (N)
                {
                    case "7":
                        index = 21;
                        break;
                    case "14":
                        index = 22;
                        break;
                }
                break;
            case 18:
                switch (N)
                {
                    case "6":
                        index = 23;
                        break;
                    case "15":
                        index = 24;
                        break;
                }
                break;
            case 30:
                switch (N)
                {
                    case "5":
                        index = 25;
                        break;
                    case "16":
                        index = 26;
                        break;
                    case "30":
                        index = 27;
                        break;
                }
                break;

            case 60:
                switch (N)
                {
                    case "4":
                        index = 28;
                        break;
                    case "17":
                        index = 29;
                        break;
                }
                break;
        }
        return index;
    }

    public void onClickGateBet(int gate)
    {

    }
    public void onClickHistory()
    {
        SoundManager.instance.soundClick();
        //require('SMLSocketIO').getInstance().emitSIOCCC(cc.js.formatStr("ClickHistory_%s", require('GameManager').getInstance().getCurrentSceneName()));
        popupHistory.show();
        popupHistory.transform.SetAsLastSibling();
    }
    public override void onClickRule()
    {
        popupRule.show();
        popupRule.transform.SetAsLastSibling();
    }
    public void onClickRebet()
    {
        SoundManager.instance.soundClick();
        List<string> arrNumBerBet = new List<string>();
        List<int> arrTypeBet = new List<int>();
        List<int> arrValue = new List<int>();
        for (int i = 0; i < boxBet.dataBet.Count; i++)
        {
            JObject dataBet = (JObject)boxBet.dataBet[i];
            JObject objDataChip = converIntegerToBet(getInt(dataBet, "numberBet"));
            arrNumBerBet.Add((string)objDataChip["numberBet"][0]);
            arrValue.Add(getInt(dataBet, "value"));
            arrTypeBet.Add((int)objDataChip["typeBet"][0]);
        }
        SocketSend.sendBetSicbo(arrNumBerBet, arrValue, arrTypeBet);
    }
    public void onClickDouble()
    {
        SoundManager.instance.soundClick();
        List<string> arrNumBerBet = new List<string>();
        List<int> arrTypeBet = new List<int>();
        List<int> arrValue = new List<int>();
        //----------- CREAT DATA BET --------//
        for (int i = 0; i < boxBet.dataBet.Count; i++)
        {
            JObject dataBet = (JObject)boxBet.dataBet[i];
            JObject objDataChip = converIntegerToBet(getInt(dataBet, "numberBet"));
            arrNumBerBet.Add((string)objDataChip["numberBet"][0]);
            arrValue.Add((int)dataBet["value"]);
            arrTypeBet.Add((int)objDataChip["typeBet"][0]);
        }
        //require("NetworkManager").getInstance().sendBetSicbo(arrNumBerBet, arrValue, arrTypeBet);
        SocketSend.sendBetSicbo(arrNumBerBet, arrValue, arrTypeBet);
    }

    public void onClickDeal()
    {
        SoundManager.instance.soundClick();
        List<string> arrNumBerBet = new List<string>();
        List<int> arrTypeBet = new List<int>();
        List<int> arrValue = new List<int>();
        JArray arrBet = boxBet.dataClickBet;
        for (int i = 0; i < arrBet.Count; i++)
        {
            JObject dataBet = (JObject)arrBet[i];
            JObject objDataChip = converIntegerToBet(getInt(dataBet, "numberBet"));
            arrNumBerBet.Add((string)objDataChip["numberBet"][0]);
            arrValue.Add((int)dataBet["value"]);
            arrTypeBet.Add((int)objDataChip["typeBet"][0]);
        }
        boxBet.resetDataClickBet();
        SocketSend.sendBetSicbo(arrNumBerBet, arrValue, arrTypeBet);
    }
    public void onClickClear()
    {
        SoundManager.instance.soundClick();
        limitTotalBet -= currentTempBet;
        currentTempBet = 0;
        boxBet.onClickClearBet();
    }
    public void setValueBtnBet(int data)
    {
        //data = parseInt(data);
        for (int i = 0; i < listValue.Count; i++)
        {
            if (data - 1 >= 0)
            {
                if (thisPlayer.ag < listValue[data - 1])
                    data = data - 2;
            }
        }
        if (data < 1) data = 1;
        chipDealLastMatch = data;
        switch (data)
        {
            case 1:
                curChipBet = agTable;
                if (thisPlayer.ag < curChipBet)
                    curChipBet = thisPlayer.ag;
                break;
            case 2:
                curChipBet = agTable * 2;
                break;
            case 3:
                curChipBet = agTable * 5;
                break;
            case 4:
                curChipBet = agTable * 10;
                break;
            case 5:
                curChipBet = agTable * 20;
                break;
        }
    }
    public void handleFinish(string objData)
    {
        //----- NEXT EVT ------//
        SocketSend.getFarmInfo();
        JObject data = JObject.Parse(objData);
        //data = JObject.Parse("{\"evt\":\"finish\",\"data\":\"{\\\"listDice\\\":[1,1,3],\\\"listNumberWin\\\":[{\\\"N\\\":\\\"1 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"3 nut\\\",\\\"typewin\\\":6},{\\\"N\\\":\\\"Lo\\\",\\\"typewin\\\":1},{\\\"N\\\":\\\"1-3\\\",\\\"typewin\\\":5},{\\\"N\\\":\\\"1-2-3\\\",\\\"typewin\\\":7},{\\\"N\\\":\\\"LO 1\\\",\\\"typewin\\\":2},{\\\"N\\\":\\\"LO 3\\\",\\\"typewin\\\":3}],\\\"listUser\\\":[{\\\"pid\\\":8240,\\\"ag\\\":99153,\\\"vip\\\":3,\\\"listNumWin\\\":[{\\\"M\\\":100,\\\"N\\\":\\\"3 nut\\\",\\\"T\\\":6,\\\"W\\\":100},{\\\"M\\\":300,\\\"N\\\":\\\"LO 3\\\",\\\"T\\\":3,\\\"W\\\":300},{\\\"M\\\":100,\\\"N\\\":\\\"Lo\\\",\\\"T\\\":1,\\\"W\\\":100}]}],\\\"History\\\":[[2,5,6],[1,5,6],[2,2,6],[1,5,5],[1,1,3]]}\"}");
        dataFinish = data;
        popupHistory.onStart();
        JArray his = (JArray)data["History"];
        List<List<int>> History = his.ToObject<List<List<int>>>();
        popupHistory.handleDataHistory(History);
        List<int> dataHistory = History.Last();
        History.RemoveAt(History.Count - 1);
        buttonBet.gameObject.SetActive(false);
        boxBet.creatDataBet();
        isBet = false;
        gateBet.resetOCuoc();
        gateBet.setStateButtonBet(true);
        onClickClear();
        //----------- START EFFECT FINISH -----------//
        TweenCallback effectOpen = () =>
        {
            aniStart.gameObject.SetActive(true);
            aniStart.Initialize(true);
            aniStart.AnimationState.SetAnimation(0, "open_thai", false);
        };
        TweenCallback effectShowResult = () =>
        {
            aniStart.gameObject.SetActive(false);
            aniXocDia.gameObject.SetActive(true);
            playSound(Globals.SOUND_HILO.DICE_OPEN);

            aniXocDia.Initialize(true);
            aniXocDia.AnimationState.SetAnimation(0, "open", false);
            showResultNumber();
        };
        TweenCallback effectNormal = () =>
        {
            aniXocDia.Initialize(true);
            aniXocDia.AnimationState.SetAnimation(0, "khong lac", true);
            nodeResult.gameObject.SetActive(false);
            //------------- CREAT HISTORY ------------//
            nodeHistory.gameObject.SetActive(true);
            int sc = 0;
            dataHistory.ForEach(dice => { sc += dice; });
            nodeHistory.transform.Find("lb_number").GetComponent<TextMeshProUGUI>().text = sc + "";
            int index = 0;
            for (int i = 0; i < listXucXacHistory.Count; i++)
            {
                Image itemHistory = listXucXacHistory[i];
                Transform itemTf = itemHistory.transform;
                if (dataHistory.Count < i) return;
                float timeDelay = 0.1f * i;
                DOTween.Sequence()
                .AppendInterval(timeDelay)
                .Append(itemTf.DOLocalMove(new Vector2(itemTf.localPosition.x, -40), 0.2f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    itemTf.localPosition = new Vector2(itemTf.localPosition.x, 60);
                }))
                .AppendInterval(0.1f)
                .Append(itemTf.DOLocalMove(new Vector2(itemTf.localPosition.x, 0), 0.2f).SetEase(Ease.InCubic));
                float timeDelay1 = 0.1f * i + 0.2f;
                DOTween.Sequence()
                 .AppendInterval(timeDelay1)
                    .AppendCallback(() =>
                      {
                          itemHistory.sprite = listSpriteFrameXucXacHis[dataHistory[index]];
                          index++;
                      });
            }
            //------------- END CREAT ---------------//

        };
        TweenCallback startAction = () =>
        {
            actionController();
        };
        //---------- RUN EFFECT ---------------//
        DOTween.Sequence()
        .AppendCallback(effectOpen)
        .AppendInterval(1.0f)
        .AppendCallback(effectShowResult)
        .AppendInterval(2.5f)
        .AppendCallback(effectNormal)
        .AppendCallback(startAction);
    }
    private void actionController()
    {
        //---------- CU CHUOI ---------//
        JArray dataWin = (JArray)dataFinish["listNumberWin"];
        for (int i = 0; i < dataWin.Count; i++)
        {
            JObject strDataWin = (JObject)dataWin[i];
            string numberBet = getString(strDataWin, "N");
            int typeBet = getInt(strDataWin, "typewin");
            int index = convertBetToInteger(numberBet, typeBet);
            gateBet.effectWinGate(index);
        }
        List<ChipBetSicbo> listChipWin = genListChipWin();
        //-----------------------------//
        TweenCallback effectWithdrawChipLose = () =>
        {
            gateBet.getChipLose();
            genAgLose();
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                PlayerViewSicbo playerV = getPlayerView(player);
                if (playerV.agLose < 0 && player._indexDynamic < 6)
                {
                    //player.ag -= playerV.agLose;
                    //player.setAg();
                    playerV.effectFlyMoney(playerV.agLose);
                    playSound(Globals.SOUND_HILO.CHIP_LOSER);
                }

            }
        };
        TweenCallback effectPayChipWin = () =>
        {
            payChipWin();
        };
        TweenCallback effectReceiveChip = () =>
        {
            receiveChip();
        };
        TweenCallback effectWinLose = () =>
         {
             showEffectWinLose();
         };
        TweenCallback effectResetGameView = () =>
        {
            resetGameView();
        };
        DOTween.Sequence()
            .AppendInterval(2.0f)
            .AppendCallback(effectWithdrawChipLose)
            .AppendInterval(listChipInTable.Count > 0 ? 1 : 0)
            .AppendCallback(effectPayChipWin)
            .AppendInterval(listChipWin.Count > 0 ? 2 : 0.4f)
            .AppendCallback(effectReceiveChip)
            .AppendInterval(listChipWin.Count > 0 ? 1 : 0)
            .AppendCallback(effectWinLose)
            .AppendInterval(2.0f)
            .AppendCallback(effectResetGameView);
    }
    private void resetGameView()
    {
        stateGame = Globals.STATE_GAME.WAITING;
        curChipBet = 0;
        totalBet = 0;
        matchCount++;
        boxBet.resetBoxBet();
        updatePositionPlayerView();
        for (int i = 0; i < listChipInTable.Count; i++)
        {
            ChipBetSicbo chip = listChipInTable[i];
            //chip.transform.SetParent(null);
            chip.gameObject.SetActive(false);
            chipBetPool.Add(chip.gameObject);
        }
        for (int i = 0; i < players.Count; i++)
        {
            getPlayerView(players[i]).listChipBetPl.ForEach(chip => chip.resetValue());
            getPlayerView(players[i]).listChipBetPl.Clear();

        }
        listChipInTable.Clear();
        gateBet.resetGateBet();
        //----- NEXT EVT ------//
        HandleGame.nextEvt();
        checkAutoExit();
        //if (cc.sys.localStorage.getItem("isBack") == 'true') require('NetworkManager').getInstance().sendExitGame();
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
        //players = [thisPlayer].concat(players);
        players.Insert(0, thisPlayer);
        for (int i = 0; i < players.Count; i++)
        {
            int index = getDynamicIndex(getIndexOf(players[i]));
            players[i]._indexDynamic = index;
            players[i].playerView.transform.localPosition = new Vector2(listPosView[index].x / 1280 * Screen.currentResolution.height, listPosView[index].y / 720 * Screen.currentResolution.width);
            //------- SET SCALE PLAYERVIEW ------//
            if (players[i] == thisPlayer)
                players[i].playerView.transform.localScale = new Vector2(0.8f, 0.8f);
            else
                players[i].playerView.transform.localScale = new Vector2(0.7f, 0.7f);
            //----------- END ----------//
            if (index >= 6)
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
    private void showEffectWinLose()
    {
        //----------- SHOW EFFECT WIN/LOSE ------------//
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            PlayerViewSicbo playerV = getPlayerView(player);
            if (player == thisPlayer)
            {
                nodeAniWinLose.transform.parent.SetSiblingIndex(transform.childCount - 1);
                if (playerV.agWin > 0)
                {
                    playSound(Globals.SOUND_HILO.WIN);
                    nodeAniWinLose.gameObject.SetActive(true);
                    nodeAniWinLose.transform.Find("ani_win").gameObject.SetActive(true);
                    nodeAniWinLose.transform.Find("ani_win").GetComponent<SkeletonGraphic>().Initialize(true);
                    nodeAniWinLose.transform.Find("ani_win").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "thai", true);
                    nodeAniWinLose.transform.Find("ani_win").Find("lb_win").GetComponent<TextMeshProUGUI>().text = "+" + Globals.Config.FormatNumber(playerV.agWin);
                    DOTween.Sequence().AppendInterval(2.0f)
                        .AppendCallback(() =>
                        {
                            nodeAniWinLose.gameObject.SetActive(false);
                            nodeAniWinLose.transform.Find("ani_win").gameObject.SetActive(false);
                        });
                }
                else if (playerV.agLose < 0)
                {
                    playSound(Globals.SOUND_HILO.LOSE);
                    nodeAniWinLose.gameObject.SetActive(true);
                    nodeAniWinLose.transform.Find("ani_lose").gameObject.SetActive(true);
                    nodeAniWinLose.transform.Find("ani_lose").GetComponent<SkeletonGraphic>().Initialize(true);
                    nodeAniWinLose.transform.Find("ani_lose").GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, "thai", true);
                    nodeAniWinLose.transform.Find("ani_lose").transform.Find("lb_lose").GetComponent<TextMeshProUGUI>().text = Globals.Config.FormatNumber(playerV.agLose);


                    DOTween.Sequence().AppendInterval(2.0f)
                      .AppendCallback(() =>
                      {
                          nodeAniWinLose.gameObject.SetActive(false);
                          nodeAniWinLose.transform.Find("ani_lose").gameObject.SetActive(false);
                      });
                }
            }
        }
        //------------ END SHOW -----------//
    }
    private void receiveChip()
    {
        Globals.Logging.Log("receiveChip");
        List<ChipBetSicbo> listChip = genListChipWin();
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            PlayerViewSicbo playerV = getPlayerView(player);
            if (player == null) continue;
            if (playerV.agWin > 0 && player._indexDynamic < 6)
            {
                playSound(Globals.SOUND_HILO.CHIP_WINNER);
                //player.ag += playerV.agWin;
                player.setAg();
                playerV.effectFlyMoney(playerV.agWin);

            }
            for (int j = 0; j < listChip.Count; j++)
            {
                ChipBetSicbo chip = listChip[j];
                if (chip.pid == player.id)
                {
                    chip.gameObject.SetActive(true);
                    chip.chipMoveToPlayer(0, playerV.transform.localPosition, false);
                }
            }
        }
        for (int i = 0; i < listPlayerSicbo.Count; i++) //list nhung thang >6
        {
            Player player = players[i];
            PlayerViewSicbo playerV = getPlayerView(player);
            if (player == null) continue;
            if (playerV.agWin > 0 && player._indexDynamic < 6)
            {
                playSound(Globals.SOUND_HILO.CHIP_WINNER);
                //player.ag += playerV.agWin;
                player.setAg();
                playerV.effectFlyMoney(playerV.agWin);

            }
            for (int j = 0; j < listChip.Count; j++)
            {
                ChipBetSicbo chip = listChip[j];
                if (chip.pid == player.id)
                {
                    chip.gameObject.SetActive(true);
                    chip.chipMoveToPlayer(0, playerV.transform.localPosition, false);
                }
            }
        }
        gateBet.removeChipInTable();
    }
    private void genAgLose()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            PlayerViewSicbo playerV = getPlayerView(player);
            playerV.agLose = 0;
            for (int j = 0; j < playerV.listChipBetPl.Count; j++)
            {

                ChipBetSicbo chipBet = playerV.listChipBetPl[j];
                if (!gateBet.listWinResult.Contains((chipBet.numberBet - 1)))
                {
                    playerV.agLose -= chipBet.chipDeal;
                }
            }
        }
    }
    private List<ChipBetSicbo> genListChipWin()
    {
        List<ChipBetSicbo> listChipWin = new List<ChipBetSicbo>();
        for (int i = 0; i < listChipInTable.Count; i++)
        {
            ChipBetSicbo chip = listChipInTable[i];
            if (gateBet.listWinResult.Contains((chip.numberBet - 1)))
            {
                listChipWin.Add(chip);
            }
        }
        return listChipWin;
    }
    private void payChipWin()
    {
        TweenCallback effectPayChip = () =>
        {
            JArray listUser = (JArray)dataFinish["listUser"];
            for (int i = 0; i < listUser.Count; i++)
            {
                JObject user = (JObject)listUser[i];
                Player player = getPlayerWithID(getInt(user, "pid"));
                PlayerViewSicbo playerV = getPlayerView(player);
                if (player == null) continue;
                playerV.agWin = 0;
                playerV.agWin = getLong(user, "ag") - player.ag;
                player.ag = getInt(user, "ag");
                JArray listNumWin = getJArray(user, "listNumWin");
                for (int j = 0; j < listNumWin.Count; j++)
                {
                    JObject gateBeted = (JObject)listNumWin[j];
                    int numberBet = convertBetToInteger(getString(gateBeted, "N"), getInt(gateBeted, "T"));
                    gateBet.setValueGateWin(numberBet, getInt(gateBeted, "M"));
                    Vector2 posGate = gateBet.getPositionGateBet(numberBet);
                    posGate = new Vector2(posGate.x + (posGate.x > 0 ? -35 : 35), posGate.y);
                    ChipBetSicbo chipWin = createChip(numberBet, getInt(gateBeted, "M"), dealer.transform.localPosition);// creat chip win
                    chipWin.pid = getInt(user, "pid");
                    chipWin.chipMoveTo(posGate, false);
                    listChipInTable.Add(chipWin);
                }
            }
        };
        TweenCallback effectCreatChipWinTable = () =>
        {
            gateBet.creatDataChipWin();
            List<JObject> dataChipWin = gateBet.dataWin;
            for (int i = 0; i < dataChipWin.Count; i++)
            {
                JObject dataChip = dataChipWin[i];
                gateBet.createChipWin((int)dataChip["numberWin"]);
            }
        };
        DOTween.Sequence()
            .AppendCallback(effectPayChip)
            .AppendInterval(0.35f)
            .AppendCallback(effectCreatChipWinTable);


    }
    private void showResultNumber()
    {
        JArray listDice = (JArray)dataFinish["listDice"];
        List<int> dataResult = getListInt(dataFinish, "listDice");
        if (dataResult.Count != 3) return;
        nodeResult.gameObject.SetActive(true);
        for (int i = 0; i < listXucXac.Count; i++)
        {
            Image xucXac = listXucXac[i];
            xucXac.sprite = listSpriteFrameXucXac[dataResult[i]];
            xucXac.SetNativeSize();
            if (dataResult[i] == 3)
            {
                xucXac.transform.localPosition = new Vector2(-6.4f, 7.2f);
            }
            else if (dataResult[i] == 4)
            {
                xucXac.transform.localPosition = new Vector2(-4, 6.2f);
            }
            else if (dataResult[i] == 5)
            {
                xucXac.transform.localPosition = new Vector2(-3.2f, 8.8f);
            }
            else
            {
                xucXac.transform.localPosition = Vector2.zero;
            }
        }
    }

    protected virtual JObject converIntegerToBet(int data)
    {
        //Globals.Logging.Log("converIntegerToBet HIlo1:" + data);
        List<string> N = new List<string>();
        List<int> T = new List<int>();
        switch (data)
        {
            case 1:
                N.Add("1 nut");
                T.Add(6);
                break;
            case 2:
                N.Add(
                    "2 nut"
                );
                T.Add(6);
                break;
            case 3:
                N.Add(
                    "3 nut"
                );
                T.Add(6);
                break;
            case 4:
                N.Add(
                    "4 nut"
                );
                T.Add(6);
                break;
            case 5:
                N.Add(
                    "5 nut"
                );
                T.Add(6);
                break;
            case 6:
                N.Add(
                    "6 nut"
                );
                T.Add(6);
                break;
            case 7:
                N.Add(
                    "1-5"
                );
                T.Add(5);
                break;
            case 8:
                N.Add(
                    "1-6"
                );
                T.Add(5);
                break;
            case 9:
                N.Add(
                    "2-5"
                );
                T.Add(5);
                break;
            case 10:
                N.Add(
                    "2-6"
                );
                T.Add(5);
                break;
            case 11:
                N.Add(
                    "1-3"
                );
                T.Add(5);
                break;
            case 12:
                N.Add(
                    "4-6"
                );
                T.Add(5);
                break;
            case 13:
                N.Add(
                    "3-6"
                );
                T.Add(5);
                break;
            case 14:
                N.Add(
                    "2-4"
                );
                T.Add(5);
                break;
            case 15:
                N.Add(
                    "3-5"
                );
                T.Add(5);
                break;
            case 16:
                N.Add(
                    "4-1"
                );
                T.Add(5);
                break;
            case 17:
                N.Add(
                    "1-2"
                );
                T.Add(5);
                break;
            case 18:
                N.Add(
                    "2-3"
                );
                T.Add(5);
                break;
            case 19:
                N.Add(
                    "4-5"
                );
                T.Add(5);
                break;
            case 20:
                N.Add(
                    "5-6"
                );
                T.Add(5);
                break;
            case 21:
                N.Add(
                    "3-4"
                );
                T.Add(5);
                break;
            case 22:
                N.Add(
                    "1-2-3"
                );
                T.Add(7);
                break;
            case 23:
                N.Add(
                    "4-5-6"
                );
                T.Add(7);
                break;
            case 24:
                N.Add(
                    "5 LO"
                );
                T.Add(5);
                break;
            case 25:
                N.Add(
                    "6 LO"
                );
                T.Add(5);
                break;
            case 26:
                N.Add(
                    "LO 1"
                );
                T.Add(2);
                break;
            case 27:
                N.Add(
                    "LO 3"
                );
                T.Add(3);
                break;
            case 28:
                N.Add(
                    "HI 6"
                );
                T.Add(2);
                break;
            case 29:
                N.Add(
                    "HI 4"
                );
                T.Add(3);
                break;
            case 30:
                N.Add(
                    "Hi"
                );
                T.Add(1);
                break;
            case 31:
                N.Add(
                    "Lo"
                );
                T.Add(1);
                break;
            case 32:
                N.Add(
                    "11HiLo"
                );
                T.Add(7);
                break;
        }

        JObject objData = new JObject();
        objData["numberBet"] = JArray.FromObject(N);
        objData["typeBet"] = JArray.FromObject(T);
        return objData;
    }
    public void onClickShowPlayer()
    {
        //if (listPlayer == null || buttonBet != null)
        listPlayer = Instantiate(prefab_popup_player, transform).GetComponent<NodePlayerSicbo>();
        listPlayer.transform.SetSiblingIndex((int)Globals.GAME_ZORDER.Z_MENU_VIEW);
    }
}
