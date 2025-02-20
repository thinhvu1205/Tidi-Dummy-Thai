using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Newtonsoft.Json.Linq;
using DG.Tweening;
using Spine.Unity;

public class SlotTarzanView : BaseSlotGameView
{
    public static SlotTarzanView instance;

    [SerializeField]
    protected TextMeshProUGUI lbCurrentChipBonus;
    [SerializeField]
    protected TextMeshProUGUI lbChipPopupReward;
    [SerializeField]
    protected Image sprProgressChipBonus;
    //[SerializeField]
    //protected SkeletonGraphic animBuiSang;

    [SerializeField]
    protected SkeletonGraphic animPopupResult;

    [SerializeField]
    protected SkeletonGraphic animPopupMiniGame;

    [SerializeField]
    protected SkeletonGraphic animPopupResultMinigame;

    [SerializeField]
    protected SkeletonGraphic animPopupGetFreeSpin;

    [SerializeField]
    protected SkeletonGraphic animPopupResultFreeSpin;

    [SerializeField]
    protected SkeletonGraphic animTarzan;

    [SerializeField]
    protected SkeletonGraphic animCharacter;

    [SerializeField]
    protected SkeletonGraphic animLightBar;


    [SerializeField]
    protected List<Image> listCharacter = new List<Image>();
    [SerializeField]
    protected List<Sprite> sprCharacterOn = new List<Sprite>();
    [SerializeField]
    protected List<Sprite> sprCharacterOff = new List<Sprite>();
    [SerializeField]
    public SkeletonDataAsset animDiamond;
    [SerializeField]
    public GameObject diamondPot;

    [SerializeField]
    public TextMeshProUGUI lbDiamondNum;

    [SerializeField]
    public SlotTarzanMiniGameView miniGameView;

