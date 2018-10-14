using UnityEngine;
using System.Collections;

public abstract class AttackSkillWithAnimal : AttackSkillBase
{
    public AnimalControlBase mAnimalControl;
    SpriteRenderer mSpriteRender;
    public ResourceBean mResource;

    public override float beAction(HurtStatus status)
    {
        return -1;
    }

    public override void init(AttackSkillManager manager, long skillId, Attacker fight)
    {
        mManager = manager;
        mFight = fight;
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mSkillJson = JsonUtils.getIntance().getSkillInfoById(skillId);
        mResource = JsonUtils.getIntance().getEnemyResourceData(mSkillJson.skill_resource);
        mAnimalControl = new AnimalControlBase(mResource, mSpriteRender);
        initEnd();
        calcuator = new CalculatorUtil(mSkillJson.calculator, mSkillJson.effects_parameter);
        calcuator.setSkill(this);
        mAnimalControl.start();
        isInit = true;
    }
    public override void update()
    {
        Debug.Log("AttackSkillWithAnimal update :" + mSkillJson.effects);
        if (!isInit)
        {
            return;
        }
        mAnimalControl.update();
        upDateEnd();
    }
    public void upDateLocal(float x,float y){
        transform.Translate(Vector2.up * y);
        transform.Translate(Vector2.left * x);
    }
}
