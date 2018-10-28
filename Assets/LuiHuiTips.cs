using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LuiHuiTips : MonoBehaviour {
    Text mDes;
    Button mSure, mClose;
    private Vector2 mFri;
    // Use this for initialization
    void Start()
    {
        mDes = GameObject.Find("lunhui_tips_des").GetComponent<Text>();
        mSure = GameObject.Find("lunhui_tips_sure").GetComponent<Button>();
        mClose = GameObject.Find("lunhui_tips_close").GetComponent<Button>();
        mFri = gameObject.transform.localPosition;
        mSure.onClick.AddListener(() =>
        {
            sure();
        });
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });
    }
    public void showUi()
    {
        gameObject.transform.localPosition = new Vector2(0, 0);
        int level = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(level);
        string dec = mDes.text;
        Level level2 = JsonUtils.getIntance().getLevelData();

        dec =dec.Replace("%D", level2.reincarnation + "");
        mDes.text = dec;
    }
    public void removeUi()
    {
        gameObject.transform.localPosition = mFri;
        
    }
    private void sure() {
        GameManager.getIntance().isAddGoodForTest = false; 
        BackpackManager.getIntance().removeAll();
        InventoryHalper.getIntance().dealClear();
        GameManager.getIntance().mCurrentCrystal = 0;
        SQLHelper.getIntance().updateHunJing(0);
        GameManager.getIntance().mHeroLv = 1;
        SQLHelper.getIntance().updateHeroLevel(1);
        GameManager.getIntance().mCurrentLevel = 1;
        SQLHelper.getIntance().updateGameLevel(1);
        Level level = JsonUtils.getIntance().getLevelData();
        GameManager.getIntance().mReincarnation += level.reincarnation*100;
        SQLHelper.getIntance().updateLunhuiValue(GameManager.getIntance().mReincarnation);
        GameObject.Find("qiehuanchangjing").GetComponent<QieHuangChangJing>().run(3);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
