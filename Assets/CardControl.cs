﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardControl : MonoBehaviour
{
    public static int STATUE_CARP_DEFAULT = 0;
    public static int STATUE_CARP_DOWN = 2;
    public static int STATUE_CARP_UP = 3;

    public int mIndex = 0;
    private Image mImageBottom ;
    private Image mImageTop ;
    private int mStatue = STATUE_CARP_DEFAULT;
    private bool isInit = false;
    public SkillJsonBean mSkill;
    public CardJsonBean mCard;
    private CardManager mManager;
    private int targetType;
    List<Attacker> mTargetList;
    private float mTargetX =-1;
    private float mWitch = -1;
    void Start()
    {
        gameObject.transform.SetSiblingIndex(400);
        mWitch = gameObject.GetComponent<RectTransform>().rect.xMax -
            gameObject.GetComponent<RectTransform>().rect.xMin;
        Debug.Log("manager 卡牌宽度 " + mWitch);

    }

    private void Update()
    {
        if (!isInit) {
            return;
        }
        if (mStatue == STATUE_CARP_DEFAULT)
        {
            mTargetX = mManager.getLocalXByIndex(mIndex);
            float distance = mTargetX - transform.position.x;
            if (distance > 0)
            {
                float run = 60 * Time.deltaTime;
                if (run > distance)
                {
                    run = distance;
                }
             
                transform.Translate(Vector2.right * run);
            }
        }
        else if (STATUE_CARP_UP == mStatue)
        {
            LocalBean mLocalLink = mManager.getLocalManager().mLocalLink;
            SkillLocalBean bean = new SkillLocalBean();
            Vector3 center = PointUtils.screenTransToWorld(transform.position);
            bean.x = center.x;
            bean.y = center.y;
            bean.type = mSkill.shape_type;
            bean.wight = mSkill.wight;
            bean.leng = mSkill.leng;
            targetType = Attacker.CAMP_TYPE_DEFAULT;
            if (mSkill.target_type == SkillJsonBean.TYPE_SELF)
            {
                targetType = Attacker.CAMP_TYPE_PLAYER;
            }
            else if (mSkill.target_type == SkillJsonBean.TYPE_ENEMY)
            {
                targetType = Attacker.CAMP_TYPE_MONSTER;
            }
            mTargetList = SkillTargetManager.getTargetList(mLocalLink, bean, targetType, true);
        }
        else if (mTargetList !=null &&  mTargetList.Count > 0) {
            foreach (Attacker a in mTargetList) {
                a.setWhith();
            }
        }

    }
    private Vector3 offset;
    public void OnpointUp() {
        if (!mManager.userCard(mIndex, mCard.cost))
        {
            setStatus(STATUE_CARP_DOWN);
        }
        if (mStatue == STATUE_CARP_UP)
        {

            Vector3 v = PointUtils.screenTransToWorld(transform.position);
            if (mSkill.shape_type == 4)
            {
                if (mTargetList != null && mTargetList.Count > 0)
                {
                    mTargetList[0].mSkillManager.addSkill(mSkill.id, mManager.getLocalManager().mLocalLink.mAttacker);
                }
            }
            else if (mSkill.shape_type == 0)
            {
                if (mSkill.effects == 5)
                {
                    float a1 = mSkill.getSpecialParameterValue()[0];
                    GameObject.Find("Manager").GetComponent<LevelManager>().addNengliangDian(a1);
                }
                else if (mSkill.effects == 5) {
                    mManager.getHero().mSkillManager.addSkill(mSkill.id, mManager.getHero());
                }
            }
            else
            {
                SkillManage.getIntance().addSkill(mManager.getHero(), mSkill, v.x, v.y, targetType);


            }
            
            Destroy(gameObject, 0);
            if(mTargetList != null && mTargetList.Count > 0)
            {
                foreach (Attacker a in mTargetList)
                {
                    a.setWhith();
                }
            }
        }
        else if (mStatue == STATUE_CARP_DOWN) {
            mTargetX = mManager.getLocalXByIndex(mIndex);
            // transform.localPosition = new Vector2(targetX, mPointTmp.y);

            transform.Translate(Vector2.right * (mTargetX - transform.position.x));
            transform.Translate(Vector2.up * ( mPointTmp.y - transform.position.y));
            // transform.Translate(new Vector2(targetX,mPointTmp.y));
        }
    }
    Vector3 mPointTmp;
    public void OnpointDown()
    {
        mStatue = STATUE_CARP_DOWN;
        offset = transform.position -Input.mousePosition;
        mPointTmp = transform.position;

    }
    public void OnDrag()
    {//第二个函数

       // transform.Translate(offset + Input.mousePosition);
        transform.position = offset +Input.mousePosition;
       
        if (transform.position.y > mManager.getUpLocalY() && mStatue == STATUE_CARP_DOWN)
        {
            setStatus(STATUE_CARP_UP);
        }
        else if(transform.position.y < mManager.getUpLocalY() && mStatue == STATUE_CARP_UP)
        {
            setStatus(STATUE_CARP_DOWN);
        }
        //Drag就是拖动的意思，在拖动的时候，物体跟着鼠标
    }
    Sprite tnp;

    private void setStatus(int status) {
        if (mStatue == status) {
            return;
        }
        mStatue = status;
        Debug.Log("mStatue = " + mStatue);
        if (mStatue == STATUE_CARP_UP && mSkill.shape_type != 0)
        {
            RectTransform rt = GetComponent<RectTransform>();
            Debug.Log("mSkill.leng =" + mSkill.leng + " mSkill.wight=" + mSkill.wight);
            Vector3 v = PointUtils.getScreenSize(new Vector3(mSkill.leng, mSkill.wight, 0));
            Debug.Log("v.x =" + v.x + " v.y=" + v.y);
            rt.sizeDelta = new Vector2(v.x, v.y);
            Sprite sprite = Resources.Load("UI/"+mSkill.shape_resource_id, typeof(Sprite)) as Sprite;
            tnp = mImageBottom.sprite;
            mImageBottom.sprite = sprite;
            mImageTop.color = new Color(1, 1, 1, 0);
        }
        else {      
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(53, 72);
            mImageBottom.sprite = tnp;
            mImageTop.color = new Color(1, 1, 1, 1);
        }
    }

    public void init(int index, CardManager manage,long cardId) {
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(cardId);
        isInit = true;
        mManager = manage;
        mCard = card;
        mSkill = JsonUtils.getIntance().getSkillInfoById(mCard.skill_id);
        if (mImageBottom == null)
        {
            mImageBottom = GetComponentsInChildren<Image>()[0];
            Debug.Log("mCard.center_resource = " + mCard.center_resource);
            Sprite sprite = Resources.Load("UI/"+ mCard.center_resource, typeof(Sprite)) as Sprite;
            mImageBottom.sprite = sprite;
        }
        if (mImageTop == null)
        {
            mImageTop = GetComponentsInChildren<Image>()[1];
            Debug.Log("mCard.top_resource = " + mCard.top_resource);
            Sprite sprite = Resources.Load("UI/" + mCard.top_resource, typeof(Sprite)) as Sprite;
            mImageTop.sprite = sprite;
        }
        mIndex = index;
    }

    public void deleteCard(int index) {
        if (index < mIndex) {
            mIndex--;
        }
    }

}