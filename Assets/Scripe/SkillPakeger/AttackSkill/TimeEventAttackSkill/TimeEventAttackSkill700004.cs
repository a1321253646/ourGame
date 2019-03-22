using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeEventAttackSkill700004: TimeEventAttackSkillBase
{
    //    public CalculatorUtil mCalcuator;
    private List<Attacker> mDebuffList = new List<Attacker>();
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegisterTimeEventSkill(this);
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_BEFORE_HURT, this);
        mStatus = SKILL_STATUS_END;
    }

    int count1 = -1;
    int count2 = -1;
    int count3 = -1;

    public override void beforeHurt(HurtStatus hurt, Attacker attacker)
    {
        if (count1 == -1)
        {
            count1 = (int)mSkillJson.getSpecialParameterValue()[0] ;
            count2 = (int)mSkillJson.getSpecialParameterValue()[1] *100;
            count3 = (int)mSkillJson.getSpecialParameterValue()[2] ;
        }
        bool isCrt = randomResult(100, count1, false);
        if (isCrt)
        {
            if (attacker.mBuffList.ContainsKey(700004))
            {
                attacker.mBuffList[700004] = TimeUtils.GetTimeStamp() + count3 * 1000;
                return;
            }
            else {
                mDebuffList.Add(attacker);
                attacker.mBuffList.Add(700004, TimeUtils.GetTimeStamp() + count3 * 1000);
                attacker.mAllAttributePre.minus(mSkillIndex, AttributePre.defense, count2);
            }
        }
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_BEFORE_HURT, this);
        mManager.mEventAttackManager.registerTimeEventSkill(this);
        value = mParam[1];
        value = mParam[0]*100;


        isInit = true;
    }


    public override void upDateSkill()
    {
        if (!isInit)
        {
            return;
        }
        if(mDebuffList.Count > 0) {
            for (int i = 0; i < mDebuffList.Count;) {
                if (mDebuffList[i].status == Attacker.PLAY_STATUS_DIE)
                {
                    mDebuffList.RemoveAt(i);
                }
                else if (mDebuffList[i].mBuffList[700004] <= TimeUtils.GetTimeStamp())
                {
                    Debug.Log("mDebuffList[i].mBuffList[700004]=" + mDebuffList[i].mBuffList[700004] + " TimeUtils.GetTimeStamp()=" + TimeUtils.GetTimeStamp());
                    mDebuffList[i].mAllAttributePre.add(mSkillIndex, AttributePre.defense, count2);
                    mDebuffList[i].mBuffList.Remove(700004);
                    mDebuffList.RemoveAt(i);
                }
                else {
                    i++;
                }
            }    
        }
        
    }
    public override bool isAnimal()
    {
        return false;
    }
    public override void addValueEnd()
    {
        mTime = 0;
        value = mParam[0];
    }
}
