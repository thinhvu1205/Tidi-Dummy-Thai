using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
using DG.Tweening;
using System.Threading.Tasks;

public class Avatar : MonoBehaviour
{
    [SerializeField]
    public Image image, imgBorder;

    [SerializeField]
    public Sprite avtDefault;


    [SerializeField]
    public List<Sprite> border;

    public int idAvt = 0;
    //public bool isMe = false;

    public void Awake()
    {

    }

    public async void loadAvatar(int idAva, string fbName, string fbId)
    {
        if (idAva > 0 && idAva <= UIManager.instance.avatarAtlas.spriteCount)
        {
            Debug.Log("loadAvatar:" + idAva);
            setSpriteWithID(idAva);
        }
        else
        {
            //http://graph.facebook.com/%fbID%/picture?type=square
            //var avtLink = "http://graph.facebook.com/%fbID%/picture?type=square";// Globals.Config.avatar_fb == "" ? "http://graph.facebook.com/%fbID%/picture?type=square" : Globals.Config.avatar_fb;
            var avtLink = Globals.Config.avatar_fb == "" ? "http://graph.facebook.com/%fbID%/picture?type=square" : Globals.Config.avatar_fb;
            //avtLink = avtLink.Replace("%fbID%", fbId);
            //avtLink = avtLink.Replace("%token%", Globals.User.userMain.AccessToken);
            Debug.Log("loadAvatar fbId:" + fbId);
            if (Globals.User.AccessToken == "" || (fbId != Globals.User.FacebookID))
            {
                avtLink = avtLink.Replace("&access_token=%token%", "");
            }

            if (fbName.Contains("fb."))
            {
                var idFb = fbName.Substring(3);

                avtLink = avtLink.Replace("%fbID%", idFb);
            }
            else if (fbId != "")
            {
                avtLink = avtLink.Replace("%fbID%", fbId);
            }

            avtLink = avtLink.Replace("%token%", Globals.User.AccessToken);
            Globals.Logging.Log("loadAvatar:" + avtLink);
            image.sprite = await Globals.Config.GetRemoteSprite(avtLink);
        }
    }
    public void loadAvatarAsync(int idAva, string fbName, string fbId = "")
    {
        if (idAva > 0 && idAva <= UIManager.instance.avatarAtlas.spriteCount)
        {
            setSpriteWithID(idAva);
        }
        else
        {
            //http://graph.facebook.com/%fbID%/picture?type=square
            //Globals.Logging.Log("Globals.Config.avatar_fb    " + Globals.Config.avatar_fb);
            //var avtLink = "http://graph.facebook.com/%fbID%/picture?type=square";// Globals.Config.avatar_fb == "" ? "http://graph.facebook.com/%fbID%/picture?type=square" : Globals.Config.avatar_fb;
            var avtLink = Globals.Config.avatar_fb == "" ? "http://graph.facebook.com/%fbID%/picture?type=square" : Globals.Config.avatar_fb;
            //avtLink = avtLink.Replace("%fbID%", fbId);
            //avtLink =avtLink avtLink.Replace("%token%", Globals.User.userMain.AccessToken);

            if (Globals.User.AccessToken == ""||(fbId!=Globals.User.FacebookID))
            {
                avtLink = avtLink.Replace("&access_token=%token%", "");
            }
            if (fbName.Contains("fb."))
            {
                var idFb = fbName.Substring(3);
                //idFb = "42625900944101";
                avtLink = avtLink.Replace("%fbID%", idFb);
            }
            else if (fbId != "")
            {
                avtLink = avtLink.Replace("%fbID%", fbId);
            }
            if (fbId == Globals.User.FacebookID)
            {
                avtLink = avtLink.Replace("%token%", Globals.User.AccessToken);
            }
            Globals.Logging.Log("loadAvatarAsync avtLink  " + avtLink);
            setSpriteWithUrl(avtLink);
        }
    }
    public void setVip(int vip)
    {
        //        v1 = VIP 0 - 1 - 2
        //v2 = VIP 3 - 4
        //v3 = VIP 5 - 6
        //v4 = VIP 7 - 8
        //v5 = VIP 9 - 10
        if (imgBorder == null) return;
        var index = 0;
        if(vip <= 2)
        {
            index = 0;
        }
        else if (vip <= 4)
        {
            index = 1;
        }
        else if (vip <= 6)
        {
            index = 2;
        }
        else if (vip <= 8)
        {
            index = 3;
        }
        else
        {
            index = 4;
        }
        imgBorder.sprite = border[index];
    }
    public void setDefault()
    {
        image.sprite = avtDefault;
    }
    public void setSpriteFrame(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void effectShowAvatar(Sprite sprite) //Keang
    {
        image.sprite = sprite;
        CanvasGroup cvGroupAvt = image.GetComponent<CanvasGroup>();
        cvGroupAvt.alpha = 0;
        cvGroupAvt.DOFade(1, 1.0f);
    }
    public void setSpriteWithID(int idAva)
    {
        var avaSp = UIManager.instance.avatarAtlas.GetSprite("avatar_" + idAva);
        if (avaSp == null)
            avaSp = UIManager.instance.getRandomAvatar();
        setSpriteFrame(avaSp);
    }
    public void setSpriteWithUrl(string url, System.Action funcComplete = null, System.Action funcError = null)
    {
        Globals.Config.loadImgFromUrlAsync(image, url);
    }

    public void setDark(bool isDark)
    {
        image.color = isDark ? Color.gray : Color.white;
    }

    /**
     * Dung cho alert
     */
    public async Task setSpriteWithID2(int idAva)
    {
        await new Task(()=> {
            Debug.Log("-=-= run task");
            setSpriteFrame(UIManager.instance.avatarAtlas.GetSprite("avatar_" + idAva));
        });
    }

    public async Task loadAvatarAsync2(int idAva, string fbName, string fbId = "")
    {
        if (idAva > 0 && idAva <= UIManager.instance.avatarAtlas.spriteCount)
        {
            await setSpriteWithID2(idAva);
        }
        else
        {
            //http://graph.facebook.com/%fbID%/picture?type=square
            //Globals.Logging.Log("Globals.Config.avatar_fb    " + Globals.Config.avatar_fb);
            //var avtLink = "http://graph.facebook.com/%fbID%/picture?type=square";// Globals.Config.avatar_fb == "" ? "http://graph.facebook.com/%fbID%/picture?type=square" : Globals.Config.avatar_fb;
            var avtLink = Globals.Config.avatar_fb == "" ? "http://graph.facebook.com/%fbID%/picture?type=square" : Globals.Config.avatar_fb;
            //avtLink = avtLink.Replace("%fbID%", fbId);
            //avtLink =avtLink avtLink.Replace("%token%", Globals.User.userMain.AccessToken);

            if (Globals.User.AccessToken == "" || (fbId != Globals.User.FacebookID))
            {
                avtLink = avtLink.Replace("&access_token=%token%", "");
            }
            if (fbName.Contains("fb."))
            {
                var idFb = fbName.Substring(3);
                //idFb = "42625900944101";
                avtLink = avtLink.Replace("%fbID%", idFb);
            }
            else if (fbId != "")
            {
                avtLink = avtLink.Replace("%fbID%", fbId);
            }
            if (fbId == Globals.User.FacebookID)
            {
                avtLink = avtLink.Replace("%token%", Globals.User.AccessToken);
            }
            Globals.Logging.Log("loadAvatarAsync avtLink  " + avtLink);
            //setSpriteWithUrl(avtLink);
            await Globals.Config.loadImgFromUrlAsync2(image, avtLink);
        }
    }
}
