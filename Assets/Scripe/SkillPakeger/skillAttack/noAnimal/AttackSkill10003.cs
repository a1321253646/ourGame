using UnityEngine;
using System.Collections;

public class AttackSkill10003 : AttackSkillNoAnimal
{
    private float mTime = -1;
    public override bool add(float time)
    {
        value = value + time;
        return true;
    }

    public override void inAction()
    {
        float hurt = calcuator.getValue(mManager.getAttacker(), mFight);
        mFight.AddBlood(hurt);
    }

    public override float beAction(HurtStatus status)
    {
        return 0;
    }

    public override void initEnd()
    {
      
    }

    public override void upDateEnd()
    {
        if (mTime != -1) {
            mTime += Time.deltaTime;       
        }
        if (mTime > value) {
            mSkillStatus = AttackSkillBase.SKILL_STATUS_END;
        }
    }
}
