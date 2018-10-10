using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject30002 : SkillObject
{
    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
       // mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
    }
    void endAnimal(int status)
    {
        mSkillStatus = SKILL_STATUS_END;
        actionEnd();
        Destroy(gameObject, 0);
    }
    public void dealNextSkillForEach(SkillJsonBean skill, Attacker a)
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
