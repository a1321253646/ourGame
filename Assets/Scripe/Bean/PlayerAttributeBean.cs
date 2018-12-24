using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttributeBean
{
    private static Dictionary<long, string> mGoodDic = new Dictionary<long, string>() { { 100,"攻击力"},{ 101,"防御"},{ 102, "生命" }
                                                , { 110, "命中" }, { 111, "闪避" }, { 112, "暴击" },
                                                  { 113, "暴击伤害" },{ 114, "攻击速度" },{ 115, "真实伤害" }};
    /**
        10001 表示等级
        10002 表示随机特殊属性
        XXXXXX 表示随机属性的值
    */
    public long type;
    public double value;

    public string getTypeStr() {
        if (mGoodDic.ContainsKey(type)){
            return mGoodDic[type];
        }
        return null;
    }
}
