using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropDeviceDetail
{
    public long id;
    private List<Detail> mItemList;
    private long weight = 10000;
    public long dropCount = 1;
    public void addItem(DropDeviceDetailJson item) {
        if (mItemList == null) {
            mItemList = new List<Detail>();
        }
        Detail detail = new Detail();
        detail.itemId = item.itemID;
        detail.minCount = item.minCount;
        detail.maxcount = item.maxCount;
        detail.weight = item.weight;
        mItemList.Add(detail);
        if (mItemList.Count == 2)
        {
            weight = mItemList[0].weight + mItemList[1].weight;
        }
        else if (mItemList.Count > 2) {
            weight += detail.weight;
        }
    }

    public List<FellObjectBean> fellObjetList() {
        Debug.Log("DropDeviceDetail dropCount="+ dropCount+ "weight =" + weight);
        List<FellObjectBean> list = new List<FellObjectBean>();
        for (int i = 0;i < dropCount; i++) {
            int obRange = getObRange();
            Debug.Log("DropDeviceDetail obRange=" + obRange );
            Detail fell = null;
            foreach (Detail dt in mItemList) {
                Debug.Log("DropDeviceDetail dt.weight=" + dt.weight);
                if (dt.weight > obRange)
                {
                    fell = dt;
                }
                else {
                    obRange -=(int) dt.weight;
                }
            }
            
            if (fell == null) {
                Debug.Log("DropDeviceDetail fell null");
                continue;
            }
            
            long count = getCount(fell.minCount, fell.minCount);
            Debug.Log("DropDeviceDetail fell id" + fell.itemId+" count = "+count);
            FellObjectBean ob = new FellObjectBean();
            ob.id = fell.itemId;
            ob.count = count;
            list.Add(ob);
        }
        if (list.Count == 0)
        {
            return null;
        }
        else {
            return list;
        }
       
    }

    private long getCount(long min, long max) {
        if (min == max) {
            return min;
        }
        return Random.Range((int)min, (int)min);
    }

    private int getObRange() {
        return Random.Range(0, (int)weight);

    }

    private class Detail {
        public long itemId;
        public long minCount;
        public long maxcount;
        public long weight;
    }
}
