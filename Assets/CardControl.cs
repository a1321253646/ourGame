using System.Collections;
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
    private SkillJsonBean mSkill;
    private CardJsonBean mCard;
    private CardManager mManager;
    private int targetType;
    void Start()
    {
        gameObject.transform.SetSiblingIndex(99999);
    }

    private void Update()
    {
        if (!isInit) {
            return;
        }
        if (mStatue == STATUE_CARP_DEFAULT)
        {
            float targetX = 600 - (mIndex - 1) * 70;
            float distance = targetX - transform.position.x;
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
        else if(STATUE_CARP_UP == mStatue) {
            LocalBean mLocalLink = mManager.getLocalManager().mLocalLink;
            SkillLocalBean bean =  new SkillLocalBean();
            Vector3 center = PointUtils.screenTransToWorld(transform.position);
            bean.x = center.x;
            bean.y = center.y;
            bean.type = mSkill.shape_type;
            bean.wight = mSkill.wight;
            bean.leng = mSkill.leng;
            int targetType = Attacker.CAMP_TYPE_DEFAULT;
            if (mSkill.target_type == SkillJsonBean.TYPE_SELF)
            {
                targetType = Attacker.CAMP_TYPE_PLAYER;
            }
            else if (mSkill.target_type == SkillJsonBean.TYPE_ENEMY) {
                targetType = Attacker.CAMP_TYPE_MONSTER;
            }

            List<Attacker> list =  SkillTargetManager.getTargetList(mLocalLink, bean, targetType,true,null);

        }

    }
    private Vector3 offset;
    public void OnpointUp() {
        if (mStatue == STATUE_CARP_UP)
        {
            mManager.userCard(mIndex);
            Vector3 v = PointUtils.screenTransToWorld(transform.position);
            SkillManage.getIntance().addSkill(mSkill, v.x, v.y, targetType);
            Destroy(gameObject, 0);
        }
        else if (mStatue == STATUE_CARP_DOWN) {
            float targetX = 600 - (mIndex - 1) * 70;
            // transform.localPosition = new Vector2(targetX, mPointTmp.y);

            transform.Translate(Vector2.right * (targetX - transform.position.x));
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
       
        if (transform.position.y >= 130 && mStatue == STATUE_CARP_DOWN)
        {
            setStatus(STATUE_CARP_UP);
        }
        else if(transform.position.y < 130 && mStatue == STATUE_CARP_UP)
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
        if (mStatue == STATUE_CARP_UP)
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
