using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUpUiControl : MonoBehaviour
{
    public Text mLevelTx;
    public Button mUpdateBt;
    public Text mUpCost;



    public CardUiControl control1 = null;
    public ItemOnDragUpCard mItemUp = null;
    public long mId = -1;
    long mLevel = 0;
    CardJsonBean mbean = null;
    YongjiuCardBean mYongjiuBean = null;
    bool mIsYongjiu = false;
    long mCost = 0;
    public bool isHave = false;
    public void init(long id, bool isYongjiu)
    {
        mId = id;
        control1.init(id, 107, 146);
        mItemUp.init(id);
        mIsYongjiu = isYongjiu;
        if (isYongjiu)
        {
            mYongjiuBean = JsonUtils.getIntance().getYongjiuCardInfoById(mId);
        }
        else
        {
            mbean = JsonUtils.getIntance().getCardInfoById(mId);
        }
        control1.init(id, CardUiControl.TYPE_CARD_PLAY, null);
        UpdateUi();
    }
    public void UpdateUi()
    {
        if (mId == -1)
        {
            return;
        }
        long level = mLevel;
        mLevel = SQLHelper.getIntance().getCardLevel(mId);
        if (mLevel > 0)
        {
            mLevelTx.text = "Lv." + mLevel;
           
        }
        else
        {
            mLevelTx.text = "";
        }
        
        long eachCost = mIsYongjiu ? mYongjiuBean.card_up_cost : mbean.card_update_cost;
        mCost = eachCost * mLevel;
        mUpCost.text = BigNumber.getBigNumForString(mCost + "").toStringWithUnit();

        if (SQLHelper.getIntance().mCardMoney >= mCost)
        {
            mUpdateBt.interactable = true;
            mUpCost.color = new Color(255, 249, 240, 255);
         
        }
        else
        {
            mUpdateBt.interactable = false;
            mUpCost.color = Color.red;
        }
        if (mLevel > -1)
        {
            isHave = true;
        }
        Debug.Log("=================================================================CardUpUiControl mId = " + mId + " isHave=" + isHave);
        if (!isHave)
        {
            /*           foreach (PlayerBackpackBean card in SQLHelper.getIntance().getAllGood())
                       {
                           if (card.goodId == mId)
                           {
                               isHave = true;
                               break;
                           }
                       }*/
            setMaterial(!isHave);
        }
        

    }

    public void clickUp()
    {
        SQLHelper.getIntance().mCardMoney -= mCost;
        SQLHelper.getIntance().updateCardMoney(SQLHelper.getIntance().mCardMoney);
        mLevel++;
        SQLHelper.getIntance().changeCardLeveL(mId, mLevel);
        UpdateUi();
        GameObject.Find("card_up_list").GetComponent<CardUpdateListControl>().upDateUi(-1);
    }

    private void setMaterial(bool isGrey)
    {
        Debug.Log("================================================CardUpUiControl setMaterial setMaterial=isGrey" + isGrey + " mId=" + mId);
        // Image[] imgs = GetComponentsInChildren<Image>();
        // foreach (Image im in imgs) {
        if (isGrey)
        {
            //    int uilevel = im.transform.GetSiblingIndex();
            //    im.material = mGreyMeteril;
            //    im.transform.SetSiblingIndex(uilevel);

            mUpdateBt.interactable = false;
            mUpCost.text = "未激活";
            mUpCost.color = Color.red;
       

        }
        else
        {
            mUpCost.color = new Color(255, 249, 240, 255);
        }
        //}
    }

}
