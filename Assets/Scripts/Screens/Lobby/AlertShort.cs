using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;
using DG.Tweening;
using System.Threading.Tasks;

public class AlertShort : MonoBehaviour
{
    // Start is called before the first frame update
    public static AlertShort instance = null;
    [SerializeField]
    TextMeshProUGUI lbNickName;
    [SerializeField]
    TextMeshProUGUI lbContent;
    [SerializeField]
    Avatar avatar;
    [SerializeField]
    GameObject alertShortView;
    [SerializeField]
    RectTransform rectTfParent;
    [SerializeField]
    RectTransform rectTf;
    private Vector2 posInView;
    private Vector2 sizeAlertView;
    private Vector2 posInAlertShortView;
    private float alvWidth = 259.0f;
    public List<GameObject> listData = new List<GameObject>();
    private bool isChangeRotate = false;
    private int tweenId = 0;
    void Start()
    {
        AlertShort.instance = this;

        DOTween.Sequence().AppendCallback(async () =>
        {
            await checkShowAlertShort();
        })
            .AppendInterval(4.5f)
            .SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        checkPosition();

    }
    public void updateChangeOrient()
    {
        alertShortView.SetActive(false);
        DOTween.Kill(alertShortView.transform);
        alertShortView.transform.localPosition = new Vector2(rectTf.rect.width / 2, 0);
    }
    private void checkPosition()
    {

        if (UIManager.instance.isLoginShow())
        {
            alertShortView.SetActive(false);
            return;
        }
        else
        {
            if (Globals.Config.list_AlertShort.Count > 0)
            {
                alertShortView.SetActive(true);
            }
        }

        bool isVertical = true;
        if (UIManager.instance.gameView != null)
        {
            if (Globals.Config.curGameId == (int)Globals.GAMEID.DUMMY
                || Globals.Config.curGameId == (int)Globals.GAMEID.KEANG
                || Globals.Config.curGameId == (int)Globals.GAMEID.SICBO)
            {

                isVertical = false;

            }
            else
            {
                isVertical = true;
            }
        }
        else
        {
            isVertical = true;
        }

        if (TableView.instance != null)
        {
            if (Globals.Config.curGameId == (int)Globals.GAMEID.KEANG || Globals.Config.curGameId == (int)Globals.GAMEID.DUMMY || Globals.Config.curGameId == (int)Globals.GAMEID.SICBO)
            {
                isVertical = false;
            }
            else
            {
                isVertical = true;
            }
            if (ShopView.instance != null)
            {

                isVertical = true;
            }
        }
        if (isVertical)
        {
            posInView = new Vector2(0, rectTfParent.rect.height / 2 - 69 - 150);
            transform.localEulerAngles = Vector3.zero;
            sizeAlertView = new Vector2(rectTfParent.rect.width, 138);
        }
        else
        {
            posInView = new Vector2(rectTfParent.rect.width / 2 - 69 - 150, 0);
            transform.localEulerAngles = new Vector3(0, 0, 270);
            sizeAlertView = new Vector2(rectTfParent.rect.height, 138);
        }

        if (transform.localPosition.x != posInView.x)
        {
            DOTween.Kill(alertShortView.transform);
            isChangeRotate = true;
        }
        transform.localPosition = posInView;
        rectTf.sizeDelta = sizeAlertView;
        if (isChangeRotate)
        {
            alertShortView.transform.localPosition = new Vector2(rectTf.rect.width / 2 + alvWidth, 0);
            isChangeRotate = false;
        }
    }
    public async Task showShortMessage()
    {
        //vip0,vip1 show all(sảnh game,sảnh bàn,ingame)
        //>=vip2 show sảnh bàn,in game
        Debug.Log("showShortMessage:"+ Globals.Config.show_new_alert);
        bool isShowAlert = true;
        if ((Globals.User.userMain.VIP >= 2 && UIManager.instance.lobbyView.getIsShow()) || !Globals.Config.show_new_alert)
        {
            isShowAlert = false;
            Globals.Config.list_AlertShort.Clear();
        }
        if (!isShowAlert)
        {
            return;
        }
        if (Globals.Config.list_AlertShort.Count > 0)
        {
            JObject data = Globals.Config.list_AlertShort[0];
            Globals.Config.list_AlertShort.RemoveAt(0);
            lbNickName.text = (string)data["title"];
            lbContent.text = (string)data["content"];
            string urlAvt = (string)data["urlAvatar"];
            if (urlAvt.Contains("fb."))
            {
                await avatar.loadAvatarAsync2(0, urlAvt);
            }
            else
            {
                avatar.setSpriteWithID(int.Parse(urlAvt));
            }
            avatar.setVip((int)data["vip"]);
            alertShortView.gameObject.SetActive(true);

            alertShortView.transform.localPosition = new Vector2(rectTf.rect.width / 2 + alvWidth, 0);
            DOTween.Sequence()
                  .Append(alertShortView.transform.DOLocalMove(new Vector2(rectTf.rect.width / 2, 0), 0.5f).SetEase(Ease.OutSine))
                  .AppendInterval(5.5f)
                  .Append(alertShortView.transform.DOLocalMove(new Vector2(rectTf.rect.width / 2 + alvWidth, 0), 0.5f)
                  .SetEase(Ease.InSine))
                  .AppendCallback(() =>
                  {
                      alertShortView.gameObject.SetActive(false);
                  }).SetTarget(alertShortView.transform);

        }

    }
    private async Task checkShowAlertShort()
    {
        //Debug.Log("-=-=checkShowAlertShort ");
        if (Globals.Config.list_AlertShort.Count > 0)
        {
            //Debug.Log("-=-=showShortMessage ");
            await showShortMessage();
        }
    }


}
