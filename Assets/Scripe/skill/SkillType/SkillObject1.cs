using UnityEngine;
using System.Collections;

public class SkillObject1 : SkillObject
{
    public override void initEnd()
    {
        mAnimalControl.start();
    }

}
