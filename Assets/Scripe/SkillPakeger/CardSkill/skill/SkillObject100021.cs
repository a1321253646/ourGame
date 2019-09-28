using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject100021 : SkillObject
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
        mAttacker.mCardManager.giveupCard(CardManagerBase.GIVEUP_CARD_RANGE);
        Destroy(gameObject, 0);
    }

    void fightEcent(int status)
    {
        if (status == ActionFrameBean.ACTION_NONE)
        {
            Debug.Log("skill fight event");
            mTargetList = SkillTargetManager.getTargetList(mFightManager, mLocal, mCamp, false);
            Debug.Log("mLocal x="+ mLocal.x+" y="+ mLocal.y);
            if (mTargetList == null || mTargetList.Count < 1) {
                Debug.Log("getTargetList null");
                return;
            }
            foreach (Attacker attack in mTargetList) {
                double hurt =  calcuator.getValue(mAttacker, attack);
                Debug.Log("========================fightEcent hurt="+ hurt);
                attack.skillAttack( hurt, mAttacker);
            }           
        }
    }
}
