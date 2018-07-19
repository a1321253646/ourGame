
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
    public void addInventory(long id, int count)
    {
        bool isNew = true;
        PlayerBackpackBean bean = null;
        foreach(PlayerBackpackBean tmp in mList ){
            if (tmp.goodId == id) {
                tmp.count += count;
                bean = tmp;
                isNew = false;
            }
        }
        if (isNew)
        {
            GoodJsonBean jb = BackpackManager.getIntance().getGoodInfoById(id);
            PlayerBackpackBean newBean = new PlayerBackpackBean();
            newBean.goodId = id;
            newBean.sortID = jb.sortID;
            newBean.count = count;
            newBean.tabId = jb.tabid;
            mList.Add(newBean);
            //修改数据
        }
        else {
            //添加数据
        }
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
        return null;
    }
}
