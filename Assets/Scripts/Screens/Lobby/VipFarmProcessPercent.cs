using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using TMPro;
using DG.Tweening;

public class VipFarmProcessPercent : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI txtPercentVipFarm;
    [SerializeField]
    List<Sprite> listFG = new List<Sprite>();
    [SerializeField]
    Image imgFG;

    [SerializeField]
    SkeletonGraphic animBar, animScore;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setInfo(float farmPercent)
    {
        txtPercentVipFarm.text = farmPercent + "%";

        animBar.AnimationState.SetAnimation(0, "bar_run", true);
        animScore.AnimationState.SetAnimation(0, farmPercent >= 100f ? "box_fullscore" : "box_normal", true);
        imgFG.sprite = listFG[farmPercent >= 100f ? 1 : 0];
        //imgFG.fillAmount = farmPercent;
        imgFG.DOFillAmount(farmPercent / 100, 0.3f);

        var pos = animBar.transform.localPosition;
        var sizeW = imgFG.rectTransform.rect.width;
        pos.x = farmPercent/100 * sizeW - sizeW / 2;
        animBar.transform.localPosition = pos;
    }
}
