using UnityEngine;
using System.Collections;

public class TimeEventAttackSkill700010 : TimeEventAttackSkillBase
{
    //    public CalculatorUtil mCalcuator;

    public override void beforeBeHurt(Attacker fighter, HurtStatus hurt) {
        if (value == 0) {
            value = mParam[0];
        }
    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeEventSkill(this);
        mManager.getAttacker().mAllAttributePre.delete(mSkillIndex);
        mStatus = SKILL_STATUS_END;
    }
    long count1;
    public override void startSkill()
    {
        
        mManager.mEventAttackManager.registerTimeEventSkill(this);
        value = mParam[1];
        count1 = ((int)mParam[0])*100;

       
        Debug.Log(" TimeEventAttackSkill200010 value = " + value);

        isInit = true;
    }
    private bool isActionIng = false;

    private void isAction() {
        if (!isActionIng) {
            mManager.getAttacker().mAllAttributePre.add(mSkillIndex, AttributePre.attackSpeed, count1);
        }
        isActionIng = true;
        mTime = 0;
        value = mParam[0];
    }
    private void isActionEnd() {
        isActionIng = false;
        mManager.getAttacker().mAllAttributePre.delete(mSkillIndex);
    }

    public override void upDateSkill()
    {

        if (!isInit)
        {
            return;
        }
        mTime += Time.deltaTime;
        Debug.Log("==================================TimeEventAttackSkill200010 value=" + value + " mTime = " + mTime);
        //}
        if (mTime > value)
        {
            endSkill();
        }
        
    }
}
