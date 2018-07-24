using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AccouterJsonBean
{
    public long id;
    public string name;
    public long type;
    public long stacking;
    public long tabid;
    public long sortID;
    public string icon;
    public string attribute_type;
    public string attribute_value;

    public List<AttributeBean> mAttribute = new List<AttributeBean>();
    public List<AttributeBean> getAttributeList()
    {
        if (mAttribute.Count == 0)
        {
            if (attribute_value == null || attribute_value.Length < 1)
            {
                return null;
            }

            string[] array = attribute_value.Split('}');
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
                AttributeBean bean = new AttributeBean();
                bean.min = long.Parse(array2[0]);
                bean.max = long.Parse(array2[1]);
                mAttribute.Add(bean);
            }
            attribute_value = null;
            if (mAttribute.Count == 0)
            {
                return null;
            }
            string[] type = attribute_type.Split(',');
            for (int i = 0; i < type.Length; i++) {
                mAttribute[i].type = int.Parse(type[i]);
            }
        }
        return mAttribute;
    }

}
