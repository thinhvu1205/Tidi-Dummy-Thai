using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotNoelItemSpin : ItemSpinController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    public override void setItemData(List<int> arrId)
    {
        listIdIcon = arrId;
        for (int i = 0; i < arrId.Count; i++)
        {

            listSprItem[i].sprite = listSpriteIcon[arrId[i]];
            listSprItem[i].SetNativeSize();
            listSprItem[i].transform.localScale = Vector2.one;
            RectTransform rt = listSprItem[i].GetComponent<RectTransform>();
            if (rt.sizeDelta.x > 120)
            {
                rt.sizeDelta = new Vector2(rt.sizeDelta.x / 1.3f, rt.sizeDelta.y / 1.3f);
            }
        }
    }
    public override void setRandomData(bool isBlur = false)
    {
        for (int i = 0; i < 3; i++)
        {
            //if (isBlur)
            //{
            //    listSprItem[i].sprite = listSpriteIconBlur[Random.Range(0, listSpriteIconBlur.Count)];
            //    listSprItem[i].SetNativeSize();
            //    listSprItem[i].transform.localScale = new Vector2(1.1f, 1.6f);
            //}
            //else
            //{
            listSprItem[i].sprite = listSpriteIcon[Random.Range(0, listSpriteIcon.Count)];
            listSprItem[i].SetNativeSize();
            listSprItem[i].transform.localScale = Vector2.one;
            //}
            RectTransform rt = listSprItem[i].GetComponent<RectTransform>();
            if (rt.sizeDelta.x > 120)
            {
                rt.sizeDelta = new Vector2(rt.sizeDelta.x / 1.3f, rt.sizeDelta.y / 1.3f);
            }
        }
    }

}
