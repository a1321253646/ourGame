using UnityEngine;
public class PackbackSkillBase
{
    public AttackSkillManager mManager;
    public Attacker mFight;
    public long value = 0;
    public PackbackSkillBase init(AttackSkillManager manager,long value, Attacker fight) {
        this.value = value;
        mManager = manager;
        mFight = fight;
        return this;
    }
    public virtual void startSkill() {

    }
    public virtual void removeSkill() {
    }
}