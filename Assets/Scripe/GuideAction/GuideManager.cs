using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideManager : MonoBehaviour {

    public static int EVENT_CLICK_BUTTON = 1;
    public static int EVENT_SHOW = 2;
    public static int EVENT_ENEMY_DEAL = 3;
    public static int EVENT_OBJECT_DROP = 4;
    public static int EVENT_BUTTON_ENABLE = 5;
    public static int EVENT_LEVEL_UP_ENABLE = 6;
    public static int EVENT_LUNHUI_UP_ENABLE = 7;
    public static int EVENT_OBJECT_CLICK = 8;
    public static int EVENT_EQUITE_UP_CLICK = 9;
    public static int EVENT_ONDRAG_UP = 10;
    public static int EVENT_SHOW_VOCATION = 11;

    public static int BUTTON_START_BOSS = 1;
    public static int BUTTON_START_HERO_UP = 2;
    public static int BUTTON_START_OPEN_BACK = 3;
    public static int BUTTON_CLICK_TIP_SURE = 4;
    public static int BUTTON_CLICK_OPEN_CARD = 5;
    public static int BUTTON_CLICK_OPEN_VOCATION = 6;
    public static int BUTTON_CLICK_CLICK_VOCATION = 7;

    public static int SHOW_BACK = 1;
    public static int SHOW_TIP = 2;
    public static int SHOW_CARD = 3;
    public static int SHOW_CARD_BACK_INFO = 4;
    public static int SHOW_CARD_VOCATION = 5;


    public static int ONDRAG_UP_CARDUI_WORK = 1;
    public static int ONDRAG_UP_CARDUI_UNWORK = 2;


    


    public GameObject mGuideDec; 
    public GameObject mMaskItem; 

    public  Transform mRootTransform;
    public GameObject mCamera;
    public  GameObject mTop;
    public  GameObject mRight ;
    public  GameObject mLeft;
    public  GameObject mBotton;
    public  GameObject mDec;

    float mxBili = 0;
    float myBili = 0;

    GameObject mGanmeTarget;

    private List<GuideControl> mGuideList = new List<GuideControl>();
    private GuideControl mCurrentGuide = null;

    //返回值-1表示该事件符合引导，返回值0表示该事件是引导，并由control来显示引导，大于0表示需要事件发生源进行显示引导,-2表示引导结束
    public long eventNotification(int eventID) {
        return notificationDeal(eventID, -1);
    }
    public long eventNotification(int eventID,long eventValue)
    {
        return notificationDeal(eventID, eventValue);
    }

    public bool guideIsHappen(long id) {
        foreach (GuideControl g in mGuideList) {
            if(g.mData.id == id) {
                return true;    
            }
        }

        return false;
    }

    public void removeGuide(long id) {
        for (int i = 0; i< mGuideList.Count;i++ )
        {
            if (mGuideList[i].mData.id == id)
            {
                mGuideList.RemoveAt(i);
                SQLHelper.getIntance().addGuide(i);
            }
        }
    }

    private long notificationDeal(int eventID, long eventValue) {
        //        Debug.Log("notificationDeal mCurrentGuide eventID= "+ eventID+ " eventValue= "+ eventValue);
        long back = -1;
        if (mGuideList.Count == 0)
        {
            return -1;
        }
        if (mCurrentGuide != null) {
            back =  mCurrentGuide.notificationDeal(eventID, eventValue);
            if (back == -2) {
                SQLHelper.getIntance().addGuide(mCurrentGuide.mData.id);
                mGuideList.Remove(mCurrentGuide);
                mCurrentGuide = null;
                back = 0;
               

            }
            return back;
        }else if (mGuideList.Count > 0) {
  //          Debug.Log("notificationDeal mCurrentGuide == null");
            foreach (GuideControl c in mGuideList) {
                if (c.mData.guideId == 4) {
                    List<PlayerBackpackBean> list =   SQLHelper.getIntance().getUserd();
                    if (list == null || list.Count == 0)
                    {
                        continue;
                    }
                    bool isHave = false;
                    for (int i = 0; i < list.Count; i++) {
                        if (list[i].goodId == 2041001) {
                            isHave = true;
                            break;
                        }
                    }
                    if (!isHave) {
                        continue;
                    }
                }
                back = c.notificationDeal(eventID, eventValue);
//                Debug.Log("notificationDeal back == "+ back);
                if (back != -1) {
                
                    mCurrentGuide = c;
                    return back;
                }
            }
        }
        return -1;
    }

    // Use this for initialization
    void Start()
    {
        mRootTransform = GameObject.Find("Canvas").GetComponent<Transform>();
        //  if (mxBili == 0) {
        getBili();
        // }

    }

    public void init() {
        List<GuideJsonBean> list = JsonUtils.getIntance().getGuideList();
        List<long> guide = SQLHelper.getIntance().getGuide();
        long level = BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel);
        foreach (GuideJsonBean g in list) {
            bool isGuide = false;
            if (g.level != -100 && level > g.level)
            {
                continue;
            }
            if (guide.Count > 0) {
                foreach (long id in guide) {
                    if (id == g.id) {
                        isGuide = true;
                        break;
                    }
                }
            }
            if (isGuide)
            {
                continue;
            }
            GuideControl c = new GuideControl();
            c.init(g, this);
            mGuideList.Add(c);
        }
    }

    private void getBili()
    {
        float x = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
        float y = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.y;
        myBili = Screen.height / y;
        mxBili = Screen.width / x;
    }
    public void ShowGuideGrideLayoutInScroll(GameObject ob, GameObject mBackScroll, GridLayoutGroup mBackListGl, int item, int hengCount)
    {
        Transform tf = ob.transform;
        float sx = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
        float sy = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.y;
        float x = 0, y = 0;
        if (mBackScroll != null) {
            x = mBackScroll.transform.position.x -  (mBackScroll.GetComponent<RectTransform>().rect.width / 2) * mxBili;
            y = mBackScroll.transform.position.y + (mBackScroll.GetComponent<RectTransform>().rect.height / 2) * myBili * GameCamera.SCREEN_BILI;
        }
        Debug.Log(" x = " + mBackScroll.transform.position.x + " y=" + mBackScroll.transform.position.y);

        int heng = item / hengCount;
        int lie = item % hengCount;
        Debug.Log(" heng = " + heng + " lie=" + lie);
        x = x + (mBackListGl.padding.left + mBackListGl.cellSize.x * lie + mBackListGl.spacing.x * lie) * mxBili ;
        y = y - (mBackListGl.padding.top - mBackListGl.cellSize.y * heng - mBackListGl.spacing.y * heng) * myBili * GameCamera.SCREEN_BILI;
        Debug.Log(" x = " + mBackScroll.transform.position.x + " y=" + mBackScroll.transform.position.y);
        Vector2 lu = new Vector2(x, y);
        Vector2 rd = new Vector2(x + mBackListGl.cellSize.x * mxBili, y - mBackListGl.cellSize.y * myBili * GameCamera.SCREEN_BILI);

        showGuide(ob, lu, rd);
    }

    public void ShowGuideHorizontalLayoutGroupInScroll(GameObject ob, ScrollRect mBackScroll, HorizontalLayoutGroup mBackListGl, int item,int childindex)
    {
        Transform tf = ob.transform;
        float sx = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.x;
        float sy = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution.y;

        Debug.Log(" sw = " + mBackScroll.GetComponent<RectTransform>().rect.width + " sh=" + mBackScroll.GetComponent<RectTransform>().rect.height);
        Debug.Log(" x = " + mBackScroll.transform.position.x + " y=" + mBackScroll.transform.position.y);
        float x = mBackScroll.transform.position.x - (mBackScroll.GetComponent<RectTransform>().rect.width / 2) * mxBili;
        float y = mBackScroll.transform.position.y + (mBackScroll.GetComponent<RectTransform>().rect.height / 2) * myBili;
        x = x + (mBackListGl.padding.left + ob.GetComponent<RectTransform>().rect.width * item + mBackListGl.spacing * item) * mxBili;
        y = y - (mBackListGl.padding.top) * myBili;

        Vector2 lu = new Vector2(x, y);
        Vector2 rd = new Vector2(x + ob.GetComponent<RectTransform>().rect.width * mxBili, y - ob.GetComponent<RectTransform>().rect.height * myBili);

        if (childindex != -1) {
            Transform child = ob.transform.GetChild(childindex);
            float y1, y2, x1, x2;
            y1 = (lu.y + rd.y) / 2 + child.gameObject.GetComponent<RectTransform>().localPosition.y * mxBili + child.gameObject.GetComponent<RectTransform>().rect.height * mxBili / 2;
            y2 = (lu.y + rd.y) / 2 + child.gameObject.GetComponent<RectTransform>().localPosition.y * mxBili - child.gameObject.GetComponent<RectTransform>().rect.height * mxBili / 2;
            x1 = (lu.x + rd.x) / 2 + child.gameObject.GetComponent<RectTransform>().localPosition.x - child.gameObject.GetComponent<RectTransform>().rect.width * mxBili / 2;
            x2 = (lu.x + rd.x) / 2 + child.gameObject.GetComponent<RectTransform>().localPosition.x + child.gameObject.GetComponent<RectTransform>().rect.width * mxBili / 2;
            lu.y = y1;
            lu.x = x1;
            rd.y = y2;
            rd.x = x2;
        }
        Debug.Log(" lu x = " + lu.x + " lu y=" + lu.y);
        Debug.Log(" rd x = " + rd.x + " rd y=" + rd.y);
        showGuide(ob, lu, rd);
    }
    public void ShowGuideNormalObject(GameObject ob)
    {
            Transform tf = ob.GetComponent<RectTransform>().transform;
            float w = ob.GetComponent<RectTransform>().rect.width * ob.transform.localScale.x * myBili * GameCamera.SCREEN_BILI;
            float h = ob.GetComponent<RectTransform>().rect.height * ob.transform.localScale.y * myBili * GameCamera.SCREEN_BILI;
            Debug.Log(" x = " + tf.position.x + " y=" + tf.position.y);
            Debug.Log(" w = " + w + " y=" + h);
            Vector2 lu = new Vector2(ob.transform.position.x - w / 2, ob.transform.position.y + h / 2);
            Vector2 rd = new Vector2(ob.transform.position.x + w / 2, ob.transform.position.y - h / 2);
            Debug.Log(" lu x = " + lu.x + " lu y=" + lu.y);
            Debug.Log(" rd x = " + rd.x + " rd y=" + rd.y);
            GameManager.getIntance().getGuideManager().showGuide(ob, lu, rd);
    }
    public void ShowGuideVertorGameObject(Vector2 v, GameObject ob) {
        float x = 0, y = 0;
        Transform tf = ob.GetComponent<RectTransform>().transform;
        float w = ob.GetComponent<RectTransform>().rect.width * ob.transform.localScale.x * myBili * GameCamera.SCREEN_BILI;
        float h = ob.GetComponent<RectTransform>().rect.height * ob.transform.localScale.y * myBili * GameCamera.SCREEN_BILI;
        Debug.Log(" x = " + tf.position.x + " y=" + tf.position.y);
        Debug.Log(" w = " + w + " y=" + h);

            x = v.x;
            y = v.y;
    
        Vector2 lu = new Vector2(x - w / 2,y+ h / 2);
        Vector2 rd = new Vector2(x + w / 2,y - h / 2);
        Debug.Log(" lu x = " + lu.x + " lu y=" + lu.y);
        Debug.Log(" rd x = " + rd.x + " rd y=" + rd.y);
        GameManager.getIntance().getGuideManager().showGuide(ob, lu, rd);
    }

    public void showGuide(GameObject go, Vector2 lefUp, Vector2 rightBottom) {
//        Debug.Log("================================showGuide  mTop ==" + mTop);
        if (mTop == null)
        {
            mTop = GameObject.Instantiate(mMaskItem,
                 new Vector2(0, 0), Quaternion.identity);
            mRight = GameObject.Instantiate(mMaskItem,
                new Vector2(0, 0), Quaternion.identity);
            mLeft = GameObject.Instantiate(mMaskItem,
                 new Vector2(0, 0), Quaternion.identity);
            mBotton = GameObject.Instantiate(mMaskItem,
                 new Vector2(0, 0), Quaternion.identity);

            mDec = GameObject.Instantiate(mGuideDec,
                 new Vector2(0, 0), Quaternion.identity);

            mTop.transform.localScale = new Vector2(1, 1);
            mRight.transform.localScale = new Vector2(1, 1);
            mLeft.transform.localScale = new Vector2(1, 1);
            mBotton.transform.localScale = new Vector2(1, 1);

            mRight.transform.parent = mRootTransform.transform;
            mTop.transform.parent = mRootTransform.transform;
            mLeft.transform.parent = mRootTransform.transform;
            mBotton.transform.parent = mRootTransform.transform;
            mDec.transform.parent = mRootTransform.transform;


            mDec.transform.localScale = new Vector2(1, 1);

        }

        mGanmeTarget = go;
        //mGanmeTarget.transform.position.x
        //float x1 = ob.gameObject.GetComponent<RectTransform>().rect.width;
        //float x1 = ob.gameObject.GetComponent<RectTransform>().rect.width;
        showGuide(lefUp,rightBottom);
    }
    private Vector2 mLefUp, mRightBottom;
    public void showGuide(Vector2 lefUp, Vector2 rightBottom) {

      //  float w = (rightBottom.x - lefUp.x)/2 * GameCamera.SCREEN_BILI;
      //  float h = (lefUp.y - rightBottom.y)/2 * GameCamera.SCREEN_BILI;
      //  Vector2 mid = new Vector2((lefUp.x + rightBottom.x) / 2, (lefUp.y + rightBottom.y) / 2);
      //  lefUp = new Vector2(mid.x - w, mid.y + h);
      //  rightBottom = new Vector2(mid.x + w, mid.y - h);

        float top = lefUp.y;
        float bot = rightBottom.y;
        float rig = rightBottom.x;
        float lef = lefUp.x ;
        mLefUp = lefUp;
        mRightBottom = rightBottom; 

//        Debug.Log("top = " + top + " bot=" + bot + " rig=" + rig + "lef = " + lef);
     //   Debug.Log("mid.y = " + mid.y + " mid.x=" + mid.x );
 //       Debug.Log("Screen.height = " + Screen.height + " Screen.width=" + Screen.width);
        //Debug.Log("topHeight = " + topHeight + " bottomHeight=" + bottomHeight + " leftWitch=" + leftWitch + " rightWitch = " + rightWitch);
        setBorder(mTop, new Vector2(Screen.width/2, (Screen.height + top)/2), Screen.width, Screen.height  - top);
        setBorder(mBotton, new Vector2(Screen.width / 2, bot  / 2), Screen.width,  bot);
        setBorder(mLeft, new Vector2(lef/2, (top+bot)/2), lef, top - bot);
        setBorder(mRight, new Vector2((Screen.width+rig) / 2, (top + bot) / 2), Screen.width-rig, top - bot);
        //setBorder(mMiddle, new Vector2(mid.x, mid.y), rig-lef, top - bot);      
    }
    private TextAnchor[] mTextAnchorList = { TextAnchor.UpperLeft, TextAnchor.UpperCenter,TextAnchor.UpperRight,
                                             TextAnchor.MiddleLeft, TextAnchor.MiddleCenter,TextAnchor.MiddleRight,
                                             TextAnchor.LowerLeft, TextAnchor.LowerCenter,TextAnchor.LowerRight};
    public void showGuideDec(int targetX, int targetY, int decX, int decY,string str) {
        float tX = 0;
        float tY = 0;
        if (targetX == 2)
        {
            tX = (mLefUp.x + mRightBottom.x) / 2;
        }
        else if (targetX == 1)
        {
            tX = mLefUp.x;
        }
        else if (targetX == 3) {
            tX = mRightBottom.x;
        }
        if (targetY == 2)
        {
            tY = (mLefUp.y + mRightBottom.y) / 2;
        }
        else if (targetY == 1)
        {
            tY = mLefUp.y;
        }
        else if (targetY == 3)
        {
            tY = mRightBottom.y;
        }
        float vX = 0;
        float vY = 0;
        float w = mDec.GetComponent<RectTransform>().rect.width *mxBili /**GameCamera.SCREEN_BILI*/;
        float h = mDec.GetComponent<RectTransform>().rect.height * myBili * GameCamera.SCREEN_BILI;
        if (decX == 1)
        {
            vX = tX + w / 2;
        }
        else if (decX == 2)
        {
            vX = tX;
        }
        else if(decX == 3){
            vX = tX - w / 2; 
        }
        if (decY == 1)
        {
            vY = tY - h / 2;
        }
        else if (decY == 2)
        {
            vY = tY;
        }
        else if (decY == 3)
        {
            vY = tY + h / 2;
        }
        //mDec.GetComponent<Text>().alignment = mTextAnchorList[(decY-1) * 3 + decX - 1];
        mDec.transform.position = new Vector2(vX,vY);
        mDec.GetComponentsInChildren<Text>()[0].text = str;
    }


    private void setBorder(GameObject go,Vector2 mid, float witch, float height) {
        
        go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, witch );
        go.transform.position = mid;
    }

    public void disableGuide() {
        Destroy(mTop);
        Destroy(mRight);
        Destroy(mLeft);
        Destroy(mBotton);
        Destroy(mDec);
        mBotton = null;
        mLeft = null;
        mRight = null;
        mTop = null;
        mDec = null;

       // mMiddle.transform.localScale = new Vector2(0, 0);
       // mMiddle2.transform.localScale = new Vector2(0, 0);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
