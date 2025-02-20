using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemChatAction : MonoBehaviour
{
    [SerializeField]
    Image img;
    [SerializeField]
    List<Sprite> lsAction = new List<Sprite>();
    [SerializeField]

    SkeletonGraphic skeletonGraphic;
    [SerializeField]
    List<SkeletonDataAsset> lsAnimData = new List<SkeletonDataAsset>();
    public void setData(int idAnimation, bool isAnim)
    {
        if (isAnim)
        {
            img.gameObject.SetActive(false);
            skeletonGraphic.gameObject.SetActive(true);
            Globals.Logging.Log("-=-=idAnimation " + idAnimation);
            skeletonGraphic.skeletonDataAsset = lsAnimData[idAnimation];
            skeletonGraphic.Initialize(true);
            skeletonGraphic.startingAnimation = "animation";
            skeletonGraphic.startingLoop = true;
            skeletonGraphic.AnimationState.Complete += delegate
            {
                Destroy(gameObject);
            };

            string sound = "";
            switch (idAnimation)
            {
                case 0:
                    sound = Globals.SOUND_CHAT.BOOM;
                    break;
                case 1:
                    sound = Globals.SOUND_CHAT.KISS;
                    break;
                case 2:
                    sound = Globals.SOUND_CHAT.ROSE;
                    break;
                case 3:
                    sound = Globals.SOUND_CHAT.BEER;
                    break;
                case 4:
                    sound = Globals.SOUND_CHAT.TOMATO;
                    break;
                case 5:
                    sound = Globals.SOUND_CHAT.WATER;
                    break;
            }
            SoundManager.instance.playEffectFromPath(sound);
        }
        else
        {
            img.gameObject.SetActive(true);
            skeletonGraphic.gameObject.SetActive(false);
            img.sprite = lsAction[idAnimation];
            img.SetNativeSize();
        }
    }
}