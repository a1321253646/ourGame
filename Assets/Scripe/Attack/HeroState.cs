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
	public void hurt(HurtStatus status)
    {
		GameObject obj = Resources.Load<GameObject> ("prefab/hurt") ;
		EnemySceenPosition = Camera.main.WorldToScreenPoint (mHero.transform.position);
		GameObject text = GameObject.Instantiate(obj,
			new Vector2 (EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
		text.transform.SetParent(HP_Parent); 
		Text tv= text.GetComponent<Text> ();
        if (!status.isRate)
        {
            tv.text = "闪避";
            tv.color = Color.yellow;
        }
        else if (status.isCrt)
        {
            tv.text = "暴击" + status.blood;
            tv.color = Color.red;
        }
        else {
            tv.text = "" + status.blood;
            tv.color = Color.yellow;
        }
        
		EnemySceenPosition= Camera.main.WorldToScreenPoint(mHero.transform.position)+new Vector3(0,0,0);  
		text.transform.position = EnemySceenPosition;  
		UiManager.FlyTo (tv);
	}
    public void add(float blood) {
        GameObject obj = Resources.Load<GameObject>("prefab/hurt");
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mHero.transform.position);
        GameObject text = GameObject.Instantiate(obj,
            new Vector2(EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
        text.transform.SetParent(HP_Parent);
        Text tv = text.GetComponent<Text>();
        tv.text = ""+blood;
        tv.color = Color.green;
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mHero.transform.position) + new Vector3(0, 0, 0);
        text.transform.position = EnemySceenPosition;
        UiManager.FlyTo(tv);
    }

}

