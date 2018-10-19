using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject6 : SkillObject
{
    List<Attacker> isStop = new List<Attacker>();
    List<Attacker> noStop = new List<Attacker>();

    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
   //     mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));        
       mAnimalControl.setTimeCountBack(ActionFrameBean.ACTION_NONE, (long)mBean.getSpecialParameterValue()[0], new AnimalStatu.animalCountTimeCallback(timeCountBack));
    }
    void endAnimal(int status) {
        mSkillStatus = SKILL_STATUS_END;
        actionEnd();
        Destroy(gameObject, 0);
    }

    void timeCountBack(int count)
    {
        mTargetList = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false);
        if (mTargetList != null && mTargetList.Count > 0) {
            foreach (Attacker attack in mTargetList)
            {
                if (attack != null)
                {
                    float hurt = calcuator.getValue(mAttacker, attack);
                    Debug.Log("skill fight event hurt=" + hurt);
                    attack.skillAttack(mBean.effects, hurt);
                }
            }
        }
    }
}
