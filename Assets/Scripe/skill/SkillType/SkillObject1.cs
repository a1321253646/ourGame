using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject1 : SkillObject
{
    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
    }
    void fightEcent(int status)
    {
        if (status == ActionFrameBean.ACTION_NONE)
        {
            Debug.Log("skill fight event");
            List<Attacker> list = SkillTargetManager.getTargetList(mLocalManager.mLocalLink, mLocal, mCamp, false, mResource);
            foreach (Attacker attack in list) {
                float hurt =  calcuator.getValue(mAttacker, attack);
                HurtStatus hurtstatus = new HurtStatus(hurt,false,false);
                attack.BeAttack(hurtstatus);
            }

        }
    }
}
