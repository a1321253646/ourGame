using UnityEngine;
using System.Collections;

public class SQLDate
{
    public static long GOOD_TYPE_BACKPACK = 1;
    public static long GOOD_TYPE_ZHUANGBEI = 2;
    public static long GOOD_TYPE_CARD = 3;
    public static long GOOD_TYPE_NOGOOD = 4;

    public static long CLEAR = 1;
    public static long CLEAR_NO = 2;

    public static long DEFAULT_GOOD_ID = -1;

    public long type;
    public long id;
    public string extan;
    public long goodId;
    public long goodType;// 1为不使用，2为装备，3为卡组
    public long isClean;// 1为清除，2为不清除
    

    public string toString() {
        return "type = " + type + " id =" + id + " extan=" + extan;
    }
}
