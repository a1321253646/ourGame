using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class BossCardControl : MonoBehaviour
{
    private CardControl mCard;
    private bool isShow = false;
    private float mTime = 0;
    private float SHOW_TIME = 0.5f;
    Attacker mAttacker;
    Attacker mHero;
    CardUiControl mCardUiControl;
    public bool show(Attacker attacker, Attacker hero, CardControl card) {
        if (isShow) {
            return false;
        }
        if (mCardUiControl == null) {
            mCardUiControl = GetComponent<CardUiControl>();
        }
        mAttacker = attacker;
        mHero = hero;
        mCard = card;
        mTime = 0;
        isShow = true;
        mCardUiControl.init(mCard.mCard.id, CardUiControl.TYPE_CARD_PLAY, mAttacker);
        mCardUiControl.init(mCard.mCard.id, 107, 146);
        transform.localScale = new Vector2(1, 1);
        return true;
    }

    public bool isShowIng() {
        return isShow;
    }

    private void Update()
    {
        if (!isShow) {
            return;
        }
        mTime += Time.deltaTime;
        if (mTime >= SHOW_TIME) {
            isShow = false;
            transform.localScale = new Vector2(0, 0);
            userCard();
        }
    }
    private void userCard() {
        int targetType = Attacker.CAMP_TYPE_DEFAULT;
        if (mCard.mSkill.target_type == SkillJsonBean.TYPE_SELF)
        {
            targetType = Attacker.CAMP_TYPE_MONSTER;
        }
        else if (mCard.mSkill.target_type == SkillJsonBean.TYPE_ENEMY)
        {
            targetType = Attacker.CAMP_TYPE_PLAYER;
        }

        if (mCard.mSkill.shape_type == 4)
        {
            if (mHero != null && mHero.status != Attacker.PLAY_STATUS_DIE)
            {
                mHero.mSkillManager.addSkill(mCard.mSkill.id, mAttacker, SkillIndexUtil.getIntance().getSkillIndexByCardId(true, mCard.mCard.id));
            }
        }
        else if (mCard.mSkill.shape_type == 6)
        {
            mAttacker.mSkillManager.addSkill(mCard.mSkill.id, mAttacker, SkillIndexUtil.getIntance().getSkillIndexByCardId(true, mCard.mCard.id));
        }
        else if (mHero != null && mHero.status != Attacker.PLAY_STATUS_DIE)
        {
            SkillManage.getIntance().bossAddSkill(mAttacker, mHero, mCard.mSkill,
                targetType, SkillIndexUtil.getIntance().getSkillIndexByCardId(true, mCard.mCard.id));
        }

    }
}
