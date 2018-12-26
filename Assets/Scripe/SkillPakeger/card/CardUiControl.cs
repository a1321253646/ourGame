using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardUiControl : MonoBehaviour {

    public static int TYPE_CARD_ITME = 1;
    public static int TYPE_CARD_GOOD = 3;
    public static int TYPE_CARD_PLAY = 2;
    private Image mImageBottom;
    public SkillJsonBean mSkill;
    public CardJsonBean mCard;
    public long mCardId = -1;
    public Text mCostCount,mSkillName,mCardCount;
    CalculatorUtil calcuator;
    Attacker firer;
    Button mBt;
    string dec;

    public int count = 1;
    public void addCount() {
        count++;
        mCardCount.text = "X" + count;
    }

    public void init(long cardId,int type, Attacker hero)
    {
        count = 1;
        mCardId = cardId;
        if (cardId == -1)
        {
            showBack();
            return;
        }
        else{
            showCard();
        }
        
        firer = hero;
        CardJsonBean card1 = JsonUtils.getIntance().getCardInfoById(cardId);
        mCard = card1;
         mSkill = JsonUtils.getIntance().getSkillInfoById(mCard.skill_id);
        if (mImageBottom == null) {
            mImageBottom = GetComponentsInChildren<Image>()[3];
        }
        Sprite sprite1 = Resources.Load("icon/card/" + mCard.center_resource+"_1", typeof(Sprite)) as Sprite;
        mImageBottom.sprite = sprite1;
      
        if (mCostCount == null) {
            Text[] listText = GetComponentsInChildren<Text>();
            mCardCount = listText[2];
            mSkillName = listText[1];
            mCostCount = listText[0];
          
        }
           
        mCostCount.text = mCard.cost + "";           
        mSkillName.text = mCard.name;
    }
    private Vector3 mOldScale;
    public void init(long cardId, float x, float y)
    {
        count = 1;
        float x1 = transform.GetChild(0).gameObject.GetComponent<RectTransform>().rect.width;
        float y1 = transform.GetChild(0).gameObject.gameObject.GetComponent<RectTransform>().rect.height;
        //        Debug.Log("x= " + x + " y= " + y + " x1= " + x1 + " y1=" + y1 + " Bilix=" + (x / x1) + " Biliy=" + (y / y1));
        mOldScale = new Vector3(x / x1, y / y1, 1);
        transform.GetChild(0).localScale = mOldScale;
        transform.GetChild(1).localScale = mOldScale;

    }

    public void showBack() {
        gameObject.transform.GetChild(0).localScale = mOldScale;
        gameObject.transform.GetChild(1).localScale = new Vector2(0, 0);
    }

    public void showCard() {
        gameObject.transform.GetChild(1).localScale = mOldScale;
        gameObject.transform.GetChild(0).localScale = new Vector2(0, 0);
    }
}
