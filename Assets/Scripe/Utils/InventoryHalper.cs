using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryHalper 
{
    List<PlayerBackpackBean> mList = new List<PlayerBackpackBean>();
    private List< PlayerBackpackBean> mRoleUseList = new List<PlayerBackpackBean>();
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
       //         AccouterJsonBean acc = BackpackManager.getIntance().getAccouterInfoById(b.goodId);
                mRoleUseList.Add(b);
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

    public static long TABID_1_START_ID = 100000;
    public static long TABID_2_START_ID = 200000;
    public static long TABID_3_START_ID = 300000;
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


    public List< PlayerBackpackBean> getRoleUseList() {
        return mRoleUseList;
    }
    public List<long> getHaveBookId() {
        return mHaveBookId;
    }
    public void updateZhuangbei(PlayerBackpackBean bean, long level)
    {
        level = level + 1;
       string extan =  SQLHelper.getIntance().getGoodExtra(bean);
        AccouterJsonBean ac = JsonUtils.getIntance().getAccouterInfoById(bean.goodId);
        foreach (PlayerAttributeBean p in bean.attributeList){
            if (p.type >= 100 && p.type <= 999 && ac.getStrengThen().ContainsKey(p.type))
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
        SQLHelper.getIntance().updateZHUANGBEI(bean, extan);
       
    }
    public bool use(PlayerBackpackBean bean, long count)
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

    public void unUse(PlayerBackpackBean bean, long count) {
        if (mRoleUseList.Remove(bean))
        {
            SQLHelper.getIntance().deleteZuangbei(bean.goodId);
            addIventory(bean);
        }
    }

    private bool deleteRoleUse( PlayerBackpackBean bean)
    {
        if (mRoleUseList.Remove(bean)) {
            SQLHelper.getIntance().deleteZuangbei(bean.goodId);
            
            return true;
        }
        return false;
        
        
    }
    private bool addRoleUse(PlayerBackpackBean bean)
    {
        if (mRoleUseList.Count < 6) {
            mRoleUseList.Add( bean);
            deleteIventory(bean);
            SQLHelper.getIntance().addZHUANGBEI(bean);
        }
        return false;
    }


    private void deleteIventory(PlayerBackpackBean bean)
    {
        Debug.Log("mList size = " + mList.Count);
        mList.Remove(bean);
        SQLHelper.getIntance().deleteGood(bean);
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
            SQLHelper.getIntance().deleteGood(bean);
        }
        else {
            SQLHelper.getIntance().ChangeGood(bean);
        }
    }
    public List<PlayerBackpackBean> getInventorys() {
        return mList;
    }
}
