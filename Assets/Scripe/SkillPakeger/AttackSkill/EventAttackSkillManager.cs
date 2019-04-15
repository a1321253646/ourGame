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
    public static int EVENT_SKILL_MONSTER_DEBUFF = 16;
    public static int EVENT_SKILL_ALL_HURT = 17;
    public static int EVENT_SKILL_BOSS_DEBUFF = 18;
    public static int EVENT_SKILL_LITTER_DEBUFF = 19;
    public static int EVENT_SKILL_BEFOOR_DIE = 20;
    public static int EVENT_SKILL_PING_A = 21;

    Dictionary<long, EventAttackSkillManagerSingle> mManagerList = new Dictionary<long, EventAttackSkillManagerSingle>();
    public List<TimeAttackSkillBase> mTimeList = new List<TimeAttackSkillBase>();
    public List<TimeEventAttackSkillBase> mTimeEvemntList = new List<TimeEventAttackSkillBase>();


    public void removeAll() {

        for (int i = 0; i < mTimeList.Count; ) {
            mTimeList[i].endSkill();
        }
        mTimeList.Clear();
        foreach (long key in mManagerList.Keys) {
            EventAttackSkillManagerSingle singel = mManagerList[key];
            if (singel != null && singel.mList.Count > 0)
            {
                for (int i = 0; i < singel.mList.Count;)
                {
                    singel.mList[i].endSkill();
                }
                singel.mList.Clear();
            }
        }
        mManagerList.Clear();
        for (int i = 0; i < mTimeEvemntList.Count;)
        {
            mTimeEvemntList[i].endSkill();
        }
        mManagerList.Clear();
    }

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
        for (int i = 0; i < mTimeList.Count; i++) {
            TimeAttackSkillBase skill = mTimeList[i];
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
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
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
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.Acttacking();
            }
        }
        return ;
    }
    public virtual void beforeHurt(HurtStatus hurt,Attacker attacker)//造成伤害前
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFORE_HURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.beforeHurt(hurt, attacker);
            }
        }
        return ;
    }
    public virtual float afterPinga()//造成伤害前
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_PING_A);
        float value = 1;
        if (singel == null || singel.mList.Count == 0)
        {
            return value;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                value = value * skill.afterPinga();
            }
        }
        return value;
    }
    public virtual void endHurt(HurtStatus hurt, Attacker attacker)//造成伤害后
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_EHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.endHurt(hurt, attacker);
            }
        }
        return ;
    }
    public virtual bool allHurt(Attacker fighter, HurtStatus hurt)//掉落前判断
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_ALL_HURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.allHurt(fighter, hurt);
            }
        }
        return false;
    }
    public virtual void beforeBeHurt(Attacker fighter, HurtStatus hurt)//被普通伤害前
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFOR_BEHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.beforeBeHurt(fighter,hurt);
            }
        }
        return ;
    }

    public virtual void endBeHurt(Attacker fighter, HurtStatus hurt)//被普通伤害后
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_END_BEHURT);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.endBeHurt(fighter,hurt);
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
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
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
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
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
    public void debuffMonster(Attacker monster)
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_MONSTER_DEBUFF);
        if (singel == null || singel.mList.Count == 0)
        {
            return ;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.debuffMonster(monster);
            }
        }
    }
    public void debuffLitterMonster(Attacker monster)
    {
//        Debug.Log("debuffLitterMonster id== "+((EnemyBase)monster).mData.id);
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_LITTER_DEBUFF);
        if (singel == null || singel.mList.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                
                EventAttackSkill skill = singel.mList[i];

                if (skill.mSkillJson != null)
                {
//                    Debug.Log("debuffLitterMonster skill id== " + skill.mSkillJson.id);
                }
                else {
//                    Debug.Log(skill +" mSkillJson == null");
                }
                
                skill.debuffLitterMonster(monster);
            }
        }
    }
    public void debuffBoss(Attacker monster)
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BOSS_DEBUFF);
        if (singel == null || singel.mList.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.debuffBoss(monster);
            }
        }
    }
    public void befroeDie()
    {
        EventAttackSkillManagerSingle singel = getSignle(EVENT_SKILL_BEFOOR_DIE);
        if (singel == null || singel.mList.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                skill.beforeDie();
            }
        }
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
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                big = skill.getDieHuijing(big);
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
            for (int i = 0; i < singel.mList.Count; i++)
            {
                EventAttackSkill skill = singel.mList[i];
                if (skill.endGetDrop()) {
                    return true;
                }
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
