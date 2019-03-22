using UnityEngine;
using System.Collections;

public class TimeEventAttackSkill700005 : TimeEventAttackSkillBase
{
    long count1 = -1;
    long count2 = -1;
    long count3 = -1;
    float count4 = 0;
    bool isAdd = false;
    public override void beforeHurt(HurtStatus hurt,Attacker attacker)
    {
        if(isAdd) {
            mTime = 0;
            return;    
        }
        if (count1 == -1)
        {
            count1 =(long) mSkillJson.getSpecialParameterValue()[0];
            count2 = (long)mSkillJson.getSpecialParameterValue()[1] *100;
            count3 = (long)mSkillJson.getSpecialParameterValue()[2] *100;
            count4 = mSkillJson.getSpecialParameterValue()[3];
        }
        bool isCrt = randomResult(100, (int)count1, false);
        if (isCrt)
        {
            mFight.mSkillAttribute.crt += count2;
            mFight.mAllAttributePre.add(mSkillIndex, AttributePre.crtHurt, count3);
            Debug.Log("beforeHurt mFight.mSkillAttribute.crt = " + mFight.mSkillAttribute.crt);
            Debug.Log("beforeHurt mFight.mAllAttributePre.getAll().crtHurt = " + mFight.mAllAttributePre.getAll().crtHurt);
            mTime = 0;
            isAdd = true;
        }
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeEventSkill(this);
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BEFORE_HURT, this);
        mStatus = SKILL_STATUS_END;
        if (isAdd) {
            mFight.mSkillAttribute.crt -= count2;
            mFight.mAllAttributePre.minus(mSkillIndex, AttributePre.crtHurt, count3);
            Debug.Log("endSkill mFight.mSkillAttribute.crt = " + mFight.mSkillAttribute.crt);
            Debug.Log("endSkill mFight.mAllAttributePre.getAll().crtHurt = " + mFight.mAllAttributePre.getAll().crtHurt);
        }
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BEFORE_HURT, this);
        mManager.mEventAttackManager.registerTimeEventSkill(this);
    }

    public override void upDateSkill()
    {
        if (!isInit || ! isAdd)
        {
            return;
        }
        mTime += Time.deltaTime;
        if (mTime > count4) {
            mFight.mSkillAttribute.crt -= count2;
            mFight.mAllAttributePre.minus(mSkillIndex, AttributePre.crtHurt, count3);
            Debug.Log("upDateSkill mFight.mSkillAttribute.crt = " + mFight.mSkillAttribute.crt);
            Debug.Log("upDateSkill mFight.mAllAttributePre.getAll().crtHurt = " + mFight.mAllAttributePre.getAll().crtHurt);
            isAdd = false;
        }
    }
}
