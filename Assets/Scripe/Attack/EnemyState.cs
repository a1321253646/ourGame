using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.UI;  
public class EnemyState{ 

	private  GameObject HP_imageGameobject;  
	private GameObject HP_imageGameObjectClone;  
	private Transform HP_Parent;  
	private Vector3 EnemySceenPosition;  
	private EnemyBase mEnemy;
	private ResourceBean mResourceData;
	Slider mHpSl;
	Vector3 bloodOffet;
	public EnemyState ( EnemyBase enemy){
		mEnemy = enemy;
		mResourceData = mEnemy.resourceData;
	//	Debug.Log ("BloodOffset.x = "+mResourceData.getBloodOffset().x+
	//		"BloodOffset.y="+mResourceData.getBloodOffset().y);
		bloodOffet = new Vector3 (mResourceData.getBloodOffset().x, mResourceData.getBloodOffset().y, 0);
		HP_imageGameobject = Resources.Load<GameObject> ("prefab/enemyblood") ;		 
		HP_Parent = GameObject.Find("enemyStatePlane").transform;  
		EnemySceenPosition=Camera.main.WorldToScreenPoint(mEnemy.transform.position);
        if (enemy.mAttackType == Attacker.ATTACK_TYPE_ENEMY) {
            HP_imageGameObjectClone = GameObject.Instantiate(HP_imageGameobject,
                new Vector2(EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
            HP_imageGameObjectClone.transform.localScale = new Vector3(mResourceData.blood_witch, 1f, 0);
            HP_imageGameObjectClone.transform.SetParent(HP_Parent);
            mHpSl = HP_imageGameObjectClone.GetComponent<Slider>();
            EnemySceenPosition = Camera.main.WorldToScreenPoint(mEnemy.transform.position) + new Vector3(0, 0, 0);
            HP_imageGameObjectClone.transform.position = EnemySceenPosition;
        }
        resetHp();

		//GameObject.Instantiate (getEnemyPrefab(res), new Vector2 (transform.position.x, transform.position.y),Quaternion.Euler(0.0f,0f,0.0f));
		 
	}
    double bili = 1;
    public void Update (){	  
		PHFollowEnemy();  
	}
    public void delectBlood() {
        GameObject.Destroy(HP_imageGameObjectClone);
    }
	public void hurt(HurtStatus status)
    {
        if (mEnemy.mAttackType == Attacker.ATTACK_TYPE_ENEMY)
        {
            mHpSl.value = (float)(mEnemy.mBloodVolume / bili);
        }
        else
        {
            GameManager.getIntance().setBossBlood(mEnemy.mBloodVolume , mEnemy.mAttribute.maxBloodVolume);
        }
		if (mEnemy.mBloodVolume <= 0 && mEnemy.mAttackType == Attacker.ATTACK_TYPE_ENEMY) {
			GameObject.Destroy (HP_imageGameObjectClone);
			HP_imageGameObjectClone = null;

		}
		GameObject obj = Resources.Load<GameObject> ("prefab/hurt") ;
		EnemySceenPosition = Camera.main.WorldToScreenPoint (mEnemy.transform.position);
		GameObject text = GameObject.Instantiate(obj,
			new Vector2 (EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
		text.transform.SetParent(HP_Parent);
        text.transform.localScale = new Vector3(1, 1, 1);
        Text tv= text.GetComponent<Text> ();
        if (status.type == HurtStatus.TYPE_RATE)
        {
            tv.text = "闪避" ;
            tv.color = new Color(0x0, 0xf9, 0xff, 0xff);
        }
        else if (status.type == HurtStatus.TYPE_CRT)
        {
            tv.text = "暴击" + StringUtils.doubleToStringShow(status.blood);
            tv.color = Color.red;
        }
        else if (status.type == HurtStatus.TYPE_FANGSHANG)
        {
            tv.text = "反伤" + StringUtils.doubleToStringShow(status.blood);
            tv.color = Color.yellow;
        }
        else
        {
            tv.text =  StringUtils.doubleToStringShow(status.blood) ;
            tv.color = Color.yellow;
        }
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mEnemy.transform.position)+bloodOffet;  
		text.transform.position = EnemySceenPosition;  
		UiManager.FlyTo (tv);
	}

	void PHFollowEnemy()  
	{  
		if (HP_imageGameObjectClone == null) {
			return;
		}
	
		EnemySceenPosition = Camera.main.WorldToScreenPoint (mEnemy.transform.position+bloodOffet)  ;  
		HP_imageGameObjectClone.transform.position = EnemySceenPosition;  
	}
    public void add(double blood)
    {
        if (blood == 0) {
            return;    
        }
        if (mEnemy.mAttackType == Attacker.ATTACK_TYPE_ENEMY)
        {
            mHpSl.value = (float)(mEnemy.mBloodVolume / bili);
        }
        else
        {
            GameManager.getIntance().setBossBlood(mEnemy.mBloodVolume, mEnemy.mAttribute.maxBloodVolume);
        }
       
        GameObject obj = Resources.Load<GameObject>("prefab/hurt");
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mEnemy.transform.position);
        GameObject text = GameObject.Instantiate(obj,
            new Vector2(EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
        text.transform.SetParent(HP_Parent);
        text.transform.localScale = new Vector3(1, 1, 1);
        Text tv = text.GetComponent<Text>();
        tv.text = "+" + StringUtils.doubleToStringShow(blood) ;
        tv.color = Color.green;
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mEnemy.transform.position) + bloodOffet;
        text.transform.position = EnemySceenPosition;
        UiManager.FlyTo(tv);
    }
    public void resetHp() {
        if (mEnemy.mAttribute.maxBloodVolume > float.MaxValue)
        {
            bili = mEnemy.mAttribute.maxBloodVolume / float.MaxValue + 1;
        }
        if (mEnemy.mAttackType == Attacker.ATTACK_TYPE_ENEMY)
        {
            mHpSl.maxValue = (float)(mEnemy.mAttribute.maxBloodVolume / bili);
            mHpSl.value = (float)(mEnemy.mBloodVolume / bili);
        }
        else {
            GameManager.getIntance().setBossBlood(mEnemy.mBloodVolume, mEnemy.mAttribute.maxBloodVolume);
        }

    }
}