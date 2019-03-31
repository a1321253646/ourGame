using UnityEngine;
using System.Collections;
using System;

public class BaseDateHelper 
{
    private static byte mFloatKey;
    private static int mIntKey;
    private static long mlongKey;
    public static  void encodeKey() {

    }

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
        float a = 0f;
        BitConverter.GetBytes(a);
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

    public static float encodeFloat(float date)
    {
        byte[] bs =BitConverter.GetBytes(date);
        for (int i = 0; i < bs.Length; i++) {
            bs[i] = (byte)(bs[i]^mFloatKey);
        }
        date = BitConverter.ToSingle(bs,0);
        return date;
    }
    public static float decodefloat(float date)
    {
        byte[] bs = BitConverter.GetBytes(date);
        for (int i = 0; i < bs.Length; i++)
        {
            bs[i] = (byte)(bs[i] ^ mFloatKey);
        }
        date = BitConverter.ToSingle(bs, 0);
        return date;
    }

}
