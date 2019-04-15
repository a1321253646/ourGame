using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EventAttackSkill : AttackerSkillBase
{
    public bool isGiveup = false;
    public override void initSkill(AttackSkillManager manager, long skillId, Attacker fight, List<float> value, GameObject newobj, bool giveup, long skillIndex)
    {
        mSkillIndex = skillIndex;
        isGiveup = giveup;
        mManager = manager;
        mFight = fight;
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        mParam = value;
        initEnd(newobj);
    }
    public virtual void initEnd(GameObject newobj)
    {

    }
    public virtual EquipKeyAndValue beforeActtack()//用于影响改成平A攻击暴击，闪避等等影响到平A的判定
    {
        return null;
    }
    public virtual void Acttacking()
    {

    }
    public virtual void beforeHurt(HurtStatus hurt,Attacker attacker) {

    }
    public virtual void endHurt(HurtStatus hurt, Attacker attacker) {

    }
    public virtual void allHurt(Attacker fighter, HurtStatus hurt)
    {

    }
    public virtual void beforeBeHurt(Attacker fighter, HurtStatus hurt)
    {

    }
    public virtual void endBeHurt(Attacker fighter, HurtStatus hurt)
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
    public virtual void debuffLitterMonster(Attacker monster)
    {

    }
    public virtual void debuffBoss(Attacker monster)
    {

    }

    public virtual void beforeDie()
    {

    }
    public virtual float afterPinga()
    {
        return 1;
    }

}
