using UnityEngine;
using System.Collections;

public class AnimalAttackSkill200002 : AnimalAttackSkillBase
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

    public override void endInitSkill()
    {
        value = mParam[0];
        Debug.Log("====================AnimalAttackSkill200002 value=" + value);
        if (mTime == -1)
        {
            mTime = 0;
        }
        mAnimal.setIsLoop(ActionFrameBean.ACTION_NONE, true);
        mManager.getAttacker().setStop();

    }

    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeSkill(this);
        GameObject.Destroy(mSprinter,0.1f);
        mStatus = SKILL_STATUS_END;
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.registerTimeSkill(this);
        isInit = true;
    }

    public override void updateSkillEnd()
    {
        Debug.Log("====================AnimalAttackSkill200002  updateSkillEnd timeOut");
        Debug.Log("====================AttackSkill10002 mTime = "+ mTime);
        Debug.Log("====================AttackSkill10002 value = " + value);
        if (!isInit)
        {
            return;
        }
        if (mTime != -1)
        {
            mTime += Time.deltaTime;
        }
        if (mTime > value)
        {
            Debug.Log("====================AttackSkill10002 timeOut");
            mManager.getAttacker().cancelStop();
            Debug.Log("=================== mManager.getAttacker().isStop = " + mManager.getAttacker().isStop);
            endSkill();
        }
    }
}
