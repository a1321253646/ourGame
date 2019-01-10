using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillObject100009 : SkillObject
{
    public override void initEnd()
    {
        GameObject.Find("jineng").GetComponent<CardManager>().addCards((long)mBean.getSpecialParameterValue()[0]);
        mSkillStatus = SKILL_STATUS_END;
    }
}
