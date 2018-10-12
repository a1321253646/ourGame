using UnityEngine;
using System.Collections;

public class AttackSkill20001 : AttackSkillNoAnimal
{

    public override bool add(float count)
    {
        value = value + count;
        return true;
    }

    public new void inAction()
    {
    }

    public override float beAction(HurtStatus status)
    {
        float hurt = calcuator.getValue(mManager.getAttacker(), mFight);
        return hurt;
    }

    public override void initEnd()
    {
        float hurt = calcuator.getValue(mManager.getAttacker(), mFight);
        HurtStatus status = new HurtStatus(hurt,false,false);
        mManager.getAttacker().BeAttack(status);

    }

    public override void upDateEnd()
    {
    }
}
