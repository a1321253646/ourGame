using UnityEngine;
using System.Collections;

public class AttributeBean 
{
    public long type;
    public long max;
    public long min;
    public long getCurrentValue() {
        int rangeRadomNum = Random.Range((int)min, (int)max);
        return rangeRadomNum;
    }
}
