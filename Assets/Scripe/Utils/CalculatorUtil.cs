using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CalculatorUtil 
{
    string mStr;
    string mParameter;

    Dictionary<string, double> parameter = new Dictionary<string, double>();
    Attacker firer;
    Attacker target;
    List<CalculatorUtilBean> mBean = new List<CalculatorUtilBean>();

//    AttackSkillBase attackSkill;
    AttackerSkillBase attackSkill2;

//    public void setSkill(AttackSkillBase attackSkill) {
 //       this.attackSkill = attackSkill;
//    }
    public void setSkill(AttackerSkillBase attackSkill)
    {
        this.attackSkill2 = attackSkill;
    }
    long mId = -1;
    public CalculatorUtil(string str, string parStr) {
        creatThis(str, parStr, -1);
    }
    public CalculatorUtil(string str, string parStr, long id)
    {
        creatThis(str, parStr, id);
    }
    private void creatThis(string str, string parStr,long id) {
        mStr = str;
        mParameter = parStr;
        bool isDeal = false;
        foreach (CardJsonBean c in JsonUtils.getIntance().getCardInfos()) {
            if (c.skill_id == id) {
                mId = c.id;
                isDeal = true;
            }
        }
        if (!isDeal) {
            foreach (YongjiuCardBean c in JsonUtils.getIntance().getYongjiuCardInfos())
            {
                if (c.skill_id == id)
                {
                    mId = c.id;
                    isDeal = true;
                }
            }
        }

        getParList();
        if (mStr != null && mStr.Length > 0) {
            string[] strs = mStr.Split('|');
            foreach (string s in strs)
            {
                if (s != null && s.Length > 0)
                {
                    CalculatorUtilBean bean = getBean(s);
                    //                Debug.Log("CalculatorUtil bean = " + bean);
                    if (bean != null)
                    {

                        //                  Debug.Log("bean = " + bean.getString());
                        mBean.Add(bean);

                    }
                }
            }
        }
    }
    public  double getValue(Attacker firer, Attacker target) {
        this.firer = firer;
        this.target = target;
        double value = -1;
        getParList();
        Debug.Log("CalculatorUtil bean = " + mBean.Count);
        foreach (CalculatorUtilBean bean in mBean) {
            if (bean == null)
            {
                continue;
            }
            if (bean.bean == -1)
            {
                continue;
            }

            value = getValue(bean);
            Debug.Log("float value = " + value);
            if (value <= 0)
            {
                continue;
            }
            else {
                return value;
            }
        }
        return value;
    }
    private CalculatorUtilBean getBean(string str)
    {
        //Debug.Log("mStr ="+ mStr);
        char[] chars = str.ToCharArray();
        CalculatorUtilBean bean = new CalculatorUtilBean();
        long back = getListBean(chars, bean, 0);

        if (back == -1)
        {
            bean = null;
            //Debug.Log("getListBean == null");
        }
        return bean;
       // if (mBean != null) {
       //     printfBean(mBean, "    ");
       // }
    }
    private void printfBean(CalculatorUtilBean bean, string start) {
        if (bean == null) {
            return;
        }
//        Debug.Log(start + "bean=" + bean.bean + " type=" + bean.type + " valueKey=" + bean.valueKey);
        if (bean.list != null && bean.list.Count > 0) {
            foreach (CalculatorUtilBean bean2 in bean.list) {
                printfBean(bean2, start + "    ");
            }
        }
    }
    private void getParList() {
        Debug.Log("mId = " + mId);
        if (mParameter != null && mParameter.Length > 0) {
            parameter.Clear();
            string[] strs = mParameter.Split(',');
            for (int i = 0; i < strs.Length; i++) {
                if (strs[i] == null || strs[i].Length == 0) {
                    continue;
                }
                string str = strs[i];
                double tmp = 0;
                if (str.Contains("L"))
                {
                    long l = SQLHelper.getIntance().getCardLevel(mId);
                    if (l == -1) {
                        l = 1;
                    }
                    Debug.Log("l = " + l);
                    string s = new string(str.Replace("L", "" + l).ToCharArray());
                    Debug.Log("tmp=" + s + "*");
                    CalculatorUtil ca = new CalculatorUtil(s, null);
                    tmp = ca.getValue(null, null);
                    Debug.Log("tmp=" + tmp);
                }
                else
                {
                    tmp = double.Parse(str);
                }

                parameter.Add("a" + (i + 1), tmp);
                Debug.Log("a" + (i + 1) + "=" + parameter["a" + (i + 1)]);

            }
        }
    }

    private  double getValue(CalculatorUtilBean bean) {
        if (mBean == null) {
            return -1;
        }
        getParList();
        CalculatorUtilBean value = new CalculatorUtilBean();
        getValueForBean(bean.list[0]);
        value.bean = bean.list[0].bean;
        value.type = bean.list[0].type;
        int start = 0;
        int tmp = 1;
        while (true) {
//            Debug.Log("value.bean =" + value.bean);
            tmp = start + 1;
            if (value.type == 0) {
                break;
            }
            if (tmp  < bean.list.Count && (value.type == CalculatorUtilBean.TYPE_DIVIDE || value.type == CalculatorUtilBean.TYPE_MULTIPLY))
            {
//                Debug.Log("bean.list[tmp].bean =" + bean.list[tmp].bean);
                getValueForBean(bean.list[tmp]);
//                Debug.Log("bean bean = " + bean.list[tmp].bean);
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
        double a = value.bean;
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
//        Debug.Log("bean bean = " + bean.bean);
    }

    private  double getValueForKey(string key) {
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
             //   if (attackSkill != null)
              //  {
              //      return attackSkill.getValueById(idd);
              //  }
            //    else {
                    return attackSkill2.getValueById(idd);
             //   }
               
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
 //               Debug.Log("mStr = " + mStr);
                return tmp.mAttribute.defense;
            }
            else if (id == 102)
            {
                return tmp.mAttribute.maxBloodVolume;
            }
            else if (id == 103)
            {
                return tmp.mBloodVolume;
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
        }
        return -1;
    }

    private  int getListBean(char[] chars, CalculatorUtilBean root,int index) {
 //       Debug.Log("getListBean");
        if (root.list == null)
        {
            root.list = new List<CalculatorUtilBean>();
        }
        for (; index < chars.Length;) {
            Debug.Log("chars[" + index + "]=" + chars[index]);
            CalculatorUtilBean bean = new CalculatorUtilBean();
            root.list.Add(bean);
            if (chars[index] == '(')
            {             
                index = getListBean(chars, bean, ++index);
     //           Debug.Log("右括号返回 " + index);
                if (index == -1)
                {
                    return -1;
                }
                if (index == chars.Length) {
                    return index;
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
       //         Debug.Log("getListBean valueKey = " + bean.valueKey + " bean.type" + bean.type);
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
               // Debug.Log("getListBean valueKey = " + bean.valueKey + " bean.type" + bean.type);

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
//                    Debug.Log("chars[" + index + "]=" + chars[index]);
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
                Debug.Log("value1 =" + value1+" value2"  + value2+ " index="+ index+ "  chars.Length=" + chars.Length);
                if (index == chars.Length)
                {
                    return index;
                }
                else
                {
                    int type = getType(chars[index]);
                    Debug.Log("type=" + type);
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
        return index;
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
