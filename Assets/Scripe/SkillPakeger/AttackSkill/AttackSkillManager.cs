using UnityEngine;
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
    private Dictionary<long, AttackerSkillBase> mLunhui = new Dictionary<long, AttackerSkillBase>();
    private Dictionary<long, AttackerSkillBase> mVocation = new Dictionary<long, AttackerSkillBase>();


    public long cardDownCardCost = 0;
    public long lunhuiDownCardCost = 0;

    public MultipleAffixBean carHurtPre = new MultipleAffixBean();
    public MultipleAffixBean cardCardHurtPre = new MultipleAffixBean();

    public MultipleAffixBean lunhuiHurtPre = new MultipleAffixBean();
    public MultipleAffixBean lunhuiCardHurtPre = new MultipleAffixBean();


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
    public double getHurtPre()
    {
        return (carHurtPre.getValue() * lunhuiHurtPre.getValue());
    }
    public double getCardHurtPre()
    {
        return (cardCardHurtPre.getValue() * lunhuiCardHurtPre.getValue());
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

    public bool addLunhuiKill(long skillId, Attacker fighter,long index) {
        if (mLunhui.ContainsKey(skillId))
        {

            mLunhui[skillId].addValue(0);
        }
        else {
//            Debug.Log("addLunhuiKill skillId= " + skillId);
            AttackerSkillBase skill = creatSkillById(skillId, null, fighter, false, index);
            if (skill == null) {
                return false;
            }
            mLunhui.Add(skillId, skill);
        }
        return true;
    }

    public void removeAllLunhuiSkill()
    {
        List<long> list = new List<long>();
        foreach (long bean in mLunhui.Keys)
        {
            list.Add(bean);
        }
        while (list.Count > 0)
        {
            if (mLunhui.ContainsKey(list[0])) {
                mLunhui[list[0]].endSkill();
                mLunhui.Remove(list[0]);
            }

            list.RemoveAt(0);

        }
    }
    public void addVocationSkill(long skillId, Attacker fighter, bool isGiveUp, long index) {
        if (getAttacker().getStatus() == ActionFrameBean.ACTION_DIE)
        {
            return;
        }
        SkillJsonBean skill = JsonUtils.getIntance().getSkillInfoById(skillId);
        if (!mVocation.ContainsKey(skillId)) {
            AttackerSkillBase bean = creatSkillById(skill.id, skill.getSpecialParameterValue(), fighter, false, index);
            mVocation.Add(skillId, bean);
        }      
    //    Debug.Log("===============addVocationSkill id=" + skillId);
    }

    public void removeAllVocationSkill() {
        foreach (long id in mVocation.Keys) {
      //      Debug.Log("===============removeAllVocationSkill id=" + id);
            mVocation[id].endSkill();
        }
        mVocation.Clear();
    }

    public void addSkill(long skillId, Attacker fighter,bool isGiveUp,long index) {
        if (getAttacker().getStatus() == ActionFrameBean.ACTION_DIE)
        {
            return;
        }
        SkillJsonBean skill = JsonUtils.getIntance().getSkillInfoById(skillId);
        addSkill(skill, fighter, isGiveUp, index);

        Debug.Log("===============addSkill id=" + skillId);
    }
    public void removeSkill(long skillId) {
        Debug.Log("===============removeSkill id=" + skillId);
        if (mIdSkill.ContainsKey(skillId)) {
            mIdSkill[skillId].endSkill();
            mIdSkill.Remove(skillId);
        }
    }


    public void addSkill(long skillId, Attacker fighter,long index)
    {
        addSkill(skillId, fighter, false,index);
    }

    public void addSkill(PlayerBackpackBean bean, Attacker fighter,long index) {
        if (mBackpackSkill.ContainsKey(bean)) {
            removeSkill(bean);
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
        List<AttackerSkillBase> list = creatSkillByAffix(list2, fighter,index);
        if (list != null) {
            mBackpackSkill.Add(bean, list);
        }
    }
    public void removeAllSkill() {
        List<PlayerBackpackBean> list = new List<PlayerBackpackBean>();
        foreach (PlayerBackpackBean bean in mBackpackSkill.Keys) {
            list.Add(bean);
        }
        while (list.Count > 0)
        {
            PlayerBackpackBean bean = list[0];
            removeSkill(bean);
            list.Remove(bean);
        }

        List<long> list2 = new List<long>();
        foreach (long id in mIdSkill.Keys)
        {
            list2.Add(id);
        }
        while (list2.Count > 0)
        {
            long bean = list2[0];
            removeSkill(bean);
            list2.Remove(bean);
        }
        List<PetJsonBean> list3 = new List<PetJsonBean>();
        foreach (PetJsonBean id in mPetSkill.Keys)
        {
            list3.Add(id);
        }
        while (list3.Count > 0)
        {
            PetJsonBean bean = list3[0];
            removeSkill(bean);
            list3.Remove(bean);
        }

    }

    public void addSkill(SkillJsonBean json, Attacker fighter,bool isGiveup,long index)
    {
        if (getAttacker().getStatus() == ActionFrameBean.ACTION_DIE) {
            return;
        }

        AttackerSkillBase skill;
        if (mIdSkill.ContainsKey(json.id))
        {
            skill = mIdSkill[json.id];
            if (skill.mStatus == AttackerSkillBase.SKILL_STATUS_END)
            {
                mIdSkill.Remove(json.id);
                skill = creatSkillById(json.id, json.getSpecialParameterValue(), fighter, isGiveup, index);
                mIdSkill.Add(json.id, skill);
            }
            else
            {
                skill.add(json.getSpecialParameterValue(), isGiveup);
            }
        }
        else
        {
            skill = creatSkillById(json.id, json.getSpecialParameterValue(), fighter, isGiveup, index);
            mIdSkill.Add(json.id, skill);
        }
    }
    public void addSkill(SkillJsonBean json, Attacker fighter,long index) {
        addSkill(json, fighter, false,index);
    }



    public void addSkill(List<PlayerAttributeBean> list, Attacker fighter,long index) {       
        creatSkillByAffix(list, fighter,index);
        mAttack.getAttribute(true);
    }


    public void addSkill(PetJsonBean pet, Attacker fighter,long index) {
        //List<AttackerSkillBase> list = creatSkillByAffix(pet.getAffixList(), fighter);
        // if (list == null)
        // {
           List<AttackerSkillBase> list = new List<AttackerSkillBase>();
        // }
        Debug.Log("=====================addSkill pet.skillId=" + pet.skillId+ "  index = "+ index);
        if (pet.skillId != 0) {
            AttackerSkillBase skill = creatSkillById(pet.skillId, null, fighter,false,index);
            list.Add(skill);
        }
        if (list.Count > 0) {
            mPetSkill.Add(pet, list);
        }
        Debug.Log("addSkill PetJsonBean");
        mAttack.getAttribute(true);
    }

    public void removeSkill(PetJsonBean pet) {
        if (mPetSkill.ContainsKey(pet)) {
            endSkill(mPetSkill[pet]);
            mPetSkill.Remove(pet);
        }
        Debug.Log("addSkill PetJsonBean");
        mAttack.getAttribute(true);
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
    private List<AttackerSkillBase> creatSkillByAffix(List<PlayerAttributeBean> affixList, Attacker fighter,long index) {
        List<AttackerSkillBase> list = new List<AttackerSkillBase>();
        foreach (PlayerAttributeBean bean in affixList)
        {
            Debug.Log("creatSkillByAffix  bean.type=" + bean.type + " bean.value=" + bean.value);
            if (bean.type < 1000)
            {
                if (mAttack is PlayControl) {
                    ((PlayControl)mAttack).addPetAttribute(bean);
                }
            }
            else {
                AttackerSkillBase skill = creatSkillById(bean.type, new List<float>() { (float)bean.value }, fighter, false,index);
                if (skill != null)
                {
                    list.Add(skill);
                }
            }

        }
        if (list.Count == 0)
        {
            return null;
        }
        return list;
    }
    private AttackerSkillBase creatSkillById(long id, List<float> value, Attacker fighter,bool isGiveups,long skillIndex)
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
            skill.initSkill(this, id, fighter, value, newobj, isGiveups, skillIndex);
        }
        else {
            skill.initSkill(this, id, fighter, value, null, isGiveups, skillIndex);
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
