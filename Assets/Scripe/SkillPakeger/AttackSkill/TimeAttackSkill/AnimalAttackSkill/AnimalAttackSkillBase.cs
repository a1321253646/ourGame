using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AnimalAttackSkillBase : TimeAttackSkillBase
{
    public SpriteRenderer mSpriteRender;
    public ResourceBean mResource;
    public AnimalControlBase mAnimal;
    public CalculatorUtil mCalcuator;
    public GameObject mSprinter;
    public override void initSkill(AttackSkillManager manager, long skillId, Attacker fight, List<float> value, GameObject newobj)
    {
        mManager = manager;
        mFight = fight;
        mSpriteRender = newobj.GetComponent<SpriteRenderer>();
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        mResource = JsonUtils.getIntance().getEnemyResourceData(mSkillJson.skill_resource);
        mAnimal = new AnimalControlBase(mResource, mSpriteRender);
        mParam = value;
        endInitSkill();
        isInit = true;
        mCalcuator = new CalculatorUtil(mSkillJson.calculator, mSkillJson.effects_parameter);

        mCalcuator.setSkill(this);
        mAnimal.start();
        mSprinter = newobj;
    }
    public override void upDateSkill() {
        if (!isInit) {
            return;
        }
        mAnimal.update();
        updateSkillEnd();
    }
    public virtual void endInitSkill()
    {
    }
    public virtual void updateSkillEnd()
    {

    }
    public virtual bool isAnimal()
    {
        return true;
    }
    public void upDateLocal(float x, float y)
    {
        mSprinter.transform.Translate(Vector2.up * y);
        mSprinter.transform.Translate(Vector2.left * x);
    }
}
