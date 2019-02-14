using UnityEngine;
public class NormalAttackSkill100009 : NormalAttackSkillBase
{
    public override void endSkill()
    {
       
    }

    public override void startSkill()
    {

        long count = (long)mParam[0];     

        GameObject.Find("jineng").GetComponent<CardManager>().addCards((long)mParam[0]);
        mStatus = SKILL_STATUS_END;
    }
}