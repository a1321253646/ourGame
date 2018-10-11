﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject6 : SkillObject
{
    long v1 = -1;
    long v2 = -1;
    List<Attacker> isStop = new List<Attacker>();
    List<Attacker> noStop = new List<Attacker>();

    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
   //     mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
        v1 = (long)mBean.getSpecialParameterValue()[0];
        v2 = (long)mBean.getSpecialParameterValue()[1];
        mTargetList = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false);
        foreach (Attacker attack in mTargetList)
        {
            if (attack.isStop) {
                isStop.Add(attack);
            }
            else{
                noStop.Add(attack);
            }
        }
       mAnimalControl.setTimeCountBack(ActionFrameBean.ACTION_NONE, v1*v2, new AnimalStatu.animalCountTimeCallback(timeCountBack));
    }
    void endAnimal(int status) {
        mSkillStatus = SKILL_STATUS_END;
        actionEnd();
        Destroy(gameObject, 0);
    }

    void timeCountBack(int count)
    {
        List<Attacker> list = null;
        if (count == v2)
        {
            list = noStop;
        }
        else if (count == v1)
        {
            list = isStop;
        }
        if (list != null && list.Count > 0) {
            foreach (Attacker attack in list)
            {
                float hurt = calcuator.getValue(mAttacker, attack);
                Debug.Log("skill fight event hurt=" + hurt);
                attack.skillAttack(mBean.effects, hurt);
            } 
        }
    }
}
