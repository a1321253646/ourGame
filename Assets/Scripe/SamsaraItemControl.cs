using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamsaraItemControl : MonoBehaviour {
    public long mId;
    private long mLevel;
    private SamsaraJsonBean mJsonBean;
    private Image mIcon,mNoStudy,mCloseImg;
    private Text mSamsaraName;
    private Text mSamsaraValue;
    private Text mLvelUpCost;
    private Text mLvel;
    private Text mButtonText;
    private Button mLevelUp,mClose;
    private SamSaraListControl mListControl;
    private bool isClose = false;


    public bool init(long id, SamSaraListControl control) {
        mId = id;
        mListControl = control;
        mJsonBean =  JsonUtils.getIntance().getSamsaraInfoById(mId);
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        mCloseImg = images[images.Length - 1];

        mNoStudy = images[4];
        mIcon = gameObject.GetComponentsInChildren<Image>()[3];
        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        mSamsaraName = texts[0];
        mSamsaraValue = texts[1];
        mLvelUpCost = texts[3];
        mLvel = texts[4];
        mButtonText = texts[2];
        mLevelUp = GetComponentsInChildren<Button>()[0];
        Sprite sprite = Resources.Load("icon/samsara/" + mJsonBean.icon, typeof(Sprite)) as Sprite;
        mIcon.sprite = sprite;
        mLevelUp.onClick.AddListener(() => {
          levelUp();
        });
        mClose = GetComponentsInChildren<Button>()[1];
        if (mId == 13 || mId == 14)
        {
            mCloseImg.transform.localScale = new Vector2(1, 1);

            if (mId == 13)
            {
                isClose = SQLHelper.getIntance().isCloseYueqiang == 1;
            }
            else if (mId == 14)
            {
                isClose = SQLHelper.getIntance().isCloseChuangye == 1;
            }
            if (isClose)
            {
                mCloseImg.sprite = Resources.Load("UI_yellow/lunhui/11", typeof(Sprite)) as Sprite;
            }
            else
            {
                mCloseImg.sprite = Resources.Load("UI_yellow/lunhui/12", typeof(Sprite)) as Sprite;
            }
        }


        mClose.onClick.AddListener(() => {
            if (isClose)
            {
                isClose = false;
                mCloseImg.sprite = Resources.Load("UI_yellow/lunhui/12", typeof(Sprite)) as Sprite;
            }
            else {
                isClose = true;
                mCloseImg.sprite = Resources.Load("UI_yellow/lunhui/11", typeof(Sprite)) as Sprite;
            }
            if (mId == 13)
            {
                SQLHelper.getIntance().updateIsCloseYueqiang(isClose);
            }
            else if (mId == 14)
            {
                SQLHelper.getIntance().updateIsCloseChuangyue(isClose);
            }
        });
        return upDate();
    }

    private void levelUp()
    {
        Debug.Log(" SamsaraItemControl levelUp " + mId);
        GameManager.getIntance().mReincarnation = BigNumber.minus(GameManager.getIntance().mReincarnation, JsonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, BaseDateHelper.decodeLong( mLevel) + 1));
        SQLHelper.getIntance().updateLunhuiValue(GameManager.getIntance().mReincarnation);
        mListControl.upDate(mId);
    }

    public bool upDate()
    {
        mLevel = InventoryHalper.getIntance().getSamsaraLevelById(mId);
//        Debug.Log("---------------------------------InventoryHalper.getIntance().getSamsaraLevelById(mId) = " + InventoryHalper.getIntance().getSamsaraLevelById(mId));
        if (BaseDateHelper.decodeLong(mLevel)  == 0)
        {

            mNoStudy.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mNoStudy.transform.localScale = new Vector2(0, 0);
        }
//        Debug.Log("---------------------------------BaseDateHelper.decodeLong(mLevel) = " + BaseDateHelper.decodeLong(mLevel));
        if (BaseDateHelper.decodeLong(mLevel) == 0)
        {
            mSamsaraName.text = "" + mJsonBean.name ;
            string str = "学习效果\n";
            str = str + getAttribute(1);
//            Debug.Log(str);
            mLvel.text = "" ;
            mSamsaraValue.text = str;
        }
        else {
            mSamsaraName.text = "" + mJsonBean.name;
            mLvel.text = "Lv:" + BaseDateHelper.decodeLong(mLevel);
            mSamsaraValue.text = getAttribute();
        }
        isEnableLevelUp();
        BigNumber big = JsonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, BaseDateHelper.decodeLong(mLevel) + 1);
