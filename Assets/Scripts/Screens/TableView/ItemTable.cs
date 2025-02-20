using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTable : MonoBehaviour
{
    [SerializeField]
    List<Image> listPlayer;

    [SerializeField]
    List<Sprite> listSpPlayer;

    [SerializeField]
    TextMeshProUGUI txtAg;
    [SerializeField]
    TextMeshProUGUI txtName;
    [SerializeField]
    TextMeshProUGUI txtID;
    //[SerializeField]
    //Button btnJoin;
    [SerializeField]
    GameObject objFull;
    System.Action callback;
    JObject dataItem;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setInfo(JObject _dataItem, System.Action _callback)
    {
        dataItem = _dataItem;
        callback = _callback;
        int sizeTable = (int)dataItem["size"];
        for (var i = 0; i < listPlayer.Count; i++)
        {
            //Globals.Logging.Log("-= player  " + (int)dataItem["player"]);
            if (i <= (int)dataItem["player"] - 1)
            {
                listPlayer[i].sprite = listSpPlayer[1];
            }
            else
            {
                listPlayer[i].sprite = listSpPlayer[0];
            }

            listPlayer[i].gameObject.SetActive(!(i >= sizeTable));
            listPlayer[i].SetNativeSize();
        }
        objFull.SetActive((int)dataItem["player"] == (int)dataItem["size"]);
        txtAg.text = Globals.Config.FormatMoney((long)dataItem["mark"], true);
        //txtName.text = (string)dataItem["N"];
        JArray arrN = (JArray)_dataItem["ArrName"];
        List<string> arrName = arrN.ToObject<List<string>>();
        string tableName = "";
        foreach (string name in arrName)
        {
            string tbName = name;
            if (name.Length > 10)
            {
                tbName = name.Substring(0, 7) + "...,";
            }
            tableName += tbName;
        }
        txtName.text = tableName;
        txtID.text = (int)dataItem["id"] + "";
    }

    public void onClickJoin()
    {
        if (callback != null)
        {
            callback.Invoke();
        }
    }
}
