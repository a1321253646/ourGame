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
        if (count < min)
        {
            group = groupId;
        }
        else if (count > max)
        {
            group = upGroupID;
            InventoryHalper.getIntance().clearDropDeviceUseCount(id);
        }
        else {
            if (count >= getRange(min, max))
            {
                group = upGroupID;
                InventoryHalper.getIntance().clearDropDeviceUseCount(id);
            }
            else {
                group = groupId;
            }
        }
        DropDeviceDetail detail =   JsonUtils.getIntance().getDropDeviceDetailById(group);
        if (detail != null) {
            return detail.fellObjetList();
        }
        else{
            return null;
        }
    }
    private long getRange(long min, long max) {
        return Random.Range((int)min, (int)min);
    }
}
