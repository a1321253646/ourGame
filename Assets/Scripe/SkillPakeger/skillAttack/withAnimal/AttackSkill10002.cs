using UnityEngine;
using System.Collections;

public class AttackSkill10002 : AttackSkillWithAnimal
{
    private float mTime = -1;
    public override bool add(float time)
    {
        value = value + time;
        Debug.Log("====================AttackSkill10002 value=" + value);
        if (mTime == -1) {
            mTime = 0;
        }       
        return true;
    }
    public override void inAction()
    {
        throw new System.NotImplementedException();
    }

    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, true);
        mManager.getAttacker().setStop();
    }

    public override void upDateEnd()
    {
        if (mTime != -1)
        {
            mTime += Time.deltaTime;
        }
        if (mTime > value)
        {
            Debug.Log("====================AttackSkill10002 timeOut");
            mManager.getAttacker().cancelStop();
            Debug.Log("=================== mManager.getAttacker().isStop = " + mManager.getAttacker().isStop);
            mSkillStatus = AttackSkillBase.SKILL_STATUS_END;
            Destroy(gameObject, 0.1f);
        }
    }
}
