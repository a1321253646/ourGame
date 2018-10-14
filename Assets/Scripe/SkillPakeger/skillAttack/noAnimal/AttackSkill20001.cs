using UnityEngine;
using System.Collections;

public class AttackSkill20001 : AttackSkillNoAnimal
{

    public override bool add(float count)
    {
        value = value + count;
        Debug.Log("====================AttackSkill20001 value=" + value);
        return true;
    }

    public override float beAction(HurtStatus status)
    {
        float hurt = calcuator.getValue(mManager.getAttacker(), mFight);
        return hurt;
    }

    public override void initEnd()
    {
        calcuator.setSkill(this);
    }

    public override void upDateEnd()
    {
    }

    public override void inAction()
    {
    }
    
}
