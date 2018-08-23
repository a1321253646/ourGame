using UnityEngine;
using System.Collections;

public abstract class SkillObject : MonoBehaviour
{

    public static int SKILL_STATUS_DEFAULT = 1;
    public static int SKILL_STATUS_START = 2;
    public static int SKILL_STATUS_RUNNING = 3;
    public static int SKILL_STATUS_END = 4;

    private SkillJsonBean mBean;
    private SkillLocalBean mLocal;
    private int mSkillStatus = SKILL_STATUS_DEFAULT;
    private bool isInit = false;
    private int mCamp;
    SpriteRenderer mSpriteRender;
    public AnimalControlBase mAnimalControl;
    public void init(SkillJsonBean bean, float x, float y,int campType) {
        mBean = bean;
        getLocal();
        mLocal.x = x;
        mLocal.y = y;
        mCamp = campType;
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        mAnimalControl = new AnimalControlBase(JsonUtils.getIntance().getEnemyResourceData(bean.skill_resource), mSpriteRender);
        initEnd();
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
