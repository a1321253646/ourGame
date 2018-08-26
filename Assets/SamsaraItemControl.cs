using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SamsaraItemControl : MonoBehaviour {
    public long mId;
    private long mLevel;
    private SamsaraJsonBean mJsonBean;
    private Image mIcon;
    private Text mSamsaraNameAndLevel;
    private Text mSamsaraValue;
    private Text mLvelUpCost;
    private Button mLevelUp;
    private SamSaraListControl mListControl;
    public void init(long id, SamSaraListControl control) {
        mId = id;
        mListControl = control;
        mJsonBean =  JsonUtils.getIntance().getSamsaraInfoById(mId);       
        mIcon = GameObject.Find("skill_icon").GetComponent<Image>();
        mSamsaraNameAndLevel = GameObject.Find("skill_name").GetComponent<Text>();
        mSamsaraValue = GameObject.Find("skill_effect_labe").GetComponent<Text>();
        mLvelUpCost = GameObject.Find("skill_effect").GetComponent<Text>();
        mLevelUp = GameObject.Find("skill_lvup_button").GetComponent<Button>();
        Sprite sprite = Resources.Load("icon/samsara" + mJsonBean.icon, typeof(Sprite)) as Sprite;
        mLevelUp.onClick.AddListener(() => {
            levelUp();
        });
    }

    private void levelUp()
    {
        mListControl.upDate(mId);
    }

    public void upDate()
    {
        mLevel = InventoryHalper.getIntance().getSamsaraLevelById(mId);
        mSamsaraNameAndLevel.text = "技能名称 " + mJsonBean.name + "Lv:" + mLevel;
        mSamsaraValue.text = getAttribute();
        mLvelUpCost.text = "消耗：" + mJsonBean.coast + "轮回点";
    }
    private string getAttribute() {
        List<SamsaraValueBean>  list = JsonUtils.getIntance().getSamsaraVulueInfoByIdAndLevel(mId, mLevel);
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
            else if (bean.type == 121)
            {
                text += "真实伤害: " + bean.value;
            }
            if(i < list.Count - 1) {
                text += "\n";
            }
        }
        return text;
    }
}
