using UnityEngine;
using System.Collections;

public class UpdateSkill600010 : TimeAttackSkillBase
{
    float count = 0;
    float count2 = 0;
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeSkill(this);
    }

    public override void startSkill()
    {
        if (count == 0) {
            count = mSkillJson.getSpecialParameterValue()[1]/100;
            count2 = mSkillJson.getSpecialParameterValue()[0];
        }
        mManager.mEventAttackManager.registerTimeSkill(this);
        isInit = true;
    }

    public override void upDateSkill()
    {
        if (!isInit) {
            return;
        }
        mTime += Time.deltaTime;
        if (mTime >= count2) {
            mTime -= count2;
            double count1 = (count * mManager.getAttacker().mAttribute.maxBloodVolume);
//            Debug.Log("===============60010 add "+ count1);
            mManager.getAttacker().AddBlood(count1);
        }
    }
}
