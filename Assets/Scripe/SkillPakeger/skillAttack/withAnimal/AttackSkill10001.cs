using UnityEngine;
using System.Collections;
/*
     
*/
public class AttackSkill10001 : AttackSkillWithAnimal
{
    public override void inAction()
    {
        throw new System.NotImplementedException();
    }

    public override void initEnd()
    {
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE, false);
        mAnimalControl.addIndexCallBack(ActionFrameBean.ACTION_NONE, (int)mResource.attack_framce, new AnimalStatu.animalIndexCallback(fightEcent));
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
    }

    public override void upDateEnd()
    {
       
    }

    void endAnimal(int status)
    {
        mSkillStatus = SKILL_STATUS_END;
        Destroy(gameObject, 0.1f);
    }
    void fightEcent(int status)
    {
        if (status == ActionFrameBean.ACTION_NONE)
        {
            float hurt = calcuator.getValue(mManager.getAttacker(), mFight);
            Debug.Log("fightEcent " + hurt);
            mManager.getAttacker().AddBlood(hurt);
        }
    }
}
