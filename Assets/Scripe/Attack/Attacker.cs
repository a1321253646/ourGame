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

    public bool isBeAttacker = false;
    public int status = PLAY_STATUS_STANDY;
	public int id = -1;
	public double mBloodVolume = 0;
	public float mRunSpeed;
	public float mAttackLeng = 1;
	public float mDieGas = 0;
	public BigNumber mDieCrysta ;

    public CardManagerBase mCardManager = null;

    public AttackSkillManager mSkillManager;

    public Attribute mAttribute = new Attribute();

    public Attribute mBaseAttribute = new Attribute();
    public Attribute mSkillAttribute = new Attribute();
    public Attribute mPetAttribute = new Attribute();
    public Attribute mEquipAttribute = new Attribute();
    public Attribute mLunhuiAttribute = new Attribute();
    
    public Attribute mAllAttribute = new Attribute();

    public AttributePre mAllAttributePre ;
    
    public LocalBean mLocalBean;
	public List<Attacker> mAttackerTargets;
	public ResourceBean resourceData;

    public Dictionary<long, long> mBuffList = new Dictionary<long, long>();

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
//        Debug.Log("mAttribute.attackSpeed = " + mAttribute.attackSpeed + " eachFors =" + eachFors);
        mAnimalControl.setSpeedData(eachFors,ActionFrameBean.ACTION_ATTACK);

    }
    public abstract double BeAttack (HurtStatus status, Attacker hurter);
    public abstract double BeKillAttack( double value, Attacker hurter);
    public abstract void getAttributeEnd();
    public abstract void AddBlood(double value);
    public abstract double allHurt(HurtStatus status, Attacker hurter);
    public int getStatus() {
        return status;
    }

    public void restart() {
        Debug.Log("--------------------------------------------startLevelSecond--------------------------------------------------------------");
        this.status = ActionFrameBean.ACTION_MOVE;
        mAnimalControl.isLastSet = false;
        changeAnim();
    }

    public void setStatus(int status) {
        if (isStop && status != ActionFrameBean.ACTION_DIE) {
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

        if (mSkillManager == null) {
            mSkillManager = new AttackSkillManager(this);
        }       

    }


	public void attack(){
		double attackeBlood = mFightManager.attackerAction (id);
	}
	public void attackSync(double blood){
		
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
    public void skillAttack(double value,Attacker hurter) {
        BeKillAttack( value,hurter);
    }

    public double bloodBili = -1;
    public double bloodDistance = -1;
    public void getAttribute(bool isGetBili)
    {
        if (isGetBili)
        {
            bloodDistance = -1;
            bloodBili = mBloodVolume / mAttribute.maxBloodVolume;
        }

        mAttribute.clear();
        mAllAttribute.clear();

        mAllAttribute.add(mBaseAttribute);


        mAllAttribute.add(mEquipAttribute);


        mAllAttribute.add(mLunhuiAttribute);


        mAllAttribute.add(mSkillAttribute);


        mAllAttribute.add(mPetAttribute);



        //Debug.Log("===============mAttribute.mEquipAttributePre = " + mEquipAttributePre.toString());
        //Debug.Log("===============mAttribute.mLunhuiAttributePre = " + mLunhuiAttributePre.toString());
        //Debug.Log("===============mAttribute.mSkillAttributePre = " + mSkillAttributePre.toString());
        //Debug.Log("===============mAttribute.mAllAttributePre = " + mAllAttributePre.toString());


        mAttribute.add(mAllAttribute);
        mAttribute.chen(mAllAttributePre.getAll());
       
//        Debug.Log("===============mAttribute.mAttribute = " + mAttribute.toString());
        if (bloodDistance != -1)
        {
            mBloodVolume = mAttribute.maxBloodVolume - bloodDistance;
        }
        else
        {
            mBloodVolume = mAttribute.maxBloodVolume * bloodBili;
        }

        getAttributeEnd();
    }
    public float getLunhuiValue(long type, long id, float valueDefault)
    {
        float result = valueDefault;
        long luihuiLevel = InventoryHalper.getIntance().getSamsaraLevelById(id);
        long value = 0;

        if (luihuiLevel != BaseDateHelper.encodeLong(0))
        {
            List<SamsaraValueBean> list = JsonUtils.getIntance().getSamsaraVulueInfoByIdAndLevel(id, BaseDateHelper.decodeLong(luihuiLevel));
            foreach (SamsaraValueBean bean in list)
            {
                if (bean.type == type)
                {
                    value = bean.value;
                    break;
                }
            }
            if (value != 0)
            {
                result = value;

            }
        }
        else
        {
            result = valueDefault;
        }
        return result;
    }

}

