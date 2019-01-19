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
    bool isInit = false;
	// Use this for initialization
	void Start () {
        GameManager.getIntance().mInitStatus = 10;
        if (GameManager.getIntance().mIsNeedToReReadAboutLevel) {
            JsonUtils.getIntance().reReadAboutLevelFile(BaseDateHelper.decodeLong( GameManager.getIntance().mCurrentLevel));

        }
        isInit = true;
        //  JsonUtils.getIntance().init();

    }
    private void Update()
    {
        if (!isInit) {
            return;
        }
       long count =  SQLManager.getIntance().getListCount();
        if (count == 0) {
            SQLHelper.getIntance().init();
            SceneManager.LoadScene(0);
        }
    }

}
