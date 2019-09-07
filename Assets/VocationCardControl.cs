using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VocationCardControl : MonoBehaviour {

    private long mVocationId = 0;
    private bool isInit = false;

    private Image mVocationImg,mSkillImg;
    private GameObject mCard, mAttribute;

    private VocationSilderControl[] mVocationSlides;

    private Text mSkillName,mSkillDec;
    private Text mVocationName,mVocationDec;
    private VocationDecBean mBean;

    private Button mButton;

    public void show(long vocationId) {
        if (isInit && mVocationId == vocationId) {
            return;
        }
        mVocationId = vocationId;
        if (!isInit) {
            
            mVocationImg = GetComponentsInChildren<Image>()[1];
            Text[] txs = GetComponentsInChildren<Text>();
            mVocationName = txs[0];
            mVocationDec = txs[1];

            mCard = transform.GetChild(4).gameObject;
            mAttribute = transform.GetChild(3).gameObject;

            mVocationSlides = GetComponentsInChildren<VocationSilderControl>();

            Text[] texts = mCard.GetComponentsInChildren<Text>();
            mSkillName = texts[1];
            mSkillDec = texts[2];
            mSkillImg = mCard.GetComponentsInChildren<Image>()[2];

            mButton = GetComponent<Button>();
            mButton.onClick.AddListener(() =>
            {
                select();
            });

            isInit = true;
        }
        showCard();
    }

    private void select() {
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_CLICK_BUTTON, GuideManager.BUTTON_CLICK_CLICK_VOCATION);
        GameObject.Find("lunhui_tips").GetComponent<LuiHuiTips>().showUi(mVocationId);
    }

    private void showCard() {
        
/*        if (mVocationId == 0) {
            mCard.transform.localScale = new Vector2(0, 0);
            mAttribute.transform.localScale = new Vector2(1, 1);
            foreach (VocationSilderControl v in mVocationSlides) {
                v.show(100);
            }
            mVocationImg.sprite = Resources.Load("icon/vocation/wait", typeof(Sprite)) as Sprite;
            return;
        }*/

        mBean = JsonUtils.getIntance().getVocationById(mVocationId);

        mVocationImg.sprite = Resources.Load("icon/vocation/" + mBean.bust, typeof(Sprite)) as Sprite;
        mVocationName.text = mBean.name;
        mVocationDec.text = mBean.dec;

        if (mBean.skill == -1)
        {
            mCard.transform.localScale = new Vector2(0, 0);
            mAttribute.transform.localScale = new Vector2(1, 1);
            for (int i = 0; i < mVocationSlides.Length; i++)
            {
                mVocationSlides[i].show(mBean.getAttribute()[i]);
            }
            mButton.interactable = true; 
        }
        else if (mBean.skill == -2) {
            mCard.transform.localScale = new Vector2(1, 1);
            mAttribute.transform.localScale = new Vector2(0, 0);
            mSkillName.text = mBean.skillName;
            mSkillDec.text = "？？？？？？？？？？？？";
            mSkillImg.sprite = Resources.Load("icon/vocation/" + mBean.skillIcon, typeof(Sprite)) as Sprite;
            mButton.interactable = false;
        }
        else
        {
            mCard.transform.localScale = new Vector2(1, 1);
            mAttribute.transform.localScale = new Vector2(0, 0);
            mSkillName.text = mBean.skillName;
            SkillJsonBean skillBean = JsonUtils.getIntance().getSkillInfoById(mBean.skill);
            string dec = skillBean.skill_describe;
            if (skillBean.getSpecialParameterValue() != null && skillBean.getSpecialParameterValue().Count > 0) {
                for (int i = 0; i < skillBean.getSpecialParameterValue().Count; i++) {
                    dec = dec.Replace("S" + (i+1), "" + skillBean.getSpecialParameterValue()[i]);
    
                }

            }
            mSkillDec.text = dec;
            mButton.interactable = true;
            mSkillImg.sprite = Resources.Load("icon/vocation/" + mBean.skillIcon, typeof(Sprite)) as Sprite;

        }
    }
   
}
