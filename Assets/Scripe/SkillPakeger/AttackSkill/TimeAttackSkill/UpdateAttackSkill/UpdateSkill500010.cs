using UnityEngine;
using System.Collections;

public class UpdateSkill500010 : TimeAttackSkillBase
{
    float count = 0;
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeSkill(this);
    }

    public override void startSkill()
    {
        if (count == 0) {
            count = mSkillJson.getEffectsParameterValue()[0]/100;
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
            int count = (int)(mSkillJson.getEffectsParameterValue()[0] * mManager.getAttacker().mAttribute.aggressivity);
            mManager.getAttacker().AddBlood(mManager.getAttacker().mAttribute.maxBloodVolume * count);
        }
    }
}
