using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackSkill500006 : EventAttackSkill
{


    float count1 = 0;
    LocalManager mLocal = null;
    public override void addValueEnd()
    {

        getLunhuiValue(500006, 11);
        addEachAlive(AttributePre.maxBloodVolume);
    }

    private void getLunhuiValue(long type ,long id) {
        long luihuiLevel = InventoryHalper.getIntance().getSamsaraLevelById(id);
        long value = 0;

        if (luihuiLevel != BaseDateHelper.encodeLong(0))
        {
            List<SamsaraValueBean> list = JsonUtils.getIntance().getSamsaraVulueInfoByIdAndLevel(id, BaseDateHelper.decodeLong(luihuiLevel));
            foreach (SamsaraValueBean bean in list)
            {
                if (bean.type == type)
                {
                    value = bean.value;
                    break;
                }
            }
            if (value != 0)
            {
                mValue = value;

            }
        }
        else
        {
            mValue = 10000;
        }

    }

    public override void debuffLitterMonster(Attacker monster)
    {
        monster.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.maxBloodVolume, (long)mValue);
        monster.getAttribute(true);
    }

    public override void endSkill()
    {

        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_LITTER_DEBUFF, this);
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (!list.mIsHero)
            {

                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, AttributePre.maxBloodVolume, 0);
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }

    public override void startSkill()
    {
        if (mValue == 0)
        {
            getLunhuiValue(500006, 11);
        }
        addEachAlive(AttributePre.maxBloodVolume);

    }
    private void addEachAlive(long type) {
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_LITTER_DEBUFF, this);
        mLocal = GameObject.Find("Manager").GetComponent<LevelManager>().mLocalManager;
        LocalBean list = mLocal.mLocalLink;
        while (list != null)
        {
            if (!list.mIsHero && list.mAttacker.mAttackType != Attacker.ATTACK_TYPE_BOSS)
            {
                list.mAttacker.mAllAttributePre.updateDebuff(mSkillIndex, type, (long)mValue); ;
                list.mAttacker.getAttribute(true);
            }
            list = list.next;
        }
    }


}
