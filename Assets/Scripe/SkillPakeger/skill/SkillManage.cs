using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManage
{
    List<SkillObject> mSkillList = new List<SkillObject>();
    public GameObject mSkillObject;
    public LocalManager mLocalManager;
    public void addSkill(Attacker attacker, SkillJsonBean skill,float x,float y,int campType) {
        Debug.Log("addSkill x=" + x + " y = " + y);
        
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(skill.skill_resource);
        GameObject newobj = GameObject.Instantiate(
                mSkillObject, new Vector2(x+ bean.getHurtOffset().x, y + bean.getHurtOffset().y), Quaternion.Euler(0.0f, 0f, 0.0f));
        dealSkillType(attacker,newobj, skill,x,y,campType);
    }

    private void dealSkillType(Attacker attacker, GameObject newobj, SkillJsonBean skill, float x, float y, int campType) {
        if (skill.effects == 1 || skill.effects == 2)
        {
            newobj.AddComponent<SkillObject1>();
        }
        else if (skill.effects == 6)
        {
            newobj.AddComponent<SkillObject6>();
        }
        else if (skill.effects == 30001) {
            newobj.AddComponent<SkillObject30001>();
        }
        else if (skill.effects == 30002)
        {
            newobj.AddComponent<SkillObject30002>();
        }
        else if (skill.effects == 30003)
        {
            newobj.AddComponent<SkillObject30003>();
        }
        else if (skill.effects == 10)
        {
            newobj.AddComponent<SkillObject10>();
        }
        else if (skill.effects == 6)
        {
            newobj.AddComponent<SkillObject6>();
        }
        else if (skill.effects == 8)
        {
            newobj.AddComponent<SkillObject8>();
        }
        else if (skill.effects == 3)
        {
            newobj.AddComponent<SkillObject3>();
        }
        SkillObject skillComponent = newobj.GetComponent<SkillObject>();
        skillComponent.init(attacker, mLocalManager,skill, x, y, campType);
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
    public void setLoclaManager(LocalManager manager) {
        mLocalManager = manager;
    }

    private static  SkillManage mIntance = new SkillManage();
    public static SkillManage getIntance() {
        return mIntance;
    }
    private SkillManage() {

    }
}
