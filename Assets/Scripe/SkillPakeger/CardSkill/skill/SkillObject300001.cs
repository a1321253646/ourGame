using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject300001 : SkillObject
{
    public override void initEnd()
    {
        float a1 = mBean.getSpecialParameterValue()[0];
        float a2 = mBean.getSpecialParameterValue()[1];
        GameObject.Find("Manager").GetComponent<LevelManager>().addNengliangDian(a1);
        mAttacker.BeKillAttack(mAttacker.mAttribute.maxBloodVolume * (a2 / 100), mAttacker);
        mSkillStatus = SKILL_STATUS_END;
    }

}
