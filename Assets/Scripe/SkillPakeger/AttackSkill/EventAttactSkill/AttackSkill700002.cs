using UnityEngine;
using System.Collections;

public class AttackSkill700002 : EventAttackSkill
{


    float count1 = -1;
    float count2 = -1;
    public override void endBeHurt(Attacker fighter, HurtStatus hurt)
    {
       // Debug.Log("AttackSkill700002 endBeHurt");
        if(count1 == -1) { 
            count1 = mSkillJson.getSpecialParameterValue()[0];
            count2 = mSkillJson.getSpecialParameterValue()[1]/100f;
        }
        if (hurt.type == HurtStatus.TYPE_RATE || hurt.type == HurtStatus.TYPE_FANGSHANG) {
            return;
        }

        bool isFangshang = randomResult(100, (int)count1, true);
        if (isFangshang) {
         //   Debug.Log("AttackSkill700002 endBeHurt hurt.blood * count2="+ hurt.blood * count2);
            HurtStatus fs = new HurtStatus(mFight.mAttribute.defense * count2, HurtStatus.TYPE_FANGSHANG);
            
            fighter.allHurt(fs, mManager.getAttacker());
        }

    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_END_BEHURT, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_END_BEHURT, this);
    }
}
