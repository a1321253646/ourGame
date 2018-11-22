using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager : MonoBehaviour {

    public  Transform mRootTransform;
    public  GameObject mTop;
    public  GameObject mRight;
    public  GameObject mLeft;
    public  GameObject mBotton;
    public  GameObject mMiddle;
	// Use this for initialization
	void Start () {
        mRootTransform = GameObject.Find("Canvas").GetComponent<Transform>();

    }
	
   // public void showGuide()

	// Update is called once per frame
	void Update () {
		
	}
}
