using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class PlayerBackpackBean : ScriptableObject
{
    public long sortID;
    public long goodId;
    public int count;
    public long tabId;
    public List<PlayerAttributeBean> attributeList;
    public long isShowPoint; //1 为显示，2为不显示
    public string toString() {
        return "sortID = " + sortID + " goodId=" + goodId + " count" + count + " tabId=" + tabId;
    }
}