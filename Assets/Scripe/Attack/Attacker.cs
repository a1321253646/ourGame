using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Attacker : MonoBehaviour 
{
	// Use this for initialization

	public static  int PLAY_STATUS_STANDY = ActionFrameBean.ACTION_STANDY;
	public static  int PLAY_STATUS_FIGHT = ActionFrameBean.ACTION_ATTACK;
	public static  int PLAY_STATUS_RUN = ActionFrameBean.ACTION_MOVE;
	public static  int PLAY_STATUS_DIE = ActionFrameBean.ACTION_DIE;
	public static  int PLAY_STATUS_HURT= ActionFrameBean.ACTION_HURT;
	public static  int PLAY_STATUS_WIN = ActionFrameBean.ACTION_WIN;

	public static  int ATTACK_TYPE_DEFAULT = 1	;
	public static  int ATTACK_TYPE_HERO = 2;
	public static  int ATTACK_TYPE_ENEMY= 3;
	public static  int ATTACK_TYPE_BOSS = 4;

    public static int CAMP_TYPE_DEFAULT = 0;
    public static int CAMP_TYPE_PLAYER = 1;
    public static int CAMP_TYPE_MONSTER = 2;

    private bool isBeAttacker = false;
	private int status = PLAY_STATUS_STANDY;
	public int id = -1;
	public float mBloodVolume = 0;
	public float mRunSpeed;
	public float mAttackLeng = 1;
	public float mDieGas = 0;
	public float mDieCrysta = 0;

    public AttackSkillManager mSkillManager;

    public Attribute mAttribute = new Attribute();
    
    public LocalBean mLocalBean;
	public List<Attacker> mAttackerTargets;
	public ResourceBean resourceData;

    public AttackSpeedBean mSpeedBean;
    public AnimationEvent mStandEvent;
    public AnimationEvent mFightEvent;
    public AnimalControlBase mAnimalControl;
    public SpriteRenderer mSpriteRender;
    public int mAttackType = ATTACK_TYPE_DEFAULT;
    public int mCampType = CAMP_TYPE_DEFAULT;

    public void upDataSpeed() {
        if (resourceData == null) {
            return;
        }
        float eachFors = JsonUtils.getIntance().getFrequencyByValue(mAttribute.attackSpeed);
        Debug.Log("mAttribute.attackSpeed = " + mAttribute.attackSpeed + " eachFors =" + eachFors);
        mAnimalControl.setSpeedData(eachFors,ActionFrameBean.ACTION_ATTACK);

    }
    public abstract float BeAttack (HurtStatus status);
    public abstract float BeKillAttack(long effect, float value);
    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
        changeAnim();
    }

	public BackgroundManager mBackManager;
	public FightManager mFightManager;
	public void toString(string er){
		Debug.Log(er+":\n mAggressivity "+ mAttribute.aggressivity +
			" mDefense = "+ mAttribute.defense +
			" mBloodVolume = "+mBloodVolume+
			" mMaxBloodVolume = "+ mAttribute.maxBloodVolume +
			" mAttackSpeed = "+ mAttribute.attackSpeed +
			" mRunSpeed = "+mRunSpeed);
	}

	public void startComment () {
        mSkillManager = new AttackSkillManager(this);

    }


	public void attack(){
		float attackeBlood = mFightManager.attackerAction (id);
	}
	public void attackSync(float blood){
		
	}
	private void changeAnim(){
        if (status == ActionFrameBean.ACTION_DIE)
        {
            mAnimalControl.setStatus(status, true);
        }
        else {
            mAnimalControl.setStatus(status);
        }
        
	}

	public void Standy(){
		status = ActionFrameBean.ACTION_STANDY;
        changeAnim();

    }
	public void Fight(){
        status = ActionFrameBean.ACTION_ATTACK;
        changeAnim();
    }
	public void Die(){
        Debug.Log("Die " );
        status = ActionFrameBean.ACTION_DIE;
        changeAnim();
    }
	public void Run(){
        status = ActionFrameBean.ACTION_MOVE;
        changeAnim();
    }
	public void Hurt(){
        status = ActionFrameBean.ACTION_HURT;
        changeAnim();
    }
	public void Win(){
        status = ActionFrameBean.ACTION_WIN;
        changeAnim();
    }
    public void setRed() {
        if (mSpriteRender != null)
        {
            mSpriteRender.color = Color.red;
        }
        
    }
    public void setWhith() {
        if (mSpriteRender != null) {
            mSpriteRender.color = Color.white;
        }
        
    }
    public void skillAttack(long effect, float value) {
        BeKillAttack(effect, value);
    }
}

