using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QiangHuaManager : MonoBehaviour
{
    public bool isShow = false;
    private Button mClose;
    private int mLevel;
    private Vector2 mFri;
    private void Start()
    {
        mFri = gameObject.transform.localPosition;
    }
    public void click()
    {
        if (isShow)
        {
            int level = GameManager.getIntance().getUiCurrentLevel();
            if (mLevel < level)
            {
                showUi();
                return;
            }
            else if (mLevel == level)
            {
                removeUi();
            }
        }
        else
        {
            showUi();
        }
    }

    private void showUi()
    {
        isShow = true;
        //gameObject.transform.TransformPoint(new Vector2(0,0));

        gameObject.transform.localPosition = new Vector2(0, 0);
        upDateUi();
        if (mClose == null)
        {

            mClose = GameObject.Find("qianghua_close").GetComponent<Button>();
            mClose.onClick.AddListener(() => {
                removeUi();
            });
        }
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);

    }
    public void removeUi()
    {
        isShow = false;
        gameObject.transform.localPosition = mFri;
    }

    public void upDateUi()
    {
    }
}
