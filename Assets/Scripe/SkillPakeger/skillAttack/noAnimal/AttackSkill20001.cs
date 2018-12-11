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
        Debug.Log("====================AttackSkill20001 beAction value =" + value);
        Debug.Log("====================AttackSkill20001 beAction  mSkillJson.getEffectsParameterValue()[0]=" + mSkillJson.getEffectsParameterValue()[0]);

        float hurt = status.blood * mSkillJson.getEffectsParameterValue()[0] * value;
        Debug.Log("====================AttackSkill20001 beAction =" + hurt);
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
