using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class PlayerBackpackBean
{
    public long sortID;
    public long goodId;
    public int count;
    public long tabId;
    public List<PlayerAttributeBean> attributeList;
    public long sqlGoodId;
    public long goodType;
    public long isShowPoint; //1 为显示，2为不显示
    public long reBuildCount;
    public string toString() {
        return "sortID = " + sortID + " goodId=" + goodId + " count" + count + " tabId=" + tabId + " sqlGoodId="+ sqlGoodId+ " goodType="+ goodType;
    }

    private List<PlayerAttributeBean> copyattribute(PlayerBackpackBean bean) {
        
        if (bean.attributeList != null && bean.attributeList.Count > 0) {
            List<PlayerAttributeBean> list = new List<PlayerAttributeBean>();
            foreach(PlayerAttributeBean p in bean.attributeList ) {
                PlayerAttributeBean pp = new PlayerAttributeBean();
                pp.type = p.type;
                pp.value = p.value;
                list.Add(pp);
            }
            return list;
        }
        return null;
    }
}