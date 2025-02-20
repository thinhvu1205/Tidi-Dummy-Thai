using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;
using DG.Tweening;
using System;
using System.Linq;
using System.Text.RegularExpressions;
//using Newtonsoft.Json.Linq;

public class LeaderBoardView : BaseView
{
    // Start is called before the first frame update
    public static LeaderBoardView instance;

    [SerializeField]
    ScrollRect scrollView;

    [SerializeField]
    GameObject itemTabGame, itemViewRank;

    [SerializeField]
    TextMeshProUGUI lbTimeRemain;

    [SerializeField]
    ScrollRect scrTab;


    [SerializeField]
    RectTransform tabParent4;

    [HideInInspector]
    JArray dataTop = new JArray();
    JArray listGameDefined = new JArray();

    [SerializeField]
    ToggleGroup tgGroup;

    [SerializeField]
    ItemTopGame itemMe;

    private long _timeRemain = -1;
    public long timeRemain   // property
    {
        get => _timeRemain;    // get method
        set
        {
            if (_timeRemain == -1)
            {
                _timeRemain = value;
            }
        }  // set method
    }

    protected override void Awake()
    {
        base.Awake();
        LeaderBoardView.instance = this;
    }
    protected override void Start()
    {
        base.Start();

        SocketIOManager.getInstance().emitSIOCCCNew(Globals.Config.formatStr("ClickShowTopGame_%s", Globals.CURRENT_VIEW.getCurrentSceneName()));
        Globals.CURRENT_VIEW.setCurView(Globals.CURRENT_VIEW.TOP_VIEW);
        List<int> listGameID = new List<int>();
        //{
        //     1111,8088,8813,8808,8803,8802,1001, 8044, 8819,9501,9500,8089,6688,8012,8011,8010,8015, 8013
        //};
        foreach(JObject data in Globals.Config.listRankGame)
        {
            Globals.Logging.Log("Clmmm" + data["id"]);
            listGameID.Add((int)data["id"]);
        }
        //List<string> listGameName = new List<string>()
        //{
        //    "Roulette","Tongits","Hong Kong Poker","Hilo","Gounds Crab Fish","Bork Kdeng","Shan Koe Mee","13 Poker","Burmese Poker","Black Jack","Baccarat","Tongits 11","Lucky 9","Fruits Slot","Three Cards Poker","ShaBong","Sicbo","Dummy","Kaeng"
        //};
        int size = listGameID.Count;

        for (int i = 0; i < size; i++)
        {
            JObject dataGameDefined = new JObject();
            dataGameDefined["id"] = listGameID[i];
            listGameDefined.Add(dataGameDefined);
        }
        genListTabGame();
    }
    int gameIDOpen = -1;
    public void openTabGameWithID(int gameID)
    {
        if (gameID <= 0) return;
        gameIDOpen = gameID;
    }

    // Update is called once per frame
   
