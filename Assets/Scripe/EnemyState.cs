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
	public EnemyState ( EnemyBase enemy){
		mEnemy = enemy;
		HP_imageGameobject = Resources.Load<GameObject> ("prefab/enemyblood") ;		 
		HP_Parent = GameObject.Find("enemyStatePlane").transform;  
		EnemySceenPosition=Camera.main.WorldToScreenPoint(mEnemy.transform.position);  
		HP_imageGameObjectClone = GameObject.Instantiate(HP_imageGameobject,
			new Vector2 (EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);  
		//GameObject.Instantiate (getEnemyPrefab(res), new Vector2 (transform.position.x, transform.position.y),Quaternion.Euler(0.0f,0f,0.0f));
		HP_imageGameObjectClone.transform.SetParent(HP_Parent);  
	} 
	public void Update (){	  
		PHFollowEnemy();  
	}  
 
	void PHFollowEnemy()  
	{  
		EnemySceenPosition= Camera.main.WorldToScreenPoint(mEnemy.transform.position)+new Vector3(0,50,0);  
		HP_imageGameObjectClone.transform.position = EnemySceenPosition;  
	}  
}