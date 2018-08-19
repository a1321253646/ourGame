using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryHalper 
{
    List<PlayerBackpackBean> mList = new List<PlayerBackpackBean>();
    private Dictionary<long, PlayerBackpackBean> mRoleUseList = new Dictionary<long, PlayerBackpackBean>();
    private Dictionary<long, long> mDropDeviceUsed = new Dictionary<long, long>();
    private List<long> mHaveBookId = new List<long>();
    public static InventoryHalper mIntance = new InventoryHalper();
    public static InventoryHalper getIntance() {
        return mIntance;
    }
    private InventoryHalper() {
        //读数据库中的玩家拥有的物品
    }

   
    public long TABID_1_START_ID = 1000000;
    public long TABID_2_START_ID = 2000000;
    public long TABID_3_START_ID = 3000000;
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
            else if (id > TABID_2_START_ID && id < TABID_3_START_ID) {
                AccouterJsonBean jb = BackpackManager.getIntance().getAccouterInfoById(id);
                
                newBean.goodId = id;             
                newBean.sortID = jb.sortID;
                newBean.count = count;
                newBean.tabId = jb.tabid;
                newBean.attributeList = new List<PlayerAttributeBean>();
                foreach (AttributeBean be in jb.getAttributeList()) {
                    PlayerAttributeBean p = new PlayerAttributeBean();
                    p.type = be.type;
                    p.value = be.getCurrentValue();
                    newBean.attributeList.Add(p);
                }
                mList.Add(newBean);
            }
            return true;
            //添加数据
        }
        else {
            //修改数据
            return false;
        }
//        Debug.Log("InventoryHalper list size  " + mList.Count);
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
            deleteRoleUse(acc.type);
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
        addIventory(bean);
    }

    private void deleteRoleUse(long type)
    {
        mRoleUseList.Remove(type);
        //删除角色数据
    }
    private void addRoleUse(long type,PlayerBackpackBean bean)
    {
        mRoleUseList.Add(type,bean);
        //添加角色数据
    }


    private void deleteIventory(PlayerBackpackBean bean)
    {
        Debug.Log("mList size = " + mList.Count);
        mList.Remove(bean);
        Debug.Log("mList size = " + mList.Count);
        //删除数据
    }
    private void addIventory(PlayerBackpackBean bean)
    {
        mList.Add(bean);
        //添加数据
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
            //删除数据
        }
        else {
            //修改数据
        }
    }
    public List<PlayerBackpackBean> getInventorys() {
        return mList;
    }
}
