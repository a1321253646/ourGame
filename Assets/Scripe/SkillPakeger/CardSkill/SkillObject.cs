using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SkillObject : MonoBehaviour
{

    public static int SKILL_STATUS_DEFAULT = 1;
    public static int SKILL_STATUS_START = 2;
    public static int SKILL_STATUS_RUNNING = 3;
    public static int SKILL_STATUS_END = 4;

    public SkillJsonBean mBean;
    public SkillLocalBean mLocal;
    public int mSkillStatus = SKILL_STATUS_DEFAULT;
    private bool isInit = false;
    public int mCamp;
    SpriteRenderer mSpriteRender;
    public ResourceBean mResource;
    public CalculatorUtil calcuator;
    public LocalManager mLocalManager;

    public AnimalControlBase mAnimalControl;
    public Attacker mAttacker;
    bool isGiveUp = false;
    public long mSkillIndex = 0;
    public List<Attacker> mTargetList;
    public bool isBoss = false;
    public void init(Attacker attacker,LocalManager manage, SkillJsonBean bean, float x, float y,int campType,bool giveup,bool boss, long skillIndex) {
        mSkillIndex = skillIndex;
        isGiveUp = giveup;
        mLocalManager = manage;
        isBoss = boss;
        mBean = bean;
        if (bean.skill_resource > 0) {
            mResource = JsonUtils.getIntance().getEnemyResourceData(bean.skill_resource);
            getLocal();
            mLocal.x = x;
            mLocal.y = y;
            mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
            mAnimalControl = new AnimalControlBase(mResource, mSpriteRender);
            mAnimalControl.start();
        }      
       
        mCamp = campType;
        mAttacker = attacker;

        initEnd();
        calcuator = new CalculatorUtil(mBean.calculator, mBean.effects_parameter);
        
        isInit = true;
    }

    public void updateLocal(float x) {
        if (!isBoss) {
            gameObject.transform.Translate(Vector2.left * x);
            mLocal.x = mLocal.x - x;
        }

    }

    public abstract void initEnd();

    public virtual void giveUpCard() {

    }

    public int getStatus() {
        return mSkillStatus;
    }

    private void getLocal() {
        mLocal = new SkillLocalBean();
        mLocal.leng = mBean.leng * mResource.zoom;
        mLocal.wight = mBean.wight * mResource.zoom;
        mLocal.type = mBean.shape_type;
    }

    void Update()
    {
        if (!isInit) {
            return;
        }
        if (mAnimalControl != null) {
            mAnimalControl.update();
        }
        
    }

    public void dealNextSkill(SkillJsonBean skill) {

    }
    public virtual void dealNextSkillForEach(SkillJsonBean skill, Attacker a)
    {

    }
    public void actionEnd() {
        if (mBean.getNextSkillList() == null || mBean.getNextSkillList().Count == 0) {
            return;
        }
        foreach (long skillid in mBean.getNextSkillList()) {
            Debug.Log("actionEnd skillid= " + skillid);
            SkillJsonBean nextSkill = JsonUtils.getIntance().getSkillInfoById(skillid);
            if (nextSkill.shape_type == 5 || nextSkill.shape_type == 4)
            {
                Debug.Log("actionEnd dealNextSkillForEach before" );
                if (mTargetList != null && mTargetList.Count > 0)
                {
                    Debug.Log("mTargetList != null");
                    foreach (Attacker a in mTargetList)
                    {
                        dealNextSkillForEach(nextSkill, a);
                        a.mSkillManager.addSkill(nextSkill, mAttacker,
                    SkillIndexUtil.getIntance().getSkillIndexByCardId(isBoss, mSkillIndex));
                    }
                }
            }
            else 
            {
                int type = Attacker.CAMP_TYPE_DEFAULT;
                int attackType = mAttacker.mCampType;
                if (mBean.target_type == SkillJsonBean.TYPE_SELF)
                {
                    type = attackType;
                }
                else if (mBean.target_type == SkillJsonBean.TYPE_ENEMY)
                {
                    if (attackType == Attacker.CAMP_TYPE_PLAYER)
                    {
                        type = Attacker.CAMP_TYPE_MONSTER;
                    }
                    else if (attackType == Attacker.CAMP_TYPE_MONSTER)
                    {
                        type = Attacker.CAMP_TYPE_PLAYER;
                    }
                }
                else if((nextSkill.shape_type != 0))
                {
                    SkillManage.getIntance().addSkill(mAttacker, nextSkill, mLocal.x, mLocal.y, type,
                          SkillIndexUtil.getIntance().getSkillIndexByCardId(isBoss, mSkillIndex));
                }               
            }
        }
    }
}
