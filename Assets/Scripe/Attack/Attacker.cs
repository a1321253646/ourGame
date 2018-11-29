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
	public BigNumber mDieCrysta ;

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

    public bool isStop = false;
    public int oldStatus = PLAY_STATUS_STANDY;
    public void setStop() {
        oldStatus = status;
        setStatus(PLAY_STATUS_STANDY);
        isStop = true;
    }
    public void cancelStop()
    {
        isStop = false;
        setStatus(oldStatus);
       
    }


    public void upDataSpeed() {
        if (resourceData == null) {
            return;
        }
        float eachFors = JsonUtils.getIntance().getFrequencyByValue(mAttribute.attackSpeed);
        Debug.Log("mAttribute.attackSpeed = " + mAttribute.attackSpeed + " eachFors =" + eachFors);
        mAnimalControl.setSpeedData(eachFors,ActionFrameBean.ACTION_ATTACK);

    }
    public abstract float BeAttack (HurtStatus status, Attacker hurter);
    public abstract float BeKillAttack(long effect, float value, Attacker hurter);
    public abstract void getAttribute();
    public abstract void AddBlood( float value);
    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        if (isStop) {
            return;
        }
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
        setStatus(ActionFrameBean.ACTION_STANDY);
    }
	public void Fight(){
        setStatus(ActionFrameBean.ACTION_ATTACK);
    }
	public void Die(){
        //        Debug.Log("Die " );
        setStatus(ActionFrameBean.ACTION_DIE);
    }
	public void Run(){
        setStatus(ActionFrameBean.ACTION_MOVE);
    }
	public void Hurt(){
        setStatus(ActionFrameBean.ACTION_HURT);
    }
	public void Win(){
        setStatus(ActionFrameBean.ACTION_WIN);
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
    public void skillAttack(long effect, float value,Attacker hurter) {
        BeKillAttack(effect, value,hurter);
    }
}

