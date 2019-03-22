using UnityEngine;
using System.Collections;

public class TimeEventAttackSkill700010 : TimeEventAttackSkillBase
{
    //    public CalculatorUtil mCalcuator;

    public override void killEnemy() {
        if (isActionIng)
        {
            mTime = 0;
        }
        else {
            mFight.mAllAttributePre.add(mSkillIndex, AttributePre.attackSpeed, count1);
            mTime = 0;
            isActionIng = true;
        }
    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeEventSkill(this);
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.getAttacker().mAllAttributePre.delete(mSkillIndex);
        mStatus = SKILL_STATUS_END;
    }
    long count1;
    float count2;
    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_HURT_DIE, this);
        mManager.mEventAttackManager.registerTimeEventSkill(this);
        count2 = mParam[1];
        count1 = ((int)mParam[0])*100;      
        isInit = true;
    }
    private bool isActionIng = false;

    public override void upDateSkill()
    {

        if (!isInit || !isActionIng)
        {
            return;
        }
        mTime += Time.deltaTime;
        Debug.Log("==================================TimeEventAttackSkill200010 value=" + value + " mTime = " + mTime);
        //}
        if (mTime > count2)
        {
            isActionIng = false;
            mFight.mAllAttributePre.minus(mSkillIndex, AttributePre.attackSpeed, count1);
        }
        
    }
}
