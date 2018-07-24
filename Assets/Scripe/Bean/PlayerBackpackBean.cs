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
    public string toString() {
        return "sortID = " + sortID + " goodId=" + goodId + " count" + count + " tabId=" + tabId;
    }
}