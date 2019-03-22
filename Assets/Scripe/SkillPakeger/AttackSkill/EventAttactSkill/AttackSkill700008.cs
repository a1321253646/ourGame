using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill700008 : EventAttackSkill
{


    int count1 = 0;
    float count2 = 0;
    public override void beforeDie()
    {
        if(count1 == 0) { 
            count1 = (int)mSkillJson.getSpecialParameterValue()[0] ;
            count2 = mSkillJson.getSpecialParameterValue()[1] / 100;
        }
        bool isAction = randomResult(100, count1, true);
        if (isAction) {
            mManager.getAttacker().AddBlood(mManager.getAttacker().mAttribute.maxBloodVolume * count2);
        }

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BEFOOR_DIE, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BEFOOR_DIE, this);
    }

}
