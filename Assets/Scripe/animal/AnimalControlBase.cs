using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//用于对象的动画控制，可以理解为动画机
public class AnimalControlBase
{

    private Dictionary<int, AnimalStatu> mAnimalStatuList = new Dictionary<int, AnimalStatu>();
    private AnimalStatu mCurrentAnimalStatu;
    private AnimalStatu mDelayAnimalStatu;
    private AnimalStatu mShowAnimalStatu;
    private ResourceBean mResource;
    private SpriteRenderer mSpriteRender;
    private float mAnimalTime = 0;

    private Dictionary<int, AnimalStatu.animalEnd> mEndCall = new Dictionary<int, AnimalStatu.animalEnd>();
    private Dictionary<int, AnimalStatu.animalBegin> mStratCall = new Dictionary<int, AnimalStatu.animalBegin>();

    private bool isStart = false;

    AnimalStatu.animalEnd mDefuleEndCallbak;
    public AnimalControlBase(ResourceBean resource, SpriteRenderer spriteRender) {
        mResource = resource;
        mSpriteRender = spriteRender;
        mDefuleEndCallbak = new AnimalStatu.animalEnd(animalEnd);
        init();
       
    }
    private void init() {
        foreach (ActionFrameBean bean in mResource.getActionFrameList()) {
            bool loop = true;
            if (bean.status == ActionFrameBean.ACTION_DIE) {
                loop = false;
            }
            AnimalStatu statu = new AnimalStatu(bean.status,bean.frame,mResource.name, mSpriteRender, loop);
            statu.setEndCallBack(mDefuleEndCallbak);
            mAnimalStatuList.Add(bean.status, statu);
        }
        if (mAnimalStatuList.Count == 1 && mAnimalStatuList[0].statue == ActionFrameBean.ACTION_NONE) {
            changeStatue(mAnimalStatuList[0]);
        }
        else{
            changeStatue(mAnimalStatuList[ActionFrameBean.ACTION_STANDY]);
        }
        
    }

    void animalEnd(int status) {
        AnimalStatu animal = mAnimalStatuList[status];
        if (mShowAnimalStatu == mCurrentAnimalStatu && mDelayAnimalStatu != null && mCurrentAnimalStatu.loopDelayTime > 0) {
            changeStatue(mDelayAnimalStatu);
            mAnimalTime = 0;
        }
        if (mEndCall.ContainsKey(status)) {
            mEndCall[status](status);
        }
    }

    public void start() {
        isStart = true;
    }
    public void setSpeedData(float speed,int status) {
        if (mAnimalStatuList.ContainsKey(status))
        {
            mAnimalStatuList[status].setSpeedDate(speed);
        }
    }

    public void setStatus(int status) {
        if (mAnimalStatuList.ContainsKey(status))
        {
            mShowAnimalStatu = mAnimalStatuList[status];
            if(mShowAnimalStatu.delayStatue != -1)
            mDelayAnimalStatu = mAnimalStatuList[mShowAnimalStatu.delayStatue];
            changeStatue(mShowAnimalStatu);
        }
    }

    private void changeStatue(AnimalStatu statue) {
        mCurrentAnimalStatu = statue;
        mCurrentAnimalStatu.initAnimal();
    }

    public void update() {
        if (!isStart) {
            return;
        }
        mAnimalTime += Time.deltaTime;

        if (mCurrentAnimalStatu == mDelayAnimalStatu && mAnimalTime > mShowAnimalStatu.loopDelayTime)
        {
            changeStatue(mShowAnimalStatu);
        }

        if (mCurrentAnimalStatu != null) {
            mCurrentAnimalStatu.upDateTime();
        }
    }

    public void setStatueDelayStatue(int statue, int delayStatue) {
        if (mAnimalStatuList.ContainsKey(statue))
        {
            mAnimalStatuList[statue].delayStatue = delayStatue;
        }
    }

    public void addIndexCallBack(int statue, int frame, AnimalStatu.animalIndexCallback index)
    {
        if (mAnimalStatuList.ContainsKey(statue))
        {
            mAnimalStatuList[statue].addIndexCallBack(frame, index);
        }
    }
    public void setEndCallBack(int statue, AnimalStatu.animalEnd end)
    {
        if (mEndCall.ContainsKey(statue))
        {
            mEndCall[statue] = end;
        }
        else {
            mEndCall.Add(statue, end);
        }
    }
    public void setBeginCallBack(int statue, AnimalStatu.animalBegin begin)
    {
        if (mAnimalStatuList.ContainsKey(statue))
        {
            mAnimalStatuList[statue].setBeginCallBack(begin);
        }
    }

    public void setIsLoop(int statue, bool isLoop) {
        if (mAnimalStatuList.ContainsKey(statue))
        {
            mAnimalStatuList[statue].setIsLoop(isLoop);
        }
    }
}
