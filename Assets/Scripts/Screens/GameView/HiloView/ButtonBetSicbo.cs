using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonBetSicbo : MonoBehaviour
{
    [SerializeField]
    public Button btn_Rebet, btn_Double;

    [SerializeField]
    List<Button> listBtnBetChip;

    [SerializeField]
    public GameObject ske_rebet, ske_double;

    [SerializeField]
    public TextMeshProUGUI label_totalBet;

    [SerializeField]
    public TextMeshProUGUI label_clear;

    [SerializeField]
    TMP_FontAsset fontChip;
    HiloView sicboGameView;
    public List<int> listValue = new List<int>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Awake()
    {
        sicboGameView = (HiloView)UIManager.instance.gameView;
        setSprChipBet();
    }
    private void OnEnable()
    {
        CanvasGroup canvasGr = GetComponent<CanvasGroup>();
        canvasGr.alpha = 0;
        for (int i = 0; i < listBtnBetChip.Count; i++)
        {
            listBtnBetChip[i].gameObject.SetActive(false);
        }
        DOTween.Sequence()
            .Append(canvasGr.DOFade(1, 0.5f))
            .AppendCallback(() =>
            {
                List<float> listTimeDelay = new List<float>();
                for (int i = 0; i < listBtnBetChip.Count; i++)
                {
                    listTimeDelay.Add(i * 0.1f);
                }
                int index = 0;
                for (int i = 0; i < listBtnBetChip.Count; i++)
                {
                    DOTween.Sequence()
                    .AppendInterval(listTimeDelay[i])
                    .AppendCallback(() =>
                    {
                        listBtnBetChip[index].gameObject.SetActive(true);
                        DOTween.Sequence()
                        .AppendCallback(() =>
                        {
                            listBtnBetChip[index].interactable = true;
                            DOTween.Sequence()
                            .Append(listBtnBetChip[index].transform.DOScale(new Vector2(1.2f, 1.2f), 0.05f))
                            .Append(listBtnBetChip[index].transform.DOScale(Vector2.one, 0.05f));
                            index++;
                        });
                    });
                }
            })
            .AppendInterval(0.6f)
            .AppendCallback(() =>
            {
                onClickChip(sicboGameView.chipDealLastMatch.ToString());
                sicboGameView.gateBet.setStateButtonBet(false);
            });
        btn_Double.interactable = false;
        //ske_double.gameObject.SetActive(false);
        if (sicboGameView.matchCount == 0)
        {
            btn_Rebet.interactable = false;
            //ske_rebet.gameObject.SetActive(false);
        }
        else
        {
            btn_Rebet.interactable = true;
            //ske_rebet.gameObject.SetActive(true);
        }
        //------ CHECK ON/OFF BTN REBET -----//
        int valueBet = 0;
        for (int i = 0; i < sicboGameView.boxBet.dataBet.Count; i++)
        {
            JObject data = (JObject)sicboGameView.boxBet.dataBet[i];
            valueBet += (int)data["value"];
        }
        if (sicboGameView.thisPlayer.ag < valueBet || sicboGameView.boxBet.dataBet.Count <= 0)
        {
            btn_Rebet.interactable = false;
            //ske_rebet.gameObject.SetActive(false);
        }
    }
    void Update()
    {

    }
    public void onClickDouble()
    {
        sicboGameView.onClickDouble();
        btn_Double.interactable = false;
        //ske_double.gameObject.SetActive(false);
    }
    public void onClickRebet()
    {
        sicboGameView.onClickRebet();
        btn_Rebet.interactable = false;
        ske_rebet.gameObject.SetActive(false);
    }
    public void onClickDeal()
    {
        sicboGameView.onClickDeal();
    }
    public void onClickClear()
    {
        sicboGameView.onClickClear();
    }
    public void onClickChip(string dataChip)
    {

        for (int i = 0; i < listBtnBetChip.Count; i++)
        {
            listBtnBetChip[i].transform.Find("border").gameObject.SetActive(false);
        }
        SoundManager.instance.soundClick();
        int data = int.Parse(dataChip);
        sicboGameView.setValueBtnBet(data);
        setStateButtonChip();
    }
    public void activeDouble()
    {
        btn_Double.interactable = true;
        //ske_double.gameObject.SetActive(true);
        if (sicboGameView.thisPlayer.ag < sicboGameView.totalBet)
        {
            btn_Double.interactable = false;
            //ske_double.gameObject.SetActive(false);
        }
    }
    public void setStateButtonChip()
    {

        var agClickBet = sicboGameView.thisPlayer.ag - sicboGameView.boxBet.totalBoxBet;

        if (agClickBet < sicboGameView.curChipBet) sicboGameView.curChipBet = agClickBet;

        for (int i = 0; i < listBtnBetChip.Count; i++)
        {
            if (agClickBet < listValue[i])
            {
                listBtnBetChip[i].interactable = false;
            }
            else
            {
                listBtnBetChip[i].interactable = true;
            }
        }
        var index = sicboGameView.chipDealLastMatch - 1;
        if (index < 1)
            index = 0;
        listBtnBetChip[index].transform.Find("border").gameObject.SetActive(true);
    }
    public void setStateButtonOnBet()
    {

        for (int i = 0; i < listBtnBetChip.Count; i++)
        {
            if (sicboGameView.thisPlayer.ag < listValue[i])
            {
                listBtnBetChip[i].interactable = false;
            }
            else
            {
                listBtnBetChip[i].interactable = true;
            }
        }
        int index = sicboGameView.chipDealLastMatch - 1;
        if (index < 1)
            index = 0;
        listBtnBetChip[index].transform.Find("border").gameObject.SetActive(true);
    }
    private void setSprChipBet()
    {
        listValue = sicboGameView.listValue;
        for (int i = 0; i < listValue.Count; i++)
        {
            TextMeshProUGUI nodeText = listBtnBetChip[i].transform.GetComponentInChildren<TextMeshProUGUI>();
            nodeText.font = fontChip;
            nodeText.text = Globals.Config.FormatMoney2(listValue[i],true);
            nodeText.transform.localScale = new Vector2(1, 1);
            nodeText.color = Color.black;
        }
    }
}
