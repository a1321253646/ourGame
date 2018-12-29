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
        GameManager.getIntance().mInitStatus = 10;
        //  JsonUtils.getIntance().init();
        SQLHelper.getIntance().init();
        SceneManager.LoadScene(0);
    }

}
