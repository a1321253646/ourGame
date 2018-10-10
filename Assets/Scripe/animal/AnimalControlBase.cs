using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
    private bool isLastSet = false;
    Image mImage;
    private Dictionary<int, AnimalStatu.animalEnd> mEndCall = new Dictionary<int, AnimalStatu.animalEnd>();
    private Dictionary<int, AnimalStatu.animalBegin> mStratCall = new Dictionary<int, AnimalStatu.animalBegin>();

    private bool isStart = false;

    AnimalStatu.animalEnd mDefuleEndCallbak;
    public AnimalControlBase(ResourceBean resource, SpriteRenderer spriteRender) {
        mResource = resource;
        mSpriteRender = spriteRender;
        mSpriteRender.transform.localScale = new Vector3(resource.zoom, resource.zoom, 1);
        mDefuleEndCallbak = new AnimalStatu.animalEnd(animalEnd);
        init(); 
    }
    public AnimalControlBase(ResourceBean resource, Image spriteRender)
    {
        mResource = resource;
        mImage = spriteRender;
        mDefuleEndCallbak = new AnimalStatu.animalEnd(animalEnd);
        init();
    }

    private void init() {
        foreach (ActionFrameBean bean in mResource.getActionFrameList()) {
            bool loop = true;
            if (bean.status == ActionFrameBean.ACTION_DIE) {
                loop = false;
            }
            AnimalStatu statu;
            if (mSpriteRender != null)
            {
                 statu = new AnimalStatu(bean.status, bean.frame, mResource.name, mSpriteRender, loop);
            }
            else {
                 statu = new AnimalStatu(bean.status, bean.frame, mResource.name, mImage, loop);
            }
            
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
    public void end(Sprite endSprite)
    {
        isStart = false;
        if (endSprite != null) {
            if (mSpriteRender != null) {
                mSpriteRender.sprite = endSprite;
            }
            else{
                mImage.sprite = endSprite;
            }
        }
    }
    public void setSpeedData(float speed,int status) {
        if (mAnimalStatuList.ContainsKey(status))
        {
            mAnimalStatuList[status].setSpeedDate(speed);
        }
    }

    public void setStatus(int status, bool isLast) {
        if (isLastSet) {
            return;
        }
   //     Debug.Log(" setStatus = " + status);
        if (mAnimalStatuList.ContainsKey(status))
        {
 //           Debug.Log(" have = " + status);
            mShowAnimalStatu = mAnimalStatuList[status];
            if (mShowAnimalStatu.delayStatue != -1)
            {
                mDelayAnimalStatu = mAnimalStatuList[mShowAnimalStatu.delayStatue];
            }

            changeStatue(mShowAnimalStatu);
        }
        else
        {
            Debug.Log("mo have = " + status);
        }
        isLastSet = isLast;
    }

    public void setStatus(int status) {
        setStatus(status, false);
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
    public void setTimeCountBack(int statue,long count, AnimalStatu.animalCountTimeCallback back)
    {
        if (mAnimalStatuList.ContainsKey(statue))
        {
            mAnimalStatuList[statue].setTimeCountBack(count, back);
        }
    }
    public void setIsLoop(int statue, bool isLoop) {
        if (mAnimalStatuList.ContainsKey(statue))
        {
            mAnimalStatuList[statue].setIsLoop(isLoop);
        }
    }
}
