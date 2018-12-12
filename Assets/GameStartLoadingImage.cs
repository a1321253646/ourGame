using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartLoadingImage : MonoBehaviour {
    private long index = 0;
    public float onceJiaodu = 0;


	// Use this for initialization
	void Start () {
        GameObject.Find("game_begin").GetComponent<GameBeginControl>().init();
	}
	
	// Update is called once per frame
	void Update () {
        index++;
    //    if (onceJiaodu * index >= 360) {
    //        index = index - (int)(360 / onceJiaodu);
    //    }
    //    Debug.Log("==============GameStartLoadingImage index = " + index);
    //    Debug.Log("==============GameStartLoadingImage index = " + index);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, -onceJiaodu*index));

        transform.rotation = rotation;
   //     Quaternion q = transform.rotation;
     //   q.z = q.z + index * onceJiaodu;
      //  Quaternion newQ = new Quaternion(q.x,q.y,q.z+index* onceJiaodu,q.w);
      //  transform.rotation = newQ;


    }
}
