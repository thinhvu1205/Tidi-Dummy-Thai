using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupMenuView : BaseView
{
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnChangeTable;

    public void onClickSwitchTable()
    {
        SoundManager.instance.soundClick();
        if (UIManager.instance.gameView.stateGame == Globals.STATE_GAME.PLAYING)
        {
            UIManager.instance.showMessageBox(Globals.Config.getTextConfig("txt_intable"));
        }
        else
        {
            //Global.MainView._isClickGame = false;
            Globals.Config.isChangeTable = true;
            onClickBack();
        }
        hide();
    }
    protected override void Start()
    {
        base.Start();
        var curGameId = Globals.Config.curGameId;
        if (curGameId == (int)Globals.GAMEID.KEANG || curGameId == (int)Globals.GAMEID.DUMMY || curGameId == (int)Globals.GAMEID.SICBO)
        {
            btnSetting.gameObject.SetActive(false);
        }
        if (curGameId == (int)Globals.GAMEID.SLOTNOEL|| (curGameId == (int)Globals.GAMEID.SLOTTARZAN) || (curGameId == (int)Globals.GAMEID.SLOT_9900))
        {
            btnChangeTable.gameObject.SetActive(false);
        }
    }

    public void onClickRule()
    {
        SoundManager.instance.soundClick();
        hide();
        var curGameId = Globals.Config.curGameId;
        var urlRule = Globals.Config.url_rule.Replace("%gameid%", curGameId + "");
        //var langLocal = cc.sys.localStorage.getItem("language_client");
        //var language = langLocal == LANGUAGE_TEXT_CONFIG.LANG_EN ? "en" : "thai"
        var language = "thai";
        urlRule = urlRule.Replace("%language%", language);
        // https://conf.topbangkokclub.com/rule/index.html?gameid=%gameid%&language=%language%&list=true
        List<int> listGameOther = new List<int> { (int)Globals.GAMEID.SLOTNOEL, (int)Globals.GAMEID.SLOTTARZAN, (int)Globals.GAMEID.LUCKY9, (int)Globals.GAMEID.SICBO, (int)Globals.GAMEID.SHABONG, (int)Globals.GAMEID.SLOT50LINE, (int)Globals.GAMEID.GAOGEA , (int)Globals.GAMEID.SLOT_9900 };
        if (listGameOther.Contains(curGameId))
        {
            UIManager.instance.gameView.onClickRule();
        }
        else
        {
            //require("Util").onCallWebView(urlRule);
            UIManager.instance.showWebView(urlRule);

        }
    }

    public void onClickSetting()
    {
        SoundManager.instance.soundClick();
        hide();
        UIManager.instance.openSetting();
    }

    public void onClickBack()
    {
        SoundManager.instance.soundClick();
        //if (Globals.Config.curGameId == (int)Globals.GAMEID.SHABONG ||
        //    Globals.Config.curGameId == (int)Globals.GAMEID.KEANG)
        //{
        //    SocketSend.sendExitGame();
        //    var str = "";
        //    if (Globals.Config.isBackGame)
        //    {
        //        //this.gameView.thisPlayer._playerView.icBack.node.active = false;
        //        str = Globals.Config.getTextConfig("minidice_unsign_leave_table");
        //    }
        //    else
        //    {
        //        //this.gameView.thisPlayer._playerView.icBack.node.active = true;
        //        str = Globals.Config.getTextConfig("wait_game_end_to_leave");
        //    }
        //    Globals.Config.isBackGame = !Globals.Config.isBackGame;
        //    if (UIManager.instance.gameView.stateGame == Globals.STATE_GAME.PLAYING)
        //        UIManager.instance.showToast(str);

        //    hide();
        //    return;
        //}

        //if (UIManager.instance.gameView.stateGame != Globals.STATE_GAME.PLAYING)
        //{
        //hide();
        //}
        //else
        //{
        //    string str;
        //    if (!Globals.Config.isBackGame)
        //    {
        //        str = Globals.Config.getTextConfig("wait_game_end_to_leave");
        //        if (Globals.Config.curGameId != (int)Globals.GAMEID.SLOT50LINE)
        //        //&&Globals.Config.curGameId != (int)Globals.GAMEID.SLOT_20_LINE_JP)
        //        {
        //            //if (this.gameView.thisPlayer._playerView !== null)
        //            //    this.gameView.thisPlayer._playerView.icBack.node.active = true;
        //        }
        //    }
        //    else
        //    {
        //        str = Globals.Config.getTextConfig("minidice_unsign_leave_table");
        //        if (Globals.Config.curGameId != (int)Globals.GAMEID.SLOT50LINE)
        //        //&& Globals.Config.curGameId != (int)Globals.GAMEID.SLOT_20_LINE_JP)
        //{
        //            //if (this.gameView.thisPlayer._playerView !== null)
        //            //    this.gameView.thisPlayer._playerView.icBack.node.active = false;
        //        }
        //    }
        //    Globals.Config.isBackGame = !Globals.Config.isBackGame;
        //    UIManager.instance.showToast(str);
        //}
        //hide();

        if (Globals.Config.curGameId == (int)Globals.GAMEID.SLOTNOEL || Globals.Config.curGameId == (int)Globals.GAMEID.SLOTTARZAN) //cac game playnow
        {
            SocketSend.sendExitGame();
            hide();
            return;
        }
        else
        {
            Globals.Logging.Log("Chay vao day!! UIManager.instance.gameView.stateGame=" + UIManager.instance.gameView.stateGame);
            if (UIManager.instance.gameView.stateGame == Globals.STATE_GAME.PLAYING&& UIManager.instance.gameView.players.Count!=1) 
            {
                Globals.Config.isBackGame = !Globals.Config.isBackGame;
                UIManager.instance.gameView.thisPlayer.playerView.setExit(Globals.Config.isBackGame);
                string msg = Globals.Config.isBackGame ? Globals.Config.getTextConfig("wait_game_end_to_leave") : Globals.Config.getTextConfig("minidice_unsign_leave_table");
                UIManager.instance.showToast(msg);

            }
            else//con moi 1 minh minh thi cung cho thoat
            {
                SocketSend.sendExitGame();
            }
            hide();
        }
    }
    public void onClickQuit()
    {
        SoundManager.instance.soundClick();
        onClickBack();
    }
}
