using System.Collections.Generic;
using UnityEngine;
public class AttackSpeedBean 
{
    public float interval = 0;
    public float speed = 1;
    public float leng = 1;
    public static  AttackSpeedBean GetAttackSpeed(ResourceBean resource,float speed)
    {
        float eachFors = JsonUtils.getIntance().getFrequencyByValue(speed);
        AttackSpeedBean bean = new AttackSpeedBean();
        float oneTime = 1 / eachFors;
        float animalTime = resource.attack_all_framce / 12;
        if (oneTime >= animalTime)
        {
            bean.interval = oneTime - animalTime;
            bean.speed = 1;
            bean.leng = animalTime / bean.speed - 0.001f;
        }
        else {         
            while (true) {
                if (oneTime / bean.speed < animalTime)
                {
                    bean.speed = bean.speed - 0.1f;
                    bean.interval = oneTime / bean.speed - animalTime;
                    bean.leng = animalTime / bean.speed - 0.001f;
                    break;
                }
                else {
                    bean.speed = bean.speed + 0.1f;
                }
            }
        }
        bean.printf();
        return bean;
    }
    private  void printf() {
        Debug.Log(" AttackSpeedBean :interval=" + interval + " speed=" + speed + " leng=" + leng);
    }
}
