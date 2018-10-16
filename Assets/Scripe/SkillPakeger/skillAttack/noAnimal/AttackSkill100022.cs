using UnityEngine;
using System.Collections;

public class AttackSkill100022 : AttackSkillNoAnimal
{
    private float mTime = -1;
    public override bool add(float time)
    {
        value = value + time;
        Debug.Log("====================AttackSkill10002 value=" + value);
        mTime = 0;
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
        mManager.getAttacker().setStop();
        Debug.Log("=================== mManager.getAttacker().isStop = " + mManager.getAttacker().isStop);
    }

    public override void upDateEnd()
    {
        if (mTime != -1) {
            mTime += Time.deltaTime;       
        }
        if (mTime > value) {
            Debug.Log("====================AttackSkill10002 timeOut");
            mManager.getAttacker().cancelStop();
            Debug.Log("=================== mManager.getAttacker().isStop = " + mManager.getAttacker().isStop);
            mSkillStatus = AttackSkillBase.SKILL_STATUS_END;
        }
    }
}
