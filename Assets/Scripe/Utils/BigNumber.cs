using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BigNumber 
{
    public List<BigNumberUnit> mList = new List<BigNumberUnit>();

    public static BigNumber getBigNumForString(string s) {
        BigNumber big = new BigNumber();
        int index = 0;
        while (s.Length >= 3) {
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

    private static string spileNumberString(string str, BigNumberUnit unit) {
        string s1 = str.Substring(str.Length - 3, str.Length);
        unit.value = int.Parse(s1);
        str = str.Substring(0, str.Length - 3);
        return str;
    }
    public string toString() {
        string str = "";
        for (int i = mList.Count-1; i >= 0; i--) {
            str += mList[i].value;
        }
        return str;
    }
    public string toStringWithUnit() {
        string str = "";
        str += mList[mList.Count - 1];
        if (mList.Count >= 2) {
            str = str + "." + mList[mList.Count - 2];
        }
        str += mList[mList.Count - 1].unit;
        return str;
    }
    public static BigNumber add(BigNumber big1, BigNumber big2) {
        BigNumber min, max;
        int minLeng, maxLeng;
        if (big1.mList.Count > big2.mList.Count)
        {
            max = big1;
            min = big2;
            minLeng = big2.mList.Count;
            maxLeng = big1.mList.Count;
        }
        else {
            max = big2;
            min = big1;
            minLeng = big1.mList.Count;
            maxLeng = big2.mList.Count;
        }
        BigNumber back = new BigNumber();
        int index = 0;
        int up = 0;
        while (index < maxLeng) {
            BigNumberUnit unit1 = new BigNumberUnit();
            if (index < minLeng)
            {
                int value = max.mList[index].value + min.mList[index].value + up;
                unit1.value = value / 1000;
                up = value % 1000;
                unit1.unit = big1.mList[index].unit;
                back.mList.Add(unit1);
            }
            else {
                int value = big2.mList[index].value + up;
                unit1.value = value /1000;
                unit1.unit = big2.mList[index].unit;
                up = value % 1000;
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

    public  void multiply(float multiplying) {
        int index = 0;
        int leng = mList.Count;
        int up = 0;
        BigNumberUnit unit1;
        int value;
        while (index < leng) {
            unit1 = mList[index];
            value = (int)(unit1.value * multiplying);
            unit1.value = value % 1000;
            up = value / 1000;
            index++;
        }
        if (up != 0) {
            unit1 = new BigNumberUnit();
            unit1.value = up;
            unit1.setUnit(index);
            mList.Add(unit1);
        }
        return;
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
                    value =  max.mList[index].value - up - min.mList[index].value;
                    up = 0;
                }
                unit1.value = value;
                unit1.unit = max.mList[index].unit;
            }
            else
            {
                int value = max.mList[index].value + up;
                unit1.value = value;
                unit1.unit = max.mList[index].unit;
               
            }
            big.mList.Add(unit1);
            index++;
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
            if (big.mList[big.mList.Count - 1].value > mList[mList.Count - 1].value)
            {
                return -1;
            }
            else if(big.mList[big.mList.Count - 1].value < mList[mList.Count - 1].value){
                return 1;
            }
            return 0;
        }
    }
}
