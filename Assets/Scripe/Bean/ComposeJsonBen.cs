using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComposeJsonBen
{
    public long id;
    public long tid;
    public string materials;
    public string materials_number;
    public long cost_crystal;
    public long classType;
    public long isShow;
    public long compensate;
    List<ComposeNeedItemBean> mList;
    public List<ComposeNeedItemBean> getNeedList() {
        if (mList == null || mList.Count <1) {
            mList = new List<ComposeNeedItemBean>();
            if (materials == null || materials.Length < 1)
            {
                return null;
            }
            materials = materials.Replace("{", "");
            materials = materials.Replace("}", "");
            materials_number = materials_number.Replace("{", "");
            materials_number = materials_number.Replace("}", "");
            string[] array = materials.Split(',');
            string[] array2 = materials_number.Split(',');
            int count = array.Length > array2.Length ? array2.Length : array.Length;
            for (int i=0; i < count; i++) {
                ComposeNeedItemBean bean = new ComposeNeedItemBean();
                bean.id = long.Parse(array[i]);
                bean.num = long.Parse(array2[i]);
                mList.Add(bean);
            }
        }
        return mList;
    }
}
