using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardUiControl : MonoBehaviour {

    public static int TYPE_CARD_ITME = 1;
    public static int TYPE_CARD_PLAY = 2;


    private Image mImageBottom;
    public SkillJsonBean mSkill;
    private Image mImageTop;
    public CardJsonBean mCard;
    public Text mCostCount, mSkillDec, mSkillName;
    CalculatorUtil calcuator;
    Attacker firer;
    Button mBt;
    string dec;
    public void init(long cardId,int type)
    {
        if (mCard == null) {
            CardJsonBean card = JsonUtils.getIntance().getCardInfoById(cardId);
            mCard = card;
        }
        if (mSkill == null) {
            mSkill = JsonUtils.getIntance().getSkillInfoById(mCard.skill_id);
        }
        if (mImageBottom == null) {
            mImageBottom = GetComponentsInChildren<Image>()[1];
            Sprite sprite1 = Resources.Load("UI/" + mCard.center_resource, typeof(Sprite)) as Sprite;
            mImageBottom.sprite = sprite1;
        }
        if(mImageTop == null) {
            mImageTop = GetComponentsInChildren<Image>()[2];
            Sprite sprite2 = Resources.Load("UI/" + mCard.top_resource, typeof(Sprite)) as Sprite;
            mImageTop.sprite = sprite2;
        }
        if (mCostCount == null) {
            Text[] listText = GetComponentsInChildren<Text>();
            mCostCount = listText[0];
            mCostCount.text = mCard.cost + "";
            mSkillDec = listText[1];
            mSkillName = listText[2];
            mSkillName.text = mCard.name;
            
        }
        if (calcuator == null)
        {
            calcuator = new CalculatorUtil(mSkill.calculator, mSkill.effects_parameter);
        }
        if (mBt == null) {
            mBt = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Button>();
            if (type == TYPE_CARD_ITME)
            {
                mBt.onClick.AddListener(() =>
                {
                    Debug.Log("onClick onClick onClick");
                    PlayerBackpackBean newBean = new PlayerBackpackBean();
                    newBean.goodId = mCard.id;
                    newBean.sortID = mCard.sortID;
                    newBean.count = 1;
                    newBean.tabId = mCard.tabid;
                    BackpackManager.getIntance().showTipUi(newBean, 1, TipControl.UNUSE_CARD_TYPE);
                });

            }
            else {
                mBt.GetComponent<Button>().enabled = false;
            }
           
        }
        update();
    }
    private void onClick()
    {
        Debug.Log("onClick onClick onClick");
        PlayerBackpackBean newBean = new PlayerBackpackBean();
        newBean.goodId = mCard.id;
        newBean.sortID = mCard.sortID;
        newBean.count = 1;
        newBean.tabId = mCard.tabid;
        BackpackManager.getIntance().showTipUi(newBean, 1, TipControl.UNUSE_CARD_TYPE);
       // BackpackManager.getIntance().setShowDate(mCard.name, mCard.describe, 1, TipControl.USE_CARD_TYPE,mCard.id,"");
    }
    public void update()
    {
        dec = mCard.describe;
        if (dec != null && dec.Contains("&n"))
        {   
            float value = calcuator.getValue(firer, null);
            dec = dec.Replace("\"n\"", "" + value);
            mSkillDec.text = dec;
        }
    }
}
