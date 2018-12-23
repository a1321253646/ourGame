using UnityEngine;
using System.Collections;

public class UpdateSkill600010 : TimeAttackSkillBase
{
    float count = 0;
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeSkill(this);
    }

    public override void startSkill()
    {
        if (count == 0) {
            count = mSkillJson.getSpecialParameterValue()[0]/100;
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
        if (mTime >= 1) {
            mTime -= 1;
            int count1 = (int)(mSkillJson.getSpecialParameterValue()[0] * mManager.getAttacker().mAttribute.aggressivity/100);
            Debug.Log("===============60010 add "+ count1);
            mManager.getAttacker().AddBlood(count1);
        }
    }
}
