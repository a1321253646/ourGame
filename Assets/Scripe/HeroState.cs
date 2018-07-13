using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
public class HeroState : MonoBehaviour
{
	private PlayControl mHero;
	private Transform HP_Parent;  
	private Vector3 EnemySceenPosition; 
	public HeroState ( PlayControl hero){
		mHero = hero;	 
		HP_Parent = GameObject.Find("enemyStatePlane").transform;  
		EnemySceenPosition=Camera.main.WorldToScreenPoint(mHero.transform.position);  
	} 
	public void hurt(int blood){
		GameObject obj = Resources.Load<GameObject> ("prefab/hurt") ;
		EnemySceenPosition = Camera.main.WorldToScreenPoint (mHero.transform.position);
		GameObject text = GameObject.Instantiate(obj,
			new Vector2 (EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
		text.transform.SetParent(HP_Parent); 
		Text tv= text.GetComponent<Text> ();
		tv.text = "" + blood;
		EnemySceenPosition= Camera.main.WorldToScreenPoint(mHero.transform.position)+new Vector3(0,0,0);  
		text.transform.position = EnemySceenPosition;  
		UiManager.FlyTo (tv);
	}
}

