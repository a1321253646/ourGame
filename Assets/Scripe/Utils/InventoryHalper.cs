using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryHalper 
{
    List<PlayerBackpackBean> mList = new List<PlayerBackpackBean>();
    private Dictionary<long, PlayerBackpackBean> mRoleUseList = new Dictionary<long, PlayerBackpackBean>();
    public static InventoryHalper mIntance = new InventoryHalper();
    public static InventoryHalper getIntance() {
        return mIntance;
    }
    private InventoryHalper() {
        //读数据库中的玩家拥有的物品
    }

   
    private long TABID_1_START_ID = 1000000;
    private long TABID_2_START_ID = 2000000;
    private long TABID_3_START_ID = 3000000;
    public bool addInventory(long id, int count)
    {
        bool isNew = true;
        PlayerBackpackBean bean = null;
        foreach(PlayerBackpackBean tmp in mList ){
            if (tmp.goodId == id && tmp.attributeList == null) {
                tmp.count += count;
                bean = tmp;
                isNew = false;
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
                Debug.Log("InventoryHalper list size  " + mList.Count);
            }
            else if (id > TABID_2_START_ID && id < TABID_3_START_ID) {
                AccouterJsonBean jb = BackpackManager.getIntance().getAccouterInfoById(id);
                
                newBean.goodId = id;             
                newBean.sortID = jb.sortID;
                newBean.count = count;
                newBean.tabId = jb.tabid;
                Debug.Log("newBean.goodId  " + newBean.goodId);
                Debug.Log("newBean.sortID  " + newBean.sortID);
                Debug.Log("newBean.count  " + newBean.count);
                Debug.Log("newBean.tabId  " + newBean.tabId);
                newBean.attributeList = new List<PlayerAttributeBean>();
                foreach (AttributeBean be in jb.getAttributeList()) {
                    PlayerAttributeBean p = new PlayerAttributeBean();
                    p.type = be.type;
                    p.value = be.getCurrentValue();
                    Debug.Log("newBean.tabId  be.min " +be.min);
                    Debug.Log("newBean.tabId be.max " + be.max);
                    Debug.Log("newBean.tabId  p.type " + p.type);
                    Debug.Log("newBean.tabId p.value " + p.value);
                    newBean.attributeList.Add(p);
                }
                mList.Add(newBean);
                Debug.Log("InventoryHalper list size  " + mList.Count);
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

    public Dictionary<long, PlayerBackpackBean> getRoleUseList() {
        return mRoleUseList;
    }

    public void use(PlayerBackpackBean bean, long count)
    {
        PlayerBackpackBean beanOld = null;
        AccouterJsonBean acc = BackpackManager.getIntance().getAccouterInfoById(bean.goodId);

        if (mRoleUseList.ContainsKey(acc.type))
        {
            beanOld = mRoleUseList[acc.id];
            mRoleUseList.Remove(acc.id);
            mRoleUseList.Add(acc.id, bean);
        }
        else
        {
            mRoleUseList.Add(acc.id, bean);
        }
        deleteIventory(bean);
        if (beanOld != null) {
            addIventory(bean);
        }
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
        mList.Remove(bean);
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
