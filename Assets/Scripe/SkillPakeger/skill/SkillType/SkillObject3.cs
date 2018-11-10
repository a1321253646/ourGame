using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject3 : SkillObject
{
    long v1;
    long v2;
    List<Attacker> noStop = new List<Attacker>();
    List<Attacker> isStop= new List<Attacker>();

    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
   //     mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
        v1 = (long)mBean.getSpecialParameterValue()[0];
        v2 = (long)mBean.getSpecialParameterValue()[1];
        mTargetList = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false);
        if (mTargetList != null && mTargetList.Count >= 0)
        {
            foreach (Attacker attack in mTargetList)
            {
                if (attack.isStop)
                {
                    isStop.Add(attack);
                }
                else {
                    noStop.Add(attack);
                }
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
        mTargetList = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false);
        if (mTargetList == null || mTargetList.Count == 0) {
            return;
        }
        if (count % v1 == 0)
        {
            foreach (Attacker attack0 in mTargetList) {
                foreach (Attacker attack in isStop)
                {
                    if (attack0 == attack) {
                        float hurt = calcuator.getValue(mAttacker, attack);
                        Debug.Log("skill fight event hurt=" + count);
                        attack.skillAttack(mBean.effects, hurt, mAttacker);
                    }
                }
            }

        }
        if (count % v2 == 0)
        {
            foreach (Attacker attack0 in mTargetList)
            {
                foreach (Attacker attack in noStop)
                {
                    if (attack0 == attack)
                    {
                        float hurt = calcuator.getValue(mAttacker, attack);
                        Debug.Log("skill fight event hurt=" + count);
                        attack.skillAttack(mBean.effects, hurt, mAttacker);
                    }

                }
            }

        }
    }
}
