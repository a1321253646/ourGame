using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttributeBean
{
    private static Dictionary<long, string> mGoodDic = new Dictionary<long, string>() { { 100,"攻击力"},{ 101,"防御"},{ 102, "生命" } };
    public long type;
    public long value;

    public string getTypeStr() {
        return mGoodDic[type];
    }
}
