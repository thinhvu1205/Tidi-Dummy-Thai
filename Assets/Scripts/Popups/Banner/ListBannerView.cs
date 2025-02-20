using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ListBannerView : BaseView
{
    [SerializeField]
    HorizontalScrollSnap scrollSnapView;
    [SerializeField]
    GameObject bannerTemp;

    [SerializeField]
    GameObject PaginationTemp;

    [SerializeField]
    Transform PaginationParent;

    protected override void Start()
    {
        Globals.CURRENT_VIEW.setCurView(Globals.CURRENT_VIEW.NEWS_VIEW);
        loadListBanner();
        //rawImg.texture = await Globals.Config.GetRemoteTexture("https://cdn.jakartagames.net/api/public/dl/sdfs45d/T12VIP01/31.png");
        //rawImg.SetNativeSize();
    }

    void loadListBanner()
    {
        Globals.Logging.Log("-=-= arrOnlistTrue.Count-----------------" + Globals.Config.arrOnlistTrue.Count);
        var parrent = scrollSnapView.GetComponent<ScrollRect>().content;
        for (var i = 0; i < Globals.Config.arrOnlistTrue.Count; i++)
        {
            var dataBanner = (JObject)Globals.Config.arrOnlistTrue[i];
            dataBanner["isClose"] = false;
            var urlImg = (string)dataBanner["urlImg"];

            //Texture2D texture = await Globals.Config.GetRemoteTexture(urlImg, true);
            //if (texture == null) return;
            var pagee = Instantiate(PaginationTemp, PaginationParent);
            pagee.gameObject.SetActive(true);
            var nodeBanner = Instantiate(bannerTemp).GetComponent<BannerView>();
            scrollSnapView.AddChild(nodeBanner.gameObject);
            nodeBanner.transform.localScale = Vector3.one;
            nodeBanner.setInfo(dataBanner, false, () =>
            {
                hide();
            });

        }
    }
}
