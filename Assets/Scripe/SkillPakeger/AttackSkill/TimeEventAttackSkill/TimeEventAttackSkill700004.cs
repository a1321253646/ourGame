using UnityEngine;
using System.Collections;

public class TimeEventAttackSkill700004: TimeEventAttackSkillBase
{
//    public CalculatorUtil mCalcuator;
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeEventSkill(this);
        mManager.getAttacker().mAllAttributePre.delete(mSkillIndex);
        mStatus = SKILL_STATUS_END;
    }

    int count1 = 0;
    public override void endHurt( HurtStatus hurt)
    {
        mFight.AddBlood(count1 * hurt.blood);
    }

    public override void startSkill()
    {
        
        mManager.mEventAttackManager.registerTimeEventSkill(this);
        value = mParam[1];
        value = mParam[0]*100;

        mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.attackSpeed, count1);
        Debug.Log(" TimeEventAttackSkill200004 value = " + value);

        isInit = true;
    }


    public override void upDateSkill()
    {

        if (!isInit)
        {
            return;
        }
        mTime += Time.deltaTime;
        Debug.Log("==================================TimeEventAttackSkill200003 value=" + value + " mTime = " + mTime);
        //}
        if (mTime > value)
        {
            endSkill();
        }
        
    }
    public override bool isAnimal()
    {
        return true;
    }
    public override void addValueEnd()
    {
        mTime = 0;
        value = mParam[0];
    }
}
