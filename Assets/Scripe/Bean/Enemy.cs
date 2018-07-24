using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy
{
	public long id;
	public float monster_hp;
	public float monster_attack;
	public float monster_defense;
	public float die_gas;
	public float die_crystal;
	public float monster_speed;
	public float attack_range;
	public float attack_speed;
	public long resource;
	public string abc;
	public long trajectory_resource;
	public long hit_resource;
    public string death_fell;
    public List<FellObjectBean> fellList = new List<FellObjectBean>();

    public List<FellObjectBean> getFellList() {
        if (fellList.Count == 0)
        {
            if (death_fell == null || death_fell.Length < 1)
            {
                return null;
            }
            string[] array = death_fell.Split('}');
            foreach (string str2 in array)
            {
                if (str2 == null || str2.Length < 1)
                {
                    continue;
                }
                string str3 = str2.Replace("{", "");
                if (str3 == null || str3.Length < 1)
                {
                    continue;
                }
                string[] array2 = str3.Split(',');
                FellObjectBean bean = new FellObjectBean(long.Parse(array2[0]), float.Parse(array2[1]));
                fellList.Add(bean);
            }
            death_fell = null;
            if (fellList.Count == 0) {
                return null;
            }
        }
        return fellList;
    }
}

