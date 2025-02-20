using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using DG.Tweening;
using Newtonsoft.Json.Linq;

public class BorkKDengView : GameView
{

    [SerializeField]
    List<Button> listBtnBet = new List<Button>();
    [SerializeField]
    Button btn_draw;
    [SerializeField]
    Button btn_dontDraw;
    [SerializeField]
    Toggle btn_preDraw;
    [SerializeField]
    Toggle btn_preDontDraw;
    [SerializeField]
    List<Card> dealerCards = new List<Card>();

    [SerializeField]
    GameObject dealerBgScore, buttonBetContainer, buttonActionContainer;
    [SerializeField]
    DealerInGameView dealerInGame;

    [SerializeField]
    public Sprite bgScoreCard8, bgScoreCard9, bgScoreCardNormal;
    [SerializeField]
    public List<Sprite> listImgBouns = new List<Sprite>();
    [SerializeField]
    Image dealerBgBonus;
    [SerializeField]
    TextMeshProUGUI lbStateGame, lbScore;

    [SerializeField]
    public SkeletonGraphic animStateRound;

    [HideInInspector]
    List<GameObject> listBoxBet = new List<GameObject>();
    List<ChipBet> listChipbet = new List<ChipBet>();

    Transform playerContainer;
    private string nameDealer = "";
    [HideInInspector]
    Player currentPlayerTurn;
    bool isDealerShowCard = false, isRJ = false;
    private int rateD = 1, dealerScore = 0;
    Vector2 sizeCard = new Vector2(147, 198);
    private List<string> listDataFake = new List<string>();


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Transform boxBetContainer = transform.Find("Table/BoxbetContainer");
        for (int i = 0, l = boxBetContainer.childCount; i < l; i++)
        {
            listBoxBet.Add(boxBetContainer.GetChild(i).gameObject);
        }
        playerContainer = transform.Find("PlayerContainer");
        resetGameView();
    }
    protected override void Start()
    {
        base.Start();

    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        chipPool.ForEach(chip =>
        {
            Destroy(chip.gameObject);
        });
    }

    public void onClickFake(Button btnFake)
    {
        Globals.Config.curGameId = (int)Globals.GAMEID.BORKDENG;
        string strData = listDataFake[0];

        //string strDataNext = listDataFake[1];
        JObject dataFake = JObject.Parse(strData);
        //JObject dataFakeNext = JObject.Parse(strDataNext);
        //btnFake.GetComponentInChildren<TextMeshProUGUI>().text = getString(dataFakeNext, "evt");
        HandleGame.processData(dataFake);
        listDataFake.RemoveAt(0);
    }
    public override void HandlerTip(JObject data)
    {
        playSound(Globals.SOUND_GAME.TIP);
        data["displayName"] = Globals.User.userMain.displayName;
        Player playerTip = getPlayer(getString(data, "N"));

        //require('SoundManager1').instance.dynamicallyPlayMusic(ResDefine.tipAudio);
        //EffectMoneyChange(-data.AGTip, players[i].ag, players[i]._playerView.lbAg);
        int AGTip = getInt(data, "AGTip");
        playerTip.playerView.effectFlyMoney(-AGTip);
        playerTip.ag -= AGTip;
        playerTip.setAg();
        if (playerTip == thisPlayer)
        {
            Globals.User.userMain.AG -= AGTip;
        }
        for (int j = 0; j < 2; j++) // sinh ra 4 chip                     //  lay tien nguoi` thua
        {
            ChipBet temp = getChipBet(transform);
            temp.transform.localPosition = playerTip.playerView.transform.localPosition;
            temp.transform.localScale = new Vector2(0.5f, 0.5f);
            DOTween.Sequence()
                .AppendInterval(j * 0.2f)
                .Append(temp.transform.DOLocalMove(temp.transform.localPosition + new Vector3(0, 80), 0.2f))
                .AppendInterval(0.3f)
                .Append(temp.transform.DOLocalMove(dealerInGame.transform.localPosition, 1.0f).SetEase(Ease.InOutSine))
                .AppendCallback(() =>
                {
                    removerChip(temp);
                });
        }
        DOTween.Sequence()
            .AppendInterval(2.0f)
            .AppendCallback(() =>
            {
                dealerInGame.show(playerTip.namePl, AGTip);
            });

    }
    public void handleStartGame(JObject jData)
    {

    }
    public void handleLc(JObject data)
    {
        playSound(Globals.SOUND_GAME.DISPATCH_CARD);
        for (int i = 0, l = players.Count; i < l; i++)
        {
            List<int> arrIDCard = new List<int> { 0, 0 };
            if (players[i] == thisPlayer)
            {
                ((PlayerViewBorkKDeng)thisPlayer.playerView).rate = getInt(data, "rate");
                ((PlayerViewBorkKDeng)thisPlayer.playerView).score = HamTinhDiem(getInt(data, "score"));
                JArray arr = (JArray)data["arr"];
                arrIDCard = arr.ToObject<List<int>>();
            }
            dispatchCardForPlayer(players[i], arrIDCard);
            buttonBetContainer.SetActive(false);
        }
        dispatchCardForDealer(false, new List<int> { 0, 0 });
        DOTween.Sequence().AppendInterval(1.5f).AppendCallback(() =>
        {
            Globals.Logging.Log("Chia card Xong----> next evt nao");
            HandleGame.nextEvt();
        });
    }
    public override void handleRJTable(string strdata)
    {
        base.handleRJTable(strdata);
        isRJ = true;
        JObject data = JObject.Parse(strdata);
        JArray arrP = (JArray)data["ArrP"];
        nameDealer = (string)data["Dealer"];
        bool isDispatchCard = true;
        for (int i = 0, l = arrP.Count; i < l; i++)
        {
            JObject dataP = (JObject)arrP[i];
            string nameP = (string)dataP["N"];
            if (nameP.Equals(Globals.User.userMain.displayName)) //rj luc chua chia bai.dang bet thi k chia bai ra nua;
            {
                JArray cardOnHandIds = (JArray)dataP["Arr"];
                List<int> arrIds = cardOnHandIds.ToObject<List<int>>();
                if (arrIds[0] == 0)
                {
                    isDispatchCard = false;
                }
            }
        }

        for (int i = 0, l = arrP.Count; i < l; i++)
        {
            JObject dataP = (JObject)arrP[i];
            string nameP = (string)dataP["N"];


            if (!nameP.Equals(nameDealer))
            {
                Player player = getPlayer(nameP);
                PlayerViewBorkKDeng playerV = (PlayerViewBorkKDeng)player.playerView;
                JArray cardOnHandIds = (JArray)dataP["Arr"];
                List<int> arrIds = cardOnHandIds.ToObject<List<int>>();
                if (isDispatchCard)
                {
                    playerV.rate = getInt(dataP, "rate");
                    playerV.score = HamTinhDiem(getInt(dataP, "score"));
                    dispatchCardForPlayer(player, arrIds, false);
                }
                int agBet = (int)dataP["AGC"];
                if (agBet != 0)
                {
                    playerBet(player, agBet, false);
                }


            }

        }
        List<int> ArrDealer = ((JArray)data["ArrDealer"]).ToObject<List<int>>();
        dispatchCardForDealer(false, ArrDealer);

    }
    public override void handleSTable(string strData)
    {
        base.handleSTable(strData);
       
    }
    public void handleStartDealer(JObject data)
    {
        playSound(Globals.SOUND_GAME.START_GAME);
        stateGame = Globals.STATE_GAME.PLAYING;
        buttonBetContainer.SetActive(true);
        lbStateGame.gameObject.SetActive(false);
        lbStateGame.text = "";
        setStateAnimRound("betime_thai");
        float time_turn = (float)data["T"];
        nameDealer = (string)data["Dealer"];
        foreach (Player player in players)
        {
            player.setTurn(true, time_turn / 1000);
            player.setDark(false);
        }
    }
    private void setStateAnimRound(string animName)
    {
        animStateRound.gameObject.SetActive(true);
        animStateRound.Initialize(true);
        animStateRound.AnimationState.SetAnimation(0, animName, true);
        animStateRound.AnimationState.Complete += delegate
        {
            animStateRound.gameObject.SetActive(false);
        };
    }
    public override void handleVTable(string strData)
    {
        base.handleVTable(strData);
        JObject data = JObject.Parse(strData);
        JArray arrP = (JArray)data["ArrP"];
        nameDealer = (string)data["Dealer"];
        for (int i = 0, l = arrP.Count; i < l; i++)
        {
            JObject dataP = (JObject)arrP[i];
            string nameP = (string)dataP["N"];
            if (nameP != nameDealer)
            {
                Player player = getPlayer(nameP);
                PlayerViewBorkKDeng playerV = (PlayerViewBorkKDeng)player.playerView;
                playerV.rate = getInt(dataP, "rate");
                playerV.score = HamTinhDiem(getInt(dataP, "score"));
                JArray cardOnHandIds = (JArray)dataP["Arr"];
                List<int> arrIds = cardOnHandIds.ToObject<List<int>>();
                dispatchCardForPlayer(player, arrIds, false);
                int agBet = (int)dataP["AGC"];
                if (agBet != 0)
                {
                    playerBet(player, agBet, false);
                }
            }
        }
        lbStateGame.gameObject.SetActive(true);
        UIManager.instance.DOTextTmp(lbStateGame, Globals.Config.getTextConfig("txt_view_table"));
        List<int> ArrDealer = ((JArray)data["ArrDealer"]).ToObject<List<int>>();
        dispatchCardForDealer(false, ArrDealer);
    }
    public void handleBm(JObject data)
    {
        playSound(Globals.SOUND_GAME.BET);
        Player player = getPlayer((string)data["N"]);
        if (player != null)
        {
            player.setTurn(false);
            playerBet(player, (int)data["AG"]);
        }

    }
    public void handlePokpok(JObject jData)
    {
        playSound(Globals.SOUND_GAME.CARD_FLIP_1);
        JArray arrData = JArray.Parse((string)jData["data"]);
        for (int i = 0, l = arrData.Count; i < l; i++)
        {
            JObject dataP = (JObject)arrData[i];
            string nameP = (string)dataP["N"];
            JArray arrid = (JArray)dataP["arr"];
            List<int> arrCardId = arrid.ToObject<List<int>>();
            if (nameP != nameDealer)
            {
                PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)getPlayer((string)dataP["N"]).playerView;
                playerView.rate = getInt(dataP, "rate");
                playerView.score = HamTinhDiem(getInt(dataP, "score"));
                playerShowCards(playerView, arrCardId);
                if (thisPlayer == getPlayer((string)dataP["N"]))
                {
                    buttonBetContainer.gameObject.SetActive(false);
                }
            }
            else
            {
                rateD = getInt(dataP, "rate");
                dealerScore = HamTinhDiem(getInt(dataP, "score"));
                dealerShowCard(arrCardId);
            }

        }
    }
    public void handleTimeOut(JObject data)
    {
        if (currentPlayerTurn != null)
        {
            currentPlayerTurn.playerView.setTurn(false);
            if (currentPlayerTurn == thisPlayer)
            {
                buttonActionContainer.gameObject.SetActive(false);
            }
        }
        string nameP = (string)data["NN"];
        Player player = getPlayer(nameP);
        if (player != null)
        {
            currentPlayerTurn = player;
            player.playerView.setTurn(true, (float)data["T"] / 1000);

            PlayerViewBorkKDeng playerV = (PlayerViewBorkKDeng)thisPlayer.playerView;
            if (player != thisPlayer && (playerV.score == 8 || playerV.score == 9))
            {
                buttonActionContainer.SetActive(false);
                return;
            }
            if (player == thisPlayer)
            {

                buttonActionContainer.SetActive(true);
                btn_dontDraw.gameObject.SetActive(true);
                btn_draw.gameObject.SetActive(true);
                btn_preDontDraw.gameObject.SetActive(false);
                btn_preDraw.gameObject.SetActive(false);
                if (btn_preDontDraw.isOn)
                {
                    onClickAction(0);
                }
                else if (btn_preDraw.isOn)
                {
                    onClickAction(1);
                }

            }
            else
            {
                //buttonActionContainer.SetActive(true);
                if (stateGame == Globals.STATE_GAME.PLAYING)
                {
                    btn_dontDraw.gameObject.SetActive(false);
                    btn_draw.gameObject.SetActive(false);
                    btn_preDontDraw.gameObject.SetActive(true);
                    btn_preDraw.gameObject.SetActive(true);
                }

            }
        }
    }
    public void handleBc(JObject data)
    {
        string name = (string)data["N"];
        int cardC = (int)data["C"];
        int score = HamTinhDiem(getInt(data, "score"));
        if (cardC != 0)
        {
            playSound(Globals.SOUND_GAME.CARD_FLIP_1);
        }
        if (name.Equals(nameDealer))
        {
            if (cardC != 0)
            {

                dispatchCardForDealer(true);
            }
        }
        else
        {
            Player player = getPlayer(name);
            if (player != null)
            {
                PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
                playerView.score = score;
                if (player == thisPlayer)
                {
                    playerView.rate = getInt(data, "rate");
                    buttonActionContainer.gameObject.SetActive(false);
                }
                playerView.setTurn(false);
                if (cardC != 0)
                {
                    List<int> arrId = new List<int>();
                    if (player == thisPlayer && cardC != 0)
                    {
                        arrId.Add(cardC);
                    }
                    else if (player != thisPlayer)
                    {
                        arrId.Add(0);
                    }
                    if (arrId.Count != 0)
                    {
                        dispatchCardForPlayer(player, arrId);
                    }
                }
            }
        }

    }
    public void handleFinish(JObject data)
    {
        SocketSend.getFarmInfo();
        setStateAnimRound("open_thai");
        playSound(Globals.SOUND_GAME.ALERT);
        JArray dataFinish = JArray.Parse((string)data["data"]);
        float timeGetChip = 0.0f;
        float timeThrowChip = 0.0f;
        for (int i = 0, l = dataFinish.Count; i < l; i++)
        {
            JObject dataPl = (JObject)dataFinish[i];
            int agChange = (int)dataPl["M"];
            if (agChange < 0)
            {
                timeGetChip = 2.0f;
            }
            if (agChange > 0)
            {
                timeThrowChip = 2.0f;
            }
        }
        DOTween.Sequence()
            .AppendCallback(() => //show card and score
            {
                for (int i = 0, l = dataFinish.Count; i < l; i++)
                {
                    JObject dataPl = (JObject)dataFinish[i];
                    string nameP = (string)dataPl["N"];
                    int idPlayer = getInt(dataPl, "id");
                    Globals.Logging.Log("idPlayer=" + idPlayer);
                    JArray arr = (JArray)dataPl["ArrCard"];
                    int agChange = (int)dataPl["M"];
                    List<int> arrIDCard = arr.ToObject<List<int>>();
                    if (nameP != nameDealer)
                    {
                        try
                        {
                            Player player = getPlayerWithID(idPlayer);

                            PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
                            playerView.rate = getInt(dataPl, "rate");
                            playerView.score = HamTinhDiem(getInt(dataPl, "S"));
                            playerShowCards(playerView, arrIDCard);
                        }
                        catch (System.Exception err)
                        {
                            Globals.Logging.LogError("Chet cmnr");
                            Globals.Logging.LogError(err);
                        }

                    }
                    else
                    {
                        rateD = getInt(dataPl, "rate");
                        dealerScore = HamTinhDiem(getInt(dataPl, "S"));
                        dealerShowCard(arrIDCard);
                    }
                }
            })
            .AppendInterval(2.0f)
            .AppendCallback(() => //resolve chip
            {
                if (timeGetChip > 0)
                {
                    playSound(Globals.SOUND_GAME.GET_CHIP);
                }
                for (int i = 0, l = dataFinish.Count; i < l; i++)
                {
                    JObject dataPl = (JObject)dataFinish[i];
                    string nameP = (string)dataPl["N"];
                    int agChange = (int)dataPl["M"];
                    long agPlayer = (long)dataPl["AG"];
                    int idPlayer = getInt(dataPl, "id");
                    if (nameP != nameDealer)
                    {
                        Player player = getPlayerWithID(idPlayer);
                        if (player != null)
                        {
                            PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
                            if (agChange < 0)
                            {
                                getChipFromPlayer(player);
                                playerView.setEffectLose();
                                if (player == thisPlayer)
                                {
                                    playSound(Globals.SOUND_GAME.LOSE);
                                }
                                player.ag = agPlayer;
                                player.setAg();
                                playerView.effectFlyMoney(agChange, 40);
                            }
                        }

                    }
                }
            })
            .AppendInterval(timeGetChip)
            .AppendCallback(() => //throw chip win
            {
                if (timeGetChip > 0)
                {
                    playSound(Globals.SOUND_GAME.THROW_CHIP);
                }
                for (int i = 0, l = dataFinish.Count; i < l; i++)
                {
                    JObject dataPl = (JObject)dataFinish[i];
                    string nameP = (string)dataPl["N"];
                    int agChange = (int)dataPl["M"];
                    long agPlayer = (long)dataPl["AG"];
                    int idPlayer = getInt(dataPl, "id");
                    if (nameP != nameDealer)
                    {
                        Player player = getPlayerWithID(idPlayer);
                        if (player != null)
                        {
                            PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
                            if (agChange > 0)
                            {
                                throwChipToPlayer(player);
                                player.ag = agPlayer;
                                player.setAg();
                                playerView.effectFlyMoney(agChange, 40);
                                playerView.setEffectWin("win_thai", false);
                                if (player == thisPlayer)
                                {
                                    playSound(Globals.SOUND_GAME.WIN);
                                }
                            }
                            else if (agChange == 0)
                            {
                                player.ag = agPlayer;
                                player.setAg();
                                playerView.setEffectDraw();
                            }
                        }

                    }
                }
            })
            .AppendInterval(timeThrowChip)
            .AppendCallback(() =>
            {
                clearAllCard();
            });

    }
    public void handleUAG(JObject data)
    {

    }
    public void cancelDealer(JObject data)
    {

    }
    public void changeDealer(JObject data)
    {

    }
    private void resetGameView()
    {
        for (int i = 0; i < 3; i++)
        {
            Card cardObj = dealerCards[i];
            cardObj.gameObject.SetActive(false);
            cardObj.gameObject.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
            cardObj.transform.localPosition = new Vector3(0, 0, 0);
            cardObj.setTextureWithCode(0);
        }
        for (int i = 0, l = listBoxBet.Count; i < l; i++)
        {
            listBoxBet[i].gameObject.SetActive(false);
        }
        foreach (ChipBet chip in listChipbet)
        {
            removerChip(chip);
        }
        listChipbet.Clear();
        foreach (Player player in players)
        {
            PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
            playerView.resetUI();
        }
        currentPlayerTurn = null;
        isDealerShowCard = false;
        stateGame = Globals.STATE_GAME.WAITING;
        buttonActionContainer.gameObject.SetActive(true);
        btn_dontDraw.gameObject.SetActive(false);
        btn_draw.gameObject.SetActive(false);
        btn_preDontDraw.gameObject.SetActive(false);
        btn_preDraw.gameObject.SetActive(false);
        btn_preDraw.SetIsOnWithoutNotify(false);
        btn_preDontDraw.SetIsOnWithoutNotify(false);
        checkAutoExit();
        Globals.Logging.Log("finish xong---> next Evt");
        HandleGame.nextEvt();
    }
    private void setupButtonBet()
    {
        List<int> listMultipleBet = new List<int> { 1, 5, 10, 50, 100 };
        for (int i = 0, l = listBtnBet.Count; i < l; i++)
        {
            var aggg = agTable * listMultipleBet[i];
            listBtnBet[i].GetComponentInChildren<TextMeshProUGUI>().text = Globals.Config.FormatMoney2(aggg, true);
            listBtnBet[i].onClick.AddListener(() =>
            {
                onClickSelectBet(long.Parse(aggg + ""));
            });

        }
    }
    private void dispatchCardForPlayer(Player player, List<int> arrCardID, bool isDispatch = true)
    {
        PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
        List<Vector2> posCardLeft = new List<Vector2> { new Vector2(40, 0), new Vector2(65, 0), new Vector2(86, -3.2f) }; //list pos card for player on the left;
        List<Vector2> posCardRight = new List<Vector2> { new Vector2(-86, 0), new Vector2(-65, 0), new Vector2(-40, -3.2f) }; //list pos card for player on the Right;
        List<int> angleCards = new List<int> { 10, -10, -15 };
        Transform playerTrans = playerView.transform;
        int deltaHs = playerTrans.localPosition.x < 0 ? 1 : -1;
        int index = 0;
        for (int i = 0, l = arrCardID.Count; i < l; i++)
        {
            float cardScale = player == thisPlayer ? 0.5f : 0.35f;
            Card card = getCard(transform, cardScale);
            playerView.cards.Add(card);
            card.transform.localPosition = new Vector2(0, 411);
            int indexCard = playerView.cards.Count - 1;
            card.setTextureWithCode(0);
            Vector2 posCardonHand = Vector2.zero;

            if (deltaHs > 0) //player ngoi ben trai
            {
                posCardonHand = playerTrans.localPosition + new Vector3(posCardLeft[1].x, posCardLeft[indexCard].y, 0);
            }
            else //player ngoi ben phai
            {
                posCardonHand = playerTrans.localPosition + new Vector3(posCardRight[1].x, posCardLeft[indexCard].y, 0);
            }
            float timeMove = isDispatch ? 0.75f : 0;
            float timeDelay = isDispatch ? 0.2f : 0;
            Sequence seq = DOTween.Sequence();
            RectTransform rect = card.gameObject.GetComponent<RectTransform>();
            seq.AppendInterval(timeDelay * (playerView.cards.Count - 1));
            seq.Append(rect.DOLocalMove(posCardonHand, timeMove).SetEase(Ease.OutSine));
            //seq.Append(rect.DOLocalMoveY(initPos.y, SPEED));
            seq.AppendCallback(() =>
            {
                card.transform.SetParent(playerView.cardContainer.transform);
                if (index == l - 1)
                {
                    if (player == thisPlayer)
                    {
                        shorCardOnHand(playerView.cards, arrCardID, cardScale, playerView, true);
                    }
                    else
                    {
                        int score = convertScore(arrCardID);
                        bool isShowScore = score != 0 ? true : false;
                        cardScale = score != 0 ? 0.5f : cardScale;
                        shorCardOnHand(playerView.cards, arrCardID, cardScale, playerView, isShowScore);
                    }
                }
                index++;
            });
        }

    }
    private void dispatchCardForDealer(bool isBc, List<int> arrId = null)
    {
        if (isBc)
        {
            Card cardObj = dealerCards[2];
            cardObj.gameObject.SetActive(true);
            cardObj.transform.localScale = new Vector2(0.4f, 0.4f);
            cardObj.setTextureWithCode(0);
            shorCardOnHand(dealerCards, new List<int> { 0, 0, 0 }, 0.4f);
        }
        else
        {
            List<Card> arrCardDealer = new List<Card>();
            for (int i = 0; i < arrId.Count; i++)
            {
                Card cardObj = dealerCards[i];
                cardObj.gameObject.SetActive(true);
                cardObj.transform.localScale = new Vector2(0.4f, 0.4f);
                cardObj.setTextureWithCode(0);
                arrCardDealer.Add(cardObj);
            }
            shorCardOnHand(arrCardDealer, arrId, 0.4f);
        }
    }

    private void shorCardOnHand(List<Card> cards, List<int> arrId, float cardScale = 0.35f, PlayerViewBorkKDeng player = null, bool isShowScore = false)
    {
        int sizeCards = cards.Count;
        int index = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            Vector3 angle = Vector3.zero;
            Vector2 posision = Vector2.zero;
            if (i == 0)
            {
                angle = new Vector3(0, 0, 10);
                posision = new Vector2(cards[1].transform.localPosition.x - (sizeCard.x / 2 * cardScale), cards[i].transform.localPosition.y);

            }
            else if (i == 1)
            {
                if (sizeCards == 2)
                {
                    angle = new Vector3(0, 0, -10);
                    posision = new Vector2(cards[i].transform.localPosition.x, cards[0].transform.localPosition.y);
                }
                else
                {
                    angle = new Vector3(0, 0, 0);
                    posision = new Vector2(cards[i].transform.localPosition.x, cards[0].transform.localPosition.y + 2);
                }
            }
            else if (i == 2)
            {
                angle = new Vector3(0, 0, -10);
                posision = new Vector2(cards[1].transform.localPosition.x + (sizeCard.x / 2 * cardScale), cards[0].transform.localPosition.y);
            }
            cards[i].transform.DOLocalRotate(angle, 0.3f, RotateMode.Fast);
            cards[i].transform.DOLocalMove(posision, 0.3f);
            cards[i].transform.DOScale(new Vector2(cardScale, cardScale), 0.3f);
            bool isBocCard = arrId.Count > 1 ? false : true;
            if (player == null)
            {
                Globals.Logging.Log("shorCardOnHand dealer=:" + arrId[i]);
            }
            try
            {
                DOTween.Sequence()
                          .AppendInterval(0.35f)
                          .AppendCallback(() =>
                          {
                              if (!isBocCard)
                              {
                                  if (isShowScore)
                                  {
                                      cards[index].setTextureWithCode(arrId[index]);

                                      if (index == cards.Count - 1)
                                      {
                                          if (player != null)
                                          {
                                              player.showScoreCard();
                                          }
                                          else
                                          {
                                              dealerShowScore();
                                          }
                                      }
                                  }

                              }
                              else //case boc card
                              {
                                  if (player != null && isShowScore)
                                  {
                                      cards[2].setTextureWithCode(arrId[0]);
                                      player.showScoreCard();
                                  }
                                  else
                                  {
                                      cards[2].setTextureWithCode(0);
                                  }
                              }
                              //if (player == null)
                              //Globals.Logging.Log("index =" + index);
                              isRJ = false;
                              index++;

                          });

            }
            catch (System.Exception e)
            {
                Globals.Logging.LogException(e);
            }

        }
    }
    public override void setGameInfo(int m, int id = 0, int maxBet = 0)
    {
        base.setGameInfo(m, id, maxBet);
        setupButtonBet();
    }
    private int convertScore(List<int> arrIds)
    {
        int cardScore = 0;
        arrIds.ForEach(number =>
        {
            int cardN = ((number - 1) % 13) + 1; //cong thuc cua BorkKdeng
            if (cardN > 10)
            {
                cardN = 0;
            }
            cardScore += cardN;
        });
        cardScore = cardScore % 10;
        return cardScore;
    }
    public int HamTinhDiem(int diem)
    {
        if (diem >= 1000 && diem < 2000) diem = 11;
        else if (diem >= 2000 && diem < 3000) diem = 12;
        else if (diem >= 3000 && diem < 4000) diem = 13;
        else if (diem >= 4000 && diem < 5000) diem = 14;
        else if (diem >= 5000)
        {
            diem = diem % 5000;
            diem = diem % 10;
        }
        return diem;
    }
    private void playerShowCards(PlayerViewBorkKDeng player, List<int> arrId)
    {
        Globals.Logging.Log("playerShowCard:" + player.isShowCard);
        if (!player.isShowCard)
        {
            player.isShowCard = true;
            List<Card> cards = player.cards;
            for (int i = 0, l = arrId.Count; i < l; i++)
            {
                cards[i].transform.localPosition = cards[1].transform.localPosition;
                cards[i].transform.localRotation = Quaternion.identity;
            }
            shorCardOnHand(player.cards, arrId, 0.45f, player, true);
        }
    }
    private void dealerShowCard(List<int> arrId)
    {
        Globals.Logging.Log("dealerShowCard=" + isDealerShowCard);
        if (!isDealerShowCard)
        {
            isDealerShowCard = true;
            List<Card> listCard = new List<Card>();
            for (int i = 0; i < arrId.Count; i++)
            {
                Card card = dealerCards[i];
                card.transform.localPosition = dealerCards[1].transform.localPosition;
                card.transform.localRotation = Quaternion.Euler(0, 0, 0);
                card.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                listCard.Add(card);
            }
            shorCardOnHand(listCard, arrId, 0.45f, null, true);
        }
    }
    private void dealerShowScore()
    {
        //TextMeshProUGUI lbScore = dealerBgScore.GetComponentInChildren<TextMeshProUGUI>();
        int arrNum = 0;
        //dealerCards.ForEach(card =>
        //{
        //    if (card.gameObject.activeSelf == true)
        //    {
        //        arrNum.Add(card.N);
        //    }
        //});
        arrNum = dealerCards.FindAll(card => card.gameObject.activeSelf == true).Count;
        RectTransform rtDealerBgScore = dealerBgScore.GetComponent<RectTransform>();
        try
        {
            if (arrNum == 2 && dealerScore >= 8)
            {
                lbScore.gameObject.SetActive(false);
                if (dealerScore == 8)
                {
                    dealerBgScore.GetComponent<Image>().sprite = bgScoreCard8;
                }
                else
                {
                    dealerBgScore.GetComponent<Image>().sprite = bgScoreCard9;
                }
                //dealerBgScore.GetComponent<Image>().SetNativeSize();
                rtDealerBgScore.sizeDelta = new Vector2(193, 73);
            }
            else
            {
                lbScore.text = getTextScore(dealerScore, arrNum);
                lbScore.gameObject.SetActive(true);
                dealerBgScore.GetComponent<Image>().sprite = bgScoreCardNormal;
                rtDealerBgScore.sizeDelta = new Vector2(210, 74);

            }
        }
        catch (System.Exception e)
        {
            Globals.Logging.LogException(e);
        }

        dealerBgScore.SetActive(true);
        float posX = 0;
        float firstPosX = dealerCards[2].transform.localPosition.x;
        if (!dealerCards[2].gameObject.activeSelf)
        {
            firstPosX = dealerCards[1].transform.localPosition.x;
        }
        float lastPosX = dealerCards[0].transform.localPosition.x;
        posX = (lastPosX - firstPosX) / 2;
        dealerBgScore.transform.localPosition = new Vector2(firstPosX + posX, dealerCards[1].transform.localPosition.y - 35);
        dealerBgBonus.transform.localPosition = arrNum == 2 ? new Vector2(61, 120) : new Vector2(71, 120);
        if (rateD > 1)
        {
            dealerBgBonus.gameObject.SetActive(true);
            if (rateD == 2)
            {
                dealerBgBonus.sprite = listImgBouns[0];
            }
            else if (rateD == 3)
            {
                dealerBgBonus.sprite = listImgBouns[1];
            }
            else
            {
                dealerBgBonus.sprite = listImgBouns[2];
            }
        }
        else
        {
            dealerBgBonus.gameObject.SetActive(false);
        }
    }
    public string getTextScore(int score, int numCard)
    {
        string txtScore = "";
        if (numCard == 2)
        {
            if (score > 7)
            {
                //bgScore.gameObject.SetActive(true);
                //lbScore.gameObject.SetActive(false);
                //if (score == 8)
                //{
                //    bgScore.GetComponent<Image>().sprite = bgScore8;
                //}
                //else
                //{
                //    bgScore.GetComponent<Image>().sprite = bgScore9;
                //}
                //bgScore.GetComponent<Image>().SetNativeSize();
            }
            else
            {
                txtScore = score + " " + Globals.Config.getTextConfig("diem");
            }
        }
        else if (numCard == 3)
        {
            // 11: "Face Cards" 12: "Straight" 13: "Straight flush" 14: "Three of a kind" 
            txtScore = score + " " + Globals.Config.getTextConfig("diem");
            if (score == 11) txtScore = Globals.Config.getTextConfig("txt_pok_3daunguoi");
            if (score == 12) txtScore = Globals.Config.getTextConfig("txt_pok_sanh");
            if (score == 13) txtScore = Globals.Config.getTextConfig("txt_pok_tpsanh");
            if (score == 14) txtScore = Globals.Config.getTextConfig("txt_pok_xam");
        }
        Globals.Logging.Log("Get text Score:" + txtScore + "====socre==" + score);
        return txtScore;
    }
    public void onClickSelectBet(long data)
    {
        if (thisPlayer != null && data > thisPlayer.ag)
        {
            data = thisPlayer.ag;
        }
        SocketSend.sendRaise(data);
        buttonBetContainer.SetActive(false);
        SoundManager.instance.soundClick();
    }
    public void onClickMaxBet()
    {
        SocketSend.sendRaise(thisPlayer.ag);
        SoundManager.instance.soundClick();
        buttonBetContainer.SetActive(false);
    }
    public void onClickAction(int type)
    {
        SoundManager.instance.soundClick();
        buttonActionContainer.SetActive(false);
        SocketSend.sendActionTurn(type);
    }
    public void onClickPreAction(Toggle tgl)
    {
        if (tgl == btn_preDontDraw)
        {
            btn_preDraw.SetIsOnWithoutNotify(false);
        }
        else
        {
            btn_preDontDraw.SetIsOnWithoutNotify(false);
        }
    }
    private void throwChipToPlayer(Player player)
    {
        int indexPlayer = getDynamicIndex(getIndexOf(player));
        GameObject boxBetPlayer = listBoxBet[indexPlayer];

        Vector2 posBoxBet = playerContainer.InverseTransformPoint(boxBetPlayer.transform.position) + new Vector3(-65, 0, 0);
        for (int i = 0; i < 5; i++)
        {
            ChipBet chip = getChipBet(playerContainer);
            listChipbet.Add(chip);
            chip.transform.localPosition = dealerBgScore.transform.parent.localPosition;
            chip.init(0, 0.35f);
            chip.move(posBoxBet + new Vector2(0, 2 * i), i * 0.1f);
        }
    }
    private void getChipFromPlayer(Player player)
    {
        int indexPlayer = getDynamicIndex(getIndexOf(player));
        PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
        GameObject boxBetPlayer = listBoxBet[indexPlayer];
        boxBetPlayer.SetActive(false);
        List<ChipBet> listChipPlayer = playerView.listChip;
        for (int i = 0, l = listChipPlayer.Count; i < l; i++)
        {
            ChipBet chip = listChipPlayer[i];
            System.Action cbRemoveChip = () =>
            {
                removerChip(chip);
                listChipbet.Remove(chip);
            };
            chip.move(dealerBgScore.transform.parent.localPosition, i * 0.1f, cbRemoveChip);
        }
    }

    private void playerBet(Player player, int valueBet, bool isEffect = true)
    {

        int indexPlayer = getDynamicIndex(getIndexOf(player));
        PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)player.playerView;
        GameObject boxBetPlayer = listBoxBet[indexPlayer];
        boxBetPlayer.GetComponentInChildren<TextMeshProUGUI>().text = Globals.Config.FormatMoney2(valueBet,true);
        Vector2 posBoxBet = playerContainer.InverseTransformPoint(boxBetPlayer.transform.position) + new Vector3(-37, 0, 0);
        for (int i = 0; i < 5; i++)
        {
            ChipBet chip = getChipBet(playerContainer);
            chip.transform.localPosition = playerView.transform.localPosition;
            chip.init(0, 0.35f);
            chip.move(posBoxBet + new Vector2(0, 2 * i), i * 0.1f, null, isEffect);
            playerView.listChip.Add(chip);
            listChipbet.Add(chip);
        }
        player.ag -= valueBet;
        player.setAg();
        //if (stateGame == Globals.STATE_GAME.PLAYING && !isRJ)
        //{
        //    playerView.effectFlyMoney(-valueBet);
        //}
        boxBetPlayer.SetActive(true);
    }
    private void clearAllCard()
    {
        playSound(Globals.SOUND_GAME.DISPATCH_CARD);
        for (int i = 0, l = players.Count; i < l; i++)
        {
            PlayerViewBorkKDeng playerView = (PlayerViewBorkKDeng)players[i].playerView;
            playerView.bgScore.SetActive(false);
            for (int j = 0; j < playerView.cards.Count; j++)
            {
                Card card = playerView.cards[j];
                card.transform.SetParent(transform);
                card.setTextureWithCode(0);
                int indexCard = playerView.cards.IndexOf(card);

                //DOTween.Sequence()
                //    .AppendInterval(indexCard * 0.1f)
                //    .Append(card.transform.DOLocalMove(new Vector2(0, 430), 0.5f)
                //        .SetEase(Ease.OutSine))
                //        .Join(card.transform.DOScale(new Vector2(0.25f, 0.25f), 0.5f))
                //    .AppendCallback(() =>
                //    {
                //        removerCard(card);
                //    });
                moveCardsFinish(card.gameObject, indexCard * 0.1f, 0);
            }
        }
        int index = 0;
        dealerBgScore.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            dealerCards[i].setTextureWithCode(0);
            DOTween.Sequence()
                .Append(dealerCards[i].transform.DOLocalMove(dealerCards[1].transform.localPosition, 0.5f)).Join(dealerCards[i].transform.DOScale(new Vector2(0.25f, 0.25f), 0.5f))
                .AppendCallback(() =>
                {
                    dealerCards[index].gameObject.SetActive(false);
                    index++;
                });
        }
        DOTween.Sequence()
            .AppendInterval(1.0f)
            .AppendCallback(() =>
            {
                resetGameView();
            });

    }
    protected override int getDynamicIndex(int index)
    {
        if (index == 0) return 0;
        var _index = index;

        if (players.Count < 5 && players.Count > 4)
        {
            if (index < 3)
            {
                _index += 1;
            }
            else
            {
                _index += 2;
            }

        }
        else if (players.Count <= 4)
        {
            if (index == 1)
            {
                _index += 1;
            }
            else
            {
                _index += 3;
            }
        }
        else if (players.Count == 2)
        {
            _index++;
            return _index;
        }

        return _index;
    }
    private void moveCardsFinish(GameObject card, float delay, int zView)
    {
        DOTween.Sequence()
            .AppendInterval(delay)
            .AppendCallback(() =>
            {
                Vector2 posRemove = new Vector2(0, 430);
                card.GetComponent<Card>().setDark(false);
                card.transform.DOLocalMove(posRemove, 0.4f).SetEase(Ease.OutCubic);
                card.transform.SetSiblingIndex((int)Globals.GAME_ZORDER.Z_CARD + zView);
                card.transform.DOScale(new Vector2(0.04f, 0.4f), 0.15f).OnComplete(() =>
                {
                    card.transform.DOScale(new Vector2(0.4f, 0.4f), 0.15f);
                });

                DOTween.Sequence()
                .Append(card.transform.DOLocalRotate(new Vector3(0, -15f, 0), 0.15f))
                .AppendCallback(() =>
                {
                    card.transform.localRotation = Quaternion.Euler(0, -15, 0);
                })
                .Append(card.transform.DOLocalRotate(Vector3.zero, 0.15f));

                DOTween.Sequence()
                .AppendInterval(0.15f)
                .AppendCallback(() =>
                {
                    card.GetComponent<Card>().setTextureWithCode(0);
                })
                .AppendInterval(0.45f)
                .AppendCallback(() =>
                {
                    removerCard(card.GetComponent<Card>());
                });
            });

    }
}
