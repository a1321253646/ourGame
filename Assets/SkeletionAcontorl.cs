using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletionAcontorl : MonoBehaviour
{
    SkeletonAnimation mAnimal;
    
    // Start is called before the first frame update
    void Start()
    {
        mAnimal = GetComponent<SkeletonAnimation>();
        mAnimal.state.Event += state_Event;

        mAnimal.AnimationName = "walk01";
       
    }

    void state_Event(Spine.AnimationState state, int trackIndex, Spine.Event e)
    {
        Debug.Log("state_Event Name=" + e.Data.Name+ " trackIndex="+ trackIndex+ " e ="+ e.String);

    }
        void Update()
    {
        
    }
}
