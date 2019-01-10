using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject100013 : SkillObject
{
    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
    }
    void endAnimal(int status) {
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
        float v1 = mBean.getSpecialParameterValue()[0];
        float v2 = mBean.getSpecialParameterValue()[1];
        float v3 = mBean.getSpecialParameterValue()[2];
        List<float> v = new List<float>();
        if (skill.id == 200001)
        {
            v.Add(v3);
        }
        else if (skill.id == 200002) {
            float ram = Random.Range(v1 * 1000, v2 * 1000);
            ram = ram / 1000;
            v.Add(ram);
        }
        if (v.Count > 0) {
            skill.setSpecialParameterValue(v);
        }
    }

}
