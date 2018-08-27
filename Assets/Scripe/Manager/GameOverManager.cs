using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

	GameObject mWin,mLost;
	public float END_SCENE_TIME = 5;
	private bool isPass;
	public float time = 0;
    private Button  mShowBt, mWinBt, mLoseBt;
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
           // GameManager.getIntance().mCurrentLevel = 1;
        }
        if (GameObject.Find("win_Button") != null)
        {
            mShowBt = GameObject.Find("win_Button").GetComponent<Button>();
        }
        else {
            mShowBt = GameObject.Find("lose_Button").GetComponent<Button>();
        }
        InventoryHalper.getIntance().dealClear();
        GameManager.getIntance().mCurrentCrystal = 0;
        GameManager.getIntance().mHeroLv = 1;
        GameManager.getIntance().mCurrentLevel = 1;
        mShowBt.onClick.AddListener(() => {
            loadScene(0);
        });
    }
    private void loadScene(int index) {
        if (!isThisSceneDie )
        {
            isThisSceneDie = true;
            SceneManager.LoadScene(0);
        }
    }
	bool isThisSceneDie = false;
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if ( time > END_SCENE_TIME) {
			SceneManager.LoadScene (0);
		}
	}

}
