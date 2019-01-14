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

    public void show(long vocationId) {
        if (isInit && mVocationId == vocationId) {
            return;
        }
        mVocationId = vocationId;
        if (!isInit) {
            
            mVocationImg = GetComponents<Image>()[1];
            Text[] txs = GetComponents<Text>();
            mVocationName = txs[0];
            mVocationDec = txs[1];

            mCard = transform.GetChild(4).gameObject;
            mAttribute = transform.GetChild(3).gameObject;

            mVocationSlides = GetComponents<VocationSilderControl>();

            Text[] texts = mCard.GetComponents<Text>();
            mSkillName = texts[1];
            mSkillDec = texts[2];
            mSkillImg = mCard.GetComponents<Image>()[3];
            isInit = true;
        }
        showCard();
    }
    private void showCard() {
        
        if (mVocationId == 0) {
            mCard.transform.localScale = new Vector2(0, 0);
            mAttribute.transform.localScale = new Vector2(1, 1);
            foreach (VocationSilderControl v in mVocationSlides) {
                v.show(100);
            }
            mVocationImg.sprite = Resources.Load("icon/vocation/wait", typeof(Sprite)) as Sprite;
            return;
        }

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
        }
        else {
            mCard.transform.localScale = new Vector2(1, 1);
            mAttribute.transform.localScale = new Vector2(0, 0);
            mSkillName.text = mBean.skillName;
            mSkillDec.text = JsonUtils.getIntance().getSkillInfoById(mBean.skill).skill_describe;
            mSkillImg.sprite = Resources.Load("icon/vocation/" + mBean.skillIcon, typeof(Sprite)) as Sprite;

        }
    }
   
}
