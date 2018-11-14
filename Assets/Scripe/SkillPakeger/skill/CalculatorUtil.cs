using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalculatorUtil 
{
    string mStr;
    string mParameter;

    Dictionary<string, float> parameter = new Dictionary<string, float>();
    Attacker firer;
    Attacker target;
    CalculatorUtilBean mBean;
    AttackSkillBase attackSkill;

    public void setSkill(AttackSkillBase attackSkill) {
        this.attackSkill = attackSkill;
    }

    public CalculatorUtil(string str, string parStr) {
        mStr = str;
        mParameter = parStr;
        getParList();
        getBean();
    }
    public  float getValue(Attacker firer, Attacker target) {
        this.firer = firer;
        this.target = target;


        if (mBean == null) {
            return -1;
        }
        if (mBean.bean == -1) {
            return -1;
        }

        float value = getValue(mBean);
        Debug.Log("float value = " + value);
        return value;
    }
    private void getBean()
    {
        //Debug.Log("mStr ="+ mStr);
        char[] chars = mStr.ToCharArray();
        mBean = new CalculatorUtilBean();
        if (getListBean(chars, mBean, 0) == -1)
        {
            mBean = null;
            //Debug.Log("getListBean == null");
        }
       // if (mBean != null) {
       //     printfBean(mBean, "    ");
       // }
    }
    private void printfBean(CalculatorUtilBean bean, string start) {
        if (bean == null) {
            return;
        }
        Debug.Log(start + "bean=" + bean.bean + " type=" + bean.type + " valueKey=" + bean.valueKey);
        if (bean.list != null && bean.list.Count > 0) {
            foreach (CalculatorUtilBean bean2 in bean.list) {
                printfBean(bean2, start + "    ");
            }
        }
    }
    private void getParList() {
        if (mParameter != null && mParameter.Length > 0) {
            string[] strs = mParameter.Split(',');
            for (int i = 0; i < strs.Length; i++) {
                if (strs[i] == null || strs[i].Length == 0) {
                    continue;
                }
                parameter.Add("a" + (i + 1), float.Parse(strs[i]));
             //   Debug.Log("a" + (i + 1) + "=" + parameter["a" + (i + 1)]);

            }
        }
    }

    private  float getValue(CalculatorUtilBean bean) {
        if (mBean == null) {
            return -1;
        }
        CalculatorUtilBean value = new CalculatorUtilBean();
        getValueForBean(bean.list[0]);
        value.bean = bean.list[0].bean;
        value.type = bean.list[0].type;
        int start = 0;
        int tmp = 1;
        while (true) {
          //  Debug.Log("value.bean =" + value.bean);
            tmp = start + 1;
            if (value.type == 0) {
                break;
            }
            if (tmp  < bean.list.Count && (value.type == CalculatorUtilBean.TYPE_DIVIDE || value.type == CalculatorUtilBean.TYPE_MULTIPLY))
            {
                getValueForBean(bean.list[tmp]);
                if (value.type == CalculatorUtilBean.TYPE_DIVIDE)
                {
                    value.bean /= bean.list[tmp].bean;
                }
                else {
                    value.bean *= bean.list[tmp].bean;
                }
                value.type = bean.list[tmp].type;
                start = tmp;
            }
            else {
                CalculatorUtilBean tmpBean = new CalculatorUtilBean();
                getValueForBean(bean.list[tmp]);
                tmpBean.bean = bean.list[tmp].bean;
                tmpBean.type = bean.list[tmp].type;
                while (tmp + 1< bean.list.Count &&( bean.list[tmp].type == CalculatorUtilBean.TYPE_DIVIDE || bean.list[tmp].type == CalculatorUtilBean.TYPE_MULTIPLY)) {
                    if (tmpBean.type == 0) {
                        break;
                    }
                    getValueForBean(bean.list[tmp+1]);
                    if (tmpBean.type == CalculatorUtilBean.TYPE_DIVIDE)
                    {
                        tmpBean.bean /= bean.list[tmp+1].bean;
                    }
                    else
                    {
                        tmpBean.bean *= bean.list[tmp+1].bean;
                    }
                    tmpBean.type = bean.list[tmp+1].type;
                    tmp++;
                }
                if (value.type == CalculatorUtilBean.TYPE_ADD)
                {
                    value.bean += tmpBean.bean;
                }
                else
                {
                    value.bean -= tmpBean.bean;
                }
                value.type = tmpBean.type;
                start = tmp;
            }
        }
        float a = value.bean;
        int tmp1 = a % 1 == 0 ? 0 : 1;
        value.bean = ((int)a) / 1 + tmp1;
        return value.bean;
    }

    private  void getValueForBean(CalculatorUtilBean bean) {
        if (bean.list != null)
        {
            bean.bean = getValue(bean);
        }
        else if (bean.valueKey != null && bean.valueKey.Length > 0)
        {
            bean.bean = getValueForKey(bean.valueKey);
        }
    }

    private  float getValueForKey(string key) {
        if (key.StartsWith("a"))
        {
            return parameter[key];
        }
        else {
            Attacker tmp = null;
            string[] strs = key.Split(',');
            string keyId = "0";
            if (strs[0].Equals("1")) {
                tmp = firer;
                keyId = strs[1];
            }
            else if (strs[0].Equals("2"))
            {
                tmp = target;
                keyId = strs[1];
            }
            else if (strs[0].Equals("3"))
            {
                keyId = strs[1];
                int idd = int.Parse(keyId);
                return attackSkill.getValueById(idd);
            }
            else if (strs[0].Equals("4"))
            {
                tmp = firer;
                return tmp.mSkillManager.getValueBySkillAndId(long.Parse(strs[1]),long.Parse(strs[2]));
            }
            else if (strs[0].Equals("5"))
            {
                tmp = target;
                return tmp.mSkillManager.getValueBySkillAndId(long.Parse(strs[1]), long.Parse(strs[2]));
            }
            else
            {
                return 0;
            }
            long id = int.Parse(keyId);
           
            if (id == 100)
            {
                return tmp.mAttribute.aggressivity;
            }
            else if (id == 101)
            {
                Debug.Log("mStr = " + mStr);
                return tmp.mAttribute.defense;
            }
            else if (id == 102)
            {
                return tmp.mAttribute.maxBloodVolume;
            }
            else if (id == 110)
            {
                return tmp.mAttribute.rate;
            }
            else if (id == 111)
            {
                return tmp.mAttribute.evd;
            }
            else if (id == 112)
            {
                return tmp.mAttribute.crt;
            }
            else if (id == 113)
            {
                return tmp.mAttribute.crtHurt ;
            }
            else if (id == 115)
            {
                return tmp.mAttribute.readHurt;
            }
            else if (id == 114)
            {
                return tmp.mAttribute.attackSpeed;
            }
            else if (id == 1)
            {
                return tmp.mBloodVolume;
            }
        }
        return -1;
    }

    private  int getListBean(char[] chars, CalculatorUtilBean root,int index) {
        if (root.list == null)
        {
            root.list = new List<CalculatorUtilBean>();
        }
        for (; index < chars.Length;) {
           // Debug.Log("chars[" + index + "]=" + chars[index]);
            CalculatorUtilBean bean = new CalculatorUtilBean();
            root.list.Add(bean);
            if (chars[index] == '(')
            {             
                index = getListBean(chars, bean, ++index);
                if (index == -1)
                {
                    return -1;
                }
            }
            else if (chars[index] == ')')
            {
                index++;
                if (index == chars.Length)
                {
                    return index;
                }
                else
                {
                    int type = getType(chars[index]);
                    if (type == -1)
                    {
                        return -1;
                    }
                    else if (type != 0)
                    {
                        index++;
                    }
                    root.type = type;
                }
                return index;
            }
            else if (chars[index] == '"')
            {
                index++;
                string str = "";
                while (index < chars.Length && chars[index] != '"')
                {
                 //   Debug.Log("chars[" + index + "]=" + chars[index]);
                    str += chars[index];
                    index++;
                }
               // Debug.Log(" 取值的值 = "+str);
                if (index == chars.Length)
                {
                    return -1;
                }
                else if (chars[index] == '"')
                {
                    bean.valueKey = str;
                    index++;
                }
                if (index == chars.Length)
                {
                    return index;
                }
                int type = getType(chars[index]);
                if (type == -1)
                {
                    return -1;
                }
                else if (type != 0)
                {
                    index++;
                }
                bean.type = type;
            }
            else if (chars[index] == 'a')
            {
                string str = "a";
                index++;
                while (index < chars.Length && chars[index] >= '0' && chars[index] <= '9')
                {
                    str += chars[index];
                    index++;
                }
                bean.valueKey = str;
                if (index == chars.Length)
                {
                    return index;
                }
                int type = getType(chars[index]);
                if (type == -1)
                {
                    return -1;
                }else if (type != 0)
                {
                    index++;
                }
                bean.type = type;
                
                
            }
            else if (chars[index] >= '0' && chars[index] <= '9')
            {
                float value = 0;

                float beishu = 10;

                float value1 = 0;
                float value2 = 0;

                bool isXiaoshu = false;

                while (index < chars.Length && ((chars[index] >= '0' && chars[index] <= '9') || chars[index] == '.'))
                {
                   // Debug.Log("chars[" + index + "]=" + chars[index]);
                    if (!isXiaoshu)
                    {
                        if (chars[index] == '.')
                        {
                            value1 = value;
                            value = 0;
                            isXiaoshu = true;
                            beishu = 0.1f;
                            index++;
                        }
                        else
                        {
                            value = value * beishu + chars[index] - '0';
                            index++;
                        }
                    }
                    else
                    {
                        if (chars[index] == '.')
                        {
                            return -1;
                        }
                        else
                        {
                            value = value + (chars[index] - '0') * beishu;
                            beishu *= 0.1f;
                            index++;
                        }
                    }
                }
                if (value != 0)
                {
                    if (value1 != 0)
                    {
                        value2 = value;
                    }
                    else
                    {
                        value1 = value;
                    }
                }
                bean.bean = value1 + value2;
               // Debug.Log("value1 =" + value1+" value2"  + value2+ " index="+ index+ "  chars.Length=" + chars.Length);
                if (index == chars.Length)
                {
                    return index;
                }
                else
                {
                    int type = getType(chars[index]);
                    if (type == -1)
                    {
                        return -1;
                    }
                    else if (type != 0)
                    {
                        index++;
                    }
                    bean.type = type;
                }
            }
            else {
                return -1;
            }
        }
        return -1;
    }
    private static int getType(char c) {
        if (c == '+')
        {
            return CalculatorUtilBean.TYPE_ADD;
        }
        else if (c == '-')
        {
            return CalculatorUtilBean.TYPE_MINUS;
        }
        else if (c == '/')
        {
            return CalculatorUtilBean.TYPE_DIVIDE;
        }
        else if (c == '*')
        {
            return CalculatorUtilBean.TYPE_MULTIPLY;
        }
        else if (c == ')')
        {
            return 0;
        }
        else {
            return -1;
        }
    }
}
