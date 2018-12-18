using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryHalper 
{
    List<PlayerBackpackBean> mList = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mUser = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mCard = new List<PlayerBackpackBean>();
    private Dictionary<long, long> mDropDeviceUsed = new Dictionary<long, long>();
    private List<long> mHaveBookId = new List<long>();
    private int mZhuangbeiCount = 0;
    private int mCount = -1;



    private Dictionary<long, long> mSamsaraLevel = new Dictionary<long, long>();
    public static InventoryHalper mIntance = new InventoryHalper();
    public static InventoryHalper getIntance() {
        return mIntance;
    }
    private InventoryHalper() {
        //读数据库中的玩家拥有的物品
        mList = SQLHelper.getIntance().getAllGood();
        foreach (PlayerBackpackBean bean in mList) {
            if (bean.goodType == SQLDate.GOOD_TYPE_CARD)
            {
                mCard.Add(bean);
            }
            else if (bean.goodType == SQLDate.GOOD_TYPE_ZHUANGBEI)
            {
                mUser.Add(bean);
            }
        }
        mSamsaraLevel = SQLHelper.getIntance().getLunHui();
        mDropDeviceUsed = SQLHelper.getIntance().getDropDevice();
    }


    public void dealClear() {
        mDropDeviceUsed.Clear();
        mList.Clear();
        mUser.Clear();
        mCard.Clear();
        SQLHelper.getIntance().deleteLuihui();
    }
    public void useCard(PlayerBackpackBean bean)
    {
        bean.goodType = SQLDate.GOOD_TYPE_CARD;
        SQLHelper.getIntance().changeGoodTyppe(bean);
        mCard.Add(bean);
    }
    public void removeUserCard(PlayerBackpackBean bean)
    {
        bean.goodType = SQLDate.GOOD_TYPE_BACKPACK;
        SQLHelper.getIntance().changeGoodTyppe(bean);
        foreach (PlayerBackpackBean b in mList)
        {
            if (b.sqlGoodId == bean.sqlGoodId) {
                b.goodType = bean.goodType;
            }
        }
        mCard.Remove(bean);
    }
    public string getIcon(long id)
    {
        string path = "";
        if (id > TABID_1_START_ID && id < TABID_2_START_ID)
        {
            path = BackpackManager.getIntance().getGoodInfoById(id).icon;
            path = "backpackIcon/" + path;
        }
        else if (id > TABID_2_START_ID && id < TABID_3_START_ID)
        {
            path = BackpackManager.getIntance().getAccouterInfoById(id).icon;
            
            path = "icon/equipicon/" + path;
        }
        else if (id > TABID_3_START_ID)
        {
            path = BackpackManager.getIntance().getCardInfoById(id).icon;
            path = "backpackIcon/" + path;

        }
        
        return path;
    }
    public static long TABID_1_START_ID = 100000;
    public static long TABID_2_START_ID = 200000;
    public static long TABID_3_START_ID = 300000;

    public void updatePoint(PlayerBackpackBean bean) {
        bean.isShowPoint = 2;
        SQLHelper.getIntance().ChangeGoodExtra(bean);
    }

    public bool addInventory(long id, int count)
    {
        bool isNew = true;
        PlayerBackpackBean bean = null;
        if (id <= TABID_2_START_ID )
        {
            foreach (PlayerBackpackBean tmp in mList)
            {
                if (tmp.goodId == id && tmp.attributeList == null)
                {
                    tmp.count += count;
                    bean = tmp;
                    isNew = false;
                }
            }
        }
        if (isNew)
        {
            PlayerBackpackBean newBean = new PlayerBackpackBean();
            if (id > TABID_1_START_ID && id < TABID_2_START_ID)
            {
                GoodJsonBean jb = BackpackManager.getIntance().getGoodInfoById(id);
                newBean.goodId = id;
                newBean.sortID = jb.sortID;
                newBean.count = count;
                newBean.tabId = jb.tabid;
                newBean.isShowPoint = 1;
                mList.Add(newBean);
            }
            else if (id > TABID_2_START_ID && id < TABID_3_START_ID)
            {
                AccouterJsonBean jb = BackpackManager.getIntance().getAccouterInfoById(id);

                newBean.goodId = id;
                newBean.sortID = jb.sortID;
                newBean.count = count;
                newBean.tabId = jb.tabid;
                newBean.isShowPoint = 1;
                newBean.attributeList = new List<PlayerAttributeBean>();
                foreach (EquipKeyAndValue be in jb.getAttributeList())
                {
                    PlayerAttributeBean p1 = new PlayerAttributeBean();
                    p1.type = be.key;
                    p1.value = be.value;
                    newBean.attributeList.Add(p1);
                    Debug.Log("newBean.attributeList.Add : type=" + p1.type + " value=" + p1.value);
                }
                PlayerAttributeBean p2 = new PlayerAttributeBean();
                p2.type = 10001;
                p2.value = 0;
                newBean.attributeList.Add(p2);
                Debug.Log("newBean.attributeList.Add : type=" + p2.type + " value=" + p2.value);

                long count1 = jb.getAffixCount();
                Debug.Log("newBean.getAffixCount " + count1);
                for (long i = 0; i < count1; i++) {
                    long affix = jb.getAffix();
                    AffixJsonBean bean2 = JsonUtils.getIntance().getAffixInfoById(affix);
                    long value2 = bean2.getAffixValue(jb.quality);
                    PlayerAttributeBean p3 = new PlayerAttributeBean();
                    p3.type = 10002;
                    p3.value = affix;
                    newBean.attributeList.Add(p3);
                    Debug.Log("newBean.attributeList.Add : type=" + p3.type + " value=" + p3.value);
                    PlayerAttributeBean p4 = new PlayerAttributeBean();
                    p4.type = affix;
                    p4.value = value2;
                    newBean.attributeList.Add(p4);
                    Debug.Log("newBean.attributeList.Add : type=" + p4.type + " value=" + p4.value);                 
                }
                mList.Add(newBean);
            }
            else if (id > TABID_3_START_ID) {
                CardJsonBean cj = BackpackManager.getIntance().getCardInfoById(id);
                newBean.goodId = id;
                newBean.sortID = cj.sortID;
                newBean.count = count;
                newBean.tabId = cj.tabid;           
                newBean.isShowPoint = 1;
                mList.Add(newBean);
            }
            newBean.sqlGoodId = SQLHelper.getIntance().getCurrentGoodId();
            newBean.goodType = SQLDate.GOOD_TYPE_BACKPACK;
            SQLHelper.getIntance().addGood(newBean);
            return true;
        }
        else {
            SQLHelper.getIntance().ChangeGoodExtra(bean);
            return false;
        }
//        Debug.Log("InventoryHalper list size  " + mList.Count);
    }

    public void upDateSamsaraLevel(long id) {
        if (mSamsaraLevel.ContainsKey(id))
        {
            mSamsaraLevel[id] = mSamsaraLevel[id] + 1;
            SQLHelper.getIntance().ChangeLuiHui(id, mSamsaraLevel[id]);
        }
        else {
            mSamsaraLevel.Add(id, 1);
            SQLHelper.getIntance().addLunhui(id);
        }
        
    }

    public Dictionary<long, long> getSamsaraLevelDate()
    {
        return mSamsaraLevel;
    }

    public long getSamsaraLevelById(long id) {
        if (mSamsaraLevel.ContainsKey(id))
        {
            return mSamsaraLevel[id];
        }
        else
        {
            return 0;
        }
    }

    public long getUseCountByDropDeviceId(long id) {
        addDropDeviceUseCount(id);
        return mDropDeviceUsed[id];
    }
    public void clearDropDeviceUseCount(long id) {
        mDropDeviceUsed[id] = 0;
        Debug.Log("addDropDeviceUseCount id = " + id + " value = " + mDropDeviceUsed[id]);
        SQLHelper.getIntance().updateDropValue(id, 0);
    }
    private void addDropDeviceUseCount(long id) {
        if (mDropDeviceUsed.ContainsKey(id))
        {
             mDropDeviceUsed[id] += 1;
        }
        else {
            mDropDeviceUsed.Add(id,  1);
        }
        Debug.Log("addDropDeviceUseCount id = " + id + " value = " + mDropDeviceUsed[id]);
        SQLHelper.getIntance().updateDropValue(id, mDropDeviceUsed[id]);

        
    }

    public List<long> getHaveBookId() {
        return mHaveBookId;
    }
    public void updateZhuangbei(PlayerBackpackBean bean, long level)
    {
        level = level + 1;
       string extan =  SQLHelper.getGoodExtra(bean);
        AccouterJsonBean ac = JsonUtils.getIntance().getAccouterInfoById(bean.goodId);
        foreach (PlayerAttributeBean p in bean.attributeList){
            if (p.type >= 100 && p.type <= 999 &&
                ac.getStrengThen().ContainsKey(p.type))
            {
                List<EquipKeyAndValue> ekv = ac.getStrengThen()[p.type];
                for (int i = 0; i < ac.getAttributeList().Count; i++)
                {
                    if (ac.getAttributeList()[i].key == p.type)
                    {
                        p.value = ac.getAttributeList()[i].value;
                        break;
                    }
                }
                long value = ac.getStrengthenByLevel(p.type,level);
                p.value = value + p.value;
            }
            else if(p.type == 10001)
            {
                p.value = level;
            }
        }
        SQLHelper.getIntance().updateZHUANGBEI(bean);
       
    }
    public bool use(PlayerBackpackBean bean)
    {        
        return addRoleUse(bean);
    }
    private bool isAddSuccess = true;
    public long useBook(PlayerBackpackBean bean,long count) {
        deleteIventory(bean.goodId, (int)count);
        GoodJsonBean good =  JsonUtils.getIntance().getGoodInfoById(bean.goodId);
        long id = good.getBookId();
        isAddSuccess = addBookId(id);
        return id;
    }


    public bool getIsAddSuccess() {
        return isAddSuccess;
    }
    private bool addBookId(long id) {
        bool isHave = false;
        foreach (int haveid in mHaveBookId) {
            if (id == haveid) {
                isHave = true;
                break;
            }
        }
        if (isHave)
        {
            return false;
        }
        else {
            mHaveBookId.Add(id);
            return true;
        }
    }
    public bool isHaveBook(long id) {
        foreach (long bookid in mHaveBookId) {
            if (bookid == id) {
                return true;
            }
        }
        return false;
    }
    public void unUse(PlayerBackpackBean bean) {
        mZhuangbeiCount--;
        bean.goodType = SQLDate.GOOD_TYPE_BACKPACK;
        mUser.Remove(bean);
        SQLHelper.getIntance().changeGoodTyppe(bean);
        foreach (PlayerBackpackBean b in mList)
        {
            if (b.sqlGoodId == bean.sqlGoodId)
            {
                b.goodType = bean.goodType;
            }
        }
    }

    private bool addRoleUse(PlayerBackpackBean bean)
    {
        if (mZhuangbeiCount < 7) {
            mZhuangbeiCount++;
            bean.goodType = SQLDate.GOOD_TYPE_ZHUANGBEI;
            SQLHelper.getIntance().changeGoodTyppe(bean);
            mUser.Add(bean);
            return true;
        }
        return false;
    }


    private void deleteIventory(PlayerBackpackBean bean)
    {
        Debug.Log("mList size = " + mList.Count);
        List<PlayerAttributeBean> list1 =  bean.attributeList;
        long leng1;
        if (list1 == null)
        {
            leng1 = 0;
        }
        else {
            leng1 = list1.Count;
        }
        for (int i = 0; i < mList.Count; i++)
        {
           
            if (mList[i].goodId == bean.goodId)
            {
                bool isSame = true;
                List<PlayerAttributeBean> list2 = mList[i].attributeList;
                long leng2;
                if (list2 == null)
                {
                    leng2 = 0;
                }
                else
                {
                    leng2 = list2.Count;
                }

                if (leng1 != leng2)
                {
                    continue;
                }
                else if (leng1 == 0)
                {
                    isSame = true;
                }
                else {
                   
                    for (int ii = 0; ii < leng1; ii++) {
                        if (list1[ii].type != list2[ii].type || list1[ii].value != list2[ii].value)
                        {
                            isSame = false;
                            break;
                        }
                    }
                }
                if (isSame) {
                    mList.RemoveAt(i);
                }              
            }
        }
            //    mList.Remove(bean);
        SQLHelper.getIntance().deleteGood(bean);
        Debug.Log("mList size = " + mList.Count);
    }
    private void addIventory(PlayerBackpackBean bean)
    {
        mList.Add(bean);
        SQLHelper.getIntance().addGood(bean);
    }
    public void deleteIventory(PlayerBackpackBean bean, int count) {
        if (bean.count == count)
        {
            mList.Remove(bean);
            SQLHelper.getIntance().deleteGood(bean);
        }
        else
        {
            bean.count -= count;
            SQLHelper.getIntance().ChangeGoodExtra(bean);
        }
        for (int i = 0; i < mUser.Count; i++) {
            PlayerBackpackBean bean2 = mUser[i];
            if (bean2 == bean)
            {
                mUser.Remove(bean2);
                break;
            }
        }
    }

    public void deleteIventory(long id, int count) {
        Debug.Log("deleteIventory id=" + id + " count=" + count);
        for(int i =0;i< mList.Count; i++) {
            PlayerBackpackBean tmp = mList[i];
            if (tmp.goodId == id)
            {
                deleteIventory(tmp,count);
                break;
            }
        }
    }
    public List<PlayerBackpackBean> getInventorys() {
        return mList;
    }

    public List<PlayerBackpackBean> getRoleUseList()
    {
        return mUser;
    }
    public List<PlayerBackpackBean> getUsercard()
    {
        return mCard;
    }
}
