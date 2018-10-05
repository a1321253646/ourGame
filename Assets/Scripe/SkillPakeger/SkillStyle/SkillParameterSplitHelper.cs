using System.Collections.Generic;
using UnityEngine;

public class SkillParameterSplitHelper
{
    public static List<List<float>> get(long effects,List<float> value) {
        List < List<float> > back = new List<List<float>>();
        if (effects == 10) {
            List<float> list1 = new List<float>();
            List<float> list2 = new List<float>();
            list1.Add(value[0]);
            list1.Add(value[1]);
            list2.Add(value[2]);
            back.Add(list1);
            back.Add(list2);
        } else {
            back.Add(value);
        }
        return back;
    }
}
