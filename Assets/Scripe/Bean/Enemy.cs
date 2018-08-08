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
    public List<long> fellList = new List<long>();

    public List<FellObjectBean> fell() {
        List<FellObjectBean> list = new List<FellObjectBean>();
        List<FellObjectBean> tmp;
        fellList = getFellList();
        foreach(long id  in fellList){
            tmp = JsonUtils.getIntance().getDropDevoiceByID(id).fell();
            if (tmp != null) {
                list.AddRange(tmp);
            }
        }
        if (list.Count == 0)
        {
            return null;
        }
        else {
            return list;
        }
    }

    private List<long> getFellList() {
        if (fellList.Count == 0)
        {
            if (death_fell == null || death_fell.Length < 1)
            {
                return null;
            }
            Debug.Log("death_fell = " + death_fell);
            string[] array = death_fell.Split(',');

            foreach (string str2 in array)
            {
                Debug.Log("str2 = " + str2);
                if (str2 == null || str2.Length < 1)
                {
                    continue;
                }
                fellList.Add(long.Parse(str2));
            }
        }
        return fellList;
    }
}

