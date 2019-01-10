using UnityEngine;
public class NormalAttackSkill100014 : NormalAttackSkillBase
{
    public override void endSkill()
    {
       
    }

    public override void startSkill()
    {
        long count = 1;
        if (isGiveup)
        {
            count = (long)mParam[1];
        }
        else {
            count = (long)mParam[0];
        }
        GameObject.Find("jineng").GetComponent<CardManager>().addCards(count);
        mStatus = SKILL_STATUS_END;
    }
}