using UnityEngine;
using System.Collections.Generic;
public class InventoryHalper 
{
    List<PlayerBackpackBean> mList = new List<PlayerBackpackBean>();
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
                newBean.attributeList = new List<PlayerAttributeBean>();
                foreach (AttributeBean be in jb.getAttributeList()) {
                    PlayerAttributeBean p = new PlayerAttributeBean();
                    p.type = be.type;
                    p.type = be.getCurrentValue();
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
