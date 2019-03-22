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
        text.transform.localScale = new Vector3(1, 1, 1);
        Text tv= text.GetComponent<Text> ();
        if (status.type == HurtStatus.TYPE_RATE)
        {
            tv.text = "闪避";
            tv.color = new Color(0x0,0xf9,0xff,0xff);
        }
        else if (status.type == HurtStatus.TYPE_CRT)
        {
            tv.text = "暴击" + StringUtils.doubleToStringShow(status.blood) ;
            tv.color = Color.red;
        }
        else if (status.type == HurtStatus.TYPE_FANGSHANG)
        {
            tv.text = "反伤" + StringUtils.doubleToStringShow(status.blood);
            tv.color = Color.yellow;
        }
        else {
            tv.text = "" + StringUtils.doubleToStringShow(status.blood);
            tv.color = Color.yellow;
        }
        tv.fontSize = 20;
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mHero.transform.position)+new Vector3(0,0,0);  
		text.transform.position = EnemySceenPosition;  
		UiManager.FlyTo (tv, UiManager.FLY_LEFT);
	}
    public void add(double blood) {
        if (blood == 0) {
            return;
        }
        GameObject obj = Resources.Load<GameObject>("prefab/hurt");
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mHero.transform.position);
        GameObject text = GameObject.Instantiate(obj,
            new Vector2(EnemySceenPosition.x, EnemySceenPosition.y), Quaternion.identity);
        
        text.transform.SetParent(HP_Parent);
        text.transform.localScale = new Vector3(1, 1, 1);
        Text tv = text.GetComponent<Text>();
        tv.text = "+"+ StringUtils.doubleToStringShow(blood);
        tv.color = Color.green;
        tv.fontSize = 20;
        EnemySceenPosition = Camera.main.WorldToScreenPoint(mHero.transform.position) + new Vector3(0, 0, 0);
        text.transform.position = EnemySceenPosition;
        UiManager.FlyTo(tv, UiManager.FLY_LEFT);
    }

}

