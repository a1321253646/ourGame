using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropDevice
{
    public long id;
    public long groupId;
    public long upGroupID;
    public long min;
    public long max;

    public List<FellObjectBean> fell() {
        long group;
        long count = InventoryHalper.getIntance().getUseCountByDropDeviceId(id);
        Debug.Log("DropDevice id =" + id + "count  = " + count);
        if (upGroupID != 0)
        {
            if (count > getRange(min, max))
            {
                group = upGroupID;
                InventoryHalper.getIntance().clearDropDeviceUseCount(id);
            }
            else
            {
                group = groupId;
            }
        }
        else {
            group = groupId;
        }
        Debug.Log("DropDevice id ="+ id+"group = " + group);
        DropDeviceDetail detail =   JsonUtils.getIntance().getDropDeviceDetailById(group);
        if (detail != null) {
            return detail.fellObjetList();
        }
        else{
            return null;
        }
    }
    private long getRange(long min, long max)
    {
        int range = Random.Range((int)min, (int)max);
        Debug.Log("DropDevice id =" + id + "getRange  = " + range);
        return range;
    }
}
