using UnityEngine;
public class NormalAttackSkill300001 : NormalAttackSkillBase
{//没被应用到
    public override void endSkill()
    {
     //   GameManager.getIntance().mCardLunhuiGet -= ((float)value / 10000);
    }

    public override void startSkill()
    {
        long a1 =(long) mParam[0];
        float a2 = mParam[1];
        mFight.mCardManager.addNengliangDian(a1);
        Attacker a = mManager.getAttacker();
        a.BeKillAttack(a.mAttribute.maxBloodVolume * (a2 / 100), a);
        mStatus = SKILL_STATUS_END;
    }
}