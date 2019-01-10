using UnityEngine;
public class AnimalAttackSkill100016 : AnimalAttackSkillBase
{

    public override void endInitSkill()
    {
        mAnimal.setIsLoop(ActionFrameBean.ACTION_NONE, false);
        mAnimal.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimal.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
    }
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeSkill(this);
        GameObject.Destroy(mSprinter, 0.1f);
        mStatus = SKILL_STATUS_END;
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.registerTimeSkill(this);
        isInit = true;
    }

    void endAnimal(int status)
    {
        endSkill();
    }
    void fightEcent(int status)
    {
        if (status == ActionFrameBean.ACTION_NONE)
        {
            double count = 1;
            if (isGiveup)
            {
                count = mParam[1] / 100;
            }
            else {
                count = mParam[0] / 100;
            }
            mManager.getAttacker().AddBlood(mManager.getAttacker().mAttribute.aggressivity  * count);
        }
    }
}