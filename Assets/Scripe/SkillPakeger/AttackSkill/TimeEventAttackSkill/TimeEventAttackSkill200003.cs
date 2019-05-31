using UnityEngine;
using System.Collections;

public class TimeEventAttackSkill200003 : TimeEventAttackSkillBase
{
//    public CalculatorUtil mCalcuator;
    public SpriteRenderer mSpriteRender;
    public AnimalControlBase mAnimal;
    public override void endSkill()
    {
        mManager.mEventAttackManager.unRegister(EventAttackSkillManager.EVENT_SKILL_END_EHURT, this);
        mManager.mEventAttackManager.unRegisterTimeEventSkill(this);
        mStatus = SKILL_STATUS_END;
        GameObject.Destroy(mSpriteRender, 0.1f);
        mSpriteRender = null;
        mAnimal = null;
    }
    public override void initEnd(GameObject newobj)
    {
        mSpriteRender = newobj.GetComponent<SpriteRenderer>();
        ResourceBean mResource = JsonUtils.getIntance().getEnemyResourceData(mSkillJson.skill_resource);
        mAnimal = new AnimalControlBase(mResource, mSpriteRender);
        mAnimal.start();
        mAnimal.setIsLoop(ActionFrameBean.ACTION_NONE, false);
        mAnimal.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(endAnimal));
    }
    void endAnimal(int status)
    {
        mSpriteRender.transform.localScale = new Vector2(0,0);
    }
    float count1 = 0;
    public override void endHurt( HurtStatus hurt, Attacker attacker)
    {
        Debug.Log(" TimeEventAttackSkill200003 hurt = "+ hurt.blood);
        if (count1 == 0)
        {
            count1 = mSkillJson.getSpecialParameterValue()[1] ;
        }
        mFight.AddBlood(count1 * mFight.mAttribute.maxBloodVolume);
    }

    public override void startSkill()
    {
        
        mManager.mEventAttackManager.register(EventAttackSkillManager.EVENT_SKILL_END_EHURT, this);
        mManager.mEventAttackManager.registerTimeEventSkill(this);
        value = mParam[0];
        Debug.Log(" TimeEventAttackSkill200003 value = " + value);
        isInit = true;
    }


    public override void upDateSkill()
    {

        if (!isInit)
        {
            return;
        }

        mTime += Time.deltaTime;
//        Debug.Log("==================================TimeEventAttackSkill200003 value=" + value + " mTime = " + mTime);
        //}
        if (mTime > value)
        {
            endSkill();
        }
        if (mAnimal != null) {
            mAnimal.update();
        }
        
    }
    public override bool isAnimal()
    {
        return true;
    }
    public override void addValueEnd()
    {
        mSpriteRender.transform.localScale = new Vector2(1, 1);
        mAnimal.reStart();
    }
}
