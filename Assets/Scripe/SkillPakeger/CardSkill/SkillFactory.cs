﻿using UnityEngine;
using System.Collections;

public class SkillFactory
{
    public static void skillObjectAddComponet( GameObject newobj, SkillJsonBean skill) {
        if (skill.id == 100001) {
            newobj.AddComponent<SkillObject100001>();
        }
        else if (skill.id == 100003)
        {
            newobj.AddComponent<SkillObject100003>();
        }
        else if (skill.id == 100004)
        {
            newobj.AddComponent<SkillObject100004>();
        }
        else if (skill.id == 100005)
        {
            newobj.AddComponent<SkillObject100005>();
        }
        else if (skill.id == 100006)
        {
            newobj.AddComponent<SkillObject100006>();
        }
        else if (skill.id == 100007)
        {
            newobj.AddComponent<SkillObject100007>();
        }
        else if (skill.id == 100009)
        {
            newobj.AddComponent<SkillObject100009>();
        }
        else if (skill.id == 100010)
        {
            newobj.AddComponent<SkillObject100010>();
        }
        else if (skill.id == 100011)
        {
            newobj.AddComponent<SkillObject100011>();
        }
        else if (skill.id == 100012)
        {
            newobj.AddComponent<SkillObject100012>();
        }
        else if (skill.id == 100013)
        {
            newobj.AddComponent<SkillObject100013>();
        }
        else if (skill.id == 100015)
        {
            newobj.AddComponent<SkillObject100015>();
        }
        else if (skill.id == 100021)
        {
            newobj.AddComponent<SkillObject100021>();
        }
        else if (skill.id == 100022)
        {
            newobj.AddComponent<SkillObject100022>();
        }
        else if (skill.id == 100024)
        {
            newobj.AddComponent<SkillObject100024>();
        }
        else if (skill.id == 100025)
        {
            newobj.AddComponent<SkillObject100025>();
        }
        else if (skill.id == 300001)
        {
            newobj.AddComponent<SkillObject300001>();
        }
    }
}
