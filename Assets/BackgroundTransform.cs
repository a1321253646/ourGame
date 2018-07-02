using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTransform : MonoBehaviour {
	private bool needReplace = false;
	private string newBg = null;
	private bool isRun = true;
	// Update is called once per frame
	public BackgroundManager mBackManager;
	void Start () {
		mBackManager = GameObject.Find ("Manager").GetComponent<LevelManager> ().getBackManager ();
	}

	void Update () {
		if (!mBackManager.isRun) {
			return;
		}
		transform.Translate (Vector2.left * mBackManager.moveSpeed * Time.deltaTime);
		Vector2 postion = transform.position;
		if (postion.x <= 0f-20.22f) {
			transform.position = new Vector2 (transform.position.x+20.22f+20.22f, postion.y);
			Debug.Log ("needReplace " + needReplace);
			Debug.Log ("newBg " + newBg);
			if (needReplace) {
				needReplace = false;
				SpriteRenderer sp1= GetComponent<SpriteRenderer>();
				sp1.sprite = Resources.Load (newBg, typeof(Sprite)) as Sprite;
			}
		}
	}
	public void setNewBg(string bg){
		newBg = bg;
		needReplace = true;
	}
}
