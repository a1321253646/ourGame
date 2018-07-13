﻿using System.Collections;  
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
		Debug.Log ("BloodOffset.x = "+mResourceData.getBloodOffset().x+
			"BloodOffset.y="+mResourceData.getBloodOffset().y);
		bloodOffet = new Vector3 (mResourceData.getBloodOffset().x, mResourceData.getBloodOffset().y, 0);
		HP_imageGameobject = Resources.Load<GameObject> ("prefab/enemyblood") ;		 
		HP_Parent = GameObject.Find("enemyStatePlane").transform;  
		EnemySceenPosition=Camera.main.WorldToScreenPoint(mEnemy.transform.position);  
		HP_imageGameObjectClone = GameObject.Instantiate(HP_imageGameobject,
			new Vector2 (EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);  
		HP_imageGameObjectClone.transform.localScale = new Vector3 (  mResourceData.getBloodWitch(),1f,0);
		HP_imageGameObjectClone.transform.SetParent(HP_Parent); 
		mHpSl = HP_imageGameObjectClone.GetComponent<Slider> ();
		mHpSl.maxValue = mEnemy.mMaxBloodVolume;
		mHpSl.value = mEnemy.mBloodVolume;
		EnemySceenPosition= Camera.main.WorldToScreenPoint(mEnemy.transform.position)+new Vector3(0,0,0);  
		HP_imageGameObjectClone.transform.position = EnemySceenPosition;
		//GameObject.Instantiate (getEnemyPrefab(res), new Vector2 (transform.position.x, transform.position.y),Quaternion.Euler(0.0f,0f,0.0f));
		 
	} 
	public void Update (){	  
		PHFollowEnemy();  
	}  
 
	public void hurt(int blood){
		mHpSl.value = mEnemy.mBloodVolume;
		if (mEnemy.mBloodVolume <= 0) {
			GameObject.Destroy (HP_imageGameObjectClone);
			HP_imageGameObjectClone = null;

		}
		GameObject obj = Resources.Load<GameObject> ("prefab/hurt") ;
		EnemySceenPosition = Camera.main.WorldToScreenPoint (mEnemy.transform.position);
		GameObject text = GameObject.Instantiate(obj,
			new Vector2 (EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
		text.transform.SetParent(HP_Parent); 
		Text tv= text.GetComponent<Text> ();
		tv.text = "" + blood;
		EnemySceenPosition= Camera.main.WorldToScreenPoint(mEnemy.transform.position)+bloodOffet;  
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


}