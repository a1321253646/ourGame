using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class AttackerSkillBase
{
    
    public static int SKILL_STATUS_DEFAULT = 1;
    public static int SKILL_STATUS_START = 2;
    public static int SKILL_STATUS_RUNNING = 3;
    public static int SKILL_STATUS_END = 4;

    public int mStatus = SKILL_STATUS_DEFAULT;
    public AttackSkillManager mManager;
    public Attacker mFight;
    public List<float> mParam;
    public SkillJsonBean mSkillJson;
    public long mSkillIndex = 0;

    public abstract void initSkill(AttackSkillManager manager, long skillId, Attacker fight,List<float> value, GameObject newobj,bool isGiveup,long skillIndex);
    public abstract void startSkill();
    public abstract void endSkill();
    public float mValue = 0;
    public float getValueById(long id)
    {
        switch (id)
        {
            case 10001:
                return value;
            default:
                return 0;
        }
    }

    public void addValue(float value) {
        mValue += value;
        addValueEnd();
    }
    public virtual void addValueEnd()
    {
    }

    public bool randomResult(int max, int value, bool isPri)
    {
        int rangeRadomNum = UnityEngine.Random.Range(0, max);
        if (isPri)
        {
            Debug.Log("fight rangeRadomNum=" + rangeRadomNum + " value=" + value);
        }
        return rangeRadomNum <= value;
    }
    public float value = 0;
    public virtual bool add(List<float> count,bool isGive)
    {
        value = value + count[0];
        return true;
    }
    public virtual bool isAnimal() {
        return false;
    }
}
