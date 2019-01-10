using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject100012 : SkillObject
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
        if (status == ActionFrameBean.ACTION_NONE) { 
            mTargetList = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false);
        }   
    }

    public override void dealNextSkillForEach(SkillJsonBean skill, Attacker a)
    {
        float count = 0;
        Debug.Log("dealNextSkillForEach count= ");
        AttackerSkillBase skillGet = a.mSkillManager.getAttackerById(skill.id);
        if (skillGet != null) {
            count = skillGet.value;
        }
        if (count != 0) {
            List<float> vals = new List<float>();
            vals.Add(count*(mBean.getSpecialParameterValue()[0]-1));
            skill.setSpecialParameterValue(vals);
        }
    }
}
