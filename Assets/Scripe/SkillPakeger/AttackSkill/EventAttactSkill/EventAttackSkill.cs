using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EventAttackSkill : AttackerSkillBase
{
    public bool isGiveup = false;
    public override void initSkill(AttackSkillManager manager, long skillId, Attacker fight, List<float> value, GameObject newobj, bool giveup)
    {
        isGiveup = giveup;
        mManager = manager;
        mFight = fight;
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        mParam = value;
    }

    public virtual EquipKeyAndValue beforeActtack()//用于影响改成平A攻击暴击，闪避等等影响到平A的判定
    {
        return null;
    }
    public virtual void Acttacking()
    {

    }
    public virtual void beforeHurt(HurtStatus hurt) {

    }
    public virtual void endHurt(HurtStatus hurt) {

    }
    public virtual void beforeBeHurt(HurtStatus hurt)
    {

    }
    public virtual void endBeHurt(HurtStatus hurt)
    {

    }
    public virtual void killEnemy() { 
        
    }
    public virtual int getCardCost(int cost) {
        return cost;
    }
    public virtual float beforeCardHurt(float hurt) {
        return hurt;
    }
    public virtual float endCardHurt(float hurt) {
        return hurt;
    }
    public virtual float beforeBeCardHurt(float hurt)
    {
        return hurt;
    }
    public virtual float endBeCardHurt(float hurt)
    {
        return hurt;
    }
    public virtual BigNumber getDieHuijing(BigNumber big) {
        return big;
    }
    public virtual bool beforeGetDrop() {
        return false;
    }
    public virtual bool endGetDrop() {
        return false;
    }
    public virtual void debuffMonster(Attacker monster)
    {
       
    }
}
