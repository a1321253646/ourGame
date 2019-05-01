using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill600003 : EventAttackSkill
{
    int count1 = 0;
    float count2 = 0;
    public override BigNumber getDieHuijing(BigNumber big)
    {
       
        if (count1 == 0)
        {
            count1 =(int) mSkillJson.getSpecialParameterValue()[0] ;
            count2 = mSkillJson.getSpecialParameterValue()[1] ;
        }
   
        bool isSuccess = randomResult(100, count1, false);
        if (isSuccess) {
            big = BigNumber.multiply(big, count2);
        }
        return big;
    }


 /*   public override void Acttacking()
    {
        if(count1 == 0) {
            count1 = mSkillJson.getSpecialParameterValue()[0] / 100;
        }
        Debug.Log("AttackSkill600003  count1 = " + count1);
        string  count = (count1 * mManager.getAttacker().mAttribute.aggressivity).ToString();
        Debug.Log("AttackSkill600003  count = " + count);
        if (count.Contains(".") && !count.Contains("E+"))
        {
            count = count.Split('.')[0];
        }
        else if (count.Contains("E+")) {
            string[] strs = count.Replace("E+", "E").Split('E');
            int wei = int.Parse(strs[1]);
            string[] strs2 = strs[0].Split('.');
            int zheng = int.Parse(strs2[0]);
            if (strs2.Length > 1)
            {
                string xiaoshu = strs2[1];
                if (xiaoshu.Length <= wei)
                {
                    count = "" + zheng + xiaoshu;
                    for (int i = 0; i < wei - xiaoshu.Length; i++)
                    {
                        count = count + "0";
                    }
                }
                else
                {
                    count = "" + zheng + xiaoshu.Substring(0, wei);
                }
            }
            else {
                count = "" + zheng;
                for (int i = 0; i < wei; i++) {
                    count = count + "0";
                }
            }
        }
        BigNumber big = BigNumber.getBigNumForString(count);
        GameManager.getIntance().mCurrentCrystal = BigNumber.add(GameManager.getIntance().mCurrentCrystal, big);
        GameManager.getIntance().updateGasAndCrystal();
    }*/
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_END_DIE_HUIJING, this);
    }

    public override void startSkill()
    {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_END_DIE_HUIJING, this);
    }
}
