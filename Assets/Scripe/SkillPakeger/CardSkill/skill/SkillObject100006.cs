﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject100006 : SkillObject
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
            Debug.Log("skill fight event");
            mTargetList = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false);
            Debug.Log("mLocal x=" + mLocal.x + " y=" + mLocal.y);
            if (mTargetList == null || mTargetList.Count < 1)
            {
                Debug.Log("getTargetList null");
                return;
            }
            foreach (Attacker attack in mTargetList)
            {
                double hurt = calcuator.getValue(mAttacker, attack);
                Debug.Log("skill fight event 30001 hurt = "+hurt);
                attack.skillAttack( hurt, mAttacker);
            }

        }
    }
    public override void dealNextSkillForEach(SkillJsonBean skill, Attacker a)
    {
        List<float> vals = new List<float>();
        vals.AddRange(mBean.getSpecialParameterValue());
        skill.setSpecialParameterValue(vals);
    }
}