//        Debug.Log("sonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, BaseDateHelper.decodeLong(mLevel) + 1) = " + big.toString());
//        Debug.Log("sonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, BaseDateHelper.decodeLong(mLevel) + 1) = " + big.isEmpty());

        if (big.isEmpty()) {
            mLvelUpCost.text = "---";
        }
        else {
            mLvelUpCost.text = "消耗：" + big.toStringWithUnit() + "轮回点";

        }
        if (mId == 12) {
            GameManager.getIntance().updateBossGase(true);
        }else if(mId == 7){
            GameObject.Find("Card2").GetComponent<CardShowControl>().upDataCardCount();
        }
        isEnableLevelUp();
        if(mId == 13 && mLevel == BaseDateHelper.encodeLong(0) && BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel)  < 1000) {
            return false;
        }
        return true;
    }

    public void isEnableLevelUp() {
        BigNumber rein =  GameManager.getIntance().mReincarnation;
        if (JsonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, BaseDateHelper.decodeLong(mLevel) + 1).isEmpty()) {
            mLevelUp.interactable = false;
            return;
        }
        
        if (GameManager.getIntance().mReincarnation.ieEquit(JsonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, BaseDateHelper.decodeLong(mLevel) + 1)) != -1)
        {
            mLevelUp.interactable = true;
        }
        else {
            mLevelUp.interactable = false;
        }
    }

    private string getAttribute() {
        return getAttribute(BaseDateHelper.decodeLong(mLevel));
    }

    private string getAttribute(long level) {
        List<SamsaraValueBean>  list = JsonUtils.getIntance().getSamsaraVulueInfoByIdAndLevel(mId, level);
        Debug.Log("====================================================================getAttribute id= " + mId + " level= " + level);
        foreach (SamsaraValueBean ssv in list)
        {
            Debug.Log("type = " + ssv.type + " value = " + ssv.value);
        }
        string text = "";

        for (int i = 0 ; i < list.Count;i++ ) {
            SamsaraValueBean bean = list[i];

            if (bean.type == 100)
            {
                text += "攻击: " + bean.value;
            }
            else if (bean.type == 101)
            {
                text += "防御: " + bean.value;
            }
            else if (bean.type == 102)
            {
                text += "生命: " + bean.value;
            }
            else if (bean.type == 110)
            {
                text += "命中: " + bean.value;
            }
            else if (bean.type == 111)
            {
                text += "闪避: " + bean.value;
            }
            else if (bean.type == 112)
            {
                text += "暴击: " + bean.value;
            }
            else if (bean.type == 113)
            {
                text += "暴击伤害: " + bean.value;
            }
            else if (bean.type == 114)
            {
                text += "攻速: " + bean.value;
            }
            else if (bean.type > 400000)
            {
                AffixJsonBean aj = JsonUtils.getIntance().getAffixInfoById(bean.type);
                if (bean.type == 500005 || bean.type == 500010 || bean.type == 500011)
                {
                    text += (aj.dec + ":" + bean.value );
                }
                else{
                    text += (aj.dec + ":" + (bean.value / 100f) + "%");
                }
                
//                Debug.Log(text);
            }

            if (i < list.Count - 1) {
                text += "\n";
            }
        }
//        Debug.Log(text);
        return text;
    }
}
