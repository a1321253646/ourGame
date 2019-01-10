using System.Collections.Generic;
using UnityEngine;
public class AnimalAttackSkill100018 : AnimalAttackSkillBase
{

    public override void endInitSkill()
    {
        mAnimal.setIsLoop(ActionFrameBean.ACTION_NONE, false);
      //  mAnimal.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimal.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeSkill(this);
        mManager.getAttacker().mSkillAttribute.evd -= value1 * 100;
        mManager.getAttacker().getAttribute();
        mStatus = SKILL_STATUS_END;
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.registerTimeSkill(this);
        if (isGiveup)
        {
            value1 = mParam[2];
            value2 = mParam[3];
        }
        else
        {
            value1 = mParam[0];
            value2 = mParam[1];
        }
        mTime = 0;
        mManager.getAttacker().mSkillAttribute.evd += value1 * 100;
        mManager.getAttacker().getAttribute();
        isInit = true;
    }

    void endAnimal(int status)
    {
        GameObject.Destroy(mSprinter, 0.1f);
    }
    public override void updateSkillEnd()
    {
        mTime += Time.deltaTime;
        if (mTime > value2) {
            endSkill();
        }
    }
    private float value1 = 0; 
    private float value2 = 0; 
    public override bool add(List<float> count, bool isGive)
    {
        mManager.getAttacker().mSkillAttribute.evd -= value1*100;
        if (isGive)
        {
            value1 = mParam[2];
            value2 = mParam[3];
        }
        else {
            value1 = mParam[0];
            value2 = mParam[1];
        }
        mTime = 0;
        mManager.getAttacker().mSkillAttribute.evd += value1*100;
        mManager.getAttacker().getAttribute();
        return true;
    }
}