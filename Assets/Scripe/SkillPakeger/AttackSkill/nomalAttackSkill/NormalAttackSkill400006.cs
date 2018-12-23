using UnityEngine;
public class NormalAttackSkill400006 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl)
        {
            PlayControl play = (PlayControl)a;
            play.mSkillAttribute.evd -= value ;
        }
    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl) {
            PlayControl play = (PlayControl)a;
            play.mSkillAttribute.evd += value ;
        }
    }
}