using UnityEngine;
using System.Collections;

public class AttackSkill10002 : AttackSkillNoAnimal
{
    private float mTime = -1;
    public override bool add(float time)
    {
        value = value + time;
        return true;
    }

    public override void inAction()
    {
    }

    public override float beAction(HurtStatus status)
    {
        return 0;
    }

    public override void initEnd()
    {
        mManager.getAttacker().isStop = true;
    }

    public override void upDateEnd()
    {
        if (mTime != -1) {
            mTime += Time.deltaTime;       
        }
        if (mTime > value) {
            mManager.getAttacker().isStop = false;
            mSkillStatus = AttackSkillBase.SKILL_STATUS_END;
        }
    }
}