    public List<Vector2> listDiamondPos = new List<Vector2>();
    private int currentChipBonus = 0;
    private List<JObject> listPosCharJungle = new List<JObject>();
    public bool isPlayingMiniGame = false;
    private int totalFreeSpinGet = 0;
    private List<int> arrayBonus = new List<int>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        BIGWIN_ANIMPATH = "GameView/SlotSpine/Tarzan/BigWin/skeleton_SkeletonData";
        MEGAWIN_ANIMPATH = "GameView/SlotSpine/Tarzan/BigWin/skeleton_SkeletonData";
        FREESPIN_ANIMPATH = "GameView/SlotSpine/Tarzan/PopupFreespin/skeleton_SkeletonData";
        ANIM_BIGWIN_NAME = "big2";
        ANIM_MEGAWIN_NAME = "mega2";
        ANIM_HUGEWIN_NAME = "huge2";
        TYPE_BIGWIN = 1;
        TYPE_HUGEWIN = 3;
        TYPE_MEGA = 2;
        indexCheck3rdScatter = 4;
    }

    public override void handleCTable(string data)
    {
        Globals.Logging.Log("HandleCtable Tarzan:" + data);
        //data = "{\"N\":\"Poker[4245]\",\"M\":10,\"ArrP\":[{\"id\":5363,\"N\":\"nanaboy12\",\"Url\":\"\",\"AG\":2621892242,\"LQ\":21,\"VIP\":2,\"isStart\":true,\"IK\":0,\"sIP\":\"10.148.0.4\",\"Av\":3,\"FId\":0,\"GId\":0,\"UserType\":1,\"TotalAG\":0,\"timeToStart\":0,\"currentExp\":0,\"currentLevel\":0,\"expNextLevel\":0,\"CO\":0.0,\"CO0\":0.0}],\"Id\":4245,\"V\":0,\"S\":1,\"issd\":true,\"views\":[[{\"id\":4,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":14,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":8,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false}],[{\"id\":2,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":1,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":0,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false}],[{\"id\":2,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":0,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":14,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false}],[{\"id\":10,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":0,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":5,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false}],[{\"id\":10,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":1,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false},{\"id\":10,\"value\":0,\"symbolAdd\":-1,\"wildFake\":false}]],\"payLine\":[[1,1,1,1,1],[0,0,0,0,0],[2,2,2,2,2],[0,1,2,1,0],[2,1,0,1,2],[1,0,0,0,1],[1,2,2,2,1],[0,0,1,2,2],[2,2,1,0,0],[1,2,1,0,1],[1,0,1,2,1],[0,1,1,1,0],[2,1,1,1,2],[0,1,0,1,0],[2,1,2,1,2],[1,1,0,1,1],[1,1,2,1,1],[0,0,2,0,0],[2,2,0,2,2],[0,2,2,2,0],[2,0,0,0,2],[1,2,0,2,1],[1,0,2,0,1],[0,2,0,2,0],[2,0,2,0,2],[2,0,1,2,0],[0,2,1,0,2],[0,2,1,2,0],[2,0,1,0,2],[2,1,0,0,1],[0,1,2,2,1],[0,0,2,2,2],[2,2,0,0,0],[1,0,2,1,2],[1,2,0,1,0],[0,1,0,1,2],[2,1,2,1,0],[1,2,2,0,0],[0,0,1,1,2],[2,2,1,1,0],[2,0,0,0,0],[0,2,2,2,2],[2,2,2,2,0],[0,0,0,0,2],[1,0,1,0,1],[1,2,1,2,1],[0,1,2,2,2],[2,1,0,0,0],[0,1,1,1,1],[2,1,1,1,1],[0,0,0,0,1],[0,0,0,1,1],[0,0,0,1,2],[0,0,1,0,0],[0,0,1,1,0],[0,0,1,1,1],[0,0,1,2,1],[0,1,0,0,0],[0,1,0,1,1],[2,0,1,0,1],[0,2,1,2,1],[0,1,0,2,0],[2,1,2,0,2],[0,1,1,1,2],[2,1,1,1,0],[0,1,1,2,2],[1,0,0,0,0],[1,0,0,1,1],[1,0,0,1,2],[0,1,1,0,1],[1,0,1,1,2],[1,0,1,2,2],[1,1,0,0,0],[1,1,0,0,1],[1,1,0,1,2],[0,0,2,1,0],[1,1,1,0,0],[1,1,2,2,2],[2,2,2,2,1],[1,1,1,1,2],[0,1,0,2,1],[1,2,2,2,2],[1,2,1,0,0],[2,2,1,0,1],[2,2,1,1,1],[2,1,2,1,1],[0,0,0,2,0],[2,2,2,0,2],[2,0,2,2,2],[0,2,0,0,0],[2,2,0,1,2],[1,0,1,1,1],[1,2,1,1,1],[1,1,2,1,0],[2,1,2,2,2],[0,2,1,1,1],[2,0,1,1,1],[1,1,1,2,0],[1,1,1,0,2],[1,1,1,1,0]],\"payTable\":[[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,2,2,2,10,2,0,0],[5,5,5,10,10,10,15,15,25,25,50,250,5,0,0],[25,25,25,50,50,50,75,75,125,125,250,2500,0,0,0],[125,125,125,250,250,250,500,500,750,750,1250,10000,0,0,0]],\"freeSpinCount\":0,\"level\":{\"levelUser\":0,\"curLevelExp\":0,\"maxLevelExp\":0,\"agUser\":19672},\"MarkBet\":[1,5,50,500,1000,5000,10000,25000,50000,100000,250000,500000],\"singleLineBet\":0,\"isBonusGame\":false,\"isFreeGame\":false,\"maxExp\":100,\"numExp\":27,\"arrayBonus\":[1,1,1,1,1,0],\"multiFreeGame\":1,\"currentChipBonus\":456950000}";
        //base.handleCTable(data);

        dataCtable = JObject.Parse(data);
        payLines = (JArray)dataCtable["payLine"];
        for (int i = 1; i < 5; i++)
        {
            CollumSpinController col = Instantiate(listCollum[0], collumContainer.transform).GetComponent<CollumSpinController>();
            col.transform.name = "Collum" + (i + 1);
            if (i == 4)
            {
                col.isLastCollum = true;
            }
            listCollum.Add(col);
        }
        JArray views = getJArray(dataCtable, "views").ToObject<JArray>();
        for (int i = 0; i < views.Count; i++)
        {
            var arr = (JArray)views[i];
            for (int j = 0; j < arr.Count; j++)
            {
                if ((int)views[i][j]["id"] == -1)
                {
                    views[i][j]["id"] = Random.Range(0, 12);
                }
                if (!(bool)dataCtable["isBonusGame"] && !(bool)dataCtable["isFreeGame"])
                {
                    int idRan = Random.Range(0, 13);
                    views[i][j]["id"] = idRan;
                }

            }
        }
        dataCtable["views"] = views;
        //getJArray(dataCtable, "views") = views;
        setStartView((JArray)dataCtable["views"]);
        JArray arrP = (JArray)dataCtable["ArrP"];
        JObject dataPlayer = (JObject)arrP[0];
        agPlayer = (long)dataPlayer["AG"];
        lbCurrentChips.text = Globals.Config.FormatNumber(agPlayer);
        listMarkbet = ((JArray)dataCtable["MarkBet"]).ToObject<List<int>>();
        listBetRoom = getListBetRoom();
        freespinLeft = getInt(dataCtable, "freeSpinCount");
        totalFreeSpinGet = getInt(dataCtable, "freeSpinCount");
        isFreeSpin = freespinLeft > 0;
        int singleLineBet = getInt(dataCtable, "singleLineBet");
        currentChipBonus = getInt(dataCtable, "currentChipBonus");
        lbCurrentChipBonus.text = Globals.Config.FormatNumber(currentChipBonus);
        lbDiamondNum.text = getInt(dataCtable, "numExp").ToString();
        float progressChipBonus = (float)getInt(dataCtable, "numExp") / getInt(dataCtable, "maxExp");
        if (progressChipBonus != 0)
        {
            //animBuiSang.gameObject.SetActive(true);
        }
        sprProgressChipBonus.DOFillAmount(progressChipBonus, 0.5f).SetEase(Ease.InSine).OnUpdate(() =>
        {
            float posLight = -163 + (396 * sprProgressChipBonus.fillAmount);
            //animLightBar.transform.DOLocalMoveX(posLight, 0.5f).SetEase(Ease.InSine);
            animLightBar.transform.localPosition = new Vector2(posLight, 3.8f);
        });

        updateCurrentMarkBet();
        if (isFreeSpin)
        {
            currentMarkBet = listMarkbet.IndexOf(singleLineBet);
            lbFreespinLeft.gameObject.SetActive(true);
            lbFreespinLeft.text = freespinLeft + "";
        }
        Globals.Logging.Log("listBetRoom,count=" + listBetRoom.Count);
        if (listBetRoom.Count > 0)
        {
            if (listBetRoom.Count - 1 >= currentMarkBet)
            {
                lbCurrentBet.text = Globals.Config.FormatMoney2(listBetRoom[currentMarkBet], true);
                lbStateBet.text = currentMarkBet == listBetRoom.Count - 1 ? Globals.Config.getTextConfig("txt_max_bet") : Globals.Config.getTextConfig("txt_bet");
            }
            else
            {
               
                lbCurrentBet.text = Globals.Config.FormatMoney2(totalListBetRoom[currentMarkBet]);
            }
          
        }
        else
        {
            if (!isFreeSpin)
            {
                lbInfoSession.text = Globals.Config.getTextConfig("msg_warrning_send");
                lbCurrentBet.text = Globals.Config.FormatMoney2(totalListBetRoom[currentMarkBet],true);
            }
            else
            {
                lbCurrentBet.text = Globals.Config.FormatMoney2(totalListBetRoom[currentMarkBet], Globals.Config.curGameId == (int)Globals.GAMEID.SLOTTARZAN);
            }
        }

        arrayBonus = getJArray(dataCtable, "arrayBonus").ToObject<List<int>>();
        //arrayBonus = new List<int> { 1, 1, 1, 1, 1, 0 };

        for (int i = 0; i < arrayBonus.Count; i++)
        {
            listCharacter[i].sprite = arrayBonus[i] != 0 ? sprCharacterOn[i] : sprCharacterOff[i];
        }
        //showEffectCharacter();
        setSpinType();
        setStateBtnSpin();
    }
    protected override void setSpinType()
    {
        if (freespinLeft > 0)
        {
            Globals.Logging.Log("isGetFreeSpin==" + isGetFreeSpin);
            Globals.Logging.Log("spintype==" + spintype);
            if (!isGetFreeSpin)
            {
                if (spintype == SPIN_TYPE.AUTO)
                {
                    spintype = SPIN_TYPE.FREE_AUTO;
                }
                else if (spintype == SPIN_TYPE.NORMAL)
                {
                    spintype = SPIN_TYPE.FREE_NORMAL;
                }
            }
            else
            {
                if (spintype == SPIN_TYPE.AUTO)
                {
                    spintype = SPIN_TYPE.FREE_AUTO;
                }
                else if (spintype == SPIN_TYPE.NORMAL)
                {
                    spintype = SPIN_TYPE.FREE_NORMAL;
                }
            }

        }
        else
        {
            if (spintype == SPIN_TYPE.FREE_AUTO)
            {
                spintype = SPIN_TYPE.AUTO;
            }
            else if (spintype == SPIN_TYPE.FREE_NORMAL)
            {
                spintype = SPIN_TYPE.NORMAL;
            }
        }
    }
    private void showEffectCharacter()
    {
        List<int> listIndex = new List<int>();
        for (int i = 0; i < arrayBonus.Count; i++)
        {
            if (arrayBonus[i] != 0)
            {
                listIndex.Add(i);
            }
        }

        List<string> listChar = new List<string> { "J", "U", "N", "G", "L", "E" };
        int index = 0;
        for (int i = 0; i < listIndex.Count; i++)
        {
            DOTween.Sequence()
                .AppendInterval(i * 2.0f)
                .AppendCallback(() =>
                {
                    Globals.Logging.Log("Show Anim Character");
                    animCharacter.gameObject.SetActive(true);
                    string CHARACTER_ANIMPATH = "GameView/SlotSpine/Tarzan/JungleCharacter/" + listChar[listIndex[index]] + "/skeleton_SkeletonData";
                    animCharacter.skeletonDataAsset = UIManager.instance.loadSkeletonData(CHARACTER_ANIMPATH);
                    animCharacter.Initialize(true);
                    animCharacter.AnimationState.SetAnimation(0, "animation", false);
                    animCharacter.transform.localPosition = listCharacter[listIndex[index]].transform.localPosition;
                    index++;
                    if (index == listIndex.Count)
                    {
                        index = 0;
                    }
                }).SetLoops(9999);
        }


    }
    protected override void setStartView(JArray dataStartView)
    {
        for (int i = 0, size = dataStartView.Count; i < size; i++)
        {
            List<int> viewCollum = new List<int>();//= dataStartView[i].ToObject<List<int>>();
            foreach (JObject data in dataStartView[i])
            {
                viewCollum.Add((int)data["id"]);
            }
            listCollum[i].setStartView(viewCollum, this);
        }
    }
    public void onClickClosePopupGem()
    {
        animPopupResult.transform.DOLocalMoveY(Screen.currentResolution.height, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            animPopupResult.gameObject.SetActive(false);
            effectContainer.SetActive(false);
            handleActionResult();
        });
    }
    public void onClickClosePopupMiniGame()
    {
        animPopupMiniGame.transform.DOLocalMoveY(Screen.currentResolution.height, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            animPopupMiniGame.gameObject.SetActive(false);
            effectContainer.SetActive(false);
            handleActionResult();
        });
    }
    protected override void resetSlotView()
    {
        if (arrayBonus.FindAll(character => character == 1).Count == 6)
        {
            listCharacter.ForEach(character =>
            {
                character.sprite = sprCharacterOff[listCharacter.IndexOf(character)];
            });
        }
        base.resetSlotView();
    }
    public void closeMiniGameView()
    {
        animPopupResultMinigame.gameObject.SetActive(false);
        showAnimChipBay();
        agPlayer += miniGameView.totalWin;
        listBetRoom = getListBetRoom();
        setCurrentMarkBet();
        Globals.Config.tweenNumberToNumber(lbCurrentChips, agPlayer, agPlayer - System.Convert.ToInt64(miniGameView.totalWin));
        miniGameView.resetUI();
        DOTween.Sequence()
            .AppendInterval(0.3f)
            .AppendCallback(() =>
            {
                handleActionResult();
                effectContainer.SetActive(false);

            });


    }
    private void showPopupResultGem()
    {
        Globals.Logging.Log("showPopupResultGem");
        string animPath = "GameView/SlotSpine/Tarzan/PopupFreeSpin/skeleton_SkeletonData";

        animPopupResult.gameObject.SetActive(true);
        animPopupResult.skeletonDataAsset = UIManager.instance.loadSkeletonData(animPath);
        animPopupResult.TrimRenderers();
        animPopupResult.transform.localScale = new Vector2(0.3f, 0.3f);
        effectContainer.SetActive(true);
        animPopupResult.Initialize(true);
        animPopupResult.AnimationState.SetAnimation(0, "getgem2", true);
        animPopupResult.transform.DOScale(new Vector2(0.5f, 0.5f), 0.3f).SetEase(Ease.OutBack);
        Globals.Config.tweenNumberTo(lbChipPopupReward, getInt(finishData, "chipBonusFullExp"), 0, 1.0f, true);
        if (spintype == SPIN_TYPE.AUTO)
        {
            DOTween.Sequence()
                .AppendInterval(10.0f)
                .AppendCallback(() =>
                {
                    if (animPopupResult.gameObject.activeSelf)
                    {
                        onClickClosePopupGem();

                    }
                });
        }
    }
    public void showPopupMiniGame()
    {
        string animPath = "GameView/SlotSpine/Tarzan/Minigame/PopupCongrat/skeleton_SkeletonData";
        animPopupMiniGame.gameObject.SetActive(true);
        animPopupMiniGame.skeletonDataAsset = UIManager.instance.loadSkeletonData(animPath);
        animPopupMiniGame.TrimRenderers();
        animPopupMiniGame.transform.localScale = new Vector2(0.25f, 0.25f);
        animPopupMiniGame.transform.localPosition = Vector2.zero;
        effectContainer.SetActive(true);
        animPopupMiniGame.Initialize(true);
        animPopupMiniGame.AnimationState.SetAnimation(0, "Thai", true);
        animPopupMiniGame.transform.DOScale(new Vector2(0.5f, 0.5f), 0.3f).SetEase(Ease.OutBack);
    }
    public void showPopupResultMinigame(int value)
    {
        string animPath = "GameView/SlotSpine/Tarzan/Minigame/Result/skeleton_SkeletonData";
        animPopupResultMinigame.gameObject.SetActive(true);
        animPopupResultMinigame.skeletonDataAsset = UIManager.instance.loadSkeletonData(animPath);
        animPopupResultMinigame.TrimRenderers();
        animPopupResultMinigame.transform.localScale = new Vector2(0.5f, 0.5f);
        effectContainer.SetActive(true);
        animPopupResultMinigame.Initialize(true);
        animPopupResultMinigame.transform.Find("lbReward").GetComponent<TextMeshProUGUI>().text = Globals.Config.FormatMoney3(value, 10000);
        animPopupResultMinigame.AnimationState.SetAnimation(0, "THAI", true);
        animPopupResultMinigame.transform.DOScale(new Vector2(1.0f, 1.0f), 0.3f).SetEase(Ease.OutBack);

        agPlayer += value;
        //lbCurrentChips.text = Globals.Config.FormatNumber(agPlayer);
        Globals.Config.tweenNumberToNumber(lbCurrentChips, agPlayer - System.Convert.ToInt64(value), agPlayer);
    }
    protected override void showFreeSpin()
    {
        playSound(Globals.SOUND_SLOT.FREESPIN);
        animPopupGetFreeSpin.gameObject.SetActive(true);
        animPopupGetFreeSpin.skeletonDataAsset = UIManager.instance.loadSkeletonData(FREESPIN_ANIMPATH);
        animPopupGetFreeSpin.TrimRenderers();
        animPopupGetFreeSpin.transform.localScale = new Vector2(0.25f, 0.25f);
        effectContainer.SetActive(true);
        animPopupGetFreeSpin.Initialize(true);
        animPopupGetFreeSpin.transform.Find("lbFreeSpinTurn").GetComponent<TextMeshProUGUI>().text = getInt(finishData, "freeSpinLeft").ToString();
        animPopupGetFreeSpin.AnimationState.SetAnimation(0, "yahoo2", true);
        animPopupGetFreeSpin.transform.DOScale(new Vector2(0.5f, 0.5f), 0.3f).SetEase(Ease.OutBack);
        TextMeshProUGUI lbMultiplier = animPopupGetFreeSpin.transform.Find("lbMultplier").GetComponent<TextMeshProUGUI>();
        totalFreeSpinGet = getInt(finishData, "freeSpinLeft");
        Button btnConfirm = animPopupGetFreeSpin.transform.GetComponentInChildren<Button>();
        btnConfirm.gameObject.SetActive(false);
        lbMultiplier.transform.localScale = Vector2.zero;
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                int number = Random.Range(1, 9);
                lbMultiplier.text = "x" + number;
            }).AppendInterval(0.05f)
            .SetLoops(30)
            .OnComplete(() =>
            {
                lbMultiplier.text = "x" + getInt(finishData, "multiFreeGame");
                btnConfirm.gameObject.SetActive(true);
            });
        lbMultiplier.transform.DOScale(Vector2.one, 1.5f).SetEase(Ease.OutBack);
        if (spintype == SPIN_TYPE.AUTO || spintype == SPIN_TYPE.FREE_AUTO)
        {
            DOTween.Sequence()
                .AppendInterval(10.0f)
                .AppendCallback(() =>
                {
                    if (animPopupGetFreeSpin.gameObject.activeSelf)
                    {
                        onClickClosePopupGetFreespin();
                    }
                });
        }
    }
    public void onClickClosePopupGetFreespin()
    {
        animPopupGetFreeSpin.transform.DOScale(new Vector2(0.25f, 0.25f), 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            animPopupGetFreeSpin.gameObject.SetActive(false);
            effectContainer.SetActive(false);
            handleActionResult();
        });
    }
    public override void setStateBtnSpin()
    {
        //base.setStateBtnSpin();
        if (gameState == GAME_STATE.SPINNING)
        {
            if (spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL)
            {
                animBtnSpin.startingAnimation = "Freespin2";
                //animBtnSpin.color = Color.gray;
            }
            else if (spintype == SPIN_TYPE.AUTO)
            {
                animBtnSpin.startingAnimation = "Stop2";
            }
            else
            {
                animBtnSpin.startingAnimation = "Spin2";
                //animBtnSpin.color = Color.gray;
            }
        }
        else
        {
            animBtnSpin.color = Color.white;
            if (gameState == GAME_STATE.SHOWING_RESULT)
            {
                if (spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL)
                {
                    animBtnSpin.startingAnimation = "Freespin2";
                }
                else
                {
                    if (spintype == SPIN_TYPE.AUTO)
                    {
                        animBtnSpin.startingAnimation = "Stop2";
                    }
                    else
                    {
                        animBtnSpin.startingAnimation = "Spin2";
                    }
                }
            }
            else if (gameState == GAME_STATE.PREPARE || gameState == GAME_STATE.JOIN_GAME)
            {
                animBtnSpin.startingAnimation = "Spin2";
                if (listBetRoom.Count == 0)
                {
                    //animBtnSpin.color = Color.gray;
                }
                else if (agPlayer < listBetRoom[currentMarkBet]) //het cmn tien roi.an di.
                {
                    animBtnSpin.color = Color.gray;
                }
                if (spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL)
                {
                    animBtnSpin.startingAnimation = "Freespin2";
                    animBtnSpin.color = Color.white;
                }
            }
        }
        animBtnSpin.Initialize(true);
    }
    public override void showAnimChipBay()
    {
        Transform transFrom = lbChipWins.transform;
        Transform transTo = lbCurrentChips.transform.parent.Find("icChip").transform;
        coinFly(transFrom, transTo);
        agPlayer = (long)finishData["AG"];
    }
    //private int indexPick = 0;



    private int indexFreeSpin = 0;
    public override void handleSpin(JObject data)
    {
        isSendingSpin = false;
        listDiamondPos.Clear();
        listPosCharJungle.Clear();
        finishData = data;
        //if (indexFreeSpin < 4)
        //{
        //    finishData = JObject.Parse(GetFakeDataTarzan.instance.getDataFreeSpinNormal(indexFreeSpin));
        //    indexFreeSpin++;
        //}

        if (miniGameView != null && miniGameView.gameObject.activeSelf)
        {
            miniGameView.setInfo((JObject)finishData["selectBonus"], true);
            return;
        }
        setFinishView((JArray)finishData["slotViews"]);
        winningLines = finishData["winningLine"].ToObject<List<int>>();
        linesDetail = (JArray)finishData["lineDetail"];
        winType = (int)finishData["winType"];
        isInFreeSpin = getBool(finishData, "freeSpin");

        JObject selectBonus = (JObject)finishData["selectBonus"];
        if (getInt(selectBonus, "typeBonus") == 1 || getInt(selectBonus, "typeBonus") == 0)
        {
            freespinLeft = (int)finishData["freeSpinLeft"];
            isFreeSpin = freespinLeft > 0;
        }
        arrayBonus = getListInt(finishData, "arrayBonus");
        lbFreespinLeft.text = "" + freespinLeft;
        if (freespinLeft == 0)
        {
            lbFreespinLeft.gameObject.SetActive(false);
        }
        checkFiveOfAKind();
        isWinScatter = checkWinScatter();
        Sequence s = DOTween.Sequence();
        s.AppendInterval(1.0f)
        .AppendCallback(() =>
        {
            listCollum[0].isStop = true;
            currentIndexStop = 0;
        });
        SocketSend.getFarmInfo();
    }
    private void showGetItemDiamond(Vector2 posItem, TweenCallback cb = null)
    {
        GameObject diamondContainer = new GameObject("Diamond");
        RectTransform trans = diamondContainer.AddComponent<RectTransform>();
        diamondContainer.transform.SetParent(transform);
        trans.localScale = Vector3.one;
        SkeletonGraphic diamond = Instantiate(animBtnSpin.gameObject, diamondContainer.transform).GetComponent<SkeletonGraphic>();

        diamond.skeletonDataAsset = animDiamond;
        diamond.transform.localScale = new Vector2(1.0f, 1.0f);
        diamond.color = Color.white;
        diamond.Initialize(true);
        diamond.AnimationState.SetAnimation(0, "animation", false);
        diamond.transform.localPosition = new Vector2(441, -970);
        diamondContainer.transform.localPosition = posItem;
        Vector2 posDiamondOnPos = transform.InverseTransformPoint(diamondPot.transform.parent.TransformPoint(diamondPot.transform.localPosition));
        diamondContainer.transform.DOLocalMove(posDiamondOnPos, 1.0f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            Destroy(diamondContainer);
            if (cb != null)
            {
                float progressChipBonus = (float)getInt(finishData, "numExp") / getInt(finishData, "maxExp");
                if (progressChipBonus != 0)
                {
                    //animBuiSang.gameObject.SetActive(true);
                    //animBuiSang.Initialize(true);
                    //animBuiSang.AnimationState.SetAnimation(0, "run", false);
                    //animBuiSang.AnimationState.Complete += delegate
                    //{
                    //    animBuiSang.AnimationState.SetAnimation(0, "normal", false);
                    //};
                }
                //sprProgressChipBonus.DOFillAmount(progressChipBonus, 0.5f);
                sprProgressChipBonus.DOFillAmount(progressChipBonus, 0.5f).SetEase(Ease.InSine).OnUpdate(() =>
                {
                    float posLight = -163 + (396 * sprProgressChipBonus.fillAmount);
                    //animLightBar.transform.DOLocalMoveX(posLight, 0.5f).SetEase(Ease.InSine);
                    animLightBar.transform.localPosition = new Vector2(posLight, 3.8f);
                });
                lbDiamondNum.text = getInt(finishData, "numExp").ToString();
                Globals.Config.tweenNumberToNumber(lbCurrentChipBonus, getInt(finishData, "currentChipBonus"), currentChipBonus, 0.3f);
                currentChipBonus = getInt(finishData, "currentChipBonus");
                handleActionResult();
            }
        });
        diamondContainer.transform.DOScale(new Vector2(0.5f, 0.5f), 1.0f).SetEase(Ease.OutCubic);

    }
    protected override int checkNumberOfItemInLine(List<int> listId)
    {
        int count = 1;
        int idCheck = listId[0];
        string line = "";
        for(int i= 0; i < listId.Count; i++)
        {

            line += listId[i] + ",";
        }
      
        List<int> listIdWild = new List<int> { 13, 15, 16, 17, 18 };
        for (int i = 1, l = listId.Count; i < l; i++)
        {
            if (idCheck != 11)
            {
                if (listId[i] == idCheck || listIdWild.Contains(listId[i]))
                    count++;
                else break;
            }
            else
            {
                idCheck = listId[i];
                count++;
            }
        }
        return count;
    }
    private void showTransformWild()
    {
        animTarzan.gameObject.SetActive(true);
        animTarzan.transform.localPosition = new Vector2(0, 26);
        animTarzan.skeletonDataAsset = UIManager.instance.loadSkeletonData("GameView/SlotSpine/Tarzan/Model/skeleton_SkeletonData");
        animTarzan.Initialize(true);
        animTarzan.AnimationState.Complete += delegate
        {
            animTarzan.Initialize(true);
            animTarzan.AnimationState.SetAnimation(0, "du_day1", false);
            animTarzan.transform.localPosition = new Vector2(0, 259);
        };
        animTarzan.AnimationState.SetAnimation(0, "du_day", false);
        DOTween.Sequence()
            .AppendInterval(2.0f)
            .AppendCallback(() =>
            {
                for (int i = 0; i < listCollum.Count; i++)
                {
                    SlotTarzanItemSpin itemResult = (SlotTarzanItemSpin)listCollum[i].itemResult;
                    itemResult.transformToWild();
                }
            })
            .AppendInterval(1.0f)
            .AppendCallback(() =>
            {
                handleActionResult();
            });

    }
    private void showGetItemJunggle(Vector2 posChar, int indexChar, TweenCallback cb = null)
    {
        //indexChar = 3;
        List<string> listIndex = new List<string> { "J", "U", "N", "G", "L", "E" };
        string CHARACTER_ANIMPATH = "GameView/SlotSpine/Tarzan/JungleCharacter/" + listIndex[indexChar] + "/skeleton_SkeletonData";
        SkeletonGraphic character = Instantiate(animBtnSpin.gameObject, transform).GetComponent<SkeletonGraphic>();
        character.skeletonDataAsset = UIManager.instance.loadSkeletonData(CHARACTER_ANIMPATH);
        character.transform.localScale = new Vector2(0.5f, 0.5f);
        character.color = Color.white;
        character.Initialize(true);
        character.AnimationState.SetAnimation(0, "animation", false);
        character.transform.localPosition = posChar;
        Vector2 posEnd = transform.InverseTransformPoint(listCharacter[indexChar].transform.position);
        //DOTween.Sequence().AppendInterval
        character.transform.DOLocalMove(posEnd, 1.0f).SetEase(Ease.OutCubic).SetDelay(1.0f).OnComplete(() =>
        {
            Destroy(character);
            listCharacter[indexChar].sprite = sprCharacterOn[indexChar];
            if (cb != null)
            {
                DOTween.Sequence().AppendCallback(cb);
            }
        });
        character.transform.DOScale(new Vector2(0.15f, 0.15f), 1.0f).SetDelay(1.0f).SetEase(Ease.OutCubic);
    }
    public override void onStopSpin()
    {
        ///------------------CHECK SHOW CHARACTER--------------------//
        JArray slotV = getJArray(finishData, "slotViews");
        for (int i = 0; i < listCollum.Count; i++)
        {
            JArray viewCol = (JArray)slotV[i];
            SlotTarzanItemSpin itemResult = (SlotTarzanItemSpin)listCollum[i].itemResult;
            for (int j = 0; j < viewCol.Count; j++)
            {
                JObject dataItem = (JObject)viewCol[j];
                if (getInt(dataItem, "symbolAdd") != -1)
                {
                    JObject dataCharacter = new JObject();
                    dataCharacter["positionX"] = itemResult.getPositionItem(j).x;
                    dataCharacter["positionY"] = itemResult.getPositionItem(j).y;
                    dataCharacter["symbolAdd"] = getInt(dataItem, "symbolAdd");
                    listPosCharJungle.Add(dataCharacter);
                }
                if (getBool(dataItem, "wildFake") == true)
                {
                    slotViews[i][j] += 11;
                }
            }
        }
        if (listPosCharJungle.Count > 0)
        {
            TweenCallback acShowCharacter = () => ///show character
            {
                listPosCharJungle.ForEach(data =>
                {
                    TweenCallback cb = () =>
                    {
                        handleActionResult();
                    };
                    Vector2 posEnd = new Vector2(getFloat(data, "positionX"), getFloat(data, "positionY"));
                    showGetItemJunggle(posEnd, getInt(data, "symbolAdd"), (listPosCharJungle.IndexOf(data) == listPosCharJungle.Count - 1) ? cb : null);
                });
            };
            listActionHandleSpin.Add(acShowCharacter);
        }

        ///------------------CHECK SHOW GET DIAMOND--------------------//
        if (listDiamondPos.Count > 0)
        {
            TweenCallback acShowDiamond = () => //show get diamond
            {
                for (int i = 0; i < listDiamondPos.Count; i++)
                {
                    Vector2 posInitDiamond = transform.InverseTransformPoint(listDiamondPos[i]);
                    TweenCallback cb = () =>
                     {
                         handleActionResult();
                     };
                    showGetItemDiamond(posInitDiamond, i == listDiamondPos.Count - 1 ? cb : null);
                }
            };
            listActionHandleSpin.Add(acShowDiamond);
        }
        ///------------------CHECK SHOW POPUP GET FULL  DIAMOND--------------------//
        if (getInt(finishData, "chipBonusFullExp") != 0)
        {
            TweenCallback acShowPopupGem = () =>
            {
                showPopupResultGem();
            };
            listActionHandleSpin.Add(acShowPopupGem);
        }
        ///------------------CHECK SHOW TRANSFORM WILD--------------------//
        List<int> lastColView = (slotViews[slotViews.Count - 1]);
        if (lastColView.IndexOf(13) != -1) //co wild cot cuoi cung
        {

            TweenCallback acShowIconWild = () =>
            {
                SlotTarzanItemSpin itemSpin = (SlotTarzanItemSpin)listCollum[listCollum.Count - 1].itemResult;
                itemSpin.showAnimWild(lastColView.IndexOf(13), () =>
                {
                    handleActionResult();
                });
            };
            listActionHandleSpin.Add(acShowIconWild);
            TweenCallback acShowWildTarzan = () =>
            {
                showTransformWild();
            };
            listActionHandleSpin.Add(acShowWildTarzan);
        }
        ///------------------CHECK SHOW POPUP RESULT FREESPIN--------------------//
        //Globals.Logging.Log("clmmmm: co show freespin ma:" + isInFreeSpin + "---isFreeSpin=" + isFreeSpin);
        if (isInFreeSpin == true && isFreeSpin == false) //freespin turn cuoi cung
        {

            //int currentTotaAgFree = countTotalAgFreespin;
            TweenCallback acShowFreeSpinResult = () =>
            {
                showPopupResultFreeSpin();
            };
            listActionHandleSpin.Add(acShowFreeSpinResult);
        }


        if (freespinLeft == 0 || (winType == 100 && !isInFreeSpin)) //spin thuong hoac spin turn do dc frespin nhung freespin nay ko trong luc dang freespin. thi moi show update tien con khong thi het freespin moi update
        {
            Globals.Config.tweenNumberToNumber(lbCurrentChips, (long)finishData["AG"], agPlayer, 0);
        }
        agPlayer = (long)finishData["AG"];
        listBetRoom = getListBetRoom();
        if (currentMarkBet >= listBetRoom.Count && listBetRoom.Count > 0)
        {
            currentMarkBet = listBetRoom.Count - 1;

            lbCurrentBet.text = Globals.Config.FormatMoney2(listBetRoom[currentMarkBet], Globals.Config.curGameId == (int)Globals.GAMEID.SLOTTARZAN);
        }
        if (!isInFreeSpin) //khong co freespin
        {
            if ((long)finishData["agWin"] != 0)
            {
                Globals.Config.tweenNumberTo(lbChipWins, (long)finishData["agWin"], 0, 0.3f, true);
                if ((long)finishData["agWin"] > 0)
                {
                    sprStateWin.sprite = listSprStateWin[0];
                    sprStateWin.SetNativeSize();
                }
            }
        }
        else if (isInFreeSpin == true && isFreeSpin == false) //freespin turn cuoi cung
        {
            //int currentTotaAgFree = countTotalAgFreespin;
            countTotalAgFreespin += (int)finishData["agWin"];
            sprStateWin.sprite = listSprStateWin[1];
            sprStateWin.SetNativeSize();
            Globals.Config.tweenNumberTo(lbChipWins, countTotalAgFreespin, countTotalAgFreespin - (int)finishData["agWin"]);
            if (countTotalAgFreespin > payLines.Count * listBetRoom[currentMarkBet])
            {
                winType = 1;
            }
            if (countTotalAgFreespin > 50 * listBetRoom[currentMarkBet])
            {
                winType = 2;
            }
        }
        else //dang trong freespin
        {
            countTotalAgFreespin += (int)finishData["agWin"];
            Globals.Config.tweenNumberTo(lbChipWins, countTotalAgFreespin, countTotalAgFreespin - (int)finishData["agWin"]);
        }
        TweenCallback acCheckNextSpin = () =>
        {
            setSpinType();
            refreshBetRoomAndMarkbet();
            if (spintype == SPIN_TYPE.AUTO || spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL)
            {
                resetSlotView();
                if ((spintype == SPIN_TYPE.FREE_AUTO || spintype == SPIN_TYPE.FREE_NORMAL))
                {
                    onClickSpin();
                }
                else if (listBetRoom.Count > 0)
                {
                    onClickSpin();
                }
                else
                {
                    setStateBtnSpin();
                }
            }
            else
            {
                resetSlotView();
                setStateBtnSpin();
            }
        };
        TweenCallback acShowOneWinLine = () =>
        {
            gameState = GAME_STATE.SHOWING_RESULT;
            if (!isFreeSpin || (!isFreeSpin && isInFreeSpin) || (isFreeSpin && !isInFreeSpin)) //khong co freespin || freespin turn cuoi || bat dau freespin
            {
                Globals.Config.tweenNumberToNumber(lbCurrentChips, (long)finishData["AG"], agPlayer);
                showAnimChipBay();
            }
            showOneByOneLine();
        };

        ///------------------CHECK SHOW WIN FREESPIN--------------------//
        if (isWinScatter)
        {
            TweenCallback acShowWinScatter = () =>
            {
                showWinScatter();
            };
            listActionHandleSpin.Add(acShowWinScatter);
        }

        ///------------------CHECK SHOW FIVE OF A KIND--------------------//
        if (isFiveOfaKind)
        {
            TweenCallback acShowFiveOfAkind = () =>
            {
                showFiveOfaKind();
            };
            listActionHandleSpin.Add(acShowFiveOfAkind);
        }

        ///------------------CHECK SHOW FREESPIN--------------------//

        if (isGetFreeSpin)
        {
            TweenCallback acShowFreeSpin = () =>
            {
                showFreeSpin();
            };
            listActionHandleSpin.Add(acShowFreeSpin);

        }

        ///------------------CHECK SHOW ALL LINE--------------------//
        if (winningLines.Count > 0)
        {
            TweenCallback acShowAllWinLine = () =>
            {
                showAllWinline();
            };
            listActionHandleSpin.Add(acShowAllWinLine);
        }
        else
        {

        }

        ///------------------CHECK SHOW TYPE WIN--------------------//
        if (winType == TYPE_BIGWIN)
        {
            TweenCallback acShowBigWin = () =>
            {
                showBigWin();
            };
            if (!isFreeSpin)
            {
                listActionHandleSpin.Add(acShowBigWin);
            }
        }
        else if (winType == TYPE_HUGEWIN)
        {
            TweenCallback acShowHugeWin = () =>
            {
                showHugeWin();
            };
            if (!isFreeSpin)
            {
                listActionHandleSpin.Add(acShowHugeWin);
            }
        }
        else if (winType == TYPE_MEGA)
        {
            TweenCallback acShowMegaWin = () =>
            {
                showMegaWin();
            };
            if (!isFreeSpin)
            {
                listActionHandleSpin.Add(acShowMegaWin);
            }

        }

        ///------------------CHECK GET MINI GAME--------------------//
        JObject selectBonus = (JObject)finishData["selectBonus"];
        if (getInt(selectBonus, "typeBonus") == 2)
        {
            TweenCallback acShowPopupMiniGame = () =>//get mini game
            {
                showPopupMiniGame();
            };
            listActionHandleSpin.Add(acShowPopupMiniGame);
            TweenCallback acShowMiniGame = () => //show mini game
            {
                showMinigameView();
            };
            listActionHandleSpin.Add(acShowMiniGame);
        }

        ///------------------CHECK SHOW ONE BY ONE--------------------//
        if (winningLines.Count > 0) //co line win va quay thuong thi moi show an tung line
        {
            if (spintype == SPIN_TYPE.NORMAL)
            {
                listActionHandleSpin.Add(acShowOneWinLine);
            }
            else if (spintype == SPIN_TYPE.AUTO)
            {
                TweenCallback acShowAnimChipBay = () =>
                {
                    showAnimChipBay();
                    handleActionResult();
                };
                listActionHandleSpin.Add(acShowAnimChipBay);
            }

        }
        else if (winningLines.Count == 0 && lastColView.IndexOf(13) != -1 && isFreeSpin == false)//an wild tarzan
        {
            TweenCallback acShowAnimChipBay = () =>
            {
                Globals.Config.tweenNumberToNumber(lbCurrentChips, (long)finishData["AG"], agPlayer);
                showAnimChipBay();
                handleActionResult();
            };
            listActionHandleSpin.Add(acShowAnimChipBay);
        }
        else if (winningLines.Count == 0 && getLong(finishData, "agWin") > 0)
        {

            TweenCallback acShowAnimChipBay = () =>
            {
                Globals.Config.tweenNumberToNumber(lbCurrentChips, (long)finishData["AG"], agPlayer);
                refreshBetRoomAndMarkbet();
                showAnimChipBay();
                handleActionResult();
            };
            listActionHandleSpin.Add(acShowAnimChipBay);
        }
        listActionHandleSpin.Add(acCheckNextSpin);
        handleActionResult();
    }


    protected override void setFinishView(JArray dataFinishView)
    {
        slotViews.Clear();
        foreach (JArray data in dataFinishView)
        {
            List<int> collumView = new List<int>();
            foreach (JObject item in data)
            {
                collumView.Add(getInt(item, "id"));
            }
            slotViews.Add(collumView);
        }
        for (int i = 0; i < dataFinishView.Count; i++)
        {

            JArray data = (JArray)dataFinishView[i];
            List<int> viewCollum = new List<int>();
            foreach (JObject item in data)
            {
                viewCollum.Add(getInt(item, "id"));
            }
            listCollum[i].setFinishView(viewCollum);
        }

    }
    protected override void checkFiveOfAKind()
    {
        for (int i = 0, l = winningLines.Count; i < l; i++)
        {
            if (!isFiveOfaKind)
            {
                List<int> listIdInLine = new List<int>();
                List<int> lineWinID = getPaylineWithID(winningLines[i]);
                int numberItem = 0;
                for (int j = 0; j < lineWinID.Count; j++)
                {
                    JArray resultView = (JArray)finishData["slotViews"];
                    List<int> collumView = slotViews[j];
                    listIdInLine.Add(collumView[lineWinID[j]]);
                    numberItem = checkNumberOfItemInLine(listIdInLine);
                    if (numberItem == 5)
                    {
                        isFiveOfaKind = true;
                        break;
                    }
                }
            }
            else
            {
                break;
            }

        }
    }
    public void showMinigameView()
    {
        isPlayingMiniGame = true;
        miniGameView.show();
        miniGameView.setInfo((JObject)finishData["selectBonus"]);
    }
    protected override void showBigWin()
    {
        playSound(Globals.SOUND_SLOT.BIG_WIN);
        effectContainer.SetActive(true);
        animEffect.gameObject.SetActive(true);
        DOTween.Sequence()
            .AppendInterval(0.85f)
            .AppendCallback(() =>
        {
            lbBigWin.gameObject.SetActive(true);
            if (isInFreeSpin == true && isFreeSpin == false) //vua quay het freespin turn cuoi cung;
            {
                Globals.Config.tweenNumberTo(lbBigWin, countTotalAgFreespin, 0, 3.0f);
            }
            else
            {
                Globals.Config.tweenNumberTo(lbBigWin, getInt(finishData, "agWin"), 0, 3.0f);
            }
        })
            .AppendInterval(3.85f)
            .AppendCallback(() =>
            {
                lbBigWin.gameObject.SetActive(false);
            });
        animEffect.TrimRenderers();
        animEffect.transform.localScale = new Vector2(1.0f, 1.0f);
        animEffect.gameObject.GetComponent<RectTransform>().localScale = new Vector2(0.8f, 0.8f);
        animEffect.skeletonDataAsset = UIManager.instance.loadSkeletonData(BIGWIN_ANIMPATH);
        animEffect.Initialize(true);
        animEffect.AnimationState.Complete += delegate
        {
            effectAnimEndListenter();
        };
        animEffect.AnimationState.SetAnimation(0, ANIM_BIGWIN_NAME, false);
        effectAnimEndListenter = () =>
        {
            lbBigWin.transform.parent.gameObject.SetActive(false);
            effectContainer.SetActive(false);
            handleActionResult();
            gameState = GAME_STATE.SHOWING_RESULT;
        };

    }
    protected override void showMegaWin()
    {
        playSound(Globals.SOUND_SLOT.MEGA_WIN);
        effectContainer.SetActive(true);
        animEffect.gameObject.SetActive(true);
        DOTween.Sequence()
           .AppendInterval(0.85f)
           .AppendCallback(() =>
           {
               lbBigWin.gameObject.SetActive(true);
               if (isInFreeSpin == true && isFreeSpin == false) //vua quay het freespin turn cuoi cung;
               {
                   Globals.Config.tweenNumberTo(lbBigWin, countTotalAgFreespin, 0, 4.0f);
               }
               else
               {
                   Globals.Config.tweenNumberTo(lbBigWin, getInt(finishData, "agWin"), 0, 4.0f);
               }
           })
           .AppendInterval(4.85f)
           .AppendCallback(() =>
           {
               lbBigWin.gameObject.SetActive(false);
           });

        //Globals.Config.tweenNumberTo(lbBigWin, 100000, 0, 3.0f);
        animEffect.TrimRenderers();
        animEffect.skeletonDataAsset = UIManager.instance.loadSkeletonData(BIGWIN_ANIMPATH);
        animEffect.transform.localScale = new Vector2(1.0f, 1.0f);
        animEffect.gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
        animEffect.Initialize(true);
        animEffect.AnimationState.SetAnimation(0, ANIM_MEGAWIN_NAME, false);
        animEffect.AnimationState.Complete += delegate
        {
            effectAnimEndListenter();
        };
        effectAnimEndListenter = () =>
        {
            effectContainer.SetActive(false);
            lbBigWin.transform.parent.gameObject.SetActive(false);
            handleActionResult();
            gameState = GAME_STATE.SHOWING_RESULT;
        };
    }
    protected override void showHugeWin()
    {
        playSound(Globals.SOUND_SLOT.MEGA_WIN);
        effectContainer.SetActive(true);
        animEffect.gameObject.SetActive(true);
        DOTween.Sequence()
           .AppendInterval(0.85f)
           .AppendCallback(() =>
           {
               lbBigWin.gameObject.SetActive(true);
               if (isInFreeSpin == true && isFreeSpin == false) //vua quay het freespin turn cuoi cung;
               {
                   Globals.Config.tweenNumberTo(lbBigWin, countTotalAgFreespin, 0, 4.0f);
               }
               else
               {
                   Globals.Config.tweenNumberTo(lbBigWin, getInt(finishData, "agWin"), 0, 4.0f);
               }
           })
           .AppendInterval(4.85f)
           .AppendCallback(() =>
           {
               lbBigWin.gameObject.SetActive(false);
           });

        //Globals.Config.tweenNumberTo(lbBigWin, 100000, 0, 3.0f);
        animEffect.TrimRenderers();
        animEffect.skeletonDataAsset = UIManager.instance.loadSkeletonData(BIGWIN_ANIMPATH);
        animEffect.transform.localScale = new Vector2(1.0f, 1.0f);
        animEffect.gameObject.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
        animEffect.Initialize(true);
        Globals.Logging.Log("ANIM_HUGEWIN_NAME=" + ANIM_HUGEWIN_NAME);
        animEffect.AnimationState.SetAnimation(0, ANIM_HUGEWIN_NAME, false);
        animEffect.AnimationState.Complete += delegate
        {
            effectAnimEndListenter();
        };
        effectAnimEndListenter = () =>
        {
            effectContainer.SetActive(false);
            lbBigWin.transform.parent.gameObject.SetActive(false);
            handleActionResult();
            gameState = GAME_STATE.SHOWING_RESULT;
        };
    }
    public void showPopupResultFreeSpin()
    {
        Globals.Logging.Log("showPopupResultFreeSpin");
        //countTotalAgFreespin = 123456;
        playSound(Globals.SOUND_SLOT.SHOW_LINE);
        animPopupResultFreeSpin.skeletonDataAsset = UIManager.instance.loadSkeletonData(FREESPIN_ANIMPATH);
        animPopupResultFreeSpin.TrimRenderers();
        animPopupResultFreeSpin.transform.localScale = new Vector2(0.25f, 0.25f);
        effectContainer.SetActive(true);
        animPopupResultFreeSpin.gameObject.SetActive(true);
        animPopupResultFreeSpin.Initialize(true);
        TextMeshProUGUI lbFreeSpinWin = animPopupResultFreeSpin.transform.Find("lbFreeSpinWin").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI lbFreeSpinNum = animPopupResultFreeSpin.transform.Find("lbTotalFreeSpin").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI lbMultiplier = animPopupResultFreeSpin.transform.Find("lbMultiplierNum").GetComponent<TextMeshProUGUI>();
        lbFreeSpinNum.text = totalFreeSpinGet + "";
        lbMultiplier.text = "x" + getInt(finishData, "multiFreeGame");
        Globals.Config.tweenNumberToMoney(lbFreeSpinWin, countTotalAgFreespin, 0, 1.0f, 10000);
        animPopupResultFreeSpin.AnimationState.SetAnimation(0, "wonderful2", true);
        animPopupResultFreeSpin.transform.DOScale(new Vector2(0.5f, 0.5f), 0.3f).SetEase(Ease.OutBack);
        totalFreeSpinGet = 0;
        if (spintype == SPIN_TYPE.FREE_AUTO)
        {
            DOTween.Sequence()
                .AppendInterval(10.0f)
                .AppendCallback(() =>
                {
                    if (animPopupResultFreeSpin.gameObject.activeSelf)
                    {
                        closePopupResultFreeSpin();
                    }
                });
        }
    }
    public void closePopupResultFreeSpin()
    {
        animPopupResultFreeSpin.transform.DOScale(new Vector2(0.25f, 0.25f), 0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            animPopupResultFreeSpin.gameObject.SetActive(false);
            effectContainer.SetActive(false);
            handleActionResult();
        });
    }
    public override void onSpinTriggerUp()
    {
        if (miniGameView.gameObject.activeSelf)
        {
            return;
        }
        base.onSpinTriggerUp();

    }

}
