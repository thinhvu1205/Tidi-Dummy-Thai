using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using DG.Tweening;
using Globals;
using Newtonsoft.Json.Linq;
public class
    ProfileView : BaseView
{
    // Start is called before the first frame update
    public static ProfileView instance;
    [SerializeField]
    Avatar _avatar;

    [SerializeField]
    TextMeshProUGUI lbName, lbChips, lbAgSafe, lbId;

    [SerializeField]
    Button btnSendGift;
    [SerializeField]
    VipContainer VipContainer;

    [SerializeField]
    ScrollRect scrListAvatar;

    [SerializeField]
    TMP_InputFieldWithEmoji edbStatus;


    [SerializeField]
    GameObject AvatarPr;


    [SerializeField]
    GameObject btnChangeName, btnRegister, btnChangePass;

    [SerializeField]
    GameObject maskName, refContainer;

    [SerializeField]
    TMP_InputField edbRef;

    [SerializeField]
    Button btnConfirmRef;

    [SerializeField]
    TextMeshProUGUI lbTimeRemainRef;
    private long currentTime = 0;

    private bool isChangeStatus = false;
    private int currentAvatarId = 0;
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    protected override void Start()
    {
        base.Start();
        currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        setInfo();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}
    public void setInfo()
    {
        lbName.text = User.userMain.displayName;
        Config.effectTextRunInMask(lbName);
        lbChips.text = Config.FormatNumber(User.userMain.AG);
        lbAgSafe.text = Config.FormatNumber(User.userMain.agSafe);
        lbId.text = "ID: " + User.userMain.Userid.ToString();
        currentAvatarId = User.userMain.Avatar;
        _avatar.loadAvatar(User.userMain.Avatar, User.userMain.Username, User.FacebookID);
        VipContainer.setVip(User.userMain.VIP);
        _avatar.setVip(User.userMain.VIP);
        loadListAvatar();
        btnSendGift.gameObject.SetActive(Config.ketT);
        SocketSend.getStatus();

        lbAgSafe.transform.parent.gameObject.SetActive(Config.ket);
        updateStateChangeName();
        btnConfirmRef.gameObject.SetActive(User.userMain.canInputInvite);
        if (User.userMain.timeInputInvite > 0)
        {
            refContainer.SetActive(true);
            countTimeRemainRef();
            edbRef.interactable = true;
        }
        else
        {
            if (User.userMain.uidInvite != 0)
            {
                refContainer.SetActive(true);
                edbRef.text = User.userMain.uidInvite.ToString();
                edbRef.interactable = false;
            }
            else
            {
                refContainer.SetActive(false);
            }
            lbTimeRemainRef.text = "";
            DOTween.Kill(DOTWEEN_TAG.PROFILE_COUNTTIME);
        }
    }
    private void countTimeRemainRef()
    {
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                currentTime += 1000;
                long deltaTime = User.userMain.timeInputInviteRemain - currentTime;
                lbTimeRemainRef.text = Config.convertSeccondToDDHHMMSS((int)deltaTime / 1000);
                if (currentTime < 1000)
                {
                    DOTween.Kill(DOTWEEN_TAG.PROFILE_COUNTTIME);
                    refContainer.SetActive(false);
                }
            })
            .AppendInterval(1.0f)
            .SetLoops(-1)
            .SetId(1911);
    }
    public void onAddFriendIdRefSuccess()
    {
        DOTween.Kill(DOTWEEN_TAG.PROFILE_COUNTTIME);
        User.userMain.timeInputInvite = 0;
        User.userMain.timeInputInviteRemain = 0;
        User.userMain.uidInvite = Config.splitToInt(edbRef.text);
        btnConfirmRef.gameObject.SetActive(false);
        edbRef.interactable = false;
        setInfo();

    }
    public void updateStateChangeName()
    {
        if (Config.typeLogin == LOGIN_TYPE.FACEBOOK
            || Config.typeLogin == LOGIN_TYPE.APPLE_ID
            || !User.userMain.Username.ToUpper().Contains("TE."))
        {
            btnChangeName.SetActive(false); //ko doi ten
          
            maskName.GetComponent<RectTransform>().sizeDelta = new Vector2(328, 62);// new Vector2(maskName.GetComponent<RectTransform>().sizeDelta.x + 65, maskName.GetComponent<RectTransform>().sizeDelta.y);
            maskName.transform.localPosition = new Vector2(-15, 0);
            if (!User.userMain.Username.ToUpper().Contains("TE."))
            {
                btnChangePass.SetActive(true);
                btnRegister.SetActive(false);
            }
            if (Config.typeLogin == LOGIN_TYPE.FACEBOOK)
            {
                btnChangePass.SetActive(false);//ko doi pass
            }
        }
        else
        {
            if (Config.typeLogin == LOGIN_TYPE.PLAYNOW && User.userMain.Username.ToUpper().Contains("TE."))
            {
                btnChangePass.SetActive(false);
                btnRegister.SetActive(true);
            }
            else
            {
                btnChangePass.SetActive(true);
                btnRegister.SetActive(false);
            }
            btnChangeName.SetActive(true);
            maskName.GetComponent<RectTransform>().sizeDelta = new Vector2(271, 62);
            maskName.transform.localPosition = new Vector2(-15, 0);
        }
    }
    public void updateAg()
    {
        lbChips.text = Config.FormatNumber(User.userMain.AG);

    }
    public void updateStatus(string status)
    {
        if (status.Contains("คุณกำลังคิดอะไรอยู่"))
        {
            edbStatus.placeholder.GetComponent<TextMeshProUGUI>().text = status;
        }
        else
            edbStatus.text = status;

        if (isChangeStatus)
        {
            //UIManager.instance.showToast("Change status successfully!");
            isChangeStatus = false;
        }

    }
    public void onClickRuleRefence()
    {
        if (Config.url_rule_refGuide != "")
        {
            UIManager.instance.showWebView(Config.url_rule_refGuide, Config.getTextConfig("txtrule"));
        }
    }
    private void loadListAvatar()
    {
        for (int i = 0; i < 36; i++)
        {
            GameObject avatarItem;
            if (i < scrListAvatar.content.childCount)
            {
                avatarItem = scrListAvatar.content.GetChild(i).gameObject;
            }
            else
            {
                avatarItem = Instantiate(AvatarPr);
                avatarItem.transform.SetParent(scrListAvatar.content);
                avatarItem.transform.localScale = AvatarPr.transform.localScale;
            }
            avatarItem.SetActive(true);
            avatarItem.GetComponent<Avatar>().setSpriteWithID((i + 1));
            avatarItem.GetComponent<Avatar>().idAvt = (i + 1);
            avatarItem.GetComponent<Avatar>().setVip(Globals.User.userMain.VIP);
        }
    }
    public void onClickQuit()
    {
        SoundManager.instance.soundClick();
        Application.Quit();
    }
    public void onClickLogout()
    {
        SoundManager.instance.soundClick();
        SocketSend.sendLogOut();
        Config.typeLogin = LOGIN_TYPE.NONE;
        PlayerPrefs.SetInt("type_login", (int)LOGIN_TYPE.NONE);
        PlayerPrefs.Save();
        UIManager.instance.showLoginScreen(false);
        SocketIOManager.getInstance().emitSIOCCCNew("ClickLogOut");
    }
    public void onClickChangeAvatar(Avatar avatarItem)
    {
        SoundManager.instance.soundClick();
        SocketSend.changeAvatar(avatarItem.idAvt);
        currentAvatarId = avatarItem.idAvt;
    }
    public void onChangeAvatar()
    {
        User.userMain.Avatar = currentAvatarId;
        _avatar.loadAvatar(User.userMain.Avatar, User.userMain.Username, "");
    }
    public void onClickChangePass()
    {
        SoundManager.instance.soundClick();
        UIManager.instance.openChangePass();
    }

    public void onClickChangeName()
    {
        SoundManager.instance.soundClick();
        UIManager.instance.openChangePass();
    }
    public void onClickSendGift()
    {
        SoundManager.instance.soundClick();
        UIManager.instance.openSendGift();
        onClickClose();
    }
    public void onClickChangeStatus()
    {
        SoundManager.instance.soundClick();
        isChangeStatus = true;
        SocketSend.changeStatus(edbStatus.text);
    }
    public void onClickAddChips()
    {
        SoundManager.instance.soundClick();
        UIManager.instance.openShop();
        this.onClickClose();
    }
    public void onOpenSafe()
    {
        SoundManager.instance.soundClick();
        UIManager.instance.openSafeView();
        this.onClickClose();
    }
    public void ondEdbStatusChange()
    {
        if (edbStatus.text != "")
        {
            SocketSend.changeStatus(edbStatus.text);
        }
    }
    public void onClickConfirmRef()
    {
        if (edbRef.text != "")
        {
            SocketSend.sendIdReferFriend(int.Parse(edbRef.text));
            UIManager.instance.showWatting();
        }
    }
}
