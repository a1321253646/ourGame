using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimalAttackSkill200002 : AnimalAttackSkillBase
{
    public override bool add(List<float> count, bool isGiveup)
    {
        if(count[0] > value- mTime) {
            value = count[0];
            mTime = 0;
        }
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
        if (mTime != -1)
        {
            mTime += Time.deltaTime;
        }
        if (mTime > value)
        {
            mManager.getAttacker().cancelStop();
            endSkill();
        }
    }
}
