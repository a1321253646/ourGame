using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RankingListControl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject mRankingItemOb;


    private Text mMyName, mNyIndex, mMyLevel;
    private Button mClose;
    private Image mNyIndexIm;
    private VerticalLayoutGroup mVertial;
    private List<RankingListItemControl> mItemList = new List<RankingListItemControl>();
    private ScrollRect mScroll;

    bool isShow = false;
    int mLevel = -1;
    int mCount = 0;


    Vector2 mFri; 

	// Use this for initialization
	void Start () {
        mMyName = GameObject.Find("ranking_list_myname").GetComponent<Text>();
        mNyIndex = GameObject.Find("ranking_list_myIndex").GetComponent<Text>();
        mNyIndexIm = GameObject.Find("ranking_list_myIndex_im").GetComponent<Image>();
        mMyLevel = GameObject.Find("ranking_list_mylevel").GetComponent<Text>();
        mClose = GameObject.Find("ranking_list_close").GetComponent<Button>();
        mVertial = GameObject.Find("ranking_list_vertical").GetComponent<VerticalLayoutGroup>();
        mScroll = GameObject.Find("ranking_list_scroll").GetComponent<ScrollRect>();

        mFri = transform.position;

        for (int i = 0; i < 12; i++) {
            createIndex();
        }
        SetGridHeight();
        mClose.onClick.AddListener(() =>
        {
            removeUi();
        });

    }

    private void createIndex() {
        GameObject good = GameObject.Instantiate(mRankingItemOb,
            new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        good.transform.parent = mVertial.transform;
        good.transform.localScale = Vector2.one;
        RankingListItemControl item = good.GetComponent<RankingListItemControl>();
        item.init(null, mCount);
        mCount++;
        mItemList.Add(item);
    }

    private float mGridHeight = 0;
    private void SetGridHeight()
    {
        //if (mItems.Count > 3) {
        int count = 0;
        if (mItemList.Count < 10)
        {
            count = 10;
        }
        else
        {
            count = mItemList.Count;
        }
        float height = 37 * count;
        mVertial.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        if (mGridHeight != height)
        {
            mGridHeight = height;
            mVertial.transform.Translate(Vector2.down * (height));
        }
        //}

    }

    public void OnPointerClick(BaseEventData eventData)
    {
       GameObject.Find("change_name_tip").GetComponent<ChangeNameTipControl>().startEdit();

    }
    public void onEndEdit(string str)
    {

    }

    public void updateName() {
        mMyName.text = SQLHelper.getIntance().mPlayName;
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
        gameObject.transform.localPosition = new Vector2(50, 0);
        mLevel = GameManager.getIntance().getUiLevel();
        gameObject.transform.SetSiblingIndex(mLevel);
 //       GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_BACK);
        NetServer.getIntance().getRanking();
        update();

    }
    public void removeUi()
    {
        isShow = false;
        // gameObject.transform.TransformPoint(new Vector2(-607, -31));
        gameObject.transform.localPosition = mFri;
    }

    private void update() {
        List<RankingListDateBean> list =  GameManager.getIntance().mRankingList;
        if (list == null) {
            return;
        }
        int rankingCount = list.Count > 101 ? 100 : list.Count-1;
        if (list != null && mCount < rankingCount) {
            for (; mCount < rankingCount;) {
                createIndex();
            }
            SetGridHeight();
        }
        for (int i = 0; i < rankingCount; i++) {
            RankingListItemControl item = mItemList[i];
            item.updateList(list);
        }


        //   foreach (RankingListItemControl item in mItemList)
        //   {
        //       item.updateList(list);
        //   }
        if (list[list.Count - 1].index != -1 && list[list.Count - 1].index < 4) {
            mNyIndex.text = "";
            mNyIndexIm.transform.localScale = new Vector2(1, 1);
            if (list[list.Count - 1].index == 1)
            {
                mNyIndexIm.sprite = Resources.Load("UI_yellow/paihang/02", typeof(Sprite)) as Sprite;
            }
            else if (list[list.Count - 1].index == 2)
            {
                mNyIndexIm.sprite = Resources.Load("UI_yellow/paihang/03", typeof(Sprite)) as Sprite;
            }
            else if (list[list.Count - 1].index == 3)
            {
                mNyIndexIm.sprite = Resources.Load("UI_yellow/paihang/04", typeof(Sprite)) as Sprite;
            }
        }
        else if (list[list.Count - 1].index != -1)
        {
            mNyIndex.text = list[list.Count - 1].index + "";
            mNyIndexIm.transform.localScale = new Vector2(0, 0);
        }
        else {
            mNyIndexIm.transform.localScale = new Vector2(0, 0);
            mNyIndex.text =  "未上榜";
        }
        if (SQLHelper.getIntance().mPlayName == null) {
            string name = SystemInfo.deviceUniqueIdentifier;
            name = "用户" + name.Substring(0, 4);
            mMyName.text = name;
        }
        else {
            mMyName.text = SQLHelper.getIntance().mPlayName;
        }
        if (list[list.Count - 1].level <= 0)
        {
            mMyLevel.text = (list[list.Count - 1].level + (long)JsonUtils.getIntance().getConfigValueForId(100019)) + "";
        }
        else
        {
            mMyLevel.text = list[list.Count - 1].level + "";
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        mScroll.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        mScroll.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        mScroll.OnEndDrag(eventData);
    }
    // Update is called once per frame
    void Update () {
        if (GameManager.getIntance().mRankingListUpdate) {
            GameManager.getIntance().mRankingListUpdate = false;
            update();
        }
	}

}
