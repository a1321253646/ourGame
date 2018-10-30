using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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
    private bool isInTarget = false;

    public SkillJsonBean mSkill;
    public CardJsonBean mCard;
    private CardManager mManager;
    private int targetType;
    List<Attacker> mTargetList;
    private float mTargetX =-1;
    private float mWitch = -1;
    private GameObject mClickShow;
    float mClickShowX = -1;
    Vector3 mClickV;

    void Start()
    {
        gameObject.transform.SetSiblingIndex(400);
        mWitch = gameObject.GetComponent<RectTransform>().rect.xMax -
            gameObject.GetComponent<RectTransform>().rect.xMin;
        //       Debug.Log("manager 卡牌宽度 " + mWitch);
        mClickShow = GameObject.Find("kapai_click");
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
                float run = 800 * Time.deltaTime;
                if (run > distance)
                {
                    run = distance;
                }

                transform.Translate(Vector2.right * run);
            }
            else {
                isInTarget = true;
            }
        }
        else if (STATUE_CARP_UP == mStatue)
        {
            LocalBean mLocalLink = mManager.getLocalManager().mLocalLink;
            SkillLocalBean bean = new SkillLocalBean();
            
            bean.x = mClickV.x;
            bean.y = mClickV.y;
            bean.type = mSkill.shape_type;
            ResourceBean res = JsonUtils.getIntance().getEnemyResourceData(mSkill.skill_resource);
            if (res != null) {
                bean.wight = mSkill.wight * res.zoom;
                bean.leng = mSkill.leng * res.zoom;
            }
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
    public void OnpointUp(BaseEventData date) {
        if (mStatue == STATUE_CARP_UP)
        {

            if (mSkill.shape_type == SkillTargetManager.TYPE_SHAPE_POINT && (mTargetList == null || mTargetList.Count == 0)) {
                setStatus(STATUE_CARP_DEFAULT);
                GameObject obj = Resources.Load<GameObject>("prefab/hurt");
                Vector3 v1 = PointUtils.worldTransToScreen(mClickV);
                GameObject text = GameObject.Instantiate(obj,
                    new Vector2(v1.x, v1.y), Quaternion.identity);
                Transform hp = GameObject.Find("enemyStatePlane").transform;
                text.transform.SetParent(hp);
                Text tv = text.GetComponent<Text>();
                tv.text = "请选择一个目标";
                tv.color = Color.red;
                tv.fontSize = 30;
                UiManager.FlyTo(tv);
                return;
            }
            else if (!mManager.userCard(mIndex, mCard.cost))
            {
                setStatus(STATUE_CARP_DEFAULT);
                GameObject obj = Resources.Load<GameObject>("prefab/hurt");
                Vector3 v1 = PointUtils.worldTransToScreen(mClickV);
                GameObject text = GameObject.Instantiate(obj,
                    new Vector2(v1.x, v1.y), Quaternion.identity);
                Transform hp = GameObject.Find("enemyStatePlane").transform;
                text.transform.SetParent(hp);
                Text tv = text.GetComponent<Text>();
                tv.text = "能量点不足";
                tv.color = Color.red;
                tv.fontSize = 30;
                UiManager.FlyTo(tv);
                return;
            }
            Vector3 v = PointUtils.screenTransToWorld(transform.position);
            if (mSkill.shape_type == 4 && mTargetList != null && mTargetList.Count > 0)
            {
                if (mTargetList != null && mTargetList.Count > 0)
                {
                    mTargetList[0].mSkillManager.addSkill(mSkill.id, mManager.getLocalManager().mLocalLink.mAttacker);
                }
            }
            else if (mSkill.shape_type == 0)
            {
                if (mSkill.effects == 4)
                {
                    float a1 = mSkill.getSpecialParameterValue()[0];
                    float a2 = mSkill.getSpecialParameterValue()[1];
                    GameObject.Find("Manager").GetComponent<LevelManager>().addNengliangDian(a1);
                    mManager.getHero().BeKillAttack(mSkill.effects, mManager.getHero().mAttribute.maxBloodVolume * (a2/100));

                }
                else if (mSkill.effects == 5)
                {
                    mManager.addCards((long)mSkill.getSpecialParameterValue()[0]);
                }
            }
            else if (mSkill.shape_type == 6) {
                mManager.getHero().mSkillManager.addSkill(mSkill.id, mManager.getHero());
               // mManager.getHero().BeKillAttack(mSkill.effects, mManager.getHero().mAttribute.maxBloodVolume * 0.2f);
            }
            else
            {
                SkillManage.getIntance().addSkill(mManager.getHero(), mSkill, mClickV.x, mClickV.y, targetType);
            }

            setStatus(STATUE_CARP_DEFAULT);                 
            if (mTargetList != null && mTargetList.Count > 0)
            {
                foreach (Attacker a in mTargetList)
                {
                    a.setWhith();
                }
            }
            Destroy(gameObject, 0);
        }
        else if (mStatue == STATUE_CARP_DOWN) {
            setStatus(STATUE_CARP_DEFAULT);
            //mClickShow.transform.GetChild(0).position = new Vector2(mClickShowX, mClickShow.transform.GetChild(0).position.y);
        }
    }
    void OnpointDown(BaseEventData  date)
    {
        if (!isInTarget) {
            return;
        }
        setStatus(STATUE_CARP_DOWN);
        Debug.Log("mStatue " + mStatue);

    }
    public void OnDrag(BaseEventData date)
    {//第二个函数
        if (!isInTarget)
        {
            return;
        }
       
        mClickV = PointUtils.screenTransToWorld(Input.mousePosition);
        mClickV.z = 0;
//        Debug.Log("mClickV.x " + mClickV.x+ " mClickV.y "+ mClickV.y);
 //       Debug.Log("mManager.getUpLocalY() " + mManager.getUpLocalY() + " mManager.getTopLocalY() " + mManager.getTopLocalY());
        if (Input.mousePosition.y >= mManager.getUpLocalY() && Input.mousePosition.y <= mManager.getTopLocalY())
        {
            setStatus(STATUE_CARP_UP);
        }
        else {
            setStatus(STATUE_CARP_DOWN);
        }
        if (mIndicator != null) {
            mIndicator.transform.position = new Vector2(mClickV.x, mClickV.y);
        }

/*
       // transform.Translate(offset + Input.mousePosition);
        transform.position = offset +Input.mousePosition;
       
        if (transform.position.y > 
            mManager.getUpLocalY() && 
            mStatue == STATUE_CARP_DOWN)
        {
            setStatus(STATUE_CARP_UP);
        }
        else if(transform.position.y < mManager.getUpLocalY() && mStatue == STATUE_CARP_UP)
        {
            setStatus(STATUE_CARP_DOWN);
        }*/
        //Drag就是拖动的意思，在拖动的时候，物体跟着鼠标
    }
    Sprite tnp;
    private GameObject mIndicator;
    private void setStatus(int status) {
        if (mStatue == status) {
            return;
        }
        mStatue = status;
        if (mStatue == STATUE_CARP_DOWN)
        {
            float x;
            if (mIndex == 1)
            {
                x = mManager.getLocalXByIndex(2);
            }
            else if (mIndex == 10)
            {
                x = mManager.getLocalXByIndex(9);
            }
            else {
                x = transform.GetChild(0).position.x;
            }
            mClickShow.GetComponent<CardUiControl>().init(mCard.id, CardUiControl.TYPE_CARD_PLAY,mManager.getHero());
            if (mClickShowX == -1)
            {
                mClickShowX = mClickShow.transform.transform.position.x;
            }
            mClickShow.transform.GetChild(0).position = new Vector2(x, mClickShow.transform.GetChild(0).position.y);
        }
        else {

            mClickShow.transform.GetChild(0).position = new Vector2(mClickShowX, mClickShow.transform.GetChild(0).position.y);
        }
        if (mStatue == STATUE_CARP_UP && mSkill.shape_type != 0 && mSkill.shape_type != 6)
        {
            Debug.Log("STATUE_CARP_UP " + mSkill.id);
            Sprite sprite = Resources.Load("UI/" + mSkill.shape_resource_id, typeof(Sprite)) as Sprite;
            mIndicator = GameObject.Instantiate(
                         mManager.getIndicator(), new Vector2(mClickV.x, mClickV.y), Quaternion.Euler(0.0f, 0f, 0.0f));
            mIndicator.GetComponent<SpriteRenderer>().sprite = sprite;
            ResourceBean res = JsonUtils.getIntance().getEnemyResourceData(mSkill.skill_resource);
            mIndicator.GetComponent<SpriteRenderer>().size = new Vector2(mSkill.leng*res.zoom, mSkill.wight * res.zoom);
        }
        else {
            Destroy(mIndicator);
            mIndicator = null;
        }
        if (mTargetList != null) {
            foreach (Attacker a in mTargetList)
            {
                a.setWhith();
            }
        }

        /*
                Debug.Log("mStatue = " + mStatue);
                if (mStatue == STATUE_CARP_UP && mSkill.shape_type != 0)
                {
                    RectTransform rt = GetComponent<RectTransform>();
                    Debug.Log("mSkill.leng =" + mSkill.leng + " mSkill.wight=" + mSkill.wight);
                    Vector3 v = PointUtils.getScreenSize(new Vector3(mSkill.leng, mSkill.wight, 0));
                    Debug.Log("v.x =" + v.x + " v.y=" + v.y);
                    rt.sizeDelta = new Vector2(v.x, v.y);

                    tnp = mImageBottom.sprite;
                    mImageBottom.sprite = sprite;
                    mImageTop.color = new Color(1, 1, 1, 0);
                }
                else {      
                    RectTransform rt = GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(53, 72);
                    mImageBottom.sprite = tnp;
                    mImageTop.color = new Color(1, 1, 1, 1);
                }*/
    }

    public void init(int index, CardManager manage,long cardId) {
        Debug.Log("init card id =" + cardId);
        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(cardId);
        isInit = true;
        mManager = manage;
        mCard = card;
        mSkill = JsonUtils.getIntance().getSkillInfoById(mCard.skill_id);
        mIndex = index;
        EventTrigger tri = gameObject.AddComponent<EventTrigger>();
        tri.triggers = new List<EventTrigger.Entry>();

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerDown;
        UnityAction<BaseEventData> callback1 = new UnityAction<BaseEventData>(OnpointDown);
        entry1.callback.AddListener(callback1);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.Drag;
        UnityAction<BaseEventData> callback2 = new UnityAction<BaseEventData>(OnDrag);
        entry2.callback.AddListener(callback2);
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerUp;
        UnityAction<BaseEventData> callback3 = new UnityAction<BaseEventData>(OnpointUp);
        entry3.callback.AddListener(callback3);
        
        tri.triggers.Add(entry1);
        tri.triggers.Add(entry2);
        tri.triggers.Add(entry3);
        //添加事件触发记录到GameObject的事件触发组件  

        if (mImageBottom == null)
        {
            mImageBottom = GetComponentsInChildren<Image>()[1];
        }
        if (mImageTop == null)
        {
            mImageTop = GetComponentsInChildren<Image>()[2];
        }

    }

    public void deleteCard(int index) {
        if (index < mIndex) {
            mIndex--;
            isInTarget =false;
        }
    }
    public void init(long cardId, float x, float y)
    {
        // float x = gameObject.GetComponent<RectTransform>().rect.width * gameObject.transform.localScale.x;
        // float y = gameObject.GetComponent<RectTransform>().rect.height * gameObject.transform.localScale.y;
        Transform ob = transform.GetChild(0).GetChild(0);
        float x1 = ob.gameObject.GetComponent<RectTransform>().rect.width;
        float y1 = ob.gameObject.gameObject.GetComponent<RectTransform>().rect.height;
        transform.GetChild(0).localScale = new Vector3(x / x1, y / y1, 1);
    }
}
