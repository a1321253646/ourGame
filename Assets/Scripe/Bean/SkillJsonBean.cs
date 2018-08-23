using UnityEngine;
using System.Collections;

public class SkillJsonBean : MonoBehaviour
{

    public static long TYPE_SELF = 1;
    public static long TYPE_ENEMY = 2;


    public long id;
    public long effects;
    public long target_type;
    public string effects_parameter;
    public string calculator;
    public string special_parameter_key;
    public string special_parameter_value;
    public long next_skill;
    public string skill_describe;
    public long skill_resource;
    public long shape_type;
    public string shape_resource_id;
    public float leng;
    public float wight;
    public long point_type;
}
