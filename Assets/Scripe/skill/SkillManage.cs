using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManage
{
    List<SkillObject> mSkillList = new List<SkillObject>();

    public void addSkill(SkillObject skill) {
        mSkillList.Add(skill);
    }

    public void init() {
        mSkillList.Clear();
    }

    public void update() {
        if (mSkillList.Count == 0) {
            return;
        }
        for (int i = 0; i < mSkillList.Count;)
        {
            mSkillList[i].upDate();
            if (mSkillList[i].getStatus() == SkillObject.SKILL_STATUS_RUNNING)
            {
                mSkillList.Remove(mSkillList[i]);
               
            }
            else
            {
                i++;
                continue;
            }
        }
    }


    private static  SkillManage mIntance = new SkillManage();
    public static SkillManage getIntance() {
        return mIntance;
    }
    private SkillManage() {

    }
}
