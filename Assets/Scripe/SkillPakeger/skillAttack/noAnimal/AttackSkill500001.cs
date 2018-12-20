using UnityEngine;
using System.Collections;

public class AttackSkill500001 : AttackSkillNoAnimal
{


    int count = 0;
    public override bool add(float count)
    {
        value = value + count;
        Debug.Log("====================AttackSkill20001 value=" + value);
        return true;
    }

    public override EquipKeyAndValue beforeHurt()
    {
        if (count == 0)
        {
            count = (int)(mSkillJson.getEffectsParameterValue()[0] * 100);
        }

        bool isCrt = randomResult(10000, count, false);
        if (!isCrt)
        {
            return null;
        }
        EquipKeyAndValue value = new EquipKeyAndValue();
        value.key = 111;
        value.value = 1;
        return value;
    }

    public override float beAction(HurtStatus status)
    {
        return 0;
    }
    public override void initEnd()
    {
    }

    public override void upDateEnd()
    {
    }

    public override void inAction()
    {
    }
    
}
