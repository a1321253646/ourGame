using UnityEngine;
using System.Collections;

public class AttackSkill600005 : AttackSkillNoAnimal
{


    int count1 = 0;
    int count2 = 0;
    public override bool add(float count)
    {
        value = value + count;
        Debug.Log("====================AttackSkill20001 value=" + value);
        return true;
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
    public override void enemyDie()
    {
        if(count1 == 0) { 
            count1 = (int)(mSkillJson.getEffectsParameterValue()[0]);
        }
        mManager.getAttacker().mSkillAttributePre.defense += count1;
//        value++;
//        mManager.getAttacker().mSkillAttributePre.defense += (value * count1);

    }

}
