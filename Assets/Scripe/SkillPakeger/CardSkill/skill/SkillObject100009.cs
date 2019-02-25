using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject100009 : SkillObject
{
    public override void initEnd()
    {
        mAttacker.mCardManager.addCards((long)mBean.getSpecialParameterValue()[0]);
        mSkillStatus = SKILL_STATUS_END;
    }
}
