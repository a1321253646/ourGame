using UnityEngine;
public class NormalAttackSkill400008 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        mManager.cardDownCardCost -=(long) value;
    }

    public override void startSkill()
    {
        mManager.cardDownCardCost += (long)value;
    }
}