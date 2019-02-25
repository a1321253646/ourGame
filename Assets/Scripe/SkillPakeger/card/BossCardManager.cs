using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossCardManager : MonoBehaviour
{
    BossCardJsonBean mBean = null;
    bool isShow = false;
    private int attckIndex = 0;
    private int cardIndex = 0;
    private long nengLiang = 0;

    private int addNengliangAttack = 0;
    private float startCardTime = -1;
    private float nextCardTime = -1;

    private int addCard = 0;
    private float mTime = 0;
    private float mAddCardTime = 0;
    private List<CardJsonBean> mCardIdList = new List<CardJsonBean>();
    private Attacker mAttacker = null;
    private Attacker mHero = null;
    public void show(Attacker boss) {
        mAttacker = boss;
        mHero = GameObject.Find("Manager").GetComponent<LevelManager>().mPlayerControl;
        if (addNengliangAttack == 0) {
            addNengliangAttack = (int)JsonUtils.getIntance().getConfigValueForId(100051);
            startCardTime = JsonUtils.getIntance().getConfigValueForId(100051);
            nextCardTime = JsonUtils.getIntance().getConfigValueForId(100052);
        }
        attckIndex = 0;
        cardIndex = 0;
        nengLiang = 0;
        mTime = 0;
        mAddCardTime = 0;
        addCard = 0;
        isShow = true;
        mCardIdList.Clear();
        Level level = JsonUtils.getIntance().getLevelData(
            BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel));
        mBean = JsonUtils.getIntance().getBossCardListById(level.card);
    }
    public void bossAttack() {
        attckIndex++;
        if (attckIndex >= addNengliangAttack) {
            attckIndex = attckIndex - addNengliangAttack;
            if (nengLiang < 10) {
                nengLiang++;
                updateBossNengliangShow();
            }
        }
    }

    private void isUserCard() {
        if (mCardIdList.Count > 0)
        {
            CardJsonBean bean = mCardIdList[0];
            if (bean.cost < nengLiang)
            {
                nengLiang = nengLiang - bean.cost;
                updateBossNengliangShow();
                useCard(bean);
                mCardIdList.Remove(bean);
                updateBossCardShow();
            }
        }
    }

    public void addNengLiang(long add) {
        nengLiang = nengLiang + add;
        if (nengLiang > 10) {
            nengLiang = 10;
            updateBossNengliangShow();
        }
    }

    public void addCardCount(int add) {
        addCard += add;
    }



    // Update is called once per frame
    void Update()
    {
        if (!isShow  ) {
            return;
        }
        if(mBean.getCardList() == null || mBean.getCardList().Count == 0) {
            return;    
        }
        mTime += Time.deltaTime;
        if (mTime < startCardTime) {
            return;
        }

        isUserCard();
        if (cardIndex >= mBean.getCardList().Count) {
            return;
        }
        if (addCard > 0) {
            mAddCardTime += Time.deltaTime;
            if (mAddCardTime > 0.1) {
                addCard--;
                mAddCardTime = mAddCardTime - 0.1f;
                addBossCard();
            }
        }
        if (mTime > startCardTime + nextCardTime) {
            mTime = mTime - nextCardTime;
            addBossCard();
        }
    }
    private void updateBossNengliangShow()
    {

    }

    private void showUi()
    {

    }

    private void updateBossCardShow() {

    }

    public void disShow()
    {
        isShow = false;
    }

    private void addBossCard() {
       
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(mBean.getCardList()[cardIndex]);
        mCardIdList.Add(card);
        cardIndex++;
        updateBossCardShow();
    }

    private void useCard(CardJsonBean bean) {
        SkillJsonBean skill = JsonUtils.getIntance().getSkillInfoById(bean.skill_id);

        if (skill.shape_type == 4)
        {
            mHero.mSkillManager.addSkill(skill.id, mAttacker);
        }
        else if (skill.shape_type == 6)
        {
            mAttacker.mSkillManager.addSkill(skill.id, mAttacker);
        }
        else
        {
            SkillManage.getIntance().bossAddSkill(mAttacker, skill);
        }
    }
}
