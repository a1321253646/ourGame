using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BigNumber 
{
    public List<BigNumberUnit> mList = new List<BigNumberUnit>();

    public static BigNumber getBigNumForString(string s) {
        if (s == null || s.Length == 0) {
            return new BigNumber();
        }
        if (s.Contains("-")) {
            BigNumber big3 = new BigNumber();
            BigNumberUnit unit3 = new BigNumberUnit();
            unit3.value = 0;
            unit3.setUnit(0);
            big3.mList.Add(unit3);
            return new BigNumber();
        }
        if (s.Contains("E+")) {
            s = s.Replace("E+", "E");
            string[] str1 = s.Split('E');
            int count = int.Parse(str1[1]);
            string[] str2 = str1[0].Split('.');
         //   Debug.Log("===================================getBigNumForString str1 1= " + str1[0]+ " str1 2="+ str1[1]+ " str2 1="+ str2[0]+ " str2 2"+ str2[1]);
            string str3 = "";
            if (str2.Length == 1)
            {
                for (int i = 0; i < count; i++)
                {
                    str3 += "0";
                }
                s = str2[0] + str3;
            }
            else if (str2.Length == 2) {
                count = count - str2[1].Length;
                for (int i = 0; i < count; i++)
                {
                    str3 += "0";
                }
                s = str2[0] + str2[1] + str3;
            }
        }
     //   Debug.Log("===================================getBigNumForString s= "+s);
        BigNumber big = new BigNumber();
        int index = 0;
        while (s.Length >= 4) {
            BigNumberUnit unit = new BigNumberUnit();
            s = spileNumberString(s, unit);
            unit.setUnit(index);
            index++;
            big.mList.Add(unit);
        }
        BigNumberUnit unit1 = new BigNumberUnit();
        unit1.value = int.Parse(s);
        unit1.setUnit(index);
        big.mList.Add(unit1);
        return big;
    }

    public bool isEmpty() {
        if (mList.Count == 0)
        {
            return true;
        }
        else {
            return false;
        }

    }

    private static string spileNumberString(string str, BigNumberUnit unit) {
        string s1 = str.Substring(str.Length - 3, 3);
//        Debug.Log("spileNumberString s="+s1);
        unit.value = int.Parse(s1);
        str = str.Substring(0, str.Length - 3);
        return str;
    }
    public string toString() {
        string str = "";
        for (int i = mList.Count-1; i >= 0; i--) {
            if (i == mList.Count - 1) {
                str += mList[i].value;
                continue;
            }
            if (mList[i].value > 99)
            {
                str += mList[i].value;
            }
            else if (mList[i].value > 9)
            {
                str = str + "0" + mList[i].value;
            }
            else if (mList[i].value > 0)
            {
                str = str + "00" + mList[i].value;
            }
            else {
                str = str + "000";
            }
        }
        return str;
    }
    public string toStringWithUnit() {
        if (isEmpty()){
            return "0";
        }
        string str = "";
        str += mList[mList.Count - 1].value;
        string strpoint = "";
        int value2 = 0;
        if (mList.Count >= 2) {
            if (mList[mList.Count - 1].value > 99)
            {
                value2 = mList[mList.Count - 2].value / 100;
                if (value2 != 0)
                {
                    strpoint = strpoint+"."+ value2;
                }
            }
            else if (mList[mList.Count - 1].value > 9)
            {
                value2 = mList[mList.Count - 2].value / 10;
                if (value2 > 9)
                {
                    if (value2 % 10 == 0)
                    {
                        strpoint = strpoint + "."+(value2 / 10);
                    }
                    else
                    {
                        strpoint = strpoint + "." + value2;
                    }
                }
                else if (value2 > 0)
                {
                    strpoint = strpoint  + ".0" + value2;
                }
            }
            else if (mList[mList.Count - 1].value > 0) {
                value2 = mList[mList.Count - 2].value;
                if (value2 > 99)
                {
                    if (value2 % 100 == 0)
                    {
                        strpoint = strpoint + "." + (value2 / 100);
                    }
                    else if (value2 % 10 == 0)
                    {
                        strpoint = strpoint + "." + (value2 / 10);
                    }
                    else
                    {
                        strpoint = strpoint + "." + value2;
                    }
                }
                else if (value2 > 9)
                {
                    strpoint = strpoint + ".0";
                    if (value2 % 10 == 0)
                    {
                        strpoint += (value2 / 10);
                    }
                    else
                    {
                        strpoint += value2;
                    }
                }
                else if (value2 > 0)
                {                    
                    strpoint = strpoint+ ".00"+value2;
                } 
            }
        }
        str = str + strpoint + mList[mList.Count - 1].unit;
        return str;
    }
    public static BigNumber add(BigNumber big1, BigNumber big2) {
        BigNumber min, max;
        int minLeng, maxLeng;
//        Debug.Log("BigNumber add bg1= " + big1.toString() + " bg2 = " + big2.toString());
//        Debug.Log("BigNumber add bg1= " + big1.toStringWithUnit() + " bg2 = " + big2.toStringWithUnit());
        if (big1.mList.Count > big2.mList.Count)
        {
            max = big1;
            min = big2;
            minLeng = min.mList.Count;
            maxLeng = max.mList.Count;
        }
        else {
            max = big2;
            min = big1;
            minLeng = min.mList.Count;
            maxLeng = max.mList.Count;
        }
        BigNumber back = new BigNumber();
        int index = 0;
        int up = 0;
        while (index < maxLeng) {
            BigNumberUnit unit1 = new BigNumberUnit();
            if (index < minLeng)
            {
                int value = max.mList[index].value + min.mList[index].value + up;
                unit1.value = value % 1000;
                up = value / 1000;
                unit1.unit = big1.mList[index].unit;
                back.mList.Add(unit1);
            }
            else {
                int value = max.mList[index].value + up;
                unit1.value = value % 1000;
                unit1.unit = max.mList[index].unit;
                up = value / 1000;
                back.mList.Add(unit1);
            }
            index++;
        }
        if(up != 0) {
            BigNumberUnit unit1 = new BigNumberUnit();
            unit1.value = up;
            unit1.setUnit(index);
            back.mList.Add(unit1);
        }
        return back;
    }
    public static BigNumber minus(BigNumber big1, BigNumber big2) {
        int isEquit = big1.ieEquit(big2);
        BigNumber back = new BigNumber();
        if (isEquit == 0)
        {
            BigNumberUnit unit1 = new BigNumberUnit();
            back.mList.Add(unit1);
        }
        else  {
            back = minusZheng(big1,big2);
        }
        return back;
    }

    public static  BigNumber multiply(BigNumber big, float multiplying) {
        int index = 0;
        int leng = big.mList.Count;
        int up = 0;
        BigNumberUnit unit1;
        int value;
        BigNumber back = new BigNumber();
        while (index < leng) {
            unit1 = new BigNumberUnit();
            float v = big.mList[index].value * multiplying;
//            Debug.Log("big.mList[index].value =" + big.mList[index].value + " multiplying = " + multiplying + " v=" + v);
            int v2 = (int)v;
//            Debug.Log("v2 =" + v2);
            float v3 = v - v2;
//            Debug.Log("v3 =" + v3);

            if (index > 0 && v3 > 0) {
                int v4 = (int)(v3 * 1000);
                int v5 = back.mList[index - 1].value + v4;
//                Debug.Log("v5 =" + v5 );
                if (v5 >= 1000)
                {
                    back.mList[index - 1].value = v5 % 1000;
                    up = up + v5 / 1000;
                }
                else {
                    back.mList[index - 1].value = v5;
                }
//                Debug.Log("big.mList[index - 1].value =" + big.mList[index - 1].value);
            }

            value = (int)(big.mList[index].value * multiplying)+up;
           
            unit1.value = value % 1000;
            unit1.unit = big.mList[index].unit;
//            Debug.Log("value =" + unit1.value+" unit = "+ unit1.unit);
            back.mList.Add(unit1);
            up = value / 1000;
            index++;
        }
        if (up != 0) {
            unit1 = new BigNumberUnit();
            unit1.value = up;
            unit1.setUnit(index);
//            Debug.Log("value =" + unit1.value + " unit = " + unit1.unit);
            back.mList.Add(unit1);
        }
        return back;
    }

    private static BigNumber minusZheng(BigNumber max, BigNumber min) {
        BigNumber big = new  BigNumber();
        int minLeng, maxLeng;
        int index = 0;
        int up = 0;
        minLeng = min.mList.Count;
        maxLeng = max.mList.Count;
        while (index < maxLeng)
        {
            BigNumberUnit unit1 = new BigNumberUnit();
            if (index < minLeng)
            {
                int value;
                if (max.mList[index].value + up < min.mList[index].value)
                {
                    value = 1000 + max.mList[index].value + up - min.mList[index].value;
                    up = -1;
                }
                else {
                    value =  max.mList[index].value + up - min.mList[index].value;
                    up = 0;
                }
                unit1.value = value;
                unit1.unit = max.mList[index].unit;
            }
            else if(index < maxLeng)
            {
                int value = max.mList[index].value + up;
                if (value < 0)
                {
                    value = 1000 + value;
                    up = -1;
                }
                else {
                    up = 0;
                }
                unit1.value = value;
                unit1.unit = max.mList[index].unit;               
            }
            big.mList.Add(unit1);
            index++;
        }
        BigNumberUnit unit2 = big.mList[big.mList.Count - 1];
        if (unit2.value == 0) {
            big.mList.Remove(unit2);
        }
        return big;
    }

    public int ieEquit(BigNumber big) {
        if (big.mList.Count > mList.Count)
        {
            return -1;
        }
        else if (big.mList.Count < mList.Count) {
            return 1;
        }else{
            for (int i = big.mList.Count - 1; i >= 0; i--) {
                if (big.mList[i].value > mList[i].value)
                {
                    return -1;
                }
                else if (big.mList[i].value < mList[i].value)
                {
                    return 1;
                }
            }


            return 0;
        }
    }
}
