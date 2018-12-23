using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventAttackSkillManagerSingle 
{

    public List<EventAttackSkill> mList = new List<EventAttackSkill>();

    public void register(EventAttackSkill skill) {
        mList.Add(skill);
    }
    public void unRegister(EventAttackSkill skill) {
        mList.Remove(skill);
    }

}
