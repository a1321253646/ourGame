using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkillManager 
{

    public long cardDownCardCost = 0;
    public float carHurtPre = 0;
    public float cardCardHurtPre = 0;

    public long lunhuiDownCardCost = 0;
    public float lunhuiHurtPre = 0;
    public float lunhuiCardHurtPre = 0;

    private Attacker mAttack;
    private Attacker mAttackFight;
    private GameObject mSkillObject;
    private List<AttackSkillWithAnimal> mAnimalActionSkill = new List<AttackSkillWithAnimal>();
    public List<AttackSkillNoAnimal> mNoAnimalActionSkill = new List<AttackSkillNoAnimal>();
    private List<AttackSkillNoAnimal> beforeBehurtActionSkill = new List<AttackSkillNoAnimal>();
    private List<AttackSkillNoAnimal> inFightActionSkill = new List<AttackSkillNoAnimal>();
    private Dictionary<PlayerBackpackBean, List<PackbackSkillBase>> mBackpackSkill = new Dictionary<PlayerBackpackBean, List<PackbackSkillBase>>();

    public long getDownCardCost() {
        return cardDownCardCost + lunhuiDownCardCost;
    }
    public float getHurtPre() {
        return 1 + carHurtPre + lunhuiCardHurtPre;
    }
    public float getCardHurtPre()
    {
        return 1 + cardCardHurtPre + lunhuiCardHurtPre;
    }


    public AttackSkillManager(Attacker attack) {
        mAttack = attack;
        mSkillObject  = GameObject.Find("Manager").GetComponent<LevelManager>().skillObject;
    }

    public void addSkill(SkillJsonBean skill, Attacker mAttackFight)
    {
        Debug.Log("AttackSkillManager skill.id="+ skill.id);
        if (isNoAnimal(skill.id))
        {
            addNoAnimal(skill, mAttackFight);
            return;
        }
        Point skillP = JsonUtils.getIntance().getEnemyResourceData(skill.skill_resource).getHurtOffset();
        Point attackP = mAttack.resourceData.getHurtOffset();

        float xDel=0, yDel=0;
        if (skill.point_type == 1)
        {
            yDel = mAttack.resourceData.getTargetBorder()[2];
        }
        else if (skill.point_type == 2)
        {
            yDel = mAttack.resourceData.getTargetBorder()[2] / 2;
        }
        else {
            yDel = 0;
        }
        Debug.Log("attackP.x =" + attackP.x);

        GameObject newobj = GameObject.Instantiate(
                mSkillObject,
                new Vector2(mAttack.transform.position.x + attackP.x+ xDel - skillP.x,
                            mAttack.transform.position.y + mAttack.resourceData.idel_y+ yDel - skillP.y),
                Quaternion.Euler(0.0f, 0f, 0.0f));
        dealSkillType(newobj, skill.id, mAttackFight, skill);
    }


    public void addSkill(long skillId, Attacker mAttackFight) {

        SkillJsonBean skill =JsonUtils.getIntance().getSkillInfoById(skillId);
        addSkill(skill, mAttackFight);
    }
    public void updateSkill(PlayerBackpackBean bean, Attacker mAttackFight)
    {
        removeSkill(bean,mAttackFight);
        addSkill(bean, mAttackFight);
    }
    public void addSkill(PlayerBackpackBean bean, Attacker mAttackFight) {
        Debug.Log("addSkill PlayerBackpackBean");
        List<PackbackSkillBase> list = new List<PackbackSkillBase>();
        if (!(mAttack is PlayControl)) {
            return;
        }
        long level = 0;
        foreach (PlayerAttributeBean p in bean.attributeList)
        {
            if (p.type == 10001) {
                level = (long) p.value;
            }
        }
        long count = JsonUtils.getIntance().getAffixEnbleByLevel(level);
        foreach (PlayerAttributeBean p in bean.attributeList) {
            if (count == 0) {
                break;
            }
            if (p.type == 10002)
            {
                long id = p.value;
                long value = 0;
                Debug.Log("addSkill PlayerBackpackBean id= "+ id);
                foreach (PlayerAttributeBean p2 in bean.attributeList)
                {
                    if (p2.type == id) {
                        value = p2.value;
                        Debug.Log("addSkill PlayerBackpackBean value= " + value);
                        break;
                    }
                }
                PackbackSkillBase pb = creatPackbackSkill(id, value);
                pb.startSkill();
                list.Add(pb);
                count--;
            }
            
        }
        
        if(list != null) {
            mBackpackSkill.Add(bean, list);
            if (mAttack is PlayControl) {
                ((PlayControl)mAttack).getAttribute();
            }
        }
    }

    private PackbackSkillBase creatPackbackSkill(long id, long value) {
        PackbackSkillBase p = null;
        if (id == 400001)
        {
            p = new PackbackSkill400001();
        }
        else if (id == 400002) {
            p = new PackbackSkill400002();
        }
        else if (id == 400003)
        {
            p = new PackbackSkill400003();
        }
        else if (id == 400004)
        {
            p = new PackbackSkill400004();
        }
        else if (id == 400005)
        {
            p = new PackbackSkill400005();
        }
        else if (id == 400006)
        {
            p = new PackbackSkill400006();
        }
        else if (id == 400007)
        {
            p = new PackbackSkill400007();
        }
        else if (id == 400008)
        {
            p = new PackbackSkill400008();
        }
        if (p != null) {
            p.init(this, value, mAttack);
        }
        return p;

    }
    public void removeSkill(PlayerBackpackBean bean, Attacker mAttackFight) {
        if (mBackpackSkill.ContainsKey(bean)) {
            List<PackbackSkillBase> list = mBackpackSkill[bean];
            foreach (PackbackSkillBase b in list) {
                b.removeSkill();
            }
            mBackpackSkill.Remove(bean);
        }
    }

    private void addNoAnimal(SkillJsonBean jsonBean, Attacker mAttackFight)
    {
        Debug.Log("addNoAnimal jsonBean id = "+ jsonBean.id);
        AttackSkillNoAnimal skill = null;
        int i = 0;
        if (mNoAnimalActionSkill.Count > 0)
        {        
            for (; i < mNoAnimalActionSkill.Count; i++) {
                AttackSkillNoAnimal s = mNoAnimalActionSkill[i];
                if (s.mSkillJson.effects == jsonBean.effects)
                {                  
                    break;
                }
            }     

        }
        if (i >= mNoAnimalActionSkill.Count) {
            i = -1;
        }
        if (jsonBean.effects == 20001)
        {
            int count = (int)jsonBean.getSpecialParameterValue()[0];

            if (i != -1)
            {
                mNoAnimalActionSkill[i].add(count);
            }
            else
            {
                skill = new AttackSkill20001();
                skill.init(this, jsonBean.id, mAttackFight);
                mNoAnimalActionSkill.Add(skill);
                beforeBehurtActionSkill.Add(skill);
                skill.add(count);
            }
        }
        else if (jsonBean.effects == 10003) {
            int time = (int)jsonBean.getSpecialParameterValue()[0];
            if (i != -1)
            {
                mNoAnimalActionSkill[i].add(time);
            }
            else
            {
                skill = new AttackSkill10003();
                skill.init(this, jsonBean.id, mAttackFight);
                mNoAnimalActionSkill.Add(skill);
                inFightActionSkill.Add(skill);
                skill.add(time);
            }
        }

    }

    private bool isNoAnimal(long skillId) {
        if (skillId == 200001 ||
            skillId == 300001 ||
            skillId == 200003) {
            return true;
        }
        return false;
    }

    private void dealSkillType(GameObject newobj,long skillId, Attacker mAttackFight, SkillJsonBean skill)
    {
        if (skill.effects == 10001)
        {
            newobj.AddComponent<AttackSkill10001>();
            AttackSkillWithAnimal skillComponent = newobj.GetComponent<AttackSkillWithAnimal>();
            skillComponent.init(this, skillId, mAttackFight);
            mAnimalActionSkill.Add(skillComponent);
        }
        else if (skill.effects == 2)
        {
            newobj.AddComponent<AttackSkill2>();
            AttackSkillWithAnimal skillComponent = newobj.GetComponent<AttackSkillWithAnimal>();
            skillComponent.init(this, skillId, mAttackFight);
            mAnimalActionSkill.Add(skillComponent);
        }
        else if (skill.effects == 10002)
        {
            int i = 0;
            if (mAnimalActionSkill.Count > 0)
            {
                for (; i < mAnimalActionSkill.Count; i++)
                {
                    AttackSkillWithAnimal s = mAnimalActionSkill[i];
                    if (s.mSkillJson.effects == skill.effects)
                    {
                        break;
                    }
                }

            }
            int time = (int)skill.getSpecialParameterValue()[0];

            if (i != mAnimalActionSkill.Count)
            {
                mAnimalActionSkill[i].add(time);
            }
            else
            {
                newobj.AddComponent<AttackSkill10002>();
                AttackSkillWithAnimal skillComponent = newobj.GetComponent<AttackSkillWithAnimal>();
                skillComponent.init(this, skillId, mAttackFight);
                mAnimalActionSkill.Add(skillComponent);
                skillComponent.add(time);
            }
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
        foreach (AttackSkillNoAnimal skill in inFightActionSkill)
        {
            skill.inAction();
        }
    }
    public void upDate() {
        removeAnumalDieSkill(mAnimalActionSkill);
        removeNoAnumalDieSkill(mNoAnimalActionSkill);
        removeNoAnumalDieSkill(beforeBehurtActionSkill);
        removeNoAnumalDieSkill(inFightActionSkill);
        
    }
    private void upDateNoAnumalDieSkill(List<AttackSkillNoAnimal> list)
    {
        for (int i = 0; i < list.Count;)
        {
            list[i].update();
        }
    }
    private void removeNoAnumalDieSkill(List<AttackSkillNoAnimal> list)
    {
        for (int i = 0; i < list.Count;)
        {
            if (list[i].getSkillStatus() == AttackSkillBase.SKILL_STATUS_END)
            {
                list.Remove(list[i]);
            }
            else
            {
                list[i].update();
                i++;
            }
        }
    }
    private void removeAnumalDieSkill(List<AttackSkillWithAnimal> list) {
        for (int i = 0; i < list.Count;)
        {
            if (list[i].getSkillStatus() == AttackSkillBase.SKILL_STATUS_END)
            {
                list.Remove(list[i]);
            }
            else
            {
                list[i].update();
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
      /*  for (int i = 0; i < beforeBehurtActionSkill.Count;)
        {
            if (beforeBehurtActionSkill[i].getSkillStatus() == AttackSkillBase.SKILL_STATUS_END)
            {
                AttackSkillNoAnimal skill = beforeBehurtActionSkill[i];
                beforeBehurtActionSkill.Remove(skill);
                mNoAnimalActionSkill.Remove(skill);
            }
        }*/
    }
    public float getValueBySkillAndId(long skillId, long statusId) {
        foreach (AttackSkillNoAnimal skill in mNoAnimalActionSkill) {
            if (skill.mSkillJson.id == skillId) {
                return skill.getValueById(statusId);
            }
        }
        foreach (AttackSkillWithAnimal skill in mAnimalActionSkill)
        {
            if (skill.mSkillJson.id == skillId)
            {
                return skill.getValueById(statusId);
            }
        }
        return 0;
    }
}
