using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamsaraItemControl : MonoBehaviour {
    public long mId;
    private long mLevel;
    private SamsaraJsonBean mJsonBean;
    private Image mIcon,mNoStudy;
    private Text mSamsaraName;
    private Text mSamsaraValue;
    private Text mLvelUpCost;
    private Text mLvel;
    private Text mButtonText;
    private Button mLevelUp;
    private SamSaraListControl mListControl;
    public void init(long id, SamSaraListControl control) {
        mId = id;
        mListControl = control;
        mJsonBean =  JsonUtils.getIntance().getSamsaraInfoById(mId);
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        mNoStudy = images[4];
        mIcon = gameObject.GetComponentsInChildren<Image>()[3];
        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        mSamsaraName = texts[0];
        mSamsaraValue = texts[1];
        mLvelUpCost = texts[3];
        mLvel = texts[4];
        mButtonText = texts[2];
        mLevelUp = GetComponentsInChildren<Button>()[1];
        Sprite sprite = Resources.Load("icon/samsara/" + mJsonBean.icon, typeof(Sprite)) as Sprite;
        mIcon.sprite = sprite;
        mLevelUp.onClick.AddListener(() => {
          levelUp();
        });
        upDate();
    }

    private void levelUp()
    {
        Debug.Log(" SamsaraItemControl levelUp " + mId);
        GameManager.getIntance().mReincarnation = BigNumber.minus(GameManager.getIntance().mReincarnation, JsonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, mLevel + 1));
        SQLHelper.getIntance().updateLunhuiValue(GameManager.getIntance().mReincarnation);
        mListControl.upDate(mId);
    }

    public void upDate()
    {
        mLevel = InventoryHalper.getIntance().getSamsaraLevelById(mId);
        if (mLevel == 0)
        {

            mNoStudy.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            mNoStudy.transform.localScale = new Vector2(0, 0);
        }
        
        if (mLevel == 0)
        {
            mSamsaraName.text = "" + mJsonBean.name ;
            string str = "学习效果\n";
            str = str + getAttribute(1);
            Debug.Log(str);
            mLvel.text = "" ;
            mSamsaraValue.text = str;
        }
        else {
            mSamsaraName.text = "" + mJsonBean.name;
            mLvel.text = "Lv:" + mLevel;
            mSamsaraValue.text = getAttribute();
        }       
        mLvelUpCost.text = "消耗：" + JsonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, mLevel+1).toStringWithUnit() + "轮回点";
        isEnableLevelUp();
    }

    public void isEnableLevelUp() {
        BigNumber rein =  GameManager.getIntance().mReincarnation;

        if (GameManager.getIntance().mReincarnation.ieEquit(JsonUtils.getIntance().getSamsaraCostByIdAndLevel(mId, mLevel + 1)) != -1)
        {
            mLevelUp.interactable = true;
        }
        else {
            mLevelUp.interactable = false;
        }
    }

    private string getAttribute() {
        return getAttribute(mLevel);
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
                text +=(aj.dec+ ":" + (bean.value/100f)+"%");
                Debug.Log(text);
            }

            if (i < list.Count - 1) {
                text += "\n";
            }
        }
        Debug.Log(text);
        return text;
    }
}
