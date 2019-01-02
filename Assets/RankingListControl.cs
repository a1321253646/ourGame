using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RankingListControl : MonoBehaviour {

    public GameObject mRankingItemOb;


    private Text mMyName, mNyIndex, mMyLevel;
    private Button mClose;
    private InputField mChangeName;
    private VerticalLayoutGroup mVertial;
    private List<RankingListItemControl> mItemList = new List<RankingListItemControl>();

    bool isShow = false;
    int mLevel = -1;
    int mCount = 0;


    Vector2 mFri; 

	// Use this for initialization
	void Start () {
        mMyName = GameObject.Find("ranking_list_myIndex").GetComponent<Text>();
        mNyIndex = GameObject.Find("ranking_list_myname").GetComponent<Text>();
        mMyLevel = GameObject.Find("ranking_list_mylevel").GetComponent<Text>();
        mClose = GameObject.Find("ranking_list_close").GetComponent<Button>();
        mChangeName = GameObject.Find("ranking_list_change_name").GetComponent<InputField>();
        mVertial = GameObject.Find("ranking_list_vertical").GetComponent<VerticalLayoutGroup>();
        mFri = transform.position;

        for (int i = 0; i < 10; i++) {
            createIndex();
        }

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

    public void OnPointerClick(BaseEventData eventData)
    {

    }
    public void onEndEdit(string str)
    {


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
        GameManager.getIntance().getGuideManager().eventNotification(GuideManager.EVENT_SHOW, GuideManager.SHOW_BACK);
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
        int rankingCount = list.Count > 100 ? 100 : list.Count;
        if (list != null && mCount < rankingCount) {
            for (; mCount < rankingCount;) {
                createIndex();
            }
        }
        foreach (RankingListItemControl item in mItemList)
        {
            item.updateList(list);
        }
        if (list.Count == 101)
        {
            mNyIndex.text = list[100].index + "";          
        }
        else {
            mNyIndex.text =  "未上榜";
        }
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.getIntance().mRankingListUpdate) {
            GameManager.getIntance().mRankingListUpdate = false;
            update();
        }
	}

}
