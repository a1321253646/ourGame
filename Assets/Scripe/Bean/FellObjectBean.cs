using UnityEngine;
using System.Collections;

public class FellObjectBean 
{
    long id;
    float probability;
    public FellObjectBean(long id, float probability) {
        this.id = id;
        this.probability = probability;
    }
    public long getId() {
        return id;
    }

    public bool isFell() {
       int rangeRadomNum = Random.Range(0, 1000000);
        Debug.Log("id = " + id + " rangeRadomNum =" + rangeRadomNum + " probability = " + probability * 10000);
        if (rangeRadomNum <= probability * 10000*5)
        {
            return true;
        }
        else {
            return false;
        }
    }
}
