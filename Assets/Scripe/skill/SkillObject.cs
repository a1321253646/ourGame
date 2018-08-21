using UnityEngine;
using System.Collections;

public class SkillObject 
{

    public static int SKILL_STATUS_DEFAULT = 1;
    public static int SKILL_STATUS_START = 2;
    public static int SKILL_STATUS_RUNNING = 3;
    public static int SKILL_STATUS_END = 4;

    private SkillJsonBean mBean;
    private SkillLocalBean mLocal;
    private int mSkillStatus = SKILL_STATUS_DEFAULT;

    public SkillObject(SkillJsonBean bean) {
        mBean = bean;
        getLocal();
    }

    public int getStatus() {
        return mSkillStatus;
    }

    private void getLocal() {
        mLocal = new SkillLocalBean();
        mLocal.leng = mBean.leng;
        mLocal.wight = mBean.wight;
        mLocal.type = mBean.shape_type;
    }

    public void setCenterPoint(float x, float y) {
        if (mLocal != null) {
            mLocal.x = x;
            mLocal.y = y;
        }
    }

    public void upDate() {

    }
}
