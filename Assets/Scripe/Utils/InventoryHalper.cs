using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryHalper 
{
    List<PlayerBackpackBean> mList = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mUser = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mCard = new List<PlayerBackpackBean>();
    List<PlayerBackpackBean> mPet = new List<PlayerBackpackBean>();
    private Dictionary<long, long> mDropDeviceUsed = new Dictionary<long, long>();
    private List<long> mHaveBookId = new List<long>();
    private int mCount = -1;



    private Dictionary<long, long> mSamsaraLevel = new Dictionary<long, long>();
    public static InventoryHalper mIntance = new InventoryHalper();
    public static InventoryHalper getIntance() {
        return mIntance;
    }
    private InventoryHalper() {
        //读数据库中的玩家拥有的物品
        init();
    }
    public void init() {
        mList = SQLHelper.getIntance().getAllGood();
        mCard = SQLHelper.getIntance().getCard();
        mUser = SQLHelper.getIntance().getUserd();
        mPet = SQLHelper.getIntance().getPet();
        mSamsaraLevel = SQLHelper.getIntance().getLunHui();
        mDropDeviceUsed = SQLHelper.getIntance().getDropDevice();
    }

    public void dealClear() {
      //  mDropDeviceUsed.Clear();
      //  mList.Clear();
      //  mUser.Clear();
      //  mCard.Clear();
        SQLHelper.getIntance().deleteLuihui();
    }
    public void useCard(long sqlGoodId)
    {   
        foreach (PlayerBackpackBean b in mList)
        {
            if (b.sqlGoodId == sqlGoodId)
            {
                b.goodType = SQLDate.GOOD_TYPE_USER_CARD;
                mCard.Add(b);
                SQLHelper.getIntance().changeGoodTyppe(b);
            }
        }
    }
    public void removeUserCard(long sqlGoodId)
    {
        foreach (PlayerBackpackBean b in mList)
        {
            if (b.sqlGoodId == sqlGoodId)
            {
                b.goodType = SQLDate.GOOD_TYPE_CARD;
                mCard.Remove(b);
                SQLHelper.getIntance().changeGoodTyppe(b);
            }
        }
    }
    public string getIcon(long id)
    {
        string path = "";
        if (id > TABID_1_START_ID && id < TABID_2_START_ID)
        {
            path = BackpackManager.getIntance().getGoodInfoById(id).icon;
            path = "backpackIcon/" + path;
        }
        else if ((id > TABID_2_START_ID && id < TABID_3_START_ID)||
            id > TABID_5_START_ID)
        {
            path = JsonUtils.getIntance().getAccouterInfoById(id).icon;

            path = "icon/equipicon/" + path;
        }
        else if (id > TABID_3_START_ID && id < TABID_4_START_ID)
        {
            path = BackpackManager.getIntance().getCardInfoById(id).icon;
            path = "backpackIcon/" + path;
        }
        
        return path;
    }
    public static long TABID_1_START_ID = 100000;
    public static long TABID_2_START_ID = 200000;
    public static long TABID_3_START_ID = 3000000;
    public static long TABID_4_START_ID = 4000000;
    public static long TABID_5_START_ID = 20000000;
                                         

    public void updatePoint(PlayerBackpackBean bean) {
        bean.isShowPoint = 2;
        SQLHelper.getIntance().ChangeGoodExtra(bean);
    }

    public void rebuild(PlayerBackpackBean bean) {
        long id = bean.goodId;
        long sql = bean.sqlGoodId;
        long level = 0;
        long rebuild = bean.reBuildCount;
        Debug.Log("rebuild  rebuild=" + rebuild);
        foreach (PlayerAttributeBean b in bean.attributeList)
        {
            if (b.type == 10001)
            {
                level = (long)b.value;
                break;
            }
        }
        deleteIventory(bean, (int)1);
        addInventory(id, 1, sql);
        for (int goodListIndex = 0; goodListIndex < mList.Count; goodListIndex++)
        {
            if (sql == mList[goodListIndex].sqlGoodId)
            {
                bean = mList[goodListIndex];
                break;
            }
        }
        bean.isShowPoint = 2;
        bean.reBuildCount = ++rebuild;
        Debug.Log("rebuild  bean.reBuildCount=" + bean.reBuildCount);
        updateZhuangbei(bean, level, true,false);
    }
    public bool addInventory(long id, int count) {
        return addInventory(id, count, false);
    }
    public bool addInventory(long id, int count, bool isYongjiu) {
        return addInventory(id, count, -1, isYongjiu);
    }
    public bool addInventory(long id, int count, long sql) {
        return addInventory(id, count, sql, false);
    }
    public bool addInventory(long id, int count,long sql, bool isYongjiu)
    {
        bool isNew = true;
        bool isCard = false;
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
            }
            else if ((id > TABID_2_START_ID && id < TABID_3_START_ID)||
                id > TABID_5_START_ID)
            {
                AccouterJsonBean jb = JsonUtils.getIntance().getAccouterInfoById(id);
                Debug.Log("addInventory == " + id);
                newBean.goodId = id;
                newBean.sortID = jb.sortID;
                newBean.count = count;
                newBean.tabId = jb.tabid;
                newBean.isShowPoint = 1;
                newBean.attributeList = new List<PlayerAttributeBean>();
                foreach (EquipKeyAndDouble be in jb.getAttributeList())
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
                newBean.goodType = SQLDate.GOOD_TYPE_BACKPACK;
            }
            else if (id > TABID_3_START_ID && id < TABID_4_START_ID) {
                CardJsonBean cj = BackpackManager.getIntance().getCardInfoById(id);
                newBean.goodId = id;
                newBean.sortID = cj.sortID;
                newBean.count = count;
                newBean.tabId = cj.tabid;           
                newBean.isShowPoint = 1;
                newBean.goodType = SQLDate.GOOD_TYPE_CARD;
                isCard = true;
            }
            else if (id > TABID_4_START_ID && id < TABID_5_START_ID)
            {
                newBean.goodId = id;
                newBean.sortID = -1;
                newBean.count = count;
                newBean.tabId = -1;
                newBean.isShowPoint = 1;
                if (GameObject.Find("Manager").GetComponent<PetManager>().isFull())
                {
                    newBean.goodType = SQLDate.GOOD_TYPE_PET;
                }
                else
                {
                    newBean.goodType = SQLDate.GOOD_TYPE_USER_PET;
                }             
                mPet.Add(newBean);
                
            }
            if (mList.Count == 0)
            {
                mList.Add(newBean);
            }
            else
            {
                int goodListIndex = 0;
                for (goodListIndex = 0; goodListIndex < mList.Count; goodListIndex++)
                {
                    if (newBean.sortID < mList[goodListIndex].sortID)
                    {
                        mList.Insert(goodListIndex, newBean);
                        break;
                    }
                }
                if (goodListIndex == mList.Count)
                {
                    mList.Add(newBean);
                }
            }
            Debug.Log("=======================goodId" + newBean.goodId+"================================");
            if (sql == -1) {
                newBean.sqlGoodId = SQLHelper.getIntance().getCurrentGoodId();
            }
            else {
                newBean.sqlGoodId = sql;
            }
            if (isYongjiu) {
                newBean.isClean = 2;
            }        
            SQLHelper.getIntance().addGood(newBean);
            if (isCard) {
                
                GameObject.Find("card_up_list").GetComponent<CardUpdateListControl>().upDateUi(newBean.goodId);
                GameObject.Find("Card2").GetComponent<CardShowControl>().upDateUi();
            }
            
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
            mSamsaraLevel[id] =  BaseDateHelper.encodeLong( BaseDateHelper.decodeLong(mSamsaraLevel[id])  + 1);
            SQLHelper.getIntance().ChangeLuiHui(id, mSamsaraLevel[id]);
        }
        else {
            mSamsaraLevel.Add(id, BaseDateHelper.encodeLong(1));
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
            return BaseDateHelper.encodeLong(0);
        }
    }

    public long getUseCountByDropDeviceId(long id) {
        addDropDeviceUseCount(id);
        return mDropDeviceUsed[id];
    }
    public void clearDropDeviceUseCount(long id) {
        mDropDeviceUsed[id] = 0;
        Debug.Log("addDropDeviceUseCount id = " + id + " value = " + mDropDeviceUsed[id]);
        SQLHelper.getIntance().updateDrop(id, mDropDeviceUsed[id]);
    }
    private void addDropDeviceUseCount(long id) {
        if (mDropDeviceUsed.ContainsKey(id))
        {
             mDropDeviceUsed[id] += 1;
            SQLHelper.getIntance().updateDrop(id, mDropDeviceUsed[id]);
        }
        else {
            mDropDeviceUsed.Add(id,  1);
            SQLHelper.getIntance().addDrop(id, mDropDeviceUsed[id]);
        }
     //   Debug.Log("addDropDeviceUseCount id = " + id + " value = " + mDropDeviceUsed[id]);

        
    }

    public List<long> getHaveBookId() {
        return mHaveBookId;
    }

    public void updateZhuangbei(PlayerBackpackBean bean, long level, bool isSave) {
        updateZhuangbei(bean, level, isSave,true);
    }

    public void updateZhuangbei(PlayerBackpackBean bean, long level,bool isSave, bool isAddLevel)
    {
        if (isAddLevel) {
            level = level + 1;
        }
        
       string extan =  SQLHelper.getGoodExtra(bean);
        AccouterJsonBean ac = JsonUtils.getIntance().getAccouterInfoById(bean.goodId);
        foreach (PlayerAttributeBean p in bean.attributeList){
            if (p.type >= 100 && p.type <= 999 &&
                ac.getStrengThen().ContainsKey(p.type))
            {
                List<EquipKeyAndDouble> ekv = ac.getStrengThen()[p.type];
                for (int i = 0; i < ac.getAttributeList().Count; i++)
                {
                    if (ac.getAttributeList()[i].key == p.type)
                    {
                        p.value = ac.getAttributeList()[i].value;
                        break;
                    }
                }
                double value = ac.getStrengthenByLevel(p.type,level);
                p.value = value + p.value;
            }
            else if(p.type == 10001)
            {
                p.value = level;
            }
        }
        if (isSave) {
            SQLHelper.getIntance().updateZHUANGBEI(bean);
        }       
    }
    public bool use(PlayerBackpackBean bean)
    {        
        return addRoleUse(bean.sqlGoodId);
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
    public void unUse(long sqlId) {        
        
        foreach (PlayerBackpackBean b in mList)
        {
            if (b.sqlGoodId == sqlId)
            {
                b.goodType = SQLDate.GOOD_TYPE_BACKPACK;
                mUser.Remove(b);
                SQLHelper.getIntance().changeGoodTyppe(b);
            }
        }
    }

    private bool addRoleUse(long sqlId)
    {
        Debug.Log("========================addRoleUse = " + mUser.Count);
        if (mUser.Count < 6) {
            SkillIndexUtil.getIntance().getEquitIndexByGoodId(false, sqlId);
            foreach (PlayerBackpackBean b in mList)
            {
                if (b.sqlGoodId == sqlId)
                {
                    b.goodType = SQLDate.GOOD_TYPE_ZHUANGBEI;
                    mUser.Add(b);
                    SQLHelper.getIntance().changeGoodTyppe(b);
                }
            }
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
    public void deleteIventory(PlayerBackpackBean bean, int count) {
        PlayerBackpackBean target = null;
        foreach (PlayerBackpackBean tmp in mList) {
   //         Debug.Log("deleteIventory (bean.sqlGoodId= " + bean.sqlGoodId+ "  tmp.sqlGoodId=" + tmp.sqlGoodId)/;
            if (bean.sqlGoodId == tmp.sqlGoodId) {
                Debug.Log("tmp.goodId= " + tmp.goodId );
                target = tmp;
                break;
            }
        }
        if (target != null)
        {
            bean = target;
        }
        else {
            return;
        }
        Debug.Log("bean.coun= " + bean.count+ " count=" + count);
        if (bean.count <= count)
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
            if (bean2.sqlGoodId == bean.sqlGoodId)
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

    public void changePetStatus(PlayerBackpackBean bean) {
        SQLHelper.getIntance().changeGoodTyppe(bean);
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
    public List<PlayerBackpackBean> getPet()
    {
        return mPet;
    }
}
