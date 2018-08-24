using UnityEngine;
using System.Collections;

public abstract class SkillObject : MonoBehaviour
{

    public static int SKILL_STATUS_DEFAULT = 1;
    public static int SKILL_STATUS_START = 2;
    public static int SKILL_STATUS_RUNNING = 3;
    public static int SKILL_STATUS_END = 4;

    public SkillJsonBean mBean;
    public SkillLocalBean mLocal;
    private int mSkillStatus = SKILL_STATUS_DEFAULT;
    private bool isInit = false;
    public int mCamp;
    SpriteRenderer mSpriteRender;
    public ResourceBean mResource;
    public CalculatorUtil calcuator;
    public LocalManager mLocalManager;

    public AnimalControlBase mAnimalControl;
    public Attacker mAttacker;
    public void init(Attacker attacker,LocalManager manage, SkillJsonBean bean, float x, float y,int campType) {
        mLocalManager = manage;
        mBean = bean;
        getLocal();
        mLocal.x = x;
        mLocal.y = y;
        mCamp = campType;
        mAttacker = attacker;
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mResource = JsonUtils.getIntance().getEnemyResourceData(bean.skill_resource);
        mAnimalControl = new AnimalControlBase(mResource, mSpriteRender);
        initEnd();
        calcuator = new CalculatorUtil(mBean.calculator, mBean.effects_parameter);
        mAnimalControl.start();
        isInit = true;

    }

    public abstract void initEnd();

    public int getStatus() {
        return mSkillStatus;
    }

    private void getLocal() {

        mLocal = new SkillLocalBean();
        mLocal.leng = mBean.leng;
        mLocal.wight = mBean.wight;
        mLocal.type = mBean.shape_type;
    }

    void Update()
    {
        if (!isInit) {
            return;
        }
        mAnimalControl.update();
    }
}
