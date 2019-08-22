using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViewContolRoot : UiControlBase
{
    public Button mUnShowUp, mUnShowCard,mClose;
    public GameObject mGameShowUp, mGameShowCard, mGameUnShowUp, mGameUnShowCard;
    public CardShowControl mCardShowControl;
    public CardUpdateListControl mUpsShowControl;
    public bool isShowEd = false;
    public override void init()
    {
        
    }

    public override void show()
    {
        Debug.Log("CardViewContolRoot show isInit = "+ isInit);
        if (!isShowEd) {
            mControlType = UiControlManager.TYPE_CARD;
            mCardShowControl.init();
            mCardShowControl.show();
            mUpsShowControl.init();
            mUnShowUp.onClick.AddListener(() =>
            {
                Debug.Log("mUnShowUp click");
                mGameShowCard.transform.localScale = new Vector2(0, 0);
                mGameShowUp.transform.localScale = new Vector2(1, 1);
                mUpsShowControl.show();
                mCardShowControl.disShow();
            });
            mUnShowCard.onClick.AddListener(() =>
            {
                mGameShowCard.transform.localScale = new Vector2(1, 1);
                mGameShowUp.transform.localScale = new Vector2(0, 0);
                mCardShowControl.show();
                mUpsShowControl.disShow();
            });
            isShowEd = true;
        }
        gameObject.transform.localPosition = new Vector2(0, 0);
    }
}
