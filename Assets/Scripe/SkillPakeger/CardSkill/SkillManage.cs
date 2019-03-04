using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManage
{
    List<SkillObject> mSkillList = new List<SkillObject>();
    public GameObject mSkillObject;
    public LocalManager mLocalManager;
    BackgroundManager mBackManager;
    public void addSkill(Attacker attacker, SkillJsonBean skill, float x, float y, int campType, bool isGiveup,bool isBoss, long skillIndex) {
        Debug.Log("addSkill x=" + x + " y = " + y);
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(skill.skill_resource);
        GameObject newobj = GameObject.Instantiate(
                mSkillObject, new Vector2(x - bean.getHurtOffset().x, y - bean.getHurtOffset().y), Quaternion.Euler(0.0f, 0f, 0.0f));
        dealSkillObject(attacker, newobj, skill, x, y, campType, isGiveup, isBoss,skillIndex);
    }
    //,
   // 
    public void bossAddSkill(Attacker attacker,Attacker hero ,SkillJsonBean skill, int campType, long skillIndex) {
        //addSkill(attacker, skill, hero.mLocalBean.mCurrentX, hero.mLocalBean.mCurrentY, campType,false,true, skillIndex);
        addSkill(attacker, skill, hero.mLocalBean.mCurrentX + hero.resourceData.getHurtOffset().x, hero.mLocalBean.mCurrentY + hero.resourceData.getHurtOffset().y, campType,false,true, skillIndex);
    }

    public void addSkill(Attacker attacker, SkillJsonBean skill,float x,float y,int campType, long skillIndex) {
        addSkill(attacker, skill, x, y, campType, false,false, skillIndex);
    }

    private void dealSkillObject(Attacker attacker, GameObject newobj, SkillJsonBean skill, float x, float y, int campType, bool isGiveup, bool isBoss,long skillIndex) {
        SkillFactory.skillObjectAddComponet(newobj, skill);
        SkillObject skillComponent = newobj.GetComponent<SkillObject>();
        skillComponent.init(attacker, mLocalManager,skill, x, y, campType,isGiveup, isBoss, skillIndex);
        mSkillList.Add(skillComponent);
    }

    public void init() {
        mSkillList.Clear();
    }
    public void update() {
        if (mSkillList.Count == 0) {
            return;
        }
        float x = mBackManager.moveSpeed * Time.deltaTime;
        for (int i = 0; i < mSkillList.Count;)
        {
            if (mSkillList[i].getStatus() == SkillObject.SKILL_STATUS_END)
            {
                mSkillList.Remove(mSkillList[i]);
            }
            else
            {
                if (mBackManager.isRun)
                {
                    mSkillList[i].updateLocal(x);
                }               
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
    public void setBackManagerManager(BackgroundManager manager)
    {
        mBackManager = manager;
    }
    private static  SkillManage mIntance = new SkillManage();
    public static SkillManage getIntance() {
        return mIntance;
    }
    private SkillManage() {

    }
}
