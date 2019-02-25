using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject300001 : SkillObject
{
    public override void initEnd()
    {
        long a1 = (long)mBean.getSpecialParameterValue()[0];
        float a2 = mBean.getSpecialParameterValue()[1];
        mAttacker.mCardManager.addNengliangDian(a1);
        mAttacker.BeKillAttack(mAttacker.mAttribute.maxBloodVolume * (a2 / 100), mAttacker);
        mSkillStatus = SKILL_STATUS_END;
    }

}
