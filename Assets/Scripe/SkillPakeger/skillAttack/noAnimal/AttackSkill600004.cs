using UnityEngine;
using System.Collections;

public class AttackSkill600004 : AttackSkillNoAnimal
{


    int count = 0;
    float count1 = 0;
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
        if (count == 0) {
            count = (int)(mSkillJson.getEffectsParameterValue()[0] * 100);
            count1 = mSkillJson.getEffectsParameterValue()[1];
        }
        
        bool isSuccess = randomResult(10000, count, false);
        if (isSuccess) {
            mManager.getAttacker().AddBlood((int)(count1 * mManager.getAttacker().mAttribute.aggressivity));
        }
    }
    
}
