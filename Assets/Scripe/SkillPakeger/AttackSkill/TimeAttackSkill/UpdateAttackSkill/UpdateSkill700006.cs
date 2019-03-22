using UnityEngine;
using System.Collections;

public class UpdateSkill700006 : TimeAttackSkillBase
{
    float count = 0;
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeSkill(this);
    }

    public override void startSkill()
    {
        if (count == 0) {
            count = mSkillJson.getSpecialParameterValue()[0]*100;
        }
        mManager.mEventAttackManager.registerTimeSkill(this);
        isInit = true;
    }
    private bool isHaveAdd = false;
    public override void upDateSkill()
    {

        if (!isInit) {
            return;
        }
        if (GameManager.getIntance().mAliveEnemy == 1 && !isHaveAdd)
        {
            isHaveAdd = true;
            mFight.mAllAttributePre.add(mSkillIndex, AttributePre.aggressivity, (int)count);
        }
        else if (GameManager.getIntance().mAliveEnemy > 1 && isHaveAdd) {
            isHaveAdd = false;
            mFight.mAllAttributePre.minus(mSkillIndex, AttributePre.aggressivity, (int)count);
        }
    }
}
