using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryHalper 
{
    List<PlayerBackpackBean> mList = new List<PlayerBackpackBean>();
    private Dictionary<long, PlayerBackpackBean> mRoleUseList = new Dictionary<long, PlayerBackpackBean>();
    private Dictionary<long, long> mDropDeviceUsed = new Dictionary<long, long>();
    private List<long> mHaveBookId = new List<long>();

    private Dictionary<long, long> mSamsaraLevel = new Dictionary<long, long>();
    public static InventoryHalper mIntance = new InventoryHalper();

    private List<long> mUserCardId = new List<long>();
    public static InventoryHalper getIntance() {
        return mIntance;
    }
    private InventoryHalper() {
        //读数据库中的玩家拥有的物品
        mUserCardId = SQLHelper.getIntance().getUserCard();
        mList = SQLHelper.getIntance().getAllGood();
        mSamsaraLevel = SQLHelper.getIntance().getLunHui();
        List<PlayerBackpackBean> list = SQLHelper.getIntance().getPlayUserZhuangbei();
        if (list != null && list.Count > 0) {
            foreach (PlayerBackpackBean b in list) {
                AccouterJsonBean acc = BackpackManager.getIntance().getAccouterInfoById(b.goodId);
                mRoleUseList.Add(acc.type, b);
            }
        }
    }


    public void dealClear() {
        mRoleUseList.Clear();
        SQLHelper.getIntance().deleteAllZuangbei();
        mDropDeviceUsed.Clear();
        mList.Clear();
      /*  for (int i = 0; i < mList.Count;) {
            PlayerBackpackBean bean = mList[i];
            if (bean.goodId >= 3000001)
            {
                i++;
                continue;
            }
            else {
                mList.Remove(bean);
            }
        }*/
        SQLHelper.getIntance().deleteAllGood();
        mUserCardId.Clear();
        SQLHelper.getIntance().deleteAllUserCard();

    }
    public void useCard(PlayerBackpackBean bean, long count)
    {
        for (int i = 0; i < count; i++)
        {
            mUserCardId.Add(bean.goodId);
            SQLHelper.getIntance().addUserCard(bean.goodId);
            deleteIventory(bean.goodId, 1);
        }
    }
    public void removeUserCard(long id)
    {
        for (int i = 0 ; i < mUserCardId.Count;i++) {
            long item = mUserCardId[i];
            if (item == id) {
                mUserCardId.Remove(item);
                SQLHelper.getIntance().deleteUserCard(id);
                addInventory(item, 1);
                return;
            }
        }
    }

    public List<long> getUsercard() {
        return mUserCardId;
    }

    public static long TABID_1_START_ID = 1000000;
    public static long TABID_2_START_ID = 2000000;
    public static long TABID_3_START_ID = 3000000;
    public bool addInventory(long id, int count)
    {
        bool isNew = true;
        PlayerBackpackBean bean = null;
        if (id <= TABID_2_START_ID || id >= TABID_3_START_ID)
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
                mList.Add(newBean);
            }
            else if (id > TABID_2_START_ID && id < TABID_3_START_ID)
            {
                AccouterJsonBean jb = BackpackManager.getIntance().getAccouterInfoById(id);

                newBean.goodId = id;
                newBean.sortID = jb.sortID;
                newBean.count = count;
                newBean.tabId = jb.tabid;
                newBean.attributeList = new List<PlayerAttributeBean>();重写
                foreach (AttributeBean be in jb.getAttributeList())
                {
                    PlayerAttributeBean p = new PlayerAttributeBean();
                    p.type = be.type;
                    p.value = be.getCurrentValue();
                    newBean.attributeList.Add(p);
                }
                mList.Add(newBean);
            }
            else if (id > TABID_3_START_ID) {
                CardJsonBean cj = BackpackManager.getIntance().getCardInfoById(id);
                newBean.goodId = id;
                newBean.sortID = cj.sortID;
                newBean.count = count;
                newBean.tabId = cj.tabid;
                mList.Add(newBean);
            }
            SQLHelper.getIntance().addGood(newBean);
            return true;
        }
        else {
            SQLHelper.getIntance().ChangeGood(bean);
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
    }
    private void addDropDeviceUseCount(long id) {
        if (mDropDeviceUsed.ContainsKey(id))
        {
             mDropDeviceUsed[id] += 1;
        }
        else {
            mDropDeviceUsed.Add(id,  1);
        }
        
    }


    public Dictionary<long, PlayerBackpackBean> getRoleUseList() {
        return mRoleUseList;
    }
    public List<long> getHaveBookId() {
        return mHaveBookId;
    }
    public void use(PlayerBackpackBean bean, long count)
    {
        PlayerBackpackBean beanOld = null;
        AccouterJsonBean acc = BackpackManager.getIntance().getAccouterInfoById(bean.goodId);

        if (mRoleUseList.ContainsKey(acc.type))
        {
            beanOld = mRoleUseList[acc.type];
            deleteRoleUse(acc.type,bean);
            
            addRoleUse(acc.type, bean);
        }
        else
        {
            addRoleUse(acc.type, bean);            
        }
        deleteIventory(bean);
        if (beanOld != null) {
            addIventory(bean);
        }
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

    public void unUse(PlayerBackpackBean bean, long count) {
        AccouterJsonBean acc = BackpackManager.getIntance().getAccouterInfoById(bean.goodId);
        mRoleUseList.Remove(acc.type);
        SQLHelper.getIntance().deleteZuangbei(bean.goodId);
        addIventory(bean);
    }

    private void deleteRoleUse(long type, PlayerBackpackBean bean)
    {
        mRoleUseList.Remove(type);
        SQLHelper.getIntance().deleteZuangbei(bean.goodId);
    }
    private void addRoleUse(long type,PlayerBackpackBean bean)
    {
        mRoleUseList.Add(type,bean);
        SQLHelper.getIntance().addZHUANGBEI(bean);
    }


    private void deleteIventory(PlayerBackpackBean bean)
    {
        Debug.Log("mList size = " + mList.Count);
        mList.Remove(bean);
        SQLHelper.getIntance().deleteGood(bean.goodId);
        Debug.Log("mList size = " + mList.Count);
    }
    private void addIventory(PlayerBackpackBean bean)
    {
        mList.Add(bean);
        SQLHelper.getIntance().addGood(bean);
    }
    public void deleteIventory(long id, int count) {
        PlayerBackpackBean bean = null;
        foreach (PlayerBackpackBean tmp in mList)
        {
            if (tmp.goodId == id)
            {
                tmp.count -= count;
                bean = tmp;
            }
        }
        if (bean == null) {
            return;
        }
        if (bean.count == 0)
        {
            mList.Remove(bean);
            SQLHelper.getIntance().deleteGood(bean.goodId);
        }
        else {
            SQLHelper.getIntance().ChangeGood(bean);
        }
    }
    public List<PlayerBackpackBean> getInventorys() {
        return mList;
    }
}
