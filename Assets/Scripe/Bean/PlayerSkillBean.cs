using UnityEngine;
using System.Collections;

public class PlayerSkillBean
{
    public long id;
    public long time;
    public long chance = 0;
    public long lastTime;
    public PlayerSkillBean(long id, long time, long chance) {
        this.id = id;
        this.time = time;
        this.chance = chance;
    }

    public bool isDeal() {
        long now = TimeUtils.GetTimeStamp();
        if (now - lastTime < time) {
            return false;
        }
        if (chance == 0)
        {
            return true;
        }
        else {
           int ran =  Random.Range(0, 10000);
            if (ran < chance) {
                return true;
            }
        }

        return false;
    }
    public void deal() {
        lastTime = TimeUtils.GetTimeStamp();
    }
}
