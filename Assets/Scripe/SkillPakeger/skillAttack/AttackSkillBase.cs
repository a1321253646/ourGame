using UnityEngine;
using System.Collections;

public abstract class AttackSkillBase : MonoBehaviour
{

    public abstract void initEnd();
    public abstract void upDateEnd();

    public static int SKILL_STATUS_DEFAULT = 1;
    public static int SKILL_STATUS_START = 2;
    public static int SKILL_STATUS_RUNNING = 3;
    public static int SKILL_STATUS_END = 4;

    public abstract float beAction(HurtStatus status);
    public abstract void init(AttackSkillManager manager, long skillId,Attacker fight);
    public abstract void update();

    public int mSkillStatus = SKILL_STATUS_DEFAULT;
    public Attacker mFight;
    public bool isInit = false;

    public AttackSkillManager mManager;
    public SkillJsonBean mSkillJson;
    public CalculatorUtil calcuator;

    public static long LEVEL_ID = 10001;
    public float level = 1;
    
    public int getSkillStatus() {
        return mSkillStatus;
    }

    public float getValueById(long id) {
        switch (id) {
            case 10001:
                return level;
            default:
                return 0;
        }
    }
}
