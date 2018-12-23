using UnityEngine;
public class NormalAttackSkill400003 : NormalAttackSkillBase
{
    public override void endSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl)
        {
            PlayControl play = (PlayControl)a;
            play.mSkillAttributePre.maxBloodVolume -= value ;
        }
    }

    public override void startSkill()
    {
        Attacker a = mManager.getAttacker();
        if (a is PlayControl) {
            PlayControl play = (PlayControl)a;
            play.mSkillAttributePre.maxBloodVolume += value;
        }
    }
}