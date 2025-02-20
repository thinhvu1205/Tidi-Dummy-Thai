using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemShop : MonoBehaviour
{
    JObject dataItem;
    //ShopView shopView;
    System.Action callback;

    [SerializeField]
    Image imgBkg;

    [SerializeField]
    TextMeshProUGUI txtPrice, txtRate, txtBonus, txtAmout;

    [SerializeField]
    Image imgIcon;
    [SerializeField]
    List<Sprite> listSfIcon;

    public void setInfo(JObject _dataItem, int index, System.Action _callback)
    {
        //shopView = _shopView;
        callback = _callback;
        dataItem = _dataItem;

        //"url": "https://pm.topbangkokclub.com/fortumo/?userid=5311&price=52.43",
        //"txtPromo": "1Baht = 3,500 Chips",
        //"txtChip": "183,505 Chips",
        //"txtBuy": "52.43 Baht",
        //"txtBonus": "0%"

        //"url": "7E26B7BB-77C6-5938-AF2B-401DFB79724A.100",
        //"txtPromo": "1USD = 398,800 Chips",
        //"txtChip": "39,876,000 Chips",
        //"txtBuy": "99.99 USD",
        //"txtBonus": "50%",
        //"cost": 100
        var bonus = (string)_dataItem["txtBonus"];
        txtPrice.text = (string)_dataItem["txtChip"];
        txtRate.text = (string)_dataItem["txtPromo"];
        txtBonus.text = bonus.Equals("0%") ? "" : bonus;
        txtBonus.transform.parent.gameObject.SetActive(!txtBonus.text.Equals(""));
        txtAmout.text = (string)_dataItem["txtBuy"];

        //imgBkg.enabled = (index % 2 == 0);

        if (index > listSfIcon.Count - 1)
        {
            imgIcon.sprite = listSfIcon[listSfIcon.Count - 1];
        }
        else
        {
            imgIcon.sprite = listSfIcon[index];
        }
    }

    public void onClickBuy()
    {
        //shopView.onBuy(dataItem);
        Globals.Config.isClickBuyItem = true;
        SoundManager.instance.soundClick();
        callback();
    }
}
