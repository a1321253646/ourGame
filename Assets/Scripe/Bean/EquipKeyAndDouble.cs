using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipKeyAndDouble
{
    public long key;
    public double value;

    public static List<EquipKeyAndDouble> getListForString(string str)
    {
        if (str == null || str.Length < 1)
        {
            return null;
        }
        List<EquipKeyAndDouble> list = new List<EquipKeyAndDouble>();
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
            EquipKeyAndDouble bean = new EquipKeyAndDouble();
            bean.key = long.Parse(array2[0]);
            bean.value = double.Parse(array2[1]);
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
    public static Dictionary<long, List<EquipKeyAndDouble>> getDicForString(List<EquipKeyAndDouble> type, string str)
    {
        Debug.Log(" getDicForString str =" + str);
        Dictionary<long, List<EquipKeyAndDouble>> dic = new Dictionary<long, List<EquipKeyAndDouble>>();
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
            Debug.Log(" getDicForString key =" + type[i].key+" replace = "+s);
            List<EquipKeyAndDouble> value2 = EquipKeyAndDouble.getListForString(s);
            dic.Add(type[i].key, value2);
        }
        if (dic.Count == 0) {
            return null;
        }
        return dic;
    }
}
