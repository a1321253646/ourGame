using UnityEngine;
using System.Collections;

public class BaseDateHelper 
{
    public static int encodeInt(int date) {
        return date;
    }
    public static int decodeInt(int date)
    {
        return date;
    }

    public static long encodeLong(long date)
    {
        long tmp = ~date;
        return tmp;
    }
    public static long decodeLong(long date)
    {
        long tmp = ~date;
       // tmp = -tmp;

        return tmp;
    }

    public static BigNumber encodeBig(BigNumber date)
    {
        return date;
    }
    public static BigNumber decodeBig(BigNumber date)
    {
        return date;
    }
}
