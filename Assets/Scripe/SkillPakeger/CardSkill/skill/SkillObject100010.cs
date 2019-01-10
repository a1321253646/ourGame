using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject100010 : SkillObject
{
    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
    }
    void endAnimal(int status)
    {
        mSkillStatus = SKILL_STATUS_END;
        actionEnd();
        Destroy(gameObject, 0);
    }
    void fightEcent(int status)
    {
        if (status == ActionFrameBean.ACTION_NONE)
        {
            mTargetList = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false);
        }
    }

    public override void dealNextSkillForEach(SkillJsonBean skill, Attacker a)
    {
        List<float> vals = new List<float>();
        if (a.mAttribute.maxBloodVolume > a.mBloodVolume)
        {
            vals.Add(mBean.getSpecialParameterValue()[1]);
        }
        else {
            vals.Add(mBean.getSpecialParameterValue()[0]);
        }
        skill.setSpecialParameterValue(vals);
    }
}
