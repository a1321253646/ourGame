﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkillManager
{
    private Attacker mAttack;
    private GameObject mSkillObject;
    public EventAttackSkillManager mEventAttackManager = new EventAttackSkillManager();
    private Dictionary<PlayerBackpackBean, List<AttackerSkillBase>> mBackpackSkill = new Dictionary<PlayerBackpackBean, List<AttackerSkillBase>>();
    private Dictionary<PetJsonBean, List<AttackerSkillBase>> mPetSkill = new Dictionary<PetJsonBean, List<AttackerSkillBase>>();
    private Dictionary<long, AttackerSkillBase> mIdSkill = new Dictionary<long, AttackerSkillBase>();

    public long cardDownCardCost = 0;
    public float carHurtPre = 0;
    public float cardCardHurtPre = 0;

    public long lunhuiDownCardCost = 0;
    public float lunhuiHurtPre = 0;
    public float lunhuiCardHurtPre = 0;

    public AttackerSkillBase getAttackerById(long id) {
        if (mIdSkill.ContainsKey(id)) {
            return mIdSkill[id];
        }
        return null;
    }

    public long getDownCardCost()
    {
        return cardDownCardCost + lunhuiDownCardCost;
    }
    public float getHurtPre()
    {
        return 1 + carHurtPre + lunhuiHurtPre;
    }
    public float getCardHurtPre()
    {
        return 1 + cardCardHurtPre + lunhuiCardHurtPre;
    }

    public AttackSkillManager(Attacker attack)
    {
        mAttack = attack;
        mSkillObject = GameObject.Find("Manager").GetComponent<LevelManager>().skillObject;
    }

    public Attacker getAttacker()
    {
        return mAttack;
    }

    public void addSkill(long skillId, Attacker fighter)
    {

        SkillJsonBean skill = JsonUtils.getIntance().getSkillInfoById(skillId);
        addSkill(skill, fighter);
    }

    public void addSkill(PlayerBackpackBean bean, Attacker fighter) {
        if (mBackpackSkill.ContainsKey(bean)) {
            mBackpackSkill.Remove(bean);
        }
        long level = 0;
        foreach (PlayerAttributeBean p in bean.attributeList)
        {
            if (p.type == 10001)
            {
                level = (long)p.value;
            }
        }
        long count = JsonUtils.getIntance().getAffixEnbleByLevel(level);
        Debug.Log("=============================count=" + count);
        List<PlayerAttributeBean> list2 = new List<PlayerAttributeBean>();
        foreach (PlayerAttributeBean p in bean.attributeList)
        {
            if (count == 0)
            {
                break;
            }
            if (p.type == 10002)
            {
                long id = (long)p.value;
                long value = 0;

                foreach (PlayerAttributeBean p2 in bean.attributeList)
                {
                    if (p2.type == id)
                    {
                        value = (long)p2.value;
                        PlayerAttributeBean bean2 = new PlayerAttributeBean();
                        bean2.type = id;
                        bean2.value = value;
                        Debug.Log("=============================id=" + id+ " value="+ value);
                        list2.Add(bean2);
                        break;
                    }
                }
                count--;
            }
        }
        List<AttackerSkillBase> list = creatSkillByAffix(list2, fighter);
        if (list != null) {
            mBackpackSkill.Add(bean, list);
        }
    }
    public void addSkill(SkillJsonBean json, Attacker fighter) {
        AttackerSkillBase skill;
        if (mIdSkill.ContainsKey(json.id))
        {
            skill = mIdSkill[json.id];
            if (skill.mStatus == AttackerSkillBase.SKILL_STATUS_END)
            {
                mIdSkill.Remove(json.id);
                skill = creatSkillById(json.id, json.getSpecialParameterValue(), fighter);
                mIdSkill.Add(json.id, skill);
            }
            else
            {
                skill.add(json.getSpecialParameterValue()[0]);
            }
        }
        else {
            skill = creatSkillById(json.id, json.getSpecialParameterValue(), fighter);
            mIdSkill.Add(json.id, skill);
        }
    }

    public void addSkill(PetJsonBean pet, Attacker fighter) {
        List<AttackerSkillBase> list = creatSkillByAffix(pet.getAffixList(), fighter);
        if (list == null)
        {
            list = new List<AttackerSkillBase>();
        }
        Debug.Log("=====================addSkill pet.skillId=" + pet.skillId);
        if (pet.skillId != 0) {
         //   AttackerSkillBase skill = creatSkillById(pet.skillId, null, fighter);
       //     list.Add(skill);
        }
        if (list.Count > 0) {
            mPetSkill.Add(pet, list);
        }
        mAttack.getAttribute();
    }

    public void removeSkill(PetJsonBean pet) {
        if (mPetSkill.ContainsKey(pet)) {
            endSkill(mPetSkill[pet]);
            mPetSkill.Remove(pet);
        }
        mAttack.getAttribute();
    }
    public void removeSkill(PlayerBackpackBean bean)
    {
        if (mBackpackSkill.ContainsKey(bean))
        {
            endSkill(mBackpackSkill[bean]);
            mBackpackSkill.Remove(bean);
        }
    }

    private void endSkill(List<AttackerSkillBase> list) {
        foreach(AttackerSkillBase skill in list) {
            skill.endSkill();    
        }
    }
    private List<AttackerSkillBase> creatSkillByAffix(List<PlayerAttributeBean> affixList, Attacker fighter) {
        List<AttackerSkillBase> list = new List<AttackerSkillBase>();
        foreach (PlayerAttributeBean bean in affixList)
        {
            AttackerSkillBase skill = creatSkillById(bean.type, new List<float>() { (float)bean.value }, fighter);
            if (skill != null)
            {
                list.Add(skill);
            }
        }
        if (list.Count == 0)
        {
            return null;
        }
        return list;
    }
    private AttackerSkillBase creatSkillById(long id, List<float> value, Attacker fighter)
    {
        AttackerSkillBase skill = AttackerFactory.getSkillById(id);
        if (skill == null) {
            return null;
        }
        if (skill.isAnimal())
        {
            SkillJsonBean bean = JsonUtils.getIntance().getSkillInfoById(id);
            Point skillP = JsonUtils.getIntance().getEnemyResourceData(bean.skill_resource).getHurtOffset();
            Point attackP = mAttack.resourceData.getHurtOffset();
            float xDel = 0, yDel = 0;
            if (bean.point_type == 1)
            {
                yDel = mAttack.resourceData.getTargetBorder()[2];
            }
            else if (bean.point_type == 2)
            {
                yDel = mAttack.resourceData.getTargetBorder()[2] / 2;
            }
            else
            {
                yDel = 0;
            }
            GameObject newobj = GameObject.Instantiate(
                mSkillObject,
                new Vector2(mAttack.transform.position.x + attackP.x + xDel - skillP.x,
                    mAttack.transform.position.y + mAttack.resourceData.idel_y + yDel - skillP.y),
                    Quaternion.Euler(0.0f, 0f, 0.0f));
            Debug.Log("============================== creatSkillById=" + newobj);
            skill.initSkill(this, id, fighter, value, newobj);
        }
        else {
            skill.initSkill(this, id, fighter, value, null);
        }
        skill.startSkill();
        return skill;
    }

    public float getValueBySkillAndId(long skillId, long statusId)
    {
        if (mIdSkill.ContainsKey(skillId)) {
            return mIdSkill[skillId].getValueById(statusId);
        }
        return 0;
    }

    public void upDate() {
        mEventAttackManager.updateSkill();
    }
}
