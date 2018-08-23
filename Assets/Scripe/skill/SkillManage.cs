using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManage
{
    List<SkillObject> mSkillList = new List<SkillObject>();
    public GameObject mSkillObject; 
    public void addSkill(SkillJsonBean skill,float x,float y,int campType) {
        GameObject newobj = GameObject.Instantiate(
                mSkillObject, new Vector2(x,y), Quaternion.Euler(0.0f, 0f, 0.0f));
        dealSkillType(newobj, skill,x,y,campType);
    }

    private void dealSkillType(GameObject newobj, SkillJsonBean skill, float x, float y, int campType) {
        if (skill.effects == 1) {
            newobj.AddComponent<SkillObject1>();
        }
        SkillObject skillComponent = newobj.GetComponent<SkillObject>();
        skillComponent.init(skill, x, y, campType);
        mSkillList.Add(skillComponent);
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
            if (mSkillList[i].getStatus() == SkillObject.SKILL_STATUS_END)
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
    public void setSkillPrefer(GameObject ob) {
        mSkillObject = ob;
    }

    private static  SkillManage mIntance = new SkillManage();
    public static SkillManage getIntance() {
        return mIntance;
    }
    private SkillManage() {

    }
}
