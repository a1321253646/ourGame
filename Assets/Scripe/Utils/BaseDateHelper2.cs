using UnityEngine;
using System.Collections;
using System;

public class BaseDateHelper2 
{
    private static byte mSqltKey;
    private static byte mRomKey;
    private static byte mNetKey;

    //Rom的加密解码
    public static int encodeRomInt(int date) {
        return encodeInt(date, mRomKey);
    }
    public static int decodeRomInt(int date)
    {
        return encodeInt(date, mRomKey);
    }
    public static long encodeRomLong(long date)
    {
        return encodeLong(date, mRomKey);
    }
    public static long decodeRomLong(int date)
    {
        return encodeLong(date, mRomKey);
    }
    public static float encodeRomfloat(float date)
    {
        return encodeFloat(date, mRomKey);
    }
    public static float decodeRomfloat(float date)
    {
        return decodefloat(date, mRomKey);
    }
    public static BigNumber encodeRomBig(BigNumber date, byte key)
    {
        return encodeBig(date, mRomKey);
    }
    private static BigNumber decodeRomBig(BigNumber date, byte key)
    {
        return decodeBig(date, mRomKey);
    }

    //sql的加密解密
    public static int encodeSqlInt(int date)
    {
        return encodeInt(date, mSqltKey);
    }
    public static int decodeSqlInt(int date)
    {
        return encodeInt(date, mSqltKey);
    }
    public static long encodeSqlLong(long date)
    {
        return encodeLong(date, mSqltKey);
    }
    public static long decodeSqlLong(int date)
    {
        return encodeLong(date, mSqltKey);
    }
    public static float encodeSqlfloat(float date)
    {
        return encodeFloat(date, mSqltKey);
    }
    public static float decodeSqlfloat(float date)
    {
        return decodefloat(date, mSqltKey);
    }
    public static BigNumber encodeSqlBig(BigNumber date, byte key)
    {
        return encodeBig(date, mSqltKey);
    }
    private static BigNumber decodeSqlBig(BigNumber date, byte key)
    {
        return decodeBig(date, mSqltKey);
    }

    //Net加密解密
    public static int encodeNetInt(int date)
    {
        return encodeInt(date, mNetKey);
    }
    public static int decodeNetInt(int date)
    {
        return encodeInt(date, mNetKey);
    }
    public static long encodeNetLong(long date)
    {
        return encodeLong(date, mNetKey);
    }
    public static long decodeNetLong(int date)
    {
        return encodeLong(date, mNetKey);
    }
    public static float encodeNetfloat(float date)
    {
        return encodeFloat(date, mNetKey);
    }
    public static float decodeNetfloat(float date)
    {
        return decodefloat(date, mNetKey);
    }
    public static BigNumber encodeNetBig(BigNumber date, byte key)
    {
        return encodeBig(date, mNetKey);
    }
    private static BigNumber decodeNetBig(BigNumber date, byte key)
    {
        return decodeBig(date, mNetKey);
    }



    //加密算法
    private static int encodeInt(int date,byte key) {
        byte[] bs = BitConverter.GetBytes(date);
        codeBytes(bs, key);
        date = BitConverter.ToInt32(bs, 0);
        return date;
    }
    private static int decodeInt(int date, byte key)
    {
        byte[] bs = BitConverter.GetBytes(date);
        codeBytes(bs, key);
        date = BitConverter.ToInt32(bs, 0);
        return date;
    }

    private static long encodeLong(long date, byte key)
    {
        byte[] bs = BitConverter.GetBytes(date);
        codeBytes(bs, key);
        date = BitConverter.ToInt64(bs, 0);
        return date;
    }
    private static long decodeLong(long date, byte key)
    {
        byte[] bs = BitConverter.GetBytes(date);
        codeBytes(bs, key);
        date = BitConverter.ToInt64(bs,0);
        return date;
    }
    private static BigNumber encodeBig(BigNumber date, byte key)
    {
        return date;
    }
    private static BigNumber decodeBig(BigNumber date, byte key)
    {
        return date;
    }

    private static float encodeFloat(float date, byte key)
    {
        byte[] bs =BitConverter.GetBytes(date);
        codeBytes(bs, key);
        date = BitConverter.ToSingle(bs,0);      
        return date;
    }
    private static float decodefloat(float date, byte key)
    {
        byte[] bs = BitConverter.GetBytes(date);
        codeBytes(bs, key);
        date = BitConverter.ToSingle(bs, 0);
        return date;
    }


    private static void codeBytes(byte[] date,byte key) {
        for (int i = 0; i < date.Length; i++)
        {
            date[i] = (byte)(date[i] ^ key);
        }
        
    }
}
