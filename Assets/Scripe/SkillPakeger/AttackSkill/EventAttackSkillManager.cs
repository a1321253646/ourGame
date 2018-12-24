using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventAttackSkillManager 
{
    public static int EVENT_SKILL_BEFOR_ATTACK = 1;
    public static int EVENT_SKILL_END_EHURT = 2;
    public static int EVENT_SKILL_BEFORE_HURT = 3;
    public static int EVENT_SKILL_BEFOR_BEHURT = 4;
    public static int EVENT_SKILL_END_BEHURT = 5;
    public static int EVENT_SKILL_HURT_DIE = 6;
    public static int EVENT_SKILL_BEFOE_CARD_COST = 7;
    public static int EVENT_SKILL_BEFORE_CARD_HURT = 8;
    public static int EVENT_SKILL_END_CARD_HURT = 9;
    public static int EVENT_SKILL_BEFORE_CARD_BEHURT = 10;
    public static int EVENT_SKILL_END_CARD_BEHURT = 11;
    public static int EVENT_SKILL_END_DIE_HUIJING = 12;
    public static int EVENT_SKILL_BEFORE_DROP = 13;
    public static int EVENT_SKILL_END_DROP = 14;
    public static int EVENT_SKILL_ATTACKING = 15;

    Dictionary<long, EventAttackSkillManagerSingle> mManagerList = new Dictionary<long, EventAttackSkillManagerSingle>();
    public List<TimeAttackSkillBase> mTimeList = new List<TimeAttackSkillBase>();
    public List<TimeEventAttackSkillBase> mTimeEvemntList = new List<TimeEventAttackSkillBase>();


    public void registerTimeEventSkill(TimeEventAttackSkillBase skill)
    {
        mTimeEvemntList.Add(skill);
    }
    public void unRegisterTimeEventSkill(TimeEventAttackSkillBase skill)
    {
        mTimeEvemntList.Remove(skill);
    }

    public void registerTimeSkill(TimeAttackSkillBase skill) {
        mTimeList.Add(skill);
    }
    public void unRegisterTimeSkill(TimeAttackSkillBase skill) {
        mTimeList.Remove(skill);
    }

    public void updateSkill() {
        for (int i = 0; i < mTimeList.Count; i++) {
            mTimeList[i].upDateSkill();
        }
        for (int i = 0; i < mTimeEvemntList.Count; i++)
        {
            mTimeEvemntList[i].upDateSkill();
        }
    }
    public void upDateLocal(float x, float y)
    {
        foreach (TimeAttackSkillBase skill in mTimeList)
        {
            skill.upDateLocal(x,y);
        }
    }
    public void register(int eventType, EventAttackSkill skill) {
        if (mManagerList.ContainsKey(eventType))
        {
            mManagerList[eventType].register(skill);
        }
        else {
            EventAttackSkillManagerSingle signle = new EventAttackSkillManagerSingle();
            signle.register(skill);
            mManagerList.Add(eventType, signle);
        }
    }
    public void unRegister(int eventType, EventAttackSkill skill) {
        mManagerList[eventType].unRegister(skill);
    }

    //111 为 无视防御

    public virtual List<EquipKeyAndValue> beforeActtack()//用于影响改成平A攻击暴击，闪避等等影响到平A的判定
    {
        List<EquipKeyAndValue> list = new List<EquipKeyAndValue>();
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFOR_ATTACK);
        if (singel == null || singel.mList.Count == 0) {
            return null;
        }
        else {
            foreach (EventAttackSkill skill in singel.mList) {
                EquipKeyAndValue value = skill.beforeActtack();
                if (value != null) {
                    list.Add(value);
                }
                
            }
            return list;
        }
    }
    public virtual void Acttacking()//判断攻击生效后进行调用
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_ATTACKING);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {
                skill.Acttacking();
            }
        }
        return ;
    }
    public virtual void beforeHurt(HurtStatus hurt)//造成伤害前
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFORE_HURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {
                skill.beforeHurt(hurt);
            }
        }
        return ;
    }
    public virtual void endHurt(HurtStatus hurt)//造成伤害后
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_EHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {
                skill.endHurt(hurt);
            }
        }
        return ;
    }
    public virtual void beforeBeHurt(HurtStatus hurt)//被普通伤害前
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFOR_BEHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {
                skill.beforeBeHurt(hurt);
            }
        }
        return ;
    }
    public virtual void endBeHurt(HurtStatus hurt)//被普通伤害后
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_BEHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {
                skill.endBeHurt(hurt);
            }
        }
        return ;
    }
    public virtual void killEnemy()//平a杀死怪物
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_HURT_DIE);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {
                skill.killEnemy();
            }
        }
        return ;
    }
    public virtual int getCardCost(int cost)//获取卡牌消耗
    {
        int cost2 = cost;
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFOE_CARD_COST);
        if (singel == null || singel.mList.Count == 0)
        {
            return cost2;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {
                cost2 = skill.getCardCost(cost2);
            }
        }
        return cost2;
    }
    public virtual float beforeCardHurt(float hurt)//卡牌伤害前
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFORE_CARD_HURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return hurt;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return hurt;
    }

    public virtual float endCardHurt(float hurt)//卡牌伤害后
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_CARD_HURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return hurt;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return hurt;
    }

    public virtual float beforeBeCardHurt(float hurt)//被卡牌伤害前
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFORE_CARD_BEHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return hurt;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return hurt;
    }

    public virtual float endBeCardHurt(float hurt)//被卡牌伤害后
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_CARD_BEHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return hurt;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return hurt;
    }
    public virtual BigNumber getDieHuijing(BigNumber big)//获取死亡魂晶
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_DIE_HUIJING);
        if (singel == null || singel.mList.Count == 0)
        {
            return big;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return big;
    }
    public virtual bool beforeGetDrop()//掉落前判断
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFORE_DROP);
        if (singel == null || singel.mList.Count == 0)
        {
            return false;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return false;
    }
    public virtual bool endGetDrop()//掉落后判断
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_DROP);
        if (singel == null || singel.mList.Count == 0)
        {
            return false;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return false;
    }




    private EventAttackSkillManagerSingle getSignle(int eventType) {
        if (mManagerList.ContainsKey(eventType)) {
            return mManagerList[eventType];
        }
        return null;
    }
}
