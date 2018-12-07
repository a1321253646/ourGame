using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SamsaraJsonBean
{
    public long id;
    public long level;
    public string type;
    public string value;
    public string coast;
    public long sort;
    public string name;
    public string icon;
    public Dictionary<long, List<SamsaraValueBean>> levelList;
    public Dictionary<long, BigNumber> costList;

    public BigNumber mCost;
    public BigNumber getCoast()
    {
        if (mCost == null)
        {
            mCost = BigNumber.getBigNumForString(coast);
        }
        return mCost;
    }

    public List<SamsaraValueBean>  getKeyAndValueList(List<SamsaraValueBean> back) {
        back.Clear();
        string[] types;
        string[] values;
        if (type != null && type.Length > 0)
        {
            types = type.Split(',');
        }
        else {
            return null;
        }
        if (value != null && value.Length > 0)
        {
            values = value.Split(',');
        }
        else {
            return null;
        }
        if (types.Length != values.Length || values.Length == 0) {
            return null;
        }
        for (int i = 0; i < types.Length; i++) {
            if (types[i] == null || types[i].Length == 0) {
                continue;
            }
            SamsaraValueBean bean = new SamsaraValueBean();
            bean.type = int.Parse(types[i]);
            bean.value = int.Parse(values[i]);
            back.Add(bean);
        }
        if (back.Count == 0) {
            return null;
        }

        return back;
    }
}
