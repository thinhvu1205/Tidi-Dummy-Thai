using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;
using Newtonsoft.Json.Linq;
using DG.Tweening;

public class VipFarmView : BaseView
{
    public static VipFarmView instance=null;
    [SerializeField]
    TextMeshProUGUI txtMoneyReceive, txtFarmPercent;
    [SerializeField]
    LevelInfo lvNext, lvCurrent;
    [SerializeField]
    GameObject gameObjectArrow, btnReceive;
    [SerializeField]
    List<SkeletonGraphic> animTrees = new List<SkeletonGraphic>();
    [SerializeField]
    GameObject lightTim, lightVang;
   [SerializeField]
    List<SkeletonDataAsset> listAnimTree = new List<SkeletonDataAsset>();

    [SerializeField]
    TextMeshProUGUI txtMoneyReward;

    [SerializeField]
    SkeletonGraphic animReward;
    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    protected override void Start()
    {
        base.Start();
        if (TableView.instance != null&& TableView.instance.isHorizontal)
        {
            transform.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -90);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        txtMoneyReward.transform.parent.gameObject.SetActive(false);
        SocketSend.getFarmInfo();
      
        //JObject data = new JObject();
        //data["farmLevel"] = 9;
        //data["farmPercent"] = 1f;
        //data["currentReward"] = 9000000;
        //data["nextReward"] = 10000000;
        //HandleInfo(data);
    }

    public void HandleInfo(JObject data)
    {
        //{ "farmLevel":10,"farmPercent":0.08,"currentReward":10000000,"nextReward":10000000,"evt":"farmInfo"}
        var farmLevel = (int)data["farmLevel"];
        float farmPercent = (float)data["farmPercent"];
        var currentReward = (int)data["currentReward"];
        var nextReward = (int)data["nextReward"];

        if(farmLevel >= 10)
        {
            lvCurrent.gameObject.SetActive(false);
            gameObjectArrow.SetActive(false);
            lvNext.SetInfo(farmLevel, currentReward, true);
        }
        else
        {
            lvCurrent.gameObject.SetActive(true);
            gameObjectArrow.SetActive(true);
            lvCurrent.SetInfo(farmLevel, currentReward, false);
            lvNext.SetInfo(farmLevel + 1, nextReward, true);
        }

        lightTim.SetActive(farmPercent >= 100);
        lightVang.SetActive(farmPercent >= 100);
        btnReceive.gameObject.SetActive(farmPercent >= 100);

        txtFarmPercent.text = farmPercent + "%";
        txtMoneyReceive.text = Globals.Config.FormatNumber(currentReward);
        var indexPer = 0;
        if (farmPercent <= 25f)
        {
            indexPer = 1;
        }
        else  if (farmPercent > 25f && farmPercent <= 50)
        {
            indexPer = 2;
        }
        else if (farmPercent > 50f && farmPercent <= 75f)
        {
            indexPer = 3;
        }
        else
        {
            indexPer = 4;
        }
        animTrees.ForEach(tree =>
        {
            tree.skeletonDataAsset = listAnimTree[farmLevel - 2];
            tree.Initialize(true);
            tree.AnimationState.SetAnimation(0, "V" + farmLevel + "_" + indexPer, true);
        });
    }


    public void HandleReward(JObject data)
    {
        SocketSend.sendUAG();
        //{ "evt":"farmReward","value":0,"msg":"get reward faild"}
        var value = (int)data["value"];
        if(value > 0)
        {
            txtMoneyReward.transform.parent.gameObject.SetActive(true);
            txtMoneyReward.text = Globals.Config.FormatNumber(value);

            animReward.AnimationState.SetAnimation(0, "animation", false);
            DOTween.Sequence().AppendInterval(3.75f).AppendCallback(()=> {
                txtMoneyReward.transform.parent.gameObject.SetActive(false);
            }).AppendInterval(1.0f).AppendCallback(() => {
                hide();
            });
        }
        else
        {
            UIManager.instance.showMessageBox((string)data["msg"]);
        }
    }


    public void OnClickReceive()
    {
        SoundManager.instance.soundClick();
        SocketSend.getFarmReward();
        //JObject data = new JObject();
        //data["value"] = 10000000;
        //data["msg"] = "get reward successfully!";
        //HandleReward(data);
    }
}
