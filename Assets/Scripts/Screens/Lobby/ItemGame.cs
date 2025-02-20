using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemGame : MonoBehaviour
{
    [SerializeField]
    SkeletonGraphic skeletonGraphic;
    [SerializeField]
    Image icon;

    [SerializeField]
    TextMeshProUGUI lb_game;

    [HideInInspector]
    public int gameID;
    System.Action callbackClick = null;
    public void setInfo(int _gameID, SkeletonDataAsset skeAnim, Sprite _icon, Material material, System.Action callback)
    {
        gameID = _gameID;
        callbackClick = callback;
        if (skeAnim != null)
        {
            skeletonGraphic.gameObject.SetActive(true);
            //icon.gameObject.SetActive(false);
            skeletonGraphic.skeletonDataAsset = skeAnim;
            skeletonGraphic.material = material;
            var ab = skeAnim.GetSkeletonData(false).Animations.ToArray();
            var nameAnim = ab[ab.Length - 1].Name;
            //Globals.Logging.Log("-=-= " + gameID + " anim " + nameAnim);

            skeletonGraphic.Initialize(true);
            skeletonGraphic.startingAnimation = nameAnim;
            skeletonGraphic.AnimationState.SetAnimation(0, nameAnim, true);
        }
        else
        {
            icon.gameObject.SetActive(true);
            skeletonGraphic.skeletonDataAsset = null;
            skeletonGraphic.gameObject.SetActive(false);
            icon.sprite = _icon;
            icon.SetNativeSize();
            GetComponent<RectTransform>().sizeDelta = icon.rectTransform.sizeDelta;
        }
    }

    public void onClick()
    {
        if (callbackClick != null)
        {
            callbackClick.Invoke();
        }
    }
}
