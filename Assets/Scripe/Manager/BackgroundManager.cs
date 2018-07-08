using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager{
	GameObject bg1;
	GameObject bg2;
	GameObject BgType;
	BackgroundTransform sp1;
	BackgroundTransform sp2;
	public bool isRun = true;
	public float moveSpeed = 2f;
	public string mPath;

	public void init(GameObject type,string path){
		BgType = type;
		mPath = path;
		creatBackgroup ();
		setBackground (mPath);
	}
	public void stop(){
		isRun = false;
	}
	public void move(){
		isRun = true;
	}
	private void creatBackgroup(){
	/*	GameObject newobj =  GameObject.Instantiate (BgType, new Vector2 (3.454f,-0.009f),Quaternion.Euler(0.0f,0.0f,0.0f));
	//	newobj.transform.localScale.Set (2.769776f, 5.359474f, 1);
		sp1 = newobj.GetComponent<BackgroundTransform> ();

		newobj =  GameObject.Instantiate (BgType, new Vector2 (23.668f,-0.009f),Quaternion.Euler(0.0f,0.0f,0.0f));
	//	newobj.transform.localScale.Set (2.769776f, 5.359474f, 1);
		sp2 = newobj.GetComponent<BackgroundTransform> ();*/
		GameObject newobj =  GameObject.Instantiate (BgType, new Vector2 (0f,-0.009f),Quaternion.Euler(0.0f,0.0f,0.0f));
		sp1 = newobj.GetComponent<BackgroundTransform> ();
//		Vector3 vertor=  newobj.GetComponent<MeshFilter> ().mesh.bounds.size;
//		sp1 = newobj.GetComponent<BackgroundTransform> ();
//		float x = vertor.x * newobj.transform.lossyScale.x;
//		Debug.Log ("x = " + x);
		newobj =  GameObject.Instantiate (BgType, new Vector2 (20.22f,-0.009f),Quaternion.Euler(0.0f,0.0f,0.0f));
		sp2 = newobj.GetComponent<BackgroundTransform> ();
	}
	public void setBackground(string path){ 
		sp1.setNewBg (path);
		sp2.setNewBg (path);
	}
}
