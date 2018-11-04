using UnityEngine;
using System.Collections;

public class AttributeBean 
{
    public long type;
    public long max;
    public long min;
    public long value = 0;
    public long getCurrentValue() {
        if (value != 0) {
            return value;
        }
        int rangeRadomNum = Random.Range((int)min, (int)max);
        return rangeRadomNum;
    }
}
