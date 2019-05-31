using UnityEngine;
using System.Collections;

public class ZhuangBeiItemLongPress : MonoBehaviour
{
    private bool IsStart = false;
    private float mLongPressTIme = 0;
    private float mTime = 0;
    ZhuangBeiItemShowControl mParent;
    private int mEachTime = 1;
    private int mHappendTime = 0;
    private void Start()
    {
        mParent = gameObject.transform.GetComponentInParent<ZhuangBeiItemShowControl>();
    }
    void Update()
    {
        if (mParent == null) {
            return;
        }
        if (mParent.mButton != null)
        {
            if (GameManager.getIntance().mCurrentCrystal.ieEquit(mParent.updateCost) != -1)
            {
                mParent.mButton.interactable = true;
            }
            else
            {
                mParent.mButton.interactable = false;
            }
        }
        if (IsStart)
        {
            mTime += Time.deltaTime;
            if (mTime >= mLongPressTIme && GameManager.getIntance().mCurrentCrystal.ieEquit(mParent.updateCost) != -1)
            {
                mTime -= mLongPressTIme;
                mHappendTime++;
                if (mHappendTime > 10) {
                    mHappendTime = 0;
                    mEachTime++;
                }
                Debug.Log("长按触发");
                for(int i = 0; i< mEachTime; i++) {
                    if (GameManager.getIntance().mCurrentCrystal.ieEquit(mParent.updateCost) != -1) {
                        BackpackManager.getIntance().UpdateZhuangBei(mParent.mBean, mParent.updateCost, mParent.level);
                    }                    
                }
                BackpackManager.getIntance().updateZhuangbeiEnd();
                    // if (GameManager.getIntance().isEnd || !GameManager.getIntance().mHeroIsAlive)
                    // {
                    BackpackManager.getIntance().saveEquipDate(mParent.mBean);
               // }
            }
        }
    }
    public void LongPress(bool bStart)
    {
        IsStart = bStart;
        if (IsStart)
        {
            if (mLongPressTIme == 0)
            {
                // mLongPressTIme = JsonUtils.getIntance().getConfigValueForId(100041);
                mLongPressTIme = JsonUtils.getIntance().getConfigValueForId(100041); ;
            }
            if (GameManager.getIntance().mCurrentCrystal.ieEquit(mParent.updateCost) != -1)
            {
                BackpackManager.getIntance().UpdateZhuangBei(mParent.mBean, mParent.updateCost, mParent.level);
                BackpackManager.getIntance().updateZhuangbeiEnd();
                //if (GameManager.getIntance().isEnd || !GameManager.getIntance().mHeroIsAlive)
                //{
                    BackpackManager.getIntance().saveEquipDate(mParent.mBean);
                //}
            }
            mTime = 0;
        }
        else {
            if (!GameManager.getIntance().isEnd && GameManager.getIntance().mHeroIsAlive)
            {
                BackpackManager.getIntance().saveEquipDate(mParent.mBean);
            }
            mEachTime = 1;
            mHappendTime = 0;
        }
    }

}

