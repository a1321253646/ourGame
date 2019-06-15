using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillJsonBean : MonoBehaviour
{

    public static long TYPE_SELF = 1;
    public static long TYPE_ENEMY = 2;


    public long id;
    public long target_type;
    public string effects_parameter;
    public string calculator;
    public string special_parameter_value;
    public string next_skill;
    public string skill_describe;
    public long skill_resource;
    public long shape_type;
    public string shape_resource_id;
    public double leng;
    public double wight;
    public long point_type;
    public List<float> specialParameterValue;
    public List<float> effectsParameterValue;
    public List<long> nextSkillList;

    public List<float> getEffectsParameterValue() {
        if (effectsParameterValue == null && effects_parameter != null)
        {
            effectsParameterValue = new List<float>();
            string[] strs = effects_parameter.Split(',');
            foreach (string str in strs)
            {
                if (str != null && str.Length > 0)
                {
                    float tmp = float.Parse(str);
                    effectsParameterValue.Add(tmp);
                }
            }
        }
        return effectsParameterValue;
    }


    public List<float> getSpecialParameterValue() {
        if (specialParameterValue == null && special_parameter_value != null) {
            specialParameterValue = new List<float>();
            string[] strs = special_parameter_value.Split(',');
            foreach (string str in strs) {
                if (str != null && str.Length > 0) {
                    float tmp = float.Parse(str);
                    specialParameterValue.Add(tmp);
                }
            }
        }
        return specialParameterValue;
    }
    public void  setSpecialParameterValue(List<float> list) {
        specialParameterValue = list;
    }
    public List<long> getNextSkillList()
    {
        if (nextSkillList == null && next_skill != null)
        {
            nextSkillList = new List<long>();
            string[] strs = next_skill.Split(',');
            foreach (string str in strs)
            {
                if (str != null && str.Length > 0)
                {
                    long tmp = long.Parse(str);
                    nextSkillList.Add(tmp);
                }
            }
        }
        return nextSkillList;
    }
}
