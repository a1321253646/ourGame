using UnityEngine;
public class PackbackSkill400006 : PackbackSkillBase
{
    public override void removeSkill()
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