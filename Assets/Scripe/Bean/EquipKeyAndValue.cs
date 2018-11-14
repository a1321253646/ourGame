using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipKeyAndValue
{
    public long key;
    public long value;

    public static List<EquipKeyAndValue> getListForString(string str)
    {
        if (str == null || str.Length < 1)
        {
            return null;
        }
        List<EquipKeyAndValue> list = new List<EquipKeyAndValue>();
        string[] array = str.Split('}');
        foreach (string str2 in array)
        {
            if (str2 == null || str2.Length < 1)
            {
                continue;
            }
            string str3 = str2.Replace("{", "");
            if (str3 == null || str3.Length < 1)
            {
                continue;
            }
            string[] array2 = str3.Split(',');
            EquipKeyAndValue bean = new EquipKeyAndValue();
            bean.key = long.Parse(array2[0]);
            bean.value = long.Parse(array2[1]);
            list.Add(bean);
        }
        if (list.Count == 0)
        {
            return null;
        }
        else
        {
            return list;
        }
    }
    public static Dictionary<long, List<EquipKeyAndValue>> getDicForString(List<EquipKeyAndValue> type, string str)
    {
        Dictionary<long, List<EquipKeyAndValue>> dic = new Dictionary<long, List<EquipKeyAndValue>>();
        string[] strs = str.Split(')');
        if (str.Length == 0)
        {
            return null;
        }
       /* if (strs.Length != type.Count)
        {
            return null;
        }*/
        for (int i = 0; i < strs.Length; i++) {
            string s = strs[i];
            if (s == null || s.Length == 0)
            {
                continue;
            }
            s = s.Replace("(", "");
            List<EquipKeyAndValue> value2 = EquipKeyAndValue.getListForString(s);
            dic.Add(type[i].key, value2);
        }
        if (dic.Count == 0) {
            return null;
        }
        return dic;
    }
}
