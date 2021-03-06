﻿using UnityEngine;
using System.Collections;

public class LevelUpLongPress : MonoBehaviour
{
    private bool IsStart = false;
    private float mLongPressTIme = -1;
    private float mTime = 0;

    void Update()
    {
        if (GameManager.getIntance().uiManager == null || GameManager.getIntance().uiManager.mLvUpBt == null) {
            return;
        }

        if (IsStart)
        {
            mTime += Time.deltaTime;
            if (mTime >= mLongPressTIme && GameManager.getIntance().uiManager.mLvUpBt.interactable)
            {
                mTime -= mLongPressTIme;
                Debug.Log("长按触发");
                
             //   if (GameManager.getIntance().isEnd || !GameManager.getIntance().mHeroIsAlive)
             //   {
                    GameManager.getIntance().heroUp();
                    saveDate();
             //   }
            }
        }
    }
    public void LongPress(bool bStart)
    {
        IsStart = bStart;
        if (IsStart)
        {
            if (mLongPressTIme == -1)
            {
                mLongPressTIme = JsonUtils.getIntance().getConfigValueForId(100041);
            }
            if (GameManager.getIntance().uiManager.mLvUpBt.interactable) {
                GameManager.getIntance().heroUp();
            //    if (GameManager.getIntance().isEnd || !GameManager.getIntance().mHeroIsAlive)
            //    {
                    saveDate();
            //    }
               
            }          
            mTime = 0;
        }
        else
        {
            if (!GameManager.getIntance().isEnd && GameManager.getIntance().mHeroIsAlive)
            {
                saveDate();
            }
        }
    }
    private void saveDate() {
//        Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>英雄入库等级=" + BaseDateHelper.decodeLong(GameManager.getIntance().mHeroLv) + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        SQLHelper.getIntance().updateHeroLevel(GameManager.getIntance().mHeroLv);
        SQLHelper.getIntance().updateHunJing(GameManager.getIntance().mCurrentCrystal);
    }
}

