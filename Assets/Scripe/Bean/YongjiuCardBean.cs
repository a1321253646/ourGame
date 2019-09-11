using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class YongjiuCardBean 
{
    public long id;
    public string name;
    public long stacking;
    public long tabid;
    public long sortID;
    public string icon;
    public string top_resource;
    public string center_resource;
    public long skill_id;
    public long cost;
    public string jihuo_card_group;
    public long card_up_cost;

    private List<long> list = null;
    public List<long> getJihuoCardList() {
        if (list == null) {
            list = new List<long>();
            string[] ids = jihuo_card_group.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                long idInt = long.Parse(ids[i]);
                list.Add(idInt);
            }
        }
        return list;
    }

}
