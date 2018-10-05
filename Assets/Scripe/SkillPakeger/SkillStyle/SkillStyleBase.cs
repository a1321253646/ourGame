using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillStyleBase
{
    public long skillStyleId = -1;

    public List<SkillJsonBean> getNextSkillList() {
        return null;
    }
    public static SkillStyleBase getNewIntance(long id) {
        SkillStyleBase intance = null;
        if (id == 10001 ||
            id == 10002 ||
            id == 10003 ||
            id == 10004 ||
            id == 10005 ||
            id == 10006 ||
            id == 10007 ||
            id == 10011 ||
            id == 10012 ||
            id == 10013 ||
            id == 10014)
        {
            intance = new SkillStyleBase();
        }
        else if (id == 10008)
        {
            intance = new SkillStyle10008();
        }
        else if (id == 10009)
        {
            intance = new SkillStyle10009();        
        }
        else if (id == 10010)
        {
            intance = new SkillStyle10010();
        }
        intance.skillStyleId = id;
        return intance;
    }
}
