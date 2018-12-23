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
        foreach (TimeAttackSkillBase skill in mTimeList) {
            skill.upDateSkill();
        }
        foreach (TimeEventAttackSkillBase skill in mTimeEvemntList)
        {
            skill.upDateSkill();
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

    public virtual EquipKeyAndValue beforeActtack()//用于影响改成平A攻击暴击，闪避等等影响到平A的判定
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFOR_ATTACK);
        if (singel == null || singel.mList.Count == 0) {
            return null;
        }
        else {
            foreach (EventAttackSkill skill in singel.mList) {

            }
        }
        return null;
    }
    public virtual void Acttacking()
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

            }
        }
        return ;
    }
    public virtual void beforeHurt(HurtStatus hurt)
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

            }
        }
        return ;
    }
    public virtual void endHurt(HurtStatus hurt)
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

            }
        }
        return ;
    }
    public virtual void beforeBeHurt(HurtStatus hurt)
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

            }
        }
        return ;
    }
    public virtual void endBeHurt(HurtStatus hurt)
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

            }
        }
        return ;
    }
    public virtual void killEnemy()
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

            }
        }
        return ;
    }
    public virtual int getCardCost(int cost)
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFOE_CARD_COST);
        if (singel == null || singel.mList.Count == 0)
        {
            return cost;
        }
        else
        {
            foreach (EventAttackSkill skill in singel.mList)
            {

            }
        }
        return cost;
    }
    public virtual float beforeCardHurt(float hurt)
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

    public virtual float endCardHurt(float hurt)
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

    public virtual float beforeBeCardHurt(float hurt)
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

    public virtual float endBeCardHurt(float hurt)
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
    public virtual BigNumber getDieHuijing(BigNumber big)
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
    public virtual bool beforeGetDrop()
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
    public virtual bool endGetDrop()
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
