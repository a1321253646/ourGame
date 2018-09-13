using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkillManager 
{
    private Attacker mAttack;
    private Attacker mAttackFight;
    private GameObject mSkillObject;
    private List<AttackSkillWithAnimal> mAnimalActionSkill = new List<AttackSkillWithAnimal>();
    private List<AttackSkillNoAnimal> mNoAnimalActionSkill = new List<AttackSkillNoAnimal>();
    private List<AttackSkillNoAnimal> beforeBehurtActionSkill = new List<AttackSkillNoAnimal>();
    public AttackSkillManager(Attacker attack) {
        mAttack = attack;
        mSkillObject  = GameObject.Find("Manager").GetComponent<LevelManager>().skillObject;
    }

    public void addSkill(long skillId, Attacker mAttackFight) {

        SkillJsonBean skill =JsonUtils.getIntance().getSkillInfoById(skillId);
        if (isNoAnimal(skill.id))
        {
            addNoAnimal(skill, mAttackFight);
            return;
        }
        Point skillP = JsonUtils.getIntance().getEnemyResourceData(skill.skill_resource).getFightOffset();
        Point attackP = mAttack.resourceData.getFightOffset();

        GameObject newobj = GameObject.Instantiate(
                mSkillObject, 
                new Vector2(mAttack.transform.position.x+ attackP.x-skillP.x,
                            mAttack.transform.position.y + attackP.y - skillP.y), 
                Quaternion.Euler(0.0f, 0f, 0.0f));
        dealSkillType(newobj,skillId, mAttackFight, skill);
    }

    private void addNoAnimal(SkillJsonBean jsonBean, Attacker mAttackFight)
    {
        foreach (AttackSkillNoAnimal skill in mNoAnimalActionSkill)
        {
            if (skill.mSkillJson.id == jsonBean.id && skill.add())
            {
                return;
            }
        }
        if (jsonBean.effects == 20001)
        {
            AttackSkillNoAnimal newSkill = new AttackSkill20001();
            newSkill.init(this, jsonBean.id, mAttackFight);
            mNoAnimalActionSkill.Add(newSkill);
            beforeBehurtActionSkill.Add(newSkill);
        }
        else if (jsonBean.effects == 30001)
        {
            AttackSkillNoAnimal newSkill = new AttackSkill4();
            newSkill.init(this, jsonBean.id, mAttackFight);
            mNoAnimalActionSkill.Add(newSkill);
            beforeBehurtActionSkill.Add(newSkill);
        }
    }

    private bool isNoAnimal(long skillId) {
        if (skillId == 200001 ||
            skillId == 300001) {
            return true;
        }
        return false;
    }

    private void dealSkillType(GameObject newobj,long skillId, Attacker mAttackFight, SkillJsonBean skill)
    {
        if (skill.effects == 10001 )
        {
            newobj.AddComponent <AttackSkill10001>();
            AttackSkillWithAnimal skillComponent = newobj.GetComponent<AttackSkillWithAnimal>();
            skillComponent.init(this, skillId, mAttackFight);
            mAnimalActionSkill.Add(skillComponent);
        }
    }
    public Attacker getAttacker() {
        return mAttack;
    }
    public void beforeBeHurt(HurtStatus status) {
        float value = 0;
        foreach (AttackSkillNoAnimal skill in beforeBehurtActionSkill) {
            value += skill.beAction(status);
        }
        status.blood += value;
    }
    public void inFight() {

    }
    public void upDate() {
        for(int i = 0; i< mAnimalActionSkill.Count; ) {
            if (mAnimalActionSkill[i].getSkillStatus() == AttackSkillBase.SKILL_STATUS_END)
            {
                mAnimalActionSkill.Remove(mAnimalActionSkill[i]);
            }
            else {
                mAnimalActionSkill[i].update();
                i++;
            }
        }
    }

    public void upDateLocal(float x,float y )
    {
        for (int i = 0; i < mAnimalActionSkill.Count;)
        {
            if (mAnimalActionSkill[i].getSkillStatus() == AttackSkillBase.SKILL_STATUS_END)
            {
                mAnimalActionSkill.Remove(mAnimalActionSkill[i]);
            }
            else
            {
                mAnimalActionSkill[i].upDateLocal(x, y);
                i++;
            }
        }
        for (int i = 0; i < beforeBehurtActionSkill.Count;)
        {
            if (beforeBehurtActionSkill[i].getSkillStatus() == AttackSkillBase.SKILL_STATUS_END)
            {
                AttackSkillNoAnimal skill = beforeBehurtActionSkill[i];
                beforeBehurtActionSkill.Remove(skill);
                mNoAnimalActionSkill.Remove(skill);
            }
        }
    }
    public float getValueBySkillAndId(long skillId, long statusId) {
        return -1;
    }
}