    public void onClickItemGame(ToggleItemCtrl itemTab)
    {
        SoundManager.instance.soundClick();
        if (itemTab != null && itemTab.toggle.isOn && itemTab.data != null && itemTab.data.ContainsKey("id"))
        {
            getDataTopGame((int)itemTab.data["id"]);
        }
    }
    private void getDataTopGame(int gameId)
    {
        SocketSend.getTopGamer(gameId, 0);
    }
    private void genListTabGame()
    {
        tgGroup.allowSwitchOff = true;
        int size = Globals.Config.listGame.Count;
        var countGame = 0;
        bool isHasGame = false;
        for (int i = 0; i < size; i++)
        {
            JObject itemData = (JObject)Globals.Config.listGame[i];
            int gameId = (int)itemData["id"];
            string name = getGameNameWithId(gameId);
            if (!name.Equals(""))
            {
                countGame++;
            }
            if(gameId == gameIDOpen)
            {
                isHasGame = true;
            }
        }
        Globals.Logging.Log("-=countGame " + countGame);
        var indexRun = 0;

        if (countGame > 4)
        {
            scrTab.gameObject.SetActive(true);
            tabParent4.gameObject.SetActive(false);
            for (int i = 0; i < size; i++)
            {
                JObject itemData = (JObject)Globals.Config.listGame[i];
                int gameId = (int)itemData["id"];
                string name = getGameNameWithId(gameId);
                if (!name.Equals(""))
                {
                    GameObject itemTab;
                    if (i < scrTab.content.childCount)
                    {
                        itemTab = scrTab.content.GetChild(indexRun).gameObject;
                    }
                    else
                    {
                        itemTab = Instantiate(itemTabGame, scrTab.content);
                    }
                    itemTab.SetActive(true);
                    ToggleItemCtrl itemTogComp = itemTab.GetComponent<ToggleItemCtrl>();
                    itemTogComp.lbTextOff.text = name;
                    itemTogComp.lbTextOn.text = name;
                    itemTogComp.data = itemData;
                    if (gameIDOpen != -1 && isHasGame)
                    {
                        itemTab.GetComponent<Toggle>().isOn = gameIDOpen == gameId;
                    }
                    else
                    {
                        itemTab.GetComponent<Toggle>().isOn = (indexRun == 0);
                    }
                    indexRun++;
                }
            }

            //scrTab.content.GetComponent<ToggleGroup>().allowSwitchOff = false;

            for (int i = indexRun; i < scrTab.content.childCount; i++)
            {
                scrTab.content.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            scrTab.gameObject.SetActive(false);
            tabParent4.gameObject.SetActive(true);

            var delta = (tabParent4.rect.width - (countGame - 1)*10) / countGame;
            Globals.Logging.Log("-=-=delta " + delta);

            for (int i = 0; i < size; i++)
            {
                JObject itemData = (JObject)Globals.Config.listGame[i];
                int gameId = (int)itemData["id"];
                string name = getGameNameWithId(gameId);
                if (!name.Equals(""))
                {
                    GameObject itemTab;
                    if (i < tabParent4.childCount)
                    {
                        itemTab = tabParent4.GetChild(indexRun).gameObject;
                    }
                    else
                    {
                        itemTab = Instantiate(itemTabGame, tabParent4);
                    }
                    itemTab.SetActive(true);
                    itemTab.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(delta, 61);
                    ToggleItemCtrl itemTogComp = itemTab.GetComponent<ToggleItemCtrl>();
                    itemTogComp.lbTextOff.text = name;
                    itemTogComp.lbTextOn.text = name;
                    itemTogComp.data = itemData;
                    if(gameIDOpen != -1 && isHasGame)
                    {
                        itemTab.GetComponent<Toggle>().isOn = gameIDOpen == gameId;
                    }
                    else
                    {
                        itemTab.GetComponent<Toggle>().isOn = (indexRun == 0);
                    }
                    
                    indexRun++;
                }
            }
            for (int i = indexRun; i < tabParent4.childCount; i++)
            {
                tabParent4.GetChild(i).gameObject.SetActive(false);
            }
        }


        tgGroup.allowSwitchOff = false;
    }
    private string getGameNameWithId(int idGame)
    {
        string name = "";
        foreach (JObject item in listGameDefined)
        {
            if ((int)item["id"] == idGame)
            {
                name = Globals.Config.getTextConfig(idGame.ToString());
                break;
            }
        }
        return name;
    }

    public void reloadList(JArray listTopGamer)
    {
        dataTop = listTopGamer;
        //int size = dataTop.Count;
        //scrList.setDataList(setDataItem, dataTop);
        JObject dataMe = null;
        for (var i = 0; i < dataTop.Count; i++)
        {
            var dataItem = (JObject)dataTop[i];
            if((int)dataItem["Id"] == Globals.User.userMain.Userid)
            {
                dataMe = dataItem;
            }
            GameObject itemView;
            if(i < scrollView.content.childCount)
            {
                itemView = scrollView.content.GetChild(i).gameObject;
            }
            else
            {
                itemView = Instantiate(itemViewRank, scrollView.content);
            }
            itemView.SetActive(true);
            itemView.GetComponent<ItemTopGame>().setInfo(dataItem);
        }
        for (int i = dataTop.Count; i < scrollView.content.childCount; i++)
        {
            scrollView.content.GetChild(i).gameObject.SetActive(false);
        }
        //scrList.ScrollToTop();
        scrollView.DOVerticalNormalizedPos(1.0f, 0.2f).SetEase(Ease.OutSine);

        //checkCardLeft = (allLayoff.FirstOrDefault(data => (int)data["idCardSend"] == cardTemp.code)) != null || checkCardLeft;
        //var dataMe = listTopGamer.FirstOrDefault(it => (int)it["Id"] == Globals.User.userMain.Userid);
        if(dataMe != null)
        {
            itemMe.gameObject.SetActive(true);
            itemMe.setInfo((JObject)dataMe, true);
        }
    }
    //public void setDataItem(GameObject item, JObject data)
    //{
    //    item.GetComponent<ItemTopGame>().setInfo(data);
    //}
    public void setTimeRemain(long time)
    {
        timeRemain = time;
        Sequence s = DOTween.Sequence();
        s.AppendInterval(1.0f)
        .AppendCallback(() =>
        {
            countDownTime();
            if (timeRemain > 0)
            {
                setTimeRemain(timeRemain--);
            }
        });

    }
    private void countDownTime()
    {
        long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        long deltaTime = timeRemain - currentTime;
        string seconds = Math.Floor(((double)(deltaTime / 1000) % 60)) + "";
        string minutes = Math.Floor(((double)(deltaTime / 1000 / 60) % 60)) + "";
        string hours = Math.Floor(((double)(deltaTime / (1000 * 60 * 60)) % 24)) + "";
        string days = Math.Floor(((double)deltaTime / (1000 * 60 * 60 * 24))) + "";
        double dayNum = Math.Floor(((double)deltaTime / (1000 * 60 * 60 * 24)));
        if (hours.Length < 2) hours = "0" + hours;
        if (minutes.Length < 2) minutes = "0" + minutes;
        if (seconds.Length < 2) seconds = "0" + seconds;

        string time_ = days + (dayNum < 2 ? " "+Globals.Config.getTextConfig("txt_day") : " " + Globals.Config.getTextConfig("txt_days")) + ", " + hours + ":" + minutes + ":" + seconds;
        lbTimeRemain.text = time_;
    }
}
