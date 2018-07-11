using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

	GameObject mWin,mLost;
	public float END_SCENE_TIME = 5;
	private bool isPass;
	public float time = 0;
	// Use this for initialization
	void Start () {
		mWin = GameObject.Find ("Win");
		mLost = GameObject.Find ("Lost");
		isPass = GameManager.getIntance ().mHeroIsAlive;
		Debug.Log ("GameOverManager isPass=" + isPass);
		Debug.Log ("GameOverManager Level =" + GameManager.getIntance ().mCurrentLevel);
		if (isPass) {
			mLost.SetActive (false);
		} else {
			mWin.SetActive (false);
		}
	}
	bool isThisSceneDie = false;
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (!isThisSceneDie && time > END_SCENE_TIME) {
			isThisSceneDie = true;
			SceneManager.LoadScene (0);
		}
	}

}
