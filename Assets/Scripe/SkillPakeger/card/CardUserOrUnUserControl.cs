using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class CardUserOrUnUserControl : MonoBehaviour
{
    public static int STATUE_CARP_DEFAULT = 0;
    public static int STATUE_CARP_DOWN = 2;
    public static int STATUE_CARP_UP = 3;

    public int mIndex = 0;
    private Image mImageBottom;
    private Image mImageTop;
    private int mStatue = STATUE_CARP_DEFAULT;
    private bool isInit = false;
    private bool isInTarget = false;

    public SkillJsonBean mSkill;
    public CardJsonBean mCard;
    private CardManager mManager;
    private int targetType;
    List<Attacker> mTargetList;
    private float mTargetX = -1;
    private float mWitch = -1;
    private GameObject mClickShow;
    private Transform HP_Parent;
    float mClickShowX = -1;
    Vector3 mClickV;

    void Start()
    {
        gameObject.transform.SetSiblingIndex(400);
        mClickShow = GameObject.Find("kapai_click");
        mWitch = mClickShow.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().rect.width / 2;
//        Debug.Log("manager 卡牌宽度 " + mWitch);
        HP_Parent = GameObject.Find("Canvas").transform;
    }
    private Vector3 offset;
    public void OnpointUp(BaseEventData date)
    {
        float hight = Screen.height / 2;
        float clickHight = mClickV.y;
        if (mStatue != STATUE_CARP_UP) {
            setStatus(STATUE_CARP_DEFAULT);
            return;
        }
        PlayerBackpackBean newBean = new PlayerBackpackBean();
        newBean.goodId = mCard.id;
        newBean.sortID = mCard.sortID;
        newBean.count = 1;
        newBean.tabId = mCard.tabid;

        if (isUser)
        {
            if (clickHight > hight + 30)
            {
              //  if (BackpackManager.getIntance().use(newBean, 1, TipControl.USE_CARD_TYPE))
             //   {
                    BackpackManager.getIntance().use(newBean, 1, TipControl.USE_CARD_TYPE);
                       Destroy(mIndicator);
                    //  Destroy(gameObject);
                    init(null, -1, false);
                    CardUiControl ui = mIndicator.GetComponent<CardUiControl>();
                    ui.init(-1, 0, null);
                    return;
             //   }
            }
        }
        else {
            if (clickHight < hight)
            {
                BackpackManager.getIntance().use(newBean, 1, TipControl.UNUSE_CARD_TYPE);
                Destroy(mIndicator);
                // Destroy(gameObject);
                init(null, -1, false);
                CardUiControl ui = mIndicator.GetComponent<CardUiControl>();
                ui.init(-1, 0, null);
                BackpackManager.getIntance().updateCardBackShow();
                return;
            }
        }
        setStatus(STATUE_CARP_DEFAULT);
    }
    void OnpointDown(BaseEventData date)
    {
        mClickV = PointUtils.screenTransToWorld(Input.mousePosition);
        setStatus(STATUE_CARP_DOWN);        
        Debug.Log("mStatue " + mStatue);

    }
    public void OnDrag(BaseEventData date)
    {//第二个函数

        mClickV = Input.mousePosition;
        mClickV.z = 0;
        setStatus(STATUE_CARP_UP);
//        Debug.Log("mClickV.x " + mClickV.x+ " mClickV.y "+ mClickV.y);
        //Debug.Log("mManager.getUpLocalY() " + mManager.getUpLocalY() + " mManager.getTopLocalY() " + mManager.getTopLocalY());
        if (mIndicator != null)
        {
            mIndicator.transform.position = new Vector2(mClickV.x, mClickV.y);
        }
    }
    Sprite tnp;
    private GameObject mIndicator;
    private void setStatus(int status)
    {
        if (mStatue == status)
        {
            return;
        }
        mStatue = status;
        if (mStatue == STATUE_CARP_DOWN)
        {
            float x = 0;
            Debug.Log("mClickV.x = " + mClickV.x);
            if (mClickV.x > 0)
            {
                x = 0+ mWitch;
            }
            else if (mClickV.x <= 0)
            {
                x = Screen.width - mWitch;
            }
            mClickShow.GetComponent<CardUiControl>().init(mCard.id, CardUiControl.TYPE_CARD_PLAY, mManager.getHero());
            if (mClickShowX == -1)
            {
                mClickShowX = mClickShow.transform.transform.position.x;
            }
            mClickShow.transform.position = new Vector2(gameObject.transform.position.x, mClickShow.transform.position.y);
            mClickShow.transform.SetSiblingIndex(99999);
            return;
        }
        else
        {
            mClickShow.transform.position = new Vector2(mClickShowX, mClickShow.transform.position.y);
        }
        if (mStatue == STATUE_CARP_UP)
        {
            Debug.Log("STATUE_CARP_UP " + mSkill.id);
            mIndicator = GameObject.Instantiate(
                         mManager.card, new Vector2(mClickV.x, mClickV.y), Quaternion.Euler(0.0f, 0f, 0.0f));
            mIndicator.transform.SetParent(HP_Parent);
            mIndicator.transform.SetSiblingIndex(99999);
            transform.GetChild(0).localScale = new Vector3(0, 0, 1);
            CardUiControl ui = mIndicator.GetComponent<CardUiControl>();
            ui.init(mCard.id, CardUiControl.TYPE_CARD_PLAY, mManager.getHero());
            ui.init(mCard.id, mX, mY);
        }
        else
        {
            transform.GetChild(0).localScale = new Vector3(mScaleX, mScaleY, 1);
            Destroy(mIndicator);
            mIndicator = null;
        }
    }
    private bool isUser = false;

    public void init(CardManager manage, long cardId,bool isUser)
    {
        this.isUser = isUser;
        Debug.Log("init card id =" + cardId);
        mManager = manage;
        
        EventTrigger tri = gameObject.AddComponent<EventTrigger>();        
        if (cardId == -1) {
            if (tri.triggers != null && tri.triggers.Count > 0) {
                tri.triggers.Clear();
            }
            
            isInit = false;
            return;
        }
        isInit = true;
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

        CardJsonBean card = JsonUtils.getIntance().getCardInfoById(cardId);
        mCard = card;
        mSkill = JsonUtils.getIntance().getSkillInfoById(mCard.skill_id);
    }

    public void deleteCard(int index)
    {
        if (index < mIndex)
        {
            mIndex--;
            isInTarget = false;
        }
    }

    private float mX = 0;
    private float mY = 0;
    private float mScaleX = 0;
    private float mScaleY = 0;
    public void init(long cardId, float x, float y)
    {
        // float x = gameObject.GetComponent<RectTransform>().rect.width * gameObject.transform.localScale.x;
        // float y = gameObject.GetComponent<RectTransform>().rect.height * gameObject.transform.localScale.y;
        mX = x;
        mY = y;
        Transform ob = transform.GetChild(0).GetChild(0);
        float x1 = ob.gameObject.GetComponent<RectTransform>().rect.width;
        float y1 = ob.gameObject.gameObject.GetComponent<RectTransform>().rect.height;
        mScaleX = x / x1;
        mScaleY = y / y1;
        transform.GetChild(0).localScale = new Vector3(mScaleX, mScaleY, 1);
    }
}
